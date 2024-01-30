////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AI;
public class PlayerAttack : MonoBehaviour
{
    [Header("UI Settings")]
    public bool showCooldownUI = false;
    [ShowIf("showCooldownUI", true)]
    public Image attackUI;

    [HideInInspector]
    public List<GameObject> AttackPausers = new List<GameObject>();

    #region private variables
    private bool canSwing = true;
    private bool swinging = false;
    private bool onCooldown = false;

    private float swingProgress = 0;
    private int swingCount = 0;
    private float timeSinceLastSwing = 8f;
    private float timeUntilNormalAnim = 8f;
    private float timeBeforeTransition = 5f;

    private Camera mainCam;

    //Cached animator hashes
    private readonly int hasHammerHash = Animator.StringToHash("hasHammer");
    private readonly int leftSwingHash = Animator.StringToHash("LeftSwing");
    private readonly int rightSwingHash = Animator.StringToHash("RightSwing");
    private readonly int topSwingHash = Animator.StringToHash("TopSwing");
    private readonly int timeSinceSwingHash = Animator.StringToHash("timeSinceSwing");
    #endregion

    void OnEnable()
    {
        InputManager.OnActionDown += Swing;
        mainCam = Camera.main;
    }

    void OnDisable()
    {
        InputManager.OnActionDown -= Swing;
    }

    void Update()
    {
        if (swinging)
        {
            swingProgress += Time.deltaTime;
            if (swingProgress > PlayerManager.Instance.equippedWeaponInfo.attackCooldown)
            {
                //can swing again
                canSwing = true;
            }
            else if (swingProgress < PlayerManager.Instance.equippedWeaponInfo.attackCooldown)
            {
                //can not combo
                canSwing = false;
            }

            if (swingProgress > PlayerManager.Instance.equippedWeaponInfo.attackFrame + PlayerManager.Instance.equippedWeaponInfo.attackCooldown)
            {
                ResetSwingProgress();
                swinging = false;
                swingCount = 0;
            }
        }

        //Handle attack cooldown UI
        if (showCooldownUI && PlayerManager.Instance.equippedWeaponInfo != null && attackUI != null)
        {
            attackUI.color = canSwing ? Color.green : Color.red;

            float totalSwingTime = PlayerManager.Instance.equippedWeaponInfo.attackFrame + PlayerManager.Instance.equippedWeaponInfo.attackCooldown;
            attackUI.fillAmount = swinging ? swingProgress / totalSwingTime : 0;
            attackUI.enabled = swinging ? true : false;
        }

        if (timeSinceLastSwing < timeBeforeTransition)
        {
            timeSinceLastSwing += Time.deltaTime;
        }
        else if (timeSinceLastSwing < timeUntilNormalAnim)
        {
            timeSinceLastSwing += Time.deltaTime;
            float timeSinceSwing = (timeSinceLastSwing - timeBeforeTransition) / (timeUntilNormalAnim - timeBeforeTransition);
            PlayerManager.Instance.playerAnimator.SetFloat(timeSinceSwingHash, timeSinceSwing);
        }
        else
        {
            timeSinceLastSwing = timeUntilNormalAnim;
        }


    }

    public void Swing()
    {
        if (AttackPausers.Count == 0 && canSwing && !onCooldown && PlayerManager.Instance.cameraScript.cameraMode != PlayerCamera.CameraMode.freezeRotation && PlayerManager.Instance.cameraScript.cameraMode != PlayerCamera.CameraMode.CamEvent && !PlayerManager.Instance.playerAnimator.GetBool("ChargingMagic"))
        {
            timeSinceLastSwing = 0f;

            PlayerManager.Instance.playerAnimator.SetFloat(timeSinceSwingHash, 0f);
            swinging = true;

            if (PlayerManager.Instance.equippedWeaponInfo.swingDash)
            {
                //propel the player forward
                StartCoroutine(AttackDash());
            }
            switch (PlayerManager.Instance.equippedWeaponInfo.weaponAnimationType)
            {
                case WeaponAnimationTypes.BigAndHeavy:
                    //
                    PlayerManager.Instance.playerAnimator.SetBool(hasHammerHash, true);
                    PlayerManager.Instance.playerAnimator.SetTrigger(leftSwingHash);
                    ResetSwingProgress();
                    break;
                case WeaponAnimationTypes.OneHanded:
                    PlayerManager.Instance.playerAnimator.SetBool(hasHammerHash, false);
                    if (swingCount == 0)
                    {//first swing (left)
                        PlayerManager.Instance.playerAnimator.ResetTrigger(rightSwingHash);
                        PlayerManager.Instance.playerAnimator.SetTrigger(leftSwingHash);
                        PlayerManager.Instance.playerAnimator.ResetTrigger(topSwingHash);
                        ResetSwingProgress();
                        swingCount++;
                    }
                    else
                    {
                        if (swingCount == PlayerManager.Instance.equippedWeaponInfo.maxComboHits - 1)
                        {//last swing (top)
                            PlayerManager.Instance.playerAnimator.SetTrigger(topSwingHash);
                            ResetSwingProgress();
                            StartCoroutine(comboCooldown());
                            swingCount = 0;

                        }
                        else
                        {//middle (right)
                            swingCount++;
                            PlayerManager.Instance.playerAnimator.ResetTrigger(leftSwingHash);
                            PlayerManager.Instance.playerAnimator.SetTrigger(rightSwingHash);
                            PlayerManager.Instance.playerAnimator.ResetTrigger(topSwingHash);
                            ResetSwingProgress();
                        }
                    }
                    break;
            }
        }
    }

    IEnumerator comboCooldown()
    {
        PlayerManager.Instance.equippedWeaponInfo.applyBonusDamage = true;
        swinging = false;
        onCooldown = true;
        yield return new WaitForSeconds(PlayerManager.Instance.equippedWeaponInfo.postComboCooldown);
        PlayerManager.Instance.equippedWeaponInfo.applyBonusDamage = false;
        onCooldown = false;
    }

    void ResetSwingProgress()
    {
        swingProgress = 0;
    }

    IEnumerator AttackDash()
    {
        PlayerManager.Instance.isDashing = true;

        Vector3 playerPos = PlayerManager.Instance.player.transform.position;
        Vector3 currentPosition = playerPos;
        Vector3 desiredPosition;

        Vector2 inputVector = InputManager.inputVector;
        GameObject target = PlayerManager.Instance.targetEnemy;
        Vector3 enemyToPlayer = Vector3.zero;
        float desiredDistanceToEnemy = 1f; 

        if (target != null) //If there is an enemy in focus (nearest one)
        {
            Vector3 enemyPos = target.transform.position;
            enemyToPlayer = playerPos - enemyPos;

            //Make sure that if the player is closer than the desired distance, they don't move away from the enemy
            desiredDistanceToEnemy = Mathf.Clamp(desiredDistanceToEnemy, 0, enemyToPlayer.magnitude);

            desiredPosition = enemyPos + enemyToPlayer.normalized * desiredDistanceToEnemy;

            if (inputVector != Vector2.zero && Vector3.Dot(Vector3.Scale(enemyToPlayer, new Vector3(1, 0, 1)).normalized, new Vector3(inputVector.x, 0, inputVector.y).normalized) > 0.75f) //Limit based on inputvector
            {
                Vector3 movementVector = mainCam.transform.right * inputVector.x + mainCam.transform.forward * inputVector.y;
                enemyToPlayer = inputVector.magnitude > 0.25f ? Vector3.Scale(movementVector, new Vector3(1, 0, 1)) : PlayerManager.Instance.player.transform.forward;
                desiredPosition = playerPos + enemyToPlayer.normalized * PlayerManager.Instance.equippedWeaponInfo.dashAmount;
            }
        }
        else
        {
            NavMeshHit hit;
            Vector3 pos = transform.position + transform.forward * PlayerManager.Instance.equippedWeaponInfo.dashAmount;
            if (NavMesh.SamplePosition(pos, out hit, 1f, LayerMask.NameToLayer("Ground"))){
                Vector3 dashDir = hit.position - transform.position;

                //Check if something is blocking the way (eg. a moving physics object)
                RaycastHit rayHit;
                Vector3 raycastPoint = transform.position + transform.up * 0.5f;
                if (Physics.Raycast(raycastPoint, dashDir, out rayHit, PlayerManager.Instance.equippedWeaponInfo.dashAmount))
                {
                    desiredPosition = transform.position + dashDir.normalized * (rayHit.distance - (PlayerManager.Instance.playerCollider as CapsuleCollider).radius);
                }
                else
                {
                    desiredPosition = hit.position;
                }
            }
            else {
                desiredPosition = transform.position;
            }
        }

        //Rotate player according to the attack direction
        Quaternion currentRotation = transform.rotation;
        Vector3 newLookDirection = Vector3.Scale(desiredPosition - currentPosition, new Vector3(1, 0, 1));
        Quaternion desiredRotation = target != null && newLookDirection != Vector3.zero ? Quaternion.LookRotation(newLookDirection) : currentRotation;

        //Move player to the desiredPosition
        float time = 0.2f;
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            if (PlayerManager.Instance.targetEnemy != null && target != null)
            {
                desiredPosition = target.transform.position + (enemyToPlayer).normalized * desiredDistanceToEnemy;

                if (desiredRotation != currentRotation)
                {
                    desiredRotation = Quaternion.LookRotation(Vector3.Scale(target.transform.position - currentPosition, new Vector3(1, 0, 1)));
                }
            }
            Vector3 currentPoint = Vector3.Lerp(transform.position, desiredPosition, t);

            transform.position = currentPoint;
            transform.rotation = Quaternion.Slerp(currentRotation, desiredRotation, t*2);

            yield return null;
        }
        transform.position = desiredPosition;
        transform.rotation = desiredRotation;

        yield return new WaitForSeconds(0.2f);
        PlayerManager.Instance.isDashing = false;
    }

}

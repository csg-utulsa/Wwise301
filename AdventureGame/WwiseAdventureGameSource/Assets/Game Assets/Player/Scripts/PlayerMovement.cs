////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
public class PlayerMovement : MonoBehaviour
{

    [Header("Wwise")]
    public AK.Wwise.Event Footstep;
    public AK.Wwise.RTPC MovementSpeed;

    //Movement variables
    [Header("Movement Variables")]
    public float maxSpeed = 10f;
    public float sprintSpeed = 12f;
    public float navMeshCheckDistance = 0.5f;
    public float speedWhileAttacking = 0.8f;

    [Header("Physics Materials")]
    public PhysicMaterial frictionPhysMat;
    public PhysicMaterial noFrictionPhysMat;

    //Ground checks
    [Header("Other Physics Stuff")]
    public LayerMask groundCheckMask;


    public List<GameObject> movementPausers = new List<GameObject>();
    [HideInInspector]

    #region private variables
    //Ground checks
    private RaycastHit[] groundCheckHits = new RaycastHit[1];
    private bool firstTimeLand;

    //Coroutine variables
    private bool wantsMove;

    //Animation
    private Animator animator;

    //playerbody
    private Rigidbody playerBody;

    //Movement related variables
    private Vector3 movementVector;
    private float sprintAdd = 0;
    private float runAmount = 0;

    private float speedModifier = 1f;

    //Cached Animator hashes
    private readonly int isMovingHash = Animator.StringToHash("isMoving");
    private readonly int inAirHash = Animator.StringToHash("inAir");
    private readonly int movementSpeedHash = Animator.StringToHash("movementSpeed");
    #endregion

    void Start()
    {
        //subscribe to events
        InputManager.OnMoveHold += OnMoveHold;
        InputManager.OnMoveUp += OnMoveUp;
        InputManager.OnSprint += Sprint;
        InputManager.OnSprintUp += StopSprint;

        //initialize variables
        playerBody = PlayerManager.
            Instance.
            playerRb;
        animator = PlayerManager.Instance.playerAnimator;
    }


    void FixedUpdate()
    {
        if (movementPausers.Count == 0)
        {
            //check for movement
            if (!PlayerManager.Instance.isMoving)
            {
                if (animator != null && animator.GetBool(isMovingHash))
                {
                    animator.SetBool(isMovingHash, false);
                }
            }

            //rotate player
            if (movementVector.magnitude > 0.01f && !PlayerManager.Instance.isDashing)
            {
                rotatePlayerAccordingToMovement(movementVector);
            }
        }

        //Ground checks
        if (Physics.SphereCastNonAlloc(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), 0.06f, -transform.up, groundCheckHits, 0.5f, groundCheckMask) > 0)
        { //We hit something
            PlayerManager.Instance.inAir = false;
            animator.SetBool(inAirHash, false);

            if (firstTimeLand)
            {
                if (animator != null)
                {
                    animator.SetBool(inAirHash, false);
                }
                //TODO: Play landing sound here!
                PlayerManager.Instance.playerCollider.material = frictionPhysMat;
                firstTimeLand = false;
            }
        }
        else
        {
            //We're in the air
            PlayerManager.Instance.inAir = true;
            animator.SetBool(inAirHash, true);
            if (!firstTimeLand)
            {
                firstTimeLand = true;
            }
        }

        //parse input
        if (wantsMove)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, navMeshCheckDistance, LayerMask.NameToLayer("Ground"))) //Limit to using the NavMesh
            {
                playerBody.velocity = new Vector3((movementVector.x * (maxSpeed + sprintAdd)) * Time.fixedDeltaTime * 20f * speedModifier, playerBody.velocity.y + (movementVector.y * Time.deltaTime), (movementVector.z * (maxSpeed + sprintAdd)) * Time.fixedDeltaTime * 20f * speedModifier); //NORMAL MOVEMENT
            }
        }


    }

    public void SlowMovement()
    {
        speedModifier = speedWhileAttacking;
    }

    public void UnslowMovement()
    {
        speedModifier = 1f;

    }

    private void LateUpdate()
    {
        runAmount = Mathf.Lerp(runAmount, playerBody.velocity.magnitude / 7.1f, Time.deltaTime * 10f); //TODO: Fix these hard-coded number.
        //Update the movementSpeed parameter in the animator
        animator.SetFloat(movementSpeedHash, runAmount);
        float RTPCValue = runAmount * 100f;
        MovementSpeed.SetGlobalValue(RTPCValue);
    }

    void Sprint()
    {
        if (maxSpeed + sprintAdd < sprintSpeed)
        {
            sprintAdd += Time.deltaTime * 100;
        }
        else if (maxSpeed + sprintAdd > sprintSpeed)
        {
            sprintAdd = sprintSpeed - maxSpeed;
        }

        if (!PlayerManager.Instance.isSprinting)
        {
            PlayerManager.Instance.isSprinting = true;
        }
    }

    void StopSprint()
    {
        StartCoroutine(decelerate());
    }

    IEnumerator decelerate()
    {
        while (sprintAdd > 0)
        {
            sprintAdd -= Time.deltaTime * 7;
            yield return null;
        }
        PlayerManager.Instance.isSprinting = false;
    }

    Vector3 botWorldColCheck;
    Vector3 midWorldColCheck;
    float botToMidLength;
    bool botSeesGround, midSeesGround;
    RaycastHit botHit, midHit;

    void OnMoveHold(Vector2 input)
    {
        Camera mainCam = Camera.main;
        if (mainCam != null && movementPausers.Count < 1)
        {
            wantsMove = true;
            PlayerManager.Instance.playerCollider.material = noFrictionPhysMat;
            
            if (animator != null)
            {
                animator.SetBool(isMovingHash, true);
            }

            //parse input vector and transform relative to the camera
            movementVector = new Vector3(mainCam.transform.forward.x, 0f, mainCam.transform.forward.z).normalized * (input.y);
            movementVector += new Vector3(mainCam.transform.right.x, 0f, mainCam.transform.right.z) * input.x;
        }
        else
        {
            wantsMove = false;
            if (Camera.main == null)
            {
                Debug.LogError("You cannot move player, since the scene has no Camera tagged as MainCamera. Movement is based on Camera orientation.");
            }
            animator.SetBool(isMovingHash, false);
            PlayerManager.Instance.playerCollider.material = frictionPhysMat;
        }
    }

    void OnMoveUp()
    {
        PlayerManager.Instance.playerCollider.material = frictionPhysMat;
        animator.SetBool(isMovingHash, false);
        movementVector = Vector3.zero;
        wantsMove = false;
    }

    void rotatePlayerAccordingToMovement(Vector3 input)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(input), 20f * Time.smoothDeltaTime);
    }

    void OnDisable()
    {
        InputManager.OnMoveHold -= OnMoveHold;
        InputManager.OnMoveUp -= OnMoveUp;
        InputManager.OnSprint -= Sprint;
        InputManager.OnSprintUp -= StopSprint;
    }
}

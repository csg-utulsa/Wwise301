////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class EvilCrawlerAI : Creature
{
    [Header("Wwise")]
    public AK.Wwise.Event MoveSound;
    public AK.Wwise.Event AttackSound;
    public AK.Wwise.Event ExplodeSound;

    [Header("Movement Settings")]
    [Range(0f, 8f)]
    public float steerSpeed = 0f;
    public float slowDown = 1f;

    [Header("Object Links")]
    public GameObject Eye;
    public GameObject ParticleEmission;
    public GameObject DeathParticles;

    #region private variables
    private float frontNess = 0f;
    private bool PleaseDie = false;

    //Cached animator hashes
    private readonly int moveHash = Animator.StringToHash("Move");
    private readonly int deathHash = Animator.StringToHash("Death");
    #endregion

    public override void Start()
    {
        base.Start();
        thisNavMeshAgent.updateRotation = false;
    }

    public void Update()
    {
        if (PleaseDie)
        {
            state = CreatureState.dead;
            anim.SetTrigger(deathHash);
        }

        if (state != CreatureState.immobilized && state != CreatureState.stunned)
        {
            SteerToSpeed();
        }


        if (isMoving && state != CreatureState.stunned && state != CreatureState.immobilized)
        {
            anim.SetBool(moveHash, true);
        }
        else
        {
            anim.SetBool(moveHash, false);
        }

        if (targetOfNPC != null && state != CreatureState.stunned)
        {
            Vector3 direction = (targetOfNPC.transform.position - transform.position).normalized;
            float angleOfTarget = Vector3.Angle(direction, RotationObject.transform.forward);
            if (angleOfTarget < 60)
            {
                Eye.transform.rotation = Quaternion.LookRotation(direction, Eye.transform.up);
            }
        }
        else
        {
            
            Eye.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.forward), Time.deltaTime);
        }
    }

    public void PlayMoveSound()
    {
        MoveSound.Post(this.gameObject);
    }

    public void PlayBiteSound()
    {
        AttackSound.Post(this.gameObject);
    }

    public void SteerToSpeed()
    {
        if (steerSpeed > 0f)
        {
            steerSpeed -= slowDown * Time.deltaTime;
        }
        else
        {
            steerSpeed = 0f;

        }
        thisNavMeshAgent.speed = steerSpeed;

    }

    public void AddSpeed(float amount)
    {
        if (targetOfNPC != null)
        {
            Vector3 direction = (targetOfNPC.transform.position - transform.position).normalized;
            float angleOfTarget = Vector3.Angle(direction, RotationObject.transform.forward * 1000);
            frontNess = Mathf.Clamp01(1 - (angleOfTarget / 180) + 0.2f);
        }
        else
        {
            Vector3 direction = (thisNavMeshAgent.pathEndPosition - transform.position).normalized;
            float angleOfTarget = Vector3.Angle(direction, RotationObject.transform.forward * 1000);
            frontNess = Mathf.Clamp01(1 - (angleOfTarget / 180) + 0.2f);
        }
        steerSpeed = amount * frontNess;
    }

    public override void OnDeathAnimation()
    {
        if (DeathAnimations.Enable)
        {
            anim.SetTrigger(DeathAnimations.FrontTrigger);
        }
    }

    public void DeathDrop()
    {
        GameObject deathFX = Instantiate(DeathParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(deathFX, 5f);
        ExplodeSound.Post(gameObject);
        Destroy(gameObject);
    }
}

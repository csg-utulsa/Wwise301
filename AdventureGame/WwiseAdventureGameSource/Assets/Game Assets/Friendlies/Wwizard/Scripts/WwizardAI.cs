////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.AI;

public class WwizardAI : Creature
{
    [Header("Wwise")]
    public AK.Wwise.Event PoofGimmickSound;
    public AK.Wwise.Event StaffHitGroundSound;
    public MaterialChecker matChecker;

    [Header("Idle Gimmick 1 Poof Objects")]
    public GameObject Gimmick1PoofParticles;
    public GameObject Gimmick1PoofTransform;
    public Vector3 Gimmick1Displacement;

    [Header("Charge Particles Settings")]
    public WwizardStaffChargeParticles chargeParticles;

    [Header("NavMesh Stuff")]
    public NavMeshAgent navMeshAgent;
    public NavMeshObstacle navMeshObstacle;

    #region private variables
    //Cached animator hashes
    private readonly int questChargeHash = Animator.StringToHash("Quest_Charge");
    private readonly int questChargeDoneHash = Animator.StringToHash("Quest_ChargeDone");
    private readonly int randomTalkHash = Animator.StringToHash("RandomTalk");

    #endregion

    public void Awake() {
        if (gameObject.GetComponent<AkRoomAwareObject>() == null) {
            gameObject.AddComponent<AkRoomAwareObject>();
        }
    }

    public void TalkAnimation_CalculateNew()
    {
        if (anim != null)
        {
            anim.SetFloat(randomTalkHash, Random.Range(0f, 1f));
        }
    }

    public override System.Collections.IEnumerator Talking()
    {
        TalkAnimation_CalculateNew();
        return base.Talking();
    }

    public void IdleAnimation_CalculateNew()
    {
        base.AllowTriggeringOfIdleAnimation(); //TODO: This shouldn't really be part of the Creature base class, as it's pretty specific to the Wwizard
    }

    public void IdleGimmick_1_Poof()
    {
        if (Gimmick1PoofParticles != null && Gimmick1PoofTransform != null)
        {
            GameObject p = Instantiate(Gimmick1PoofParticles, Gimmick1PoofTransform.transform.position + Gimmick1Displacement, Quaternion.identity) as GameObject;
            PoofGimmickSound.Post(p);
            Destroy(p, 5f);
        }
    }

    public void StartCharging()
    {
        chargeParticles.enabled = true;
    }

    public void DoneCharging()
    {
        chargeParticles.enabled = false;
    }

    public void EnableNavMeshAgent()
    {
        navMeshAgent.enabled = true;
        navMeshObstacle.enabled = false;
    }

    public void DisableNavMeshAgent()
    {
        navMeshAgent.enabled = false;
        navMeshObstacle.enabled = true;
    }

    public void TriggerAnimation_Charge()
    {
        anim.SetTrigger(questChargeHash);
    }
    public void TriggerAnimation_ChargeEnd()
    {
        anim.SetTrigger(questChargeDoneHash);
    }

    public void PlayStaffSound()
    {
        matChecker.CheckMaterial(gameObject);
        StaffHitGroundSound.Post(gameObject);
    }
}

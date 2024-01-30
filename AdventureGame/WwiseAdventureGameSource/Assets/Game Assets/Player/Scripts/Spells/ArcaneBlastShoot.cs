////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class ArcaneBlastShoot : Spell
{
    public GameObject SpellBullet;
    public GameObject ShotFx;

    #region private variables
    private GameObject instantiatedBullet;
    private GameObject instantiatedFx;
    #endregion

    public override void Activate()
    {
        instantiatedBullet = Instantiate(SpellBullet, transform.position + Vector3.up * 1.5f + PlayerManager.Instance.playerTransform.forward * 1.5f, Quaternion.identity) as GameObject;
        instantiatedBullet.GetComponent<EvilSpitPlantProjectile>().parent = PlayerManager.Instance.player;

        instantiatedFx = Instantiate(ShotFx, transform.position + Vector3.up * 1.5f, Quaternion.identity) as GameObject;

        Quaternion shootDirection = Quaternion.LookRotation(PlayerManager.Instance.playerTransform.forward);
        instantiatedBullet.transform.rotation = shootDirection;
        instantiatedFx.transform.rotation = shootDirection;
        Destroy(instantiatedFx, 5f);

        GameManager.Instance.gameSpeedHandler.SetGameSpeed(gameObject.GetInstanceID(), 0.5f, 0.5f, 0.5f, 0.5f);
    }

}

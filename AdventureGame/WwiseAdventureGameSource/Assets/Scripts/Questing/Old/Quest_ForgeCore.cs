////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class Quest_ForgeCore : MonoBehaviour
{
    public AK.Wwise.Event CoreDestructionSound;
    public GameObject spawnOnDestruction;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            CoreDestructionSound.Post(gameObject);
            PlayerManager.Instance.cameraScript.CamShake(new PlayerCamera.CameraShake(0.5f, 0.5f));
            GameManager.Instance.gameSpeedHandler.SetGameSpeed(gameObject.GetInstanceID(), 0.1f, 0.1f, 0.1f, 1f);

            GameObject vfx = Instantiate(spawnOnDestruction, transform.position, Quaternion.identity, SceneStructure.Instance.TemporaryInstantiations.transform);
            Destroy(vfx, 5f);

            Destroy(gameObject);
        }

    }
}

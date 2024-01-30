////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

namespace QuestSystem
{
    public class Quest_Forge_Shields : MonoBehaviour
    {
        public AK.Wwise.Event DestructionEvent;
        public Quest_Forge ForgeScript;
        public bool Destroyed = false;
        public int shieldNumber = 0;

        [Header("Destruction FX")]
        public GameObject shieldDestroyedParticles;
        public Camera shieldDestroyedCamera;

        void OnTriggerEnter(Collider col)
        {
            if (!Destroyed && col.CompareTag("Spell") && ForgeScript.GetCurrentWave() == shieldNumber)
            {
                StartCoroutine(AnimateShieldExplosion());
            }
        }

        IEnumerator AnimateShieldExplosion()
        {
            Destroyed = true;
            PlayerManager.Instance.cameraScript.StartShake(0.2f);

            //TODO: play rumble sound here

            yield return new WaitForSeconds(1f);
            PlayerManager.Instance.cameraScript.StopShake();
            PlayerManager.Instance.cameraScript.ChangeCamera(new PlayerCamera.CameraEvent(shieldDestroyedCamera, 0.3f, 3f, true));

            yield return new WaitForSeconds(0.3f);

            PlayerManager.Instance.cameraScript.CamShake(new PlayerCamera.CameraShake(1f, 0.3f));
            GameManager.Instance.gameSpeedHandler.SetGameSpeed(gameObject.GetInstanceID(), 0.5f, 0.2f, 0.3f, 1f, Curves.Instance.SmoothOut);

            // DEstruction particles
            GameObject go = Instantiate(shieldDestroyedParticles, transform.position, Quaternion.identity, SceneStructure.Instance.TemporaryInstantiations.transform);
            Destroy(go, 10f);

            DestructionEvent.Post(gameObject);

            ForgeScript.NextWave(shieldNumber);
            Destroy(gameObject);
        }
    }
}
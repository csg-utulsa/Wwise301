////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnImpact : MonoBehaviour
{

    public bool OnlyDamagePlayer = true;
    public float MaxDamage = 50f;
    public float DamageRadius = 5f;
    public GameObject explosionFX;

    private void OnCollisionEnter(Collision col)
    {
        GameObject vfx = Instantiate(explosionFX, transform.position, Quaternion.identity, transform.parent) as GameObject; //TODO: Pool
        Destroy(vfx, 5f);

        //Do camera shake based on distance
        float maxSqrDistance = 100f;
        float distance = Vector3.SqrMagnitude(transform.position - PlayerManager.Instance.playerTransform.position);
        float normalizedDistance = 1 - Mathf.Clamp(distance / maxSqrDistance, 0f, 1f);
        PlayerManager.Instance.cameraScript.CamShake(new PlayerCamera.CameraShake(normalizedDistance, normalizedDistance));

        //Do damage to nearby IDamageables
        Attack attack = new Attack(normalizedDistance * MaxDamage);

        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, DamageRadius);
        foreach (Collider c in nearbyColliders)
        {
            if (OnlyDamagePlayer)
            {
                if (c.gameObject.CompareTag("Player"))
                {
                    GameManager.DamageObject(c.gameObject, attack);
                    break;
                }
            }
            else
            {
                GameManager.DamageObject(c.gameObject, attack);
            }
        }

        //Release remaining particles and let them play out
        foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
        {
            p.transform.parent = null;
            p.Stop();
            Destroy(p.gameObject, 5f);
        }

        Destroy(gameObject);
    }
}

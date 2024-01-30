////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvilSpitPlantProjectile : MonoBehaviour
{
    [Header("Wwise")]
    public AK.Wwise.Event ContinuousSoundStart;
    public AK.Wwise.Event ContinuousSoundStop;

    [Header("Prefab Links")]
    public GameObject explodeParticles;
    public GameObject deflectParticles;

    [Header("Projectile Settings")]
    public float speed = 5;
    public float duration = 3;
    public float damage = 40f;

    public bool ignoreCollisionWithWwizard = false;

    [HideInInspector]
    public GameObject parent;

    public AK.Wwise.Event ImpactSound = new AK.Wwise.Event();
    public AK.Wwise.Event NoImpactSound = new AK.Wwise.Event();

    #region private variables
    private Rigidbody rb;
    private float time = 0;
    private bool isExploding;
    private IEnumerator movementRoutine;
    #endregion

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        PlayerCamera.OnCameraEventStart += ForceExplode;

        movementRoutine = MoveSpitBullet();
        StartCoroutine(movementRoutine);
    }

    private void OnDisable()
    {
        PlayerCamera.OnCameraEventStart -= ForceExplode;
    }

    IEnumerator MoveSpitBullet()
    {
        ContinuousSoundStart.Post(gameObject);
        while (time < duration)
        {
            rb.velocity = transform.forward * speed;
            time += Time.deltaTime;
            yield return null;
        }

        if (!isExploding)
        {
            //SpitBullet explodes because it didn't hit something within the set duration.
            Explode(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject != parent && !col.isTrigger && (ignoreCollisionWithWwizard ? !col.CompareTag("Wwizard") : true))
        {
            //If player deflects the spit ball
            if (col.gameObject.CompareTag("Player") || col.gameObject.layer == LayerMask.NameToLayer("Weapon"))
            {
                //Only deflect if the player is somewhat facing the spitbullet
                bool allowToReflect = Vector3.Dot(col.transform.forward, rb.velocity.normalized) < 0;

                if (allowToReflect && PlayerManager.Instance.isDashing)
                {
                    Deflect(col.gameObject);
                    return; // don't explode yet!
                }
            }

            Explode();

            Vector3 dir = (transform.position - parent.transform.position).normalized;
            GameManager.DamageObject(col.gameObject, new Attack(damage, dir, 5f, SwingTypes.Top, WeaponTypes.EvilSpitPlant));
        }
    }

    void Deflect(GameObject deflector)
    {
        //Reset timer
        time = 0;

        //flip the rotation, effectively sending the spitbullet right back to its shooter
        transform.rotation = Quaternion.LookRotation(-rb.velocity);

        GameObject deflect = Instantiate(deflectParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(deflect, 5f);

        //make sure the weapon doesn't accidentally explode the bullet just after deflecting
        if (deflector.layer == LayerMask.NameToLayer("Weapon"))
        {
            parent = deflector;
        }
        else
        {
            parent = PlayerManager.Instance.equippedWeapon;
        }
    }

    private void ForceExplode()
    {
        Explode(false);
    }

    void Explode(bool hitSomething = true)
    {
        if (!isExploding)
        {
            isExploding = true;

            ContinuousSoundStop.Post(gameObject);

            GetComponent<Collider>().enabled = false;
            time = duration;
            rb.velocity = Vector3.zero;

            //Spawn explodeParticles
            GameObject go = Instantiate(explodeParticles, transform.position, Quaternion.identity) as GameObject; //TODO: Pool explode particles

            if (hitSomething)
            {
                ImpactSound.Post(go.gameObject);
            }
            else
            {
                NoImpactSound.Post(go.gameObject);
            }

            Destroy(go, 5f);

            //Stop all currently active particles
            foreach (ParticleSystem p in transform.GetComponentsInChildren<ParticleSystem>())
            {
                p.Stop();
            }
            Destroy(gameObject, 5f);
        }
    }
}

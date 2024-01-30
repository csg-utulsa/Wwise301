////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomParticleEmitter : MonoBehaviour
{
    public bool SeparateEmitterLocation = false;
    [ShowIf("SeparateEmitterLocation", true)]
    public Transform EmitterLocation;

    public bool StartOnAwake = true;
    public bool useCustomOcclusion = true;

    [Space(20f)]
    [ShowIf("useCustomOcclusion", true)]
    public float cullingRange = 10f;
    [ShowIf("useCustomOcclusion", true)]
    public bool destroyActiveObjectsOnOccluded = true;

    [Header("Emitter Setup (Line)")]
    public float length;

    [Header("Emission Setup")]
    public GameObject particlePrefab;
    public float rate;

    public float minSpeed;
    public float maxSpeed;

    public float gravity;

    public float minLifetime;
    public float maxLifetime;

    [Header("Random Rotation")]
    public Vector3 rotationSpeedFrom = Vector3.zero;
    public Vector3 rotationSpeedTo = Vector3.zero;

    #region private variables
    private Vector3 left;
    private Vector3 right;

    private IEnumerator emissionRoutine;
    private IEnumerator distanceCheckRoutine;

    private bool playerIsInRange = false;

    private List<ParticleWithInfo> activeParticles = new List<ParticleWithInfo>();
    #endregion

    [System.Serializable]
    public class ParticleWithInfo
    {
        public GameObject particle;
        public Rigidbody rb;
        public float speed;
        public float gravity;
        public float lifetime;
    }

    private void OnEnable()
    {
        if (StartOnAwake)
        {
            emissionRoutine = ParticleEmission();
            StartCoroutine(emissionRoutine);

            if (!useCustomOcclusion)
            {
                playerIsInRange = true;
            }
        }
    }

    private void OnDisable()
    {
        if (emissionRoutine != null)
        {
            StopCoroutine(emissionRoutine);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (useCustomOcclusion)
        {
            if (other.CompareTag("MainCamera"))
            {
                playerIsInRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (useCustomOcclusion)
        {
            if (other.CompareTag("MainCamera"))
            {
                playerIsInRange = false;

                if (destroyActiveObjectsOnOccluded)
                {
                    CleanUpActiveObjects();
                }
            }
        }
    }

    IEnumerator ParticleEmission()
    {
        while (true)
        {
            while (playerIsInRange)
            {
                EmitParticle();
                yield return new WaitForSeconds(rate);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void EmitParticle()
    {
        Vector3 l = GetLeftVector();
        Vector3 r = GetRightVector();
        Vector3 startPos = new Vector3(Random.Range(l.x, r.x), Random.Range(l.y, r.y), Random.Range(l.z, r.z));
        GameObject go = Instantiate(particlePrefab, startPos, Quaternion.identity, transform) as GameObject; //TODO: Definitely pool
        ParticleWithInfo p = new ParticleWithInfo();
        p.particle = go;
        p.speed = Random.Range(minSpeed, maxSpeed);
        p.gravity = gravity;
        p.lifetime = Random.Range(minLifetime, maxLifetime);
        p.rb = go.GetComponent<Rigidbody>();
        if (p.rb == null)
        {
            p.rb = go.AddComponent<Rigidbody>();
            p.rb.mass = gravity;
            p.rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            p.rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        //Add force
        p.rb.AddForce(transform.forward * p.speed, ForceMode.Impulse);

        activeParticles.Add(p);
        StartCoroutine(DestroyParticleAfterLifetime(p));
    }

    private Vector3 GetLeftVector()
    {
        Transform emitterLocation = GetEmitterLocation();
        return emitterLocation.position + (-emitterLocation.right * length);
    }

    private Vector3 GetRightVector()
    {
        Transform emitterLocation = GetEmitterLocation();
        return emitterLocation.position + emitterLocation.right * length;
    }

    private Transform GetEmitterLocation()
    {
        return SeparateEmitterLocation && EmitterLocation != null ? EmitterLocation : transform;
    }

    void OnDrawGizmos()
    {
        Transform emitterLocation = GetEmitterLocation();
        left = GetLeftVector();
        right = GetRightVector();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(left, right);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(emitterLocation.position, emitterLocation.position + emitterLocation.forward * 10f);
    }

    IEnumerator DestroyParticleAfterLifetime(ParticleWithInfo p)
    {
        yield return new WaitForSeconds(p.lifetime);

        if (p != null)
        {
            Destroy(p.particle);
        }
    }

    private void CleanUpActiveObjects()
    {
        for (int i = activeParticles.Count - 1; i >= 0; i--)
        {
            var currentParticle = activeParticles[i];
            if (currentParticle != null && currentParticle.particle != null)
            {
                Destroy(currentParticle.particle);
            }
        }
		activeParticles = new List<ParticleWithInfo>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = playerIsInRange ? Color.yellow.WithAlpha(0.3f) : Color.red.WithAlpha(0.3f);
        Gizmos.DrawSphere(transform.position, cullingRange);
    }
}

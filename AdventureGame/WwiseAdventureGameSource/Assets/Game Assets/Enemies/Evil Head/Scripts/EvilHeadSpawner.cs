////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class EvilHeadSpawner : MonoBehaviour
{
    [Header("Object Links")]
    public GameObject EvilHead;
    public Vector3 SpawnOffset;
    public GameObject DestructionParticles;
    public AK.Wwise.Event DestructionSound;
    public bool SpawnOnAwake = false;
    public bool SpawnInifitely = false;
    private float SpawnInterval = 15f;

	private GameObject EH = null;

	void OnEnable()
    {
        if (SpawnOnAwake)
        {
            EH = SpawnEvilHead();
            EH.name = "Evil Head";
            //Disable the Sphere Trigger in order to not spawn any more Evil Heads
            GetComponent<SphereCollider>().enabled = false;
        }

        if (SpawnInifitely && !SpawnOnAwake) {
            StartCoroutine(SpawnEvilHeads());
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!SpawnOnAwake && col.CompareTag("Player") && !SpawnInifitely)
        {
            EH = SpawnEvilHead();
            EH.name = "Evil Head";
            //Disable the Sphere Trigger in order to not spawn any more Evil Heads
            GetComponent<SphereCollider>().enabled = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!SpawnOnAwake && other.CompareTag("Player") && SpawnInifitely)
        {
            SpawnContinuously = true;
        }
     }
    private bool SpawnContinuously = false;
    IEnumerator SpawnEvilHeads() {
        while (true) {
            GetComponent<SphereCollider>().enabled = true;
            while (!SpawnContinuously) {
                yield return new WaitForEndOfFrame();
            }

            EH = SpawnEvilHead();
            EH.name = "Evil Head";
            SpawnContinuously = false;
            GetComponent<SphereCollider>().enabled = false;
            yield return new WaitForSeconds(SpawnInterval);
        }
        
    }

    public GameObject SpawnEvilHead()
    {
        return Instantiate(EvilHead, transform.position + SpawnOffset, Quaternion.identity) as GameObject;
    }

    void OnCollisionEnter(Collision col)
    {
        //Destroy the spawner when the Player walks into it.
        if (col.collider.CompareTag("Player"))
        {
            GameObject destructionParticles = (Instantiate(DestructionParticles, transform.position, Quaternion.identity)) as GameObject;
            Destroy(destructionParticles, 5f);
            DestructionSound.Post(destructionParticles);
            Destroy(this.gameObject);
        }
    }

	public void StopSoundOnSpawnedEvilHead()
	{
		if (EH)
		{
			AkSoundEngine.StopAll(EH);
		}
	}
}

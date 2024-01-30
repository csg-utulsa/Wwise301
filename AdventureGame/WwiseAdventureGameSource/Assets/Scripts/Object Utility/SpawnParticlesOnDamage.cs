////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticlesOnDamage : MonoBehaviour, IDamageable {

	[System.Serializable]
	public struct ParticleSpawn
	{
		public ParticleSystem particles;
		public bool instantiateOnCollisionPoint;
		public Transform customPosition;
	}

	public List<ParticleSpawn> particlesToSpawn;

	public void OnDamage(Attack a){
		ParticleSystem particles;
		foreach (ParticleSpawn p in particlesToSpawn) {
            Vector3 spawnPosition = transform.position;

            if (p.instantiateOnCollisionPoint && a.impactPoint != Vector3.zero) {
                spawnPosition = a.impactPoint;
			}else{
                if (p.customPosition != null)
                {
                    spawnPosition = p.customPosition.position;
                }
                particles = Instantiate(p.particles, spawnPosition, Quaternion.identity, transform);
                Destroy(particles, 10f);
			}
		}
	}
}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class DestructibleObject : MonoBehaviour, IDamageable
{

    [Header("Settings")]
    public MeshFilter meshFilter;
    public float HP = 50f;

    [Range(0f, 1f)]
    public float knockbackStrength = 1f;
    public List<CanBeDestroyedBySettings> canBeDestroyedBy;

    public List<Mesh> intermediateMeshes;
    public GameObject destroyedParticles;
    public float destroyParticlesAfterSeconds = 10f;
    public Vector3 particleSpawnOffset = Vector3.zero;

    public bool spawnsItem; //TODO: Move into a separate DropOnDestroy behavior

    [ShowIf("spawnsItem", true)]
    public GameObject itemToSpawn;

    [ShowIf("spawnsItem", true)]
    public float SpawnScale = 1f;

    [ShowIf("spawnsItem", true)]
    public Vector3 SpawnOffset = Vector3.zero;

    [ShowIf("spawnsItem", true)]
    public bool ParentToThisParent = false;

    public bool enableItem;
    [ShowIf("enableItem", true)]
    public GameObject itemToEnable;

    [Header("WWISE")]
    public AK.Wwise.Event Destruction = new AK.Wwise.Event();
    public AK.Wwise.RTPC DestructionLevel = new AK.Wwise.RTPC();

    [Header("Unity Events")]
    public UnityEvent OnDestruction;

    private float destructionProgress = 0;
    private float origHP;
    private int meshIndex;
    private Rigidbody rb;

    [System.Serializable]
    public struct CanBeDestroyedBySettings
    {
        public WeaponTypes WeaponType;
        public float damageMultiplier;
    }

    void Awake()
    {
        origHP = HP;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = GetComponentInChildren<Rigidbody>();
        }

        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }
    }

    public void OnDamage(Attack a)
    {
        if (canBeDestroyedBy != null && canBeDestroyedBy.Count > 0)
        {//if any weapon types has been specified
            
            destructionProgress = 1 - (HP / origHP);
            DestructionLevel.SetValue(PlayerManager.Instance.weaponSlot, destructionProgress * 100f);
            //print(destructionProgress * 100f);

            for (int i = 0; i < canBeDestroyedBy.Count; i++)
            {
                if (canBeDestroyedBy[i].WeaponType == a.weaponType)
                {
                    if (rb != null)
                    {
                        rb.AddForce(a.attackDir * knockbackStrength * rb.mass * a.knockbackStrength / 8f, ForceMode.Impulse);
                    }

                    HP -= a.damage * canBeDestroyedBy[i].damageMultiplier;

                    if (HP > 0)
                    {
                        destructionProgress = 1 - (HP / origHP);
                        DestructionLevel.SetValue(this.gameObject, (1 - destructionProgress) * 100f);
                        
                        if (intermediateMeshes != null && intermediateMeshes.Count > 0)
                        {
                            meshIndex = (int)(destructionProgress * intermediateMeshes.Count);

                            //Change mesh?
                            meshIndex = Mathf.Clamp(meshIndex, 0, intermediateMeshes.Count - 1);
                            if (meshFilter.mesh != intermediateMeshes[meshIndex])
                            {
                                meshFilter.mesh = intermediateMeshes[meshIndex];
                            }
                        }
                    }
                    else
                    {
                        if (destroyedParticles != null)
                        {
                            GameObject p = Instantiate(destroyedParticles, transform.position + particleSpawnOffset, Quaternion.identity) as GameObject; //TODO: Pool particles
                            Destroy(p, destroyParticlesAfterSeconds);
                        }

                        if (spawnsItem)
                        {
                            if (itemToSpawn != null)
                            {
                                GameObject newSpawn = (Instantiate(itemToSpawn, transform.position + SpawnOffset, Quaternion.identity)) as GameObject;
                                newSpawn.transform.localScale = Vector3.one * SpawnScale;
                                if (ParentToThisParent)
                                {
                                    newSpawn.transform.parent = this.transform.parent;
                                }
                            }
                            else
                            {
                                Debug.LogError("DestructibleObject (" + gameObject.name + "): It seems you forgot to add an item to spawn! Otherwise, disable 'Spawns Item'.");
                            }
                        }

                        if (enableItem)
                        {
                            if (itemToEnable != null)
                            {
                                itemToEnable.SetActive(true);
                            }
                            else
                            {
                                Debug.LogError("DestructibleObject (" + gameObject.name + "): It seems you forgot to add an item to enable! Otherwise, disable 'Enable Item'.");
                            }
                        }
                        OnDestruction.Invoke();
                        Destruction.Post(gameObject);
                        Destroy(gameObject);
                    }
                    break;
                }
            }
        }
    }
}

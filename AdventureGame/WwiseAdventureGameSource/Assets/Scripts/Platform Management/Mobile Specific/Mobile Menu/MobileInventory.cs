////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using UnityEngine;

public class MobileInventory : MonoBehaviour
{

    #region private variables
    private AnimatedObjectActiveHandler[] animationHandlers;
    private GameObject[] objects;
    private InventoryWeapon[] invWeaponScripts;

    private float itemSpawnInterval = 0.1f;
    private IEnumerator spawnRoutine;
    #endregion

    private void OnEnable()
    {
        MobileEvents.OnMobileInventory += OnInventory;
        MobileEvents.OnMobileInventoryWantsClosed += CloseInventory;

        animationHandlers = GetComponentsInChildren<AnimatedObjectActiveHandler>();
        invWeaponScripts = GetComponentsInChildren<InventoryWeapon>();

        PlayerManager.OnWeaponInventoryChanged += CheckAndUpdate;

    }

    private void OnDisable()
    {
        MobileEvents.OnMobileInventory -= OnInventory;
        MobileEvents.OnMobileInventoryWantsClosed -= CloseInventory;

        PlayerManager.OnWeaponInventoryChanged -= CheckAndUpdate;
    }

    private void CheckAndUpdate()
    {
        if (PlayerManager.Instance.equippedWeapon != null)
        {
            UpdateWeapons();
        }
    }

    void UpdateWeapons() //TODO: Account for receiving a new weapon while the inventory is open!
    {
        for (int i = 0; i < invWeaponScripts.Length; i++)
        {
            InventoryWeapon w = invWeaponScripts[i];

            if (PlayerManager.Instance.pickedUpWeapons.Contains(w.weaponType))
            {
                w.gameObject.SetActive(true);

                if (PlayerManager.Instance.equippedWeaponInfo.weaponType == w.weaponType)
                {
                    w.outlineObj.gameObject.SetActive(true);
                }
                else
                {
                    w.outlineObj.gameObject.SetActive(false);
                }
            }
            else
            {
                w.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator SpawnItems(bool active)
    {
        UpdateWeapons();

        if (active)
        {
            for (int i = animationHandlers.Length - 1; i >= 0; i--)
            {
                if (animationHandlers[i].gameObject.activeInHierarchy)
                {
                    animationHandlers[i].EnableObject(0.5f);
                    yield return new WaitForSeconds(itemSpawnInterval);
                }

            }
        }
        else
        {
            for (int i = 0; i < animationHandlers.Length; i++)
            {
                if (animationHandlers[i].gameObject.activeInHierarchy)
                {
                    animationHandlers[i].DisableObject(0.5f);
                    yield return new WaitForSeconds(itemSpawnInterval);
                }
            }

        }
        yield return null;
    }

    private void OnInventory()
    {
        //spawn items
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
        spawnRoutine = SpawnItems(true);
        StartCoroutine(spawnRoutine);
    }

    public void CloseInventory()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
        spawnRoutine = SpawnItems(false);
        StartCoroutine(spawnRoutine);

        if (MobileEvents.OnMobileInventoryClosed != null)
        {
            MobileEvents.OnMobileInventoryClosed();
        }
    }
}

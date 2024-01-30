////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DLC_Purchase : MonoBehaviour, IInteractable
{
    [Header("DLC Options")]
    public DLC purchasableDLC;

    public GameObject purchaseParticles;
    public Transform purchaseParticlesSpawnPoint;
    public AK.Wwise.Event purchaseSuccessSound;
    public AK.Wwise.Event purchaseFailedSound;

    public UnityEvent OnPurchaseComplete;

    public bool Purchase()
    {
        if (GameManager.Instance.coinHandler.SpendCoins(purchasableDLC.cost))
        {
            //DLC purchased
            GameManager.Instance.activeDLCs.Add(purchasableDLC);
            //Debug.Log("Added " + purchasableDLC.name + " to active DLCs");
            return true;
        }
        else
        {
            //Not enough coins!
            return false;
        }
    }

    public void OnInteract()
    {
        bool success = Purchase();

        if (success)
        {
            if (purchaseParticles != null)
            {
                GameObject fx = Instantiate(purchaseParticles, purchaseParticlesSpawnPoint == null ? transform.position : purchaseParticlesSpawnPoint.position, Quaternion.identity) as GameObject;
                Destroy(fx, 5f);
            }
            purchaseSuccessSound.Post(gameObject);
            OnPurchaseComplete.Invoke();
        }
        else
        {
            purchaseFailedSound.Post(gameObject);
        }

        //Debug.LogFormat(
        //    success ? "Purchased {0} for {1} coins" : "Not enough coins!",
        //    purchasableDLC.name,
        //    purchasableDLC.cost
        //);
    }
}

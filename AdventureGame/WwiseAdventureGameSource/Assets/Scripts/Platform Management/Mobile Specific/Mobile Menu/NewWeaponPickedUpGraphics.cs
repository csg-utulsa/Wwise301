////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using UnityEngine;

public class NewWeaponPickedUpGraphics : MonoBehaviour
{
    public GameObject notificationGraphics;
    public float showDuration = 5f;

    void OnEnable()
    {
        PlayerManager.OnNewWeaponPickedUp += OnWeaponPickup;
    }

    private void OnDisable()
    {
        PlayerManager.OnNewWeaponPickedUp -= OnWeaponPickup;
    }

    void OnWeaponPickup()
    {
        StartCoroutine(ShowNotification());
    }

    IEnumerator ShowNotification()
    {
        notificationGraphics.SetActive(true);
        yield return new WaitForSecondsRealtime(showDuration);
        notificationGraphics.SetActive(false);
    }
}

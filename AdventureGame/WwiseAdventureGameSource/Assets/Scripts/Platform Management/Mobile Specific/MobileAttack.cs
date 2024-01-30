////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.UI;

public class MobileAttack : MonoBehaviour
{
    public Image cooldownImage;

    private void Start()
    {
        PlayerManager.Instance.attackSystem.attackUI = cooldownImage;
    }

    public void OpenInventory()
    {
        if (MobileEvents.OnMobileInventory != null)
        {
            MobileEvents.OnMobileInventory();
        }
    }

    public void CloseInventory()
    {
        if (MobileEvents.OnMobileInventoryWantsClosed != null)
        {
            MobileEvents.OnMobileInventoryWantsClosed();
        }
    }
}

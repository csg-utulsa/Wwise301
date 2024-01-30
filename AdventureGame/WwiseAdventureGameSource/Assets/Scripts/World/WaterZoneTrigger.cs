////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterZoneTrigger : MonoBehaviour
{
    public AK.Wwise.Switch surfaceSwitch;

    PlayerFoot[] feet;

    void Start()
    {
        feet = PlayerManager.Instance.player.GetComponentsInChildren<PlayerFoot>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            for (int i = 0; i < feet.Length; i++)
            { //I mean, we kinda KNOW there'll only be two feet, but who knows if the Adventuress suddenly grows another one? ;)
                feet[i].EnterWaterZone();
                surfaceSwitch.SetValue(feet[i].gameObject);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            for (int i = 0; i < feet.Length; i++)
            {
                feet[i].ExitWaterZone();
                surfaceSwitch.SetValue(feet[i].gameObject);
            }
        }
    }
}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DLCAction : MonoBehaviour
{
    public DLC_Requirement requirement;
    public UnityEvent Activate;

    public void ValidateAndLoadContents()
    {
        if (requirement.ValidateRequirement())
        {
            LoadDLCContents();
        }
    }

    public void LoadDLCContents()
    {
        Activate.Invoke();
    }
}

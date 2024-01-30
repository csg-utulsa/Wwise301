////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DLC (Downloadable Content)/DLC Object)"), System.Serializable]
public class DLC : ScriptableObject
{
    [Header("DLC objects")]
    public string sceneName;
    public AK.Wwise.Bank bank; //TODO: Eventually we'll use the File Packager in Wwise to create packages

    [Header("Price")]
    public int cost;
}

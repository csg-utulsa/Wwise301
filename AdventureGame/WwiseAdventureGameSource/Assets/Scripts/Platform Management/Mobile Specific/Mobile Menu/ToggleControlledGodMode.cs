////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class ToggleControlledGodMode : MonoBehaviour
{
    public void SetGodMode(bool on)
    {
        GameManager.Instance.SetGodMode(on);
    }
}

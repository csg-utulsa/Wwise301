////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class ToggleControlledBigHeadMode : MonoBehaviour
{
    public void SetBigHeadMode(bool on)
    {
        GameManager.Instance.SetBigHeadMode(on);
    }
}

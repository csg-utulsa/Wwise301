////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class AIPauseOnToggle : MonoBehaviour
{
    public void SetAIPaused(bool state)
    {
        GameManager.Instance.AIPaused = state;
    }
}

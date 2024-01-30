////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class TimePauseOnClick : MonoBehaviour
{
    private bool paused = false;

    public void ToggleTimePause()
    {
        if (paused)
        {
            //Resume
            GameManager.Instance.gameSpeedHandler.UnPauseGameSpeed(gameObject.GetInstanceID());
            paused = false;
        }
        else
        {
            //Pause
            GameManager.Instance.gameSpeedHandler.PauseGameSpeed(gameObject.GetInstanceID());
            paused = true;
        }
    }
}

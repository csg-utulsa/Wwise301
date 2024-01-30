////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLCDoor : MonoBehaviour
{
    public AK.Wwise.Event OpenSound;
    public AK.Wwise.Event StopSound;

    public void OpenDoor()
    {
        OpenSound.Post(gameObject);
        PlayerManager.Instance.cameraScript.StartShake(0.02f);
    }

    public void StopDoor()
    {
        StopSound.Post(gameObject);
        PlayerManager.Instance.cameraScript.StopShake();
        PlayerManager.Instance.cameraScript.CamShake(new PlayerCamera.CameraShake(0.1f, 0.6f));

    }

}

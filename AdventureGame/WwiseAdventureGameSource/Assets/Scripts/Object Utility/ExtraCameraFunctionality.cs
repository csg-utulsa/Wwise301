////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class ExtraCameraFunctionality : MonoBehaviour
{
    [Header("KeyCodes")]
    public KeyCode accessorKey;
    public KeyCode cameraLockKey;
    public KeyCode cameraFreezeKey;

    #region private variables
    private bool lockCamera = false;
    private bool freezeCamera = false;
    #endregion

    void Update()
    {
        //Lock camera in place, still looking at the player
        if (Input.GetKey(accessorKey) && Input.GetKeyDown(cameraLockKey))
        {
            if (lockCamera)
            {
                PlayerManager.Instance.cameraScript.cameraMode = PlayerCamera.CameraMode.normal;
            }
            else
            {
                PlayerManager.Instance.cameraScript.cameraMode = PlayerCamera.CameraMode.lockPosition;
            }
            lockCamera = !lockCamera;
        }

        //Lock camera completely – both position and rotation
        if (Input.GetKey(accessorKey) && Input.GetKeyDown(cameraFreezeKey))
        {
            if (freezeCamera)
            {
                PlayerManager.Instance.cameraScript.cameraMode = PlayerCamera.CameraMode.normal;
            }
            else
            {
                PlayerManager.Instance.cameraScript.cameraMode = PlayerCamera.CameraMode.lockCameraCompletely;
            }
            freezeCamera = !freezeCamera;
        }

    }
}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

[ExecuteInEditMode]
public class SkyboxCamera : MonoBehaviour
{
    [Header("Skybox Settings")]
    public Transform SkyboxLocation;
    public float scale = 0.05f;

    private Camera thisCam;

    void OnEnable()
    {
        if (SkyboxLocation == null)
        {
            GameObject newG = new GameObject("SkyboxLocation");
            newG.transform.position = transform.position;
            SkyboxLocation = newG.transform;
        }

        thisCam = GetComponent<Camera>();
    }

    public void SetRotation(Quaternion rot)
    {
        transform.rotation = rot;
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = SkyboxLocation.position + pos * scale;
    }

    public void SetFOV(float fov)
    {
        thisCam.fieldOfView = fov;
    }

}

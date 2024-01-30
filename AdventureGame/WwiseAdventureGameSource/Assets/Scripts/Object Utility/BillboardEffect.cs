////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    public bool animate;

    [ShowIf("animate", true)]
    public float hoverScale = 0.2f;
    [ShowIf("animate", true)]
    public float hoverSpeed = 3f;

    private Vector3 origPosition;
    private Camera mainCam;

    void Start()
    {
        origPosition = transform.localPosition;
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        transform.rotation = mainCam.transform.rotation;

        if (animate)
        {
            float hoverDistance = Mathf.Sin(Time.time * hoverSpeed) * hoverScale;
            transform.localPosition = origPosition + new Vector3(0f, hoverDistance, 0f);
        }
    }
}

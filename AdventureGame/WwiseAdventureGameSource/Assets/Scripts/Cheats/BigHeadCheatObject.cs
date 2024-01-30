////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class BigHeadCheatObject : MonoBehaviour
{
    public float scaling = 1.5f;

    private Vector3 origScale;

    void Awake()
    {
        origScale = transform.localScale;
    }

    void LateUpdate()
    {
        if (GameManager.Instance.BigHeadMode)
        {
            transform.localScale = origScale * scaling;
        }
        else
        {
            transform.localScale = origScale;
        }
    }

}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotationToGlobalAxis : MonoBehaviour {

    public Vector3 axis = Vector3.zero;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(axis);
    }
}

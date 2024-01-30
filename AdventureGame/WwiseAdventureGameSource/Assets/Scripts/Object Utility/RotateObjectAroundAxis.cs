////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class RotateObjectAroundAxis : MonoBehaviour {

	public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);
    public bool UseUnscaledTime = false;

	void Update () {
        transform.Rotate(rotationSpeed * (UseUnscaledTime ? Time.unscaledTime : Time.deltaTime));
	}
}

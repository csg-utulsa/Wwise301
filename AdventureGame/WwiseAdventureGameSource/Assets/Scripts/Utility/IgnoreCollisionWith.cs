////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionWith : MonoBehaviour {

    public Collider IgnoreObject;
    public Collider IgnoreWith;

	// Use this for initialization
	void Start () {
        if (IgnoreObject != null && IgnoreWith != null) {
            Physics.IgnoreCollision(IgnoreObject, IgnoreWith);
        }
	}

}

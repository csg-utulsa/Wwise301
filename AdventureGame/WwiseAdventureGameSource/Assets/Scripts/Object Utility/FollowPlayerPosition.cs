////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class FollowPlayerPosition : MonoBehaviour {
	
	void Update () {
		transform.position = PlayerManager.Instance.playerTransform.position;
	}
}

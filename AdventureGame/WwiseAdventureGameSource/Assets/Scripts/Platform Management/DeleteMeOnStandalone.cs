////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class DeleteMeOnStandalone : MonoBehaviour {
	#if UNITY_STANDALONE || UNITY_WEBGL
	void Awake(){
		Destroy (gameObject);
	}
	#endif
}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShaderOnIOS : MonoBehaviour
{
    public MeshRenderer Renderer;
    public Material newMaterial;

    private void Awake()
    {
#if UNITY_IOS
        Renderer.sharedMaterial = newMaterial;
#endif
    }
}

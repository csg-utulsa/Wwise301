////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlatformDependentParticles
{
    public ParticlesWithPlatform[] particleVariations;

    public PlatformDependentParticles()
    {
        string[] enumNames = System.Enum.GetNames(typeof(PlatformManager.PlatformType));
        particleVariations = new ParticlesWithPlatform[enumNames.Length];
        for (int i = 0; i < particleVariations.Length; i++)
        {
            particleVariations[i] = new ParticlesWithPlatform((PlatformManager.PlatformType)System.Enum.Parse(typeof(PlatformManager.PlatformType), i.ToString()));
        }
    }

    public GameObject GetParticles()
    {
        return particleVariations.FirstOrDefault(x => x.platform == PlatformManager.Instance.platform).particles;
    }
}

[System.Serializable]
public class ParticlesWithPlatform
{
    public GameObject particles;
    public PlatformManager.PlatformType platform;

    public ParticlesWithPlatform(PlatformManager.PlatformType type)
    {
        platform = type;
    }
}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿
public class PlatformManager : Singleton<PlatformManager>
{

    public enum PlatformType
    {
        standalone, mobile
    }

    public bool overridePlatform = false;

    [ShowIf("overridePlatform", true)]
    public PlatformType platform;

    void Awake()
    {
        if (overridePlatform)
            return;

#if UNITY_ANDROID || UNITY_MOBILE
        platform = PlatformType.mobile;
#endif

#if UNITY_STANDALONE || UNITY_WEBGL
        platform = PlatformType.standalone;
#endif
    }
}

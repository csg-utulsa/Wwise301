////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResolutionChange : MonoBehaviour
{
    public Dropdown dropdown;

    private Resolution[] resolutions;

    void OnEnable()
    {
        bool isMobile = false;
        Resolution currentResolution = Screen.currentResolution;

#if UNITY_STANDALONE || UNITY_WEBGL
        resolutions = Screen.resolutions;
        System.Array.Reverse(resolutions); //Reverse the array so the higher values are on top
#endif

#if UNITY_ANDROID || UNITY_IOS
        isMobile = true;

        resolutions = new Resolution[] {
            new Resolution() { width = (int)(currentResolution.width), height = (int)(currentResolution.height)},
            new Resolution() { width = (int)(currentResolution.width/1.5f), height = (int)(currentResolution.height/1.5f) }, //eg. 1080p to 720p
            new Resolution() { width = (int)(currentResolution.width/1.5f/1.5f), height = (int)(currentResolution.height/1.5f/1.5f) } //eg. 720p to 480p
        };

        Screen.SetResolution(resolutions[1].width, resolutions[1].height, true); //Automatically downscale for cheap performance gains on mobile! ≈720p still looks great on a 1080p display!
        dropdown.value = 1;
        currentResolution = resolutions[1];

        //Set the target framerate to 30 to preserve batterylife
        Application.targetFrameRate = 30;
#endif
        dropdown.options = new List<Dropdown.OptionData>();
        dropdown.captionText.text = (currentResolution.width + " × " + currentResolution.height + (isMobile ? "" : " @ " +currentResolution.refreshRate));

        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution r = resolutions[i];
            Dropdown.OptionData option = new Dropdown.OptionData((r.width + " × " + r.height + (isMobile ? "" : " @ " + r.refreshRate)));
            dropdown.options.Add(option);
        }
    }

    public void SetResolution(int idx)
    {
        Resolution r = resolutions[idx];
        Screen.SetResolution(r.width, r.height, true);
    }
}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualitySettingsChange : MonoBehaviour
{
    public Dropdown dropdown;

    #region private variables
    private int qualityLevels;
    #endregion

    private void Awake()
    {
        string[] qualitySettingsNames = QualitySettings.names;
        System.Array.Reverse(qualitySettingsNames);
        qualityLevels = qualitySettingsNames.Length;

        dropdown.options = new List<Dropdown.OptionData>();
        int currentQuality = QualitySettings.GetQualityLevel();
        dropdown.value = currentQuality;
        dropdown.captionText.text = qualitySettingsNames[qualityLevels - currentQuality - 1];

        for (int i = 0; i < qualitySettingsNames.Length; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(qualitySettingsNames[i]);
            dropdown.options.Add(option);
        }
    }

    public void SetQuality(int idx)
    {
        QualitySettings.SetQualityLevel(qualityLevels - idx - 1);
    }
}

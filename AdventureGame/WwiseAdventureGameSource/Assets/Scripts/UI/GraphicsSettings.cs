////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    public Dropdown dropdown;

    void OnEnable()
    {
        dropdown.options = new List<Dropdown.OptionData>();
        string[] names = QualitySettings.names;
        for (int i = 0; i < names.Length; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(names[i]);
            dropdown.options.Add(option);
        }
        dropdown.value = QualitySettings.GetQualityLevel();
        dropdown.captionText.text = names[QualitySettings.GetQualityLevel()];
    }

    public void ChangeGraphicsSettings()
    {
        dropdown.Hide();
        QualitySettings.SetQualityLevel(dropdown.value);
    }

}

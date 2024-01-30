////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LanguageChanger : MonoBehaviour
{

    [Header("UI Objects")]
    public Dropdown dropdown;

    private List<string> languages;

    void OnEnable()
    {
        dropdown.options = new List<Dropdown.OptionData>();
        languages = LanguageManager.GetAvailableLanguages();
        for (int i = 0; i < languages.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(languages[i]);
            dropdown.options.Add(option);
        }
        dropdown.captionText.text = LanguageManager.GetCurrentLanguage();
    }

    /// <summary>
    /// Hooked up in the editor in a UnityEvent
    /// </summary>
    public void ChangeLanguage()
    {
        LanguageManager.ChangeLanguage(dropdown.captionText.text);
        dropdown.captionText.text = LanguageManager.GetCurrentLanguage();
    }

}

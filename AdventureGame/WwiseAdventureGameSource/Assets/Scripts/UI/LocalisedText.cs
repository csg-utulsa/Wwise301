////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LocalisedText : MonoBehaviour
{
    public Text text;
    public string key;
    public bool forceUppercase;

    void OnEnable()
    {
        LanguageManager.OnLanguageChange += OnLanguageChange;
    }

    void Start()
    {
        SetText();
    }

    public void SetText()
    {
        if (key != null)
        {
            if (text != null)
            {
                if (forceUppercase)
                {
                    text.text = LanguageManager.GetText(key).ToUpper();
                }
                else
                {
                    text.text = LanguageManager.GetText(key);
                }
            }
        }
    }

    public void SetKey(string newKey)
    {
        key = newKey;
        SetText();
    }

    void OnLanguageChange()
    {
        SetText();
    }
}

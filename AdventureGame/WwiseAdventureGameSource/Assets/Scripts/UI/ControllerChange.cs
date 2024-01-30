////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.UI;

public class ControllerChange : MonoBehaviour
{
    [Header("Options")]
    public bool changeImage;
    [ShowIf("changeImage", true), Tooltip("If left blank, it will try to find an Image component on this GameObject. If unsuccesful, this script will be disabled.")]
    public Image image;

    [Space()]

    public bool changeText;
    [ShowIf("changeText", true)]
    public Text text;

    [Header("Control Axis")]
    public string control;

    [Header("Localised Text")]
    public bool localisedText;

    [ShowIf("localisedText", true)]
    public Text textObject;
    [ShowIf("localisedText", true)]
    public string key;

    void Awake()
    {
        InputManager.OnControlChange += OnControlChange;
        LanguageManager.OnLanguageChange += OnLanguageChange;

        if (changeImage && image == null)
        {
            image = GetComponent<Image>();

            if (image == null)
            {
                Debug.LogError("ControllerChange (" + gameObject.name + "): You forgot to assign the Image (UI.Image) component! Disabling ...");
                this.enabled = false;
            }
        }
        if (changeText && text == null)
        {
            text = GetComponent<Text>();

            if (text == null)
            {
                Debug.LogError("ControllerChange (" + gameObject.name + "): You forgot to assign the Text (UI.Text) component! Disabling ...");
                this.enabled = false;
            }
        }

        if (localisedText && textObject == null)
        {
            Debug.LogError("ControllerChange (" + gameObject.name + "): You forgot to assign the Text Object (UI.Text) component! Disabling ...");
            this.enabled = false;
        }
    }

    private void OnDestroy()
    {
        InputManager.OnControlChange -= OnControlChange;
        LanguageManager.OnLanguageChange -= OnLanguageChange;
    }

    void OnControlChange()
    {
        if (changeImage)
        {
            Sprite newSprite = InputManager.GetControlImage(control);
            if (newSprite != null)
            {
                if (image != null)
                {
                    image.sprite = newSprite;
                }
            }
        }

        if (changeText)
        {
            string newText = InputManager.GetControlString(control);
            if (newText != null)
            {
                text.text = newText;
            }
        }
    }

    void Start()
    {
        OnControlChange();
    }

    void OnLanguageChange()
    {
        if (localisedText)
        {
            textObject.text = LanguageManager.GetText(key);
        }
    }

}

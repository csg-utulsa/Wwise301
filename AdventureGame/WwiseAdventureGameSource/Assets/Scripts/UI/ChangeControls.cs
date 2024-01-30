////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ChangeControls : MonoBehaviour
{
    ToggleGroup toggleGroup;

    void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();

    }

    public void ChangeController()
    {
        if (toggleGroup.AnyTogglesOn())
        {
            Toggle tog = toggleGroup.ActiveToggles().FirstOrDefault();

            InputManager.ChangeController((InputManager.ControllerMode)System.Enum.Parse(typeof(InputManager.ControllerMode), tog.gameObject.name));
        }
    }
}

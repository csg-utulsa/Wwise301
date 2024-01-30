////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System;
using UnityEngine;
using System.Collections;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ShowIf : PropertyAttribute
{

    public string Conditional = ""; //the conditional to control the visibility
    public bool HideInInspector = false;
    public string enumValue;
    public float floatValue;

    public bool isBool;
    public bool isEnum;
    public bool isFloatHigherThan;

    public ShowIf(string boolVariable, bool hide)
    {
        this.Conditional = boolVariable;
        this.HideInInspector = hide;
        this.isBool = true;
    }

    public ShowIf(string enumInstance, string enumState)
    {
        this.Conditional = enumInstance;
        this.enumValue = enumState;
        this.HideInInspector = true;
        this.isEnum = true;
    }

    public ShowIf(string enumInstance, string enumState, bool hide)
    {
        this.Conditional = enumInstance;
        this.enumValue = enumState;
        this.HideInInspector = hide;
        this.isEnum = true;
    }

    public ShowIf(string floatVariable, float higherThan, bool hide)
    {
        this.Conditional = floatVariable;
        this.floatValue = higherThan;
        this.HideInInspector = hide;
        this.isFloatHigherThan = true;
    }

}

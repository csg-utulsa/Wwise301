////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DLC_Requirement : MonoBehaviour
{
    public DLC requiredDLC;

    public UnityEvent OnUnavailable;
    public UnityEvent OnAvailable;

    private bool available = false;

    void Start()
    {
        ValidateRequirement();

    }

    private bool HasDLC()
    {
        return !GameManager.InstanceIsNull() && GameManager.Instance.activeDLCs.Contains(requiredDLC);
    }

    public bool ValidateRequirement()
    {

        available = HasDLC();

        if (available)
        {
            OnAvailable.Invoke();
        }
        else
        {
            OnUnavailable.Invoke();
        }

        return available;
    }

    //Useful for UnityEvent hookups.
    public void ValidateNoReturn()
    {
        ValidateRequirement();
    }

}

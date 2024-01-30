////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class UnityEventOnTrigger : MonoBehaviour
{
    public GameObject ObjectThatTriggers;

    public UnityEvent OnTriggerEntered;
    public UnityEvent OnTriggerExited;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ObjectThatTriggers)
        {
            OnTriggerEntered.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == ObjectThatTriggers)
        {
            OnTriggerExited.Invoke();
        }
    }
}

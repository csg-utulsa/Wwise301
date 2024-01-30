////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PointerDownSound : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public AK.Wwise.Event mouseDownSound;
    public AK.Wwise.Event mouseUpSound;

    public void OnPointerDown(PointerEventData e)
    {
        mouseDownSound.Post(gameObject);
    }

    public void OnPointerUp(PointerEventData e)
    {
        mouseUpSound.Post(gameObject);
    }
}

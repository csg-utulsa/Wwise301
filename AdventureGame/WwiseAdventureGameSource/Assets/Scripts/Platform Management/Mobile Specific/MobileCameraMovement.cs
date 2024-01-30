////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MobileCameraMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float drag = 0.1f;

    #region private variables
    private Vector2 mouseMovement;
    private Vector2 origOffsetMax;
    private float sensitivity = 1f;
    private RectTransform rectTransform;

    private IEnumerator momentumRoutine;
    #endregion

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        origOffsetMax = rectTransform.offsetMax;

        GrowSpace();

        MobileEvents.OnMobileInventory += ShrinkSpace;
        MobileEvents.OnMobileInventoryClosed += GrowSpace;
    }

    public void OnCustomDrag(PointerEventData eventData)
    {
        mouseMovement = eventData.delta * Time.deltaTime * sensitivity;
    }

    PointerEventData ED; 

    void Update(){
        if(ED != null){
            OnCustomDrag(ED);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ED = eventData;
        if (momentumRoutine != null)
        {
            StopCoroutine(momentumRoutine);
        }
        StopMouse();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ED = null;
        momentumRoutine = MouseMomentum();
        StartCoroutine(momentumRoutine);
    }

    public Vector2 GetMouseMovement()
    {
        return mouseMovement;
    }

    IEnumerator MouseMomentum()
    {
        while (mouseMovement.magnitude > 0.1f)
        {
            mouseMovement = mouseMovement * (1 - drag);
            yield return null;
        }
        StopMouse();
    }

    void StopMouse()
    {
        mouseMovement = Vector3.zero;
    }

    void ShrinkSpace()
    {
        rectTransform.offsetMax = origOffsetMax;
    }

    void GrowSpace()
    {
        rectTransform.offsetMax = new Vector2(0, 0);
    }

    public void ChangeSensitivity(float sensitivity)
    {
        this.sensitivity = sensitivity;
    }
}

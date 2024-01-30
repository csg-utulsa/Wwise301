////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Image links")]
    public Image JoystickBackground;
    public Image JoystickHandle;

    #region private variables
    private Vector3 direction;
    private Vector2 inputVector;
    private bool sentSprint;
    #endregion

    void Start()
    {
        JoystickBackground.gameObject.SetActive(false);
    }

    public void OnCustomDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickBackground.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {

            //Recalculate to local space 
            pos.x /= JoystickBackground.rectTransform.sizeDelta.x;
            pos.y /= JoystickBackground.rectTransform.sizeDelta.y;

            inputVector = pos;
            float inputLength = inputVector.magnitude;
            if (inputLength > 1)
            {
                inputVector.Normalize();
            }

            if (inputLength >= 0.98f)
            {
                if (MobileEvents.OnMobileSprintDown != null)
                {
                    MobileEvents.OnMobileSprintDown();
                }
                sentSprint = true;
            }
            else
            {
                if (sentSprint)
                {
                    if (MobileEvents.OnMobileSprintUp != null)
                    {
                        MobileEvents.OnMobileSprintUp();
                    }
                    sentSprint = false;
                }
            }

            //Move handle image
            JoystickHandle.rectTransform.position = JoystickBackground.rectTransform.position + new Vector3(JoystickBackground.rectTransform.sizeDelta.x * inputVector.x, JoystickBackground.rectTransform.sizeDelta.y * inputVector.y, 0);

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ED = eventData;
        JoystickBackground.gameObject.SetActive(true);
        JoystickBackground.rectTransform.position = eventData.position;
        JoystickHandle.rectTransform.localPosition = Vector3.zero;
        
    }

    PointerEventData ED; 

    void Update(){
        if(ED != null){
            OnCustomDrag(ED);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        JoystickBackground.gameObject.SetActive(false);
        inputVector = Vector2.zero;
        ED = null;
    }

    public Vector2 GetInputVector()
    {
        return inputVector;
    }
}

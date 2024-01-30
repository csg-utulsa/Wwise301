////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class VirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

    public float holdThreshold = 0.6f;

	public UnityEvent OnClick;
    public UnityEvent OnHold;
    public UnityEvent OnSwipeUp;

    #region private variables
    private float value;
    private float counterValue;
    private Vector2 swipeDirection;
    private bool tappedDownOnButton = false;

    private IEnumerator counterRoutine;
    private IEnumerator directionCheckRoutine;
    #endregion

    public void OnPointerDown(PointerEventData eventData){
        ED = eventData;
		//value = 1f;
        value = 0f;
        tappedDownOnButton = true;

        counterRoutine = Counter();
        StartCoroutine(counterRoutine);

        swipeDirection = Vector2.zero;
        directionCheckRoutine = CheckDirection();
        StartCoroutine(directionCheckRoutine);
    }

    public void OnPointerUp(PointerEventData eventData){

		ED = null;
        if (tappedDownOnButton){
            value = 1f;
			OnClick.Invoke();
            tappedDownOnButton = false;
        }

        StopCoroutine(counterRoutine);
        StopCoroutine(directionCheckRoutine);
	}

    PointerEventData ED; 

    void Update(){
        if(ED != null){
            OnCustomDrag(ED);
        }
    }
	public void OnCustomDrag(PointerEventData eventData)
	{
        swipeDirection += eventData.delta;
	}

	public float GetButtonInput(){
		return value;
	}

    IEnumerator Counter(){
        counterValue = 0f;
        while(counterValue < holdThreshold){
            counterValue += Time.unscaledDeltaTime;
            yield return null;
        }
        OnHold.Invoke();
    }

    IEnumerator CheckDirection()
    {
        while (counterValue < holdThreshold){
            
            if (Mathf.Abs(swipeDirection.y) > Mathf.Abs(swipeDirection.x) && swipeDirection.y > Screen.height / 4f)
            {
                OnSwipeUp.Invoke();
            }

            yield return null;

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tappedDownOnButton = false;
    }
}

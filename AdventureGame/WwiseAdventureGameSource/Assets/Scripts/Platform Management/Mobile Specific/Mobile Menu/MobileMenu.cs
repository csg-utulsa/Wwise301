////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileMenu : MonoBehaviour
{
    public GameObject DefaultSelection;
    public float transitionTime = 0.5f;
    public float ClosedPositionY = 30f;

    #region private variables
    private bool isOpen = false;
    private Vector2 closedPosition, openPosition;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private IEnumerator tweenRoutine;
    #endregion

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        openPosition = rectTransform.anchoredPosition;

        closedPosition = openPosition;
        closedPosition.y = rectTransform.sizeDelta.y;
        StartCoroutine(SetInitialPosition());
    }

    public void SetMenu(bool active){
        if (active == isOpen) { return; }

        if (tweenRoutine != null)
        {
            StopCoroutine(tweenRoutine);
        }
        tweenRoutine = TweenMove(transitionTime, active);
        StartCoroutine(tweenRoutine);

        EventSystem.current.SetSelectedGameObject(DefaultSelection);
    }

    //always start transitioning from where it is at (in case it's closed mid-transition)
    float currentTime = 0f;
    IEnumerator TweenMove(float time, bool open)
    {
        isOpen = open;
        canvasGroup.interactable = isOpen;

        for (float t = currentTime; t < 1f; t += Time.unscaledDeltaTime / time)
        {
            float s = Curves.Instance.OvershootMenu.Evaluate(t);
            rectTransform.anchoredPosition = Vector3.LerpUnclamped(isOpen ? closedPosition : openPosition, isOpen ? openPosition : closedPosition, s);
            yield return null;
        }
        rectTransform.anchoredPosition = isOpen ? openPosition : closedPosition;
    }

    // I spent sooooo long figuring out why sizeDelta was always == Vector2.zero... Turns out if you just wait a frame, you get the correct values.
    IEnumerator SetInitialPosition()
    {
        yield return null;
        closedPosition.y = rectTransform.sizeDelta.y + ClosedPositionY;
        rectTransform.anchoredPosition = closedPosition;
    }
}

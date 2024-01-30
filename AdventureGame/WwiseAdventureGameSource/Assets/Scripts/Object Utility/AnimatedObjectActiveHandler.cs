////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ObjectAnimationEvent(bool active);
public class AnimatedObjectActiveHandler : MonoBehaviour {

	public ObjectAnimationEvent OnAnimationDone;
	public GameObject objectToAnimate;
    public bool deactivateOnDisable = true;
	public bool isActive = false;

    #region private variables
    private Transform trn;
    private Vector3 origScale;
    IEnumerator animationRoutine;
    #endregion

    public enum TweenType{ 
		Scale
	};

	private struct AnimationParameters{
		public TweenType type;
		public float time;
		public AnimationCurve curve;
		public bool active;
	}

	void Awake(){
		trn = objectToAnimate.transform;
		origScale = trn.localScale;
		trn.localScale = Vector3.zero;
	}

	public void EnableObject(float transitionTime){
        EnableObject(TweenType.Scale, transitionTime, Curves.Instance.Overshoot);
	}

	public void EnableObject(TweenType type, float transitionTime, AnimationCurve curve){
        if(animationRoutine != null){
            StopCoroutine(animationRoutine);
        }
        animationRoutine = Animate(new AnimationParameters() { type = type, time = transitionTime, curve = curve, active = true });
        StartCoroutine(animationRoutine);
	}

	public void DisableObject(float transitionTime){
        DisableObject(TweenType.Scale, transitionTime, Curves.Instance.SmoothIn);
	}


	public void DisableObject(TweenType type, float transitionTime, AnimationCurve curve){
        if(animationRoutine != null){
            StopCoroutine(animationRoutine);
        }
        animationRoutine = Animate(new AnimationParameters() { type = type, time = transitionTime, curve = curve, active = false });
        StartCoroutine(animationRoutine);
	}

	float currentTime = 0f;
    IEnumerator Animate(AnimationParameters param)
    {
        Vector3 destination = param.active ? origScale : Vector3.zero;
        isActive = param.active;

        if (deactivateOnDisable) {
            if (param.active)
            {
                objectToAnimate.SetActive(true);
            }
        }

		Vector3 currentScale = trn.localScale;
		for (float t = currentTime; t < 1f; t += Time.unscaledDeltaTime / param.time) {
			float s = param.curve.Evaluate (t);
			trn.localScale = Vector3.LerpUnclamped (currentScale, destination, s);
			yield return null;
		}
		trn.localScale = destination;

        if (deactivateOnDisable) {
            objectToAnimate.SetActive(param.active);
        }
		

		if (OnAnimationDone != null) {
			OnAnimationDone (param.active);
		}
	}

}

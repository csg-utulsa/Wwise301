////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinHandler : MonoBehaviour {

	public int coinCount{ 
		get; 
		private set;
	}

	[Header("UI Objects")]
	public AnimatedObjectActiveHandler coinCanvasAnimator;
	public Text coinCountText;
	public float showDuration = 5f;

    private IEnumerator animationRoutine;

	void Start(){
        if (coinCountText == null)
        {
            coinCountText = GameObject.Find("Coin count").GetComponent<Text>();
        }
        if (coinCanvasAnimator == null)
        {
            coinCanvasAnimator = GameObject.Find("Coin UI Parent").GetComponent<AnimatedObjectActiveHandler>();
        }
        coinCount = 0;
		SetCoinText ();
    }
		
	public void AddCoin(){
		coinCount++;
		
		//Update text
		SetCoinText();
	}

	public bool SpendCoins(int amount){
		if (amount <= coinCount) {
			//Enough coins! Wuhu!
			coinCount -= amount;
			SetCoinText ();
			return true;
		} else {
			//Not enough coins!
			return false;
		}
	}

	void SetCoinText(){
		coinCountText.text = coinCount.ToString ();

        if(animationRoutine != null){
            StopCoroutine(animationRoutine);
        }
        animationRoutine = ShowCoinCountForSeconds(showDuration);
        StartCoroutine(animationRoutine);
	}

	IEnumerator ShowCoinCountForSeconds(float s){
		//show
		coinCanvasAnimator.EnableObject(AnimatedObjectActiveHandler.TweenType.Scale, 1f, Curves.Instance.Overshoot);
		yield return new WaitForSecondsRealtime(s);
		coinCanvasAnimator.DisableObject(AnimatedObjectActiveHandler.TweenType.Scale, 0.5f, Curves.Instance.SmoothInOut);
	}
}

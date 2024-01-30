////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPickup : MonoBehaviour {

    public bool playSpawnSoundAtSpawn = true;
    public AK.Wwise.Event spawnSound;
	public string NameOfTag = "Player";
	public AK.Wwise.Event OnTriggerEnterSound;

	void Start(){
        if (playSpawnSoundAtSpawn){
            spawnSound.Post(gameObject);
        }
	}

	public void AddCoinToCoinHandler(){
		InteractionManager.SetCanInteract(this.gameObject, false);
		GameManager.Instance.coinHandler.AddCoin ();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag( NameOfTag)) {
			OnTriggerEnterSound.Post(gameObject);
		}
	}
}

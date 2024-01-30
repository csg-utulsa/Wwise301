////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class ShowInInventory : MonoBehaviour 
{
	public bool PickedUp = false;

	[Header("Inventory")]
	public bool VisibleInInventory = true;

	public bool showFullGameobject = false;
	public bool isAmountItem = false;

	[ShowIf("isAmountItem", true)]
	public int currentAmount = 0;

	[Header("Showing in Inventory")]
	public float SizeMultiplier = 1f;
	public bool SpecialUIScaling = false;
	public Vector3 Offset = Vector3.zero;

	[Header("QuestStatusScript")]
	public float LastTimeUpdated = 0f;
	public bool SeenBeenInQuestLog = true;
        
	public void AddAmount2Object(int sendAmount)
	{
		currentAmount += sendAmount;
		SeenBeenInQuestLog = false;
		LastTimeUpdated = Time.realtimeSinceStartup;
	}

	public void SetItemToPickedUp(bool pickedUp){
        PickedUp = pickedUp;
        SeenBeenInQuestLog = !pickedUp;
		LastTimeUpdated = Time.realtimeSinceStartup;
	}
		
}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSoundBankByName : MonoBehaviour {

	public string NameOfBank = "";

	void Awake()
	{
		if(NameOfBank != "")
		{
			AkBankManager.LoadBank(NameOfBank, false, false);

		}
	}

}

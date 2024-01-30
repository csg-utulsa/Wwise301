////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FPS_Display : MonoBehaviour
{
    public Text displayText;
    public int avgSize = 5;

    #region private variables
    private float time;
    private List<float> times = new List<float>();
    #endregion

    void OnEnable()
    {
        StartCoroutine(UpdateText());
    }

    //A simple running average
    void Update()
    {
        time = 1f / Time.unscaledDeltaTime;
        times.Add(time);

        if (times.Count > avgSize)
        {
            times.RemoveAt(0);
        }
    }

    IEnumerator UpdateText()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(0.25f);
            displayText.text = System.Math.Round((double)times.Average(), 1).ToString();
        }
    }
}

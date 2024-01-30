////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Overlayers : Singleton<UI_Overlayers>
{

    protected UI_Overlayers() { }

    [System.Serializable]
    public class ImageLayerElement
    {
        public Image imageLayer;
        [HideInInspector]
        public IEnumerator Handler;
    }
    public List<ImageLayerElement> ElementLayers;

    public enum TypesOfLayers
    {
        Blackscreen, CompleteScreen
    }
    public TypesOfLayers L_Types;

    float AlphaValue = 0f;

    void Start()
    {
        if (ElementLayers.Count > 0)
        {
            FadeLayer(false, 1.0f);
        }
    }

    // USE THIS TO FADE OUT AND IN.
    /// <summary>
    /// Fadelayer fades the screen to black or from it. If C is true, you'll get a black screen. 
    /// </summary>
    /// <param name="C"></param>
    /// <param name="seconds"></param>
    public void FadeLayer(bool C, float seconds)
    {
        // this could be used to have several image layers
        Image ImageLayer = ElementLayers[0].imageLayer;

        if (ElementLayers[0].Handler != null)
        {
            StopCoroutine(ElementLayers[0].Handler);
        }

        if (C)
        {
            ElementLayers[0].Handler = LerpScreen2(1.0f, seconds, ImageLayer);
            StartCoroutine(ElementLayers[0].Handler);
        }
        else
        {
            ElementLayers[0].Handler = LerpScreen2(0.0f, seconds, ImageLayer);
            StartCoroutine(ElementLayers[0].Handler);
        }
    }

    IEnumerator LerpScreen2(float AlphaSet, float seconds, Image Layer)
    {
        if (AlphaSet == 1f)
        {
            GetComponent<Canvas>().enabled = true;
        }

        if (AlphaSet > 0.5f)
        {
            for (float t = 0; t < 1; t += Time.unscaledDeltaTime / seconds)
            {
                Color col = Layer.color;
                AlphaValue = t;
                col.a = AlphaValue;
                Layer.color = col;
                yield return null;
            }
        }
        else
        {
            for (float t = 1; t > 0; t -= Time.unscaledDeltaTime / seconds)
            {
                Color col = Layer.color;
                AlphaValue = t;
                col.a = AlphaValue;
                Layer.color = col;
                yield return null;
            }
        }

        Color Ecol = Layer.color;
        AlphaValue = AlphaSet;
        Ecol.a = AlphaValue;
        Layer.color = Ecol;

        if (AlphaSet == 0f)
        {
            GetComponent<Canvas>().enabled = false;
        }

        yield return null;
    }

}

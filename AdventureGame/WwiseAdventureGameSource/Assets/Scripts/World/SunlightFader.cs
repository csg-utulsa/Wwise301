////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SunlightFader : MonoBehaviour
{
    public float NewIntensity = 0f;
    public Color NewAmbientColor;

    public bool SetFogColor = false;
    [ShowIf("SetFogColor", true)]
    public Color NewFogColor;

    [Space(15f)]

    public DayNightCycle sunScript;
    public Transform fadeStart, fadeEnd;
    public CheckForPlayer FadeArea;

    [Range(0.01f, 0.5f)]
    public float debugLineWidth = 0.1f;

    #region private variables
    private Collider triggerCollider;
    private IEnumerator fadeRoutine;
    private Transform player;

    private float fadeLineLength;
    private float progress = 0f;

    private Color origFogColor;
    #endregion

    private void OnEnable()
    {
        triggerCollider = GetComponent<Collider>();
        sunScript = GameObject.FindWithTag("Sun").GetComponent<DayNightCycle>();

        fadeLineLength = (fadeStart.position - fadeEnd.position).magnitude;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            player = col.transform;

            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
            }
            fadeRoutine = FadeSun();
            StartCoroutine(fadeRoutine);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            player = null;

            if (fadeRoutine != null)
            {
                StopCoroutine(fadeRoutine);
            }
            fadeRoutine = FadeSun();
            StartCoroutine(fadeRoutine);
        }
    }

    IEnumerator FadeSun()
    {
        while (player != null) //Check for reaching max distance instead maybe?
        {
            sunScript.setLightIntensity = false;
            sunScript.setAmbientColor = false;

            if (SetFogColor)
            {
                sunScript.setFogColor = false;
            }

            origFogColor = sunScript.fogColor.Evaluate(0f);

            if ((FadeArea != null && FadeArea.playerInside) || FadeArea == null) {
                Vector3 startToEnd = fadeEnd.position - fadeStart.position;
                Vector3 startToPlayer = player.position - fadeStart.position;
                Vector3 positionOnLine = Vector3.Project(startToPlayer, startToEnd);

                if (Vector3.Dot(positionOnLine, startToEnd) < 0)
                {
                    positionOnLine = Vector3.zero;
                }

                progress = positionOnLine.magnitude / fadeLineLength;
                progress = Mathf.Clamp01(progress);

                Color origAmb = sunScript.ambientColor.Evaluate(sunScript.timeOfDay / 24f);
                RenderSettings.ambientLight = Color.Lerp(origAmb, NewAmbientColor, progress);

                origFogColor = sunScript.fogColor.Evaluate(sunScript.timeOfDay / 24f);
            }

            float origSunIntensity = sunScript.GetCurrentSunIntensity();
            if (NewIntensity - origSunIntensity > 0.1f)
            {
                sunScript.GetSunLight().intensity = Mathf.Lerp(origSunIntensity, NewIntensity, progress);
            }
            else
            {
                sunScript.GetSunLight().intensity = NewIntensity;
            }

            RenderSettings.fogColor = Color.Lerp(origFogColor, NewFogColor, progress);

            yield return null;
        }
        sunScript.setLightIntensity = true;
        sunScript.setAmbientColor = true;

        if (SetFogColor)
        {
            sunScript.setFogColor = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (triggerCollider == null)
        {
            triggerCollider = GetComponent<Collider>();
        }

        if (sunScript != null && fadeStart != null && fadeEnd != null)
        {

            //Limit fade transforms to bounds
            fadeStart.position = triggerCollider.ClosestPoint(fadeStart.position);
            fadeStart.transform.rotation = Quaternion.LookRotation(fadeEnd.position - fadeStart.position);

            fadeEnd.position = triggerCollider.ClosestPoint(fadeEnd.position);
            fadeEnd.transform.rotation = Quaternion.LookRotation(fadeStart.position - fadeEnd.position);

            float lineSegments = 10f;

            Color origAmb = sunScript.ambientColor.Evaluate(sunScript.timeOfDay / 24f);
            Color targetAmb = NewAmbientColor;

            Vector3 startToEnd = fadeEnd.position - fadeStart.position;
            float segmentLength = startToEnd.magnitude / lineSegments;
            Vector3 segment = startToEnd.normalized * segmentLength;

            //Draw fading lines to represent the ambient color change
            for (int i = 0; i < lineSegments; i++)
            {
                Vector3 lineSegmentStart = fadeStart.position + (segment * i);
                Vector3 lineSegmentEnd = fadeStart.position + (segment * (i + 1));

                Gizmos.color = Color.Lerp(origAmb, targetAmb, i / lineSegments);
                DrawThickLine(lineSegmentStart, lineSegmentEnd, debugLineWidth, fadeStart);
            }
			Gizmos.color = origAmb;
			Gizmos.DrawSphere(fadeStart.position, 0.75f);
			Gizmos.color = targetAmb;
			Gizmos.DrawSphere(fadeEnd.position, 0.75f);
        }

        //Player along vector
        if (player != null)
        {
            Vector3 startToEnd2 = fadeEnd.position - fadeStart.position;
            Vector3 startToPlayer = player.position - fadeStart.position;
            Vector3 positionOnLine = Vector3.Project(startToPlayer, startToEnd2);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(fadeStart.position + positionOnLine, 1f);
        }
    }

    void DrawThickLine(Vector3 startPos, Vector3 endPos, float width, Transform origin)
    {
        for (float x = -width / 2; x < width / 2; x += 0.01f)
        {
            for (float y = -width / 2; y < width / 2; y += 0.01f)
            {
                Vector3 offset = origin.right * x + origin.up * y;
                Gizmos.DrawLine(startPos + offset, endPos + offset);
            }
        }
    }
}
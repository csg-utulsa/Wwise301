////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using UnityEngine;

public class DisableLightAfterDistance : MonoBehaviour
{
    public Light lightToDisable;
    public float checkInterval = 1f;

    public float activeDistance = 100f;

    #region private variables
	private Transform lampTransform;
    private Transform targetTransform;
    #endregion

    private void Start()
    {
        targetTransform = PlayerManager.Instance.PlayerCam.transform;
        lampTransform = lightToDisable.transform;
        StartCoroutine(CheckPlayerDistance(checkInterval));
    }

    IEnumerator CheckPlayerDistance(float interval)
    {
        while (true)
        {
            float distance = (targetTransform.position - lampTransform.position).sqrMagnitude;

            if (distance < activeDistance * activeDistance)
            {
                lightToDisable.enabled = true;
            }
            else
            {
                lightToDisable.enabled = false;
            }

            yield return new WaitForSeconds(interval);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (lampTransform == null)
        {
            lampTransform = transform;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(lampTransform.position, activeDistance);
    }
}

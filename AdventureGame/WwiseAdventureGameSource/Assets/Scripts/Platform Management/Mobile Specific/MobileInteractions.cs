////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInteractions : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public LayerMask interactionLayers;

    #region private variables
    private Ray ray;
    private RaycastHit[] hits = new RaycastHit[4];
    private List<Transform> potentialInteractions;
    private Camera mainCam;
    #endregion

    private void OnEnable()
    {
        mainCam = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        potentialInteractions = new List<Transform>();

        ray = mainCam.ScreenPointToRay(eventData.position);
        if (Physics.RaycastNonAlloc(ray, hits, 10f, interactionLayers, QueryTriggerInteraction.Collide) > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform != null)
                {
                    Pickup p = hits[i].transform.GetComponent<Pickup>();
                    if (p != null)
                    {
                        if (p.addedToInteractManager)
                        {
                            potentialInteractions.Add(hits[i].transform);
                        }
                    }
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ray = mainCam.ScreenPointToRay(eventData.position);

        if (Physics.RaycastNonAlloc(ray, hits, 10f, interactionLayers, QueryTriggerInteraction.Collide) > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform != null)
                {
                    if (potentialInteractions.Contains(hits[i].transform))
                    {
                        GameManager.InteractWithObject(hits[i].transform.gameObject);
                    }
                }
            }
        }

        if (MobileEvents.OnMobileUseUp != null)
        {
            MobileEvents.OnMobileUseUp();
        }
    }
}

public delegate void MobileInputEvent();
public static class MobileEvents
{
    public static MobileInputEvent OnMobileUse;
    public static MobileInputEvent OnMobileUseUp;
    public static MobileInputEvent OnMobileSprintDown;
    public static MobileInputEvent OnMobileSprintUp;
    public static MobileInputEvent OnMobileInventory;
    public static MobileInputEvent OnMobileInventoryClosed;
    public static MobileInputEvent OnMobileInventoryWantsClosed;
    public static MobileInputEvent OnMobileMenu;
}


////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionManager : MonoBehaviour
{
    public static List<GameObject> InteractableObjects = new List<GameObject>();
    public static bool inConversation = false;

	private IEnumerator MarkerUpdate;

    [ContextMenu("PRINT ZE OBJEKTS")]
    public void PrintInteractableObjects()
    {
        for (int i = 0; i < InteractableObjects.Count; i++)
        {
            print(InteractableObjects[i]);
        }
    }

    public static void SetCanInteract(GameObject gameObj, bool canInteract)
    {
        if (gameObj != null)
        {
            if (canInteract)
            {
                InteractableObjects.Add(gameObj);
            }
            else
            {
                if (InteractableObjects.Contains(gameObj))
                {
                    InteractableObjects.Remove(gameObj);
                }
            }
        }
    }

    public static void RescanInteractions()
    {
        for (int i = InteractableObjects.Count - 1; i >= 0; i--)
        {
            if (!InteractableObjects[i].GetComponent<Pickup>().InteractionEnabled)
            {
                InteractableObjects.RemoveAt(i);
            }
        }
    }

    void SortInteractablesByDistance()
    {
        if (InteractableObjects.Count > 1)
        {
            InteractableObjects = InteractableObjects.OrderBy(x => Vector3.SqrMagnitude(PlayerManager.Instance.playerTransform.position - x.transform.position)).ToList();
        }
    }

    void OnEnable()
    {
        InputManager.OnMoveDown += StartMarkerUpdate;
        InputManager.OnMoveUp += EndMarkerUpdate;
        InputManager.OnUseDown += TryInteract;
    }

    void OnDisable()
    {
        InputManager.OnMoveDown -= StartMarkerUpdate;
		InputManager.OnMoveUp -= EndMarkerUpdate;
		InputManager.OnUseDown -= TryInteract;
    }

    void TryInteract()
    {
        if (!inConversation) // If not currently chatting
        {
            if (InteractableObjects.Count > 0)
            {
                GameManager.InteractWithObject(InteractableObjects[0]);
            }
        }
    }

    public static void BreakConversation()
    {
        inConversation = false;
        PlayerManager.Instance.cameraScript.ResetCamera();
    }

    IEnumerator UpdateMarkerSelect()
    {
        while (true)
        {
            SortInteractablesByDistance();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StartMarkerUpdate()
    {
        if (MarkerUpdate == null)
        {
            MarkerUpdate = UpdateMarkerSelect();
            StartCoroutine(MarkerUpdate);
        }
    }

    public void EndMarkerUpdate()
    {
        if (MarkerUpdate != null)
        {
            StopCoroutine(MarkerUpdate);
            MarkerUpdate = null;
        }
    }
}

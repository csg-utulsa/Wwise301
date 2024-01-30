////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QuestSystem;
public class QuestItemPreviewItem : MonoBehaviour
{

    public ItemType itemType;
    public int progress;
    public int required;

    public AnimatedObjectActiveHandler animationHandler;

    public Transform itemParent;
    public Text itemCountText;
    public float itemScaleMultiplier = 5f;

    #region private variables
    CollectionObjective currentObjective;
    private bool subscribedForUpdates = false;
    #endregion

    public void SetItemPreview(GameObject item, CollectionObjective objective)
    {
        if(currentObjective != null)
        {
            SetUpdateDisabled(objective);
        }
        currentObjective = objective;
        
        foreach (Transform child in item.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("UI");
        }

        item.gameObject.layer = LayerMask.NameToLayer("UI");
        item.transform.SetParent(itemParent);
        item.transform.localScale = Vector3.one;
        item.transform.localPosition = Vector3.zero;

        SetUpdateEnabled(currentObjective);
    }

    private void SetUpdateEnabled(QuestObjective objective)
    {
        objective.OnUpdate += UpdateItem;
        objective.OnComplete += SetUpdateDisabled;
        subscribedForUpdates = true;
    }

    private void SetUpdateDisabled(QuestObjective objective)
    {
        if (subscribedForUpdates)
        {
            objective.OnUpdate -= UpdateItem;
            objective.OnComplete -= SetUpdateDisabled;
            subscribedForUpdates = false;
        }
    }

    void UpdateItem(QuestObjective sender)
    {
        RefreshItemCount();
    }

    public void RefreshItemCount()
    {
        if (itemCountText != null)
        {
            itemCountText.text = currentObjective.GetObjectiveStatusString();
        }
        else {
            print("itemCountText is null");
        }
        
    }
}

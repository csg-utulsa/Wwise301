////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using QuestSystem;

public class QuestItemPreview : MonoBehaviour
{
    public QuestGiver QuestGiver;

    public GameObject ItemPreviewTemplate;
    public float DialogueOpenYPosition = 75;
    public float DialogueOpenTransitionTime = 1f;

    public List<GameObject> itemTemplates = new List<GameObject>();

    #region private variables
    private List<QuestItemPreviewItem> currentPreviewItems = new List<QuestItemPreviewItem>();
    private Quest currentlyDisplayedQuest;
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private IEnumerator lerpRoutine;
    private IEnumerator itemInitRoutine;
    #endregion

    private void OnEnable()
    {
        QuestGiver.OnNewQuest += InitializeItemPreview;

        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        DialogueManager.OnDialogueBegin += OffsetDisplayPosition;
        DialogueManager.OnDialogueEnd += ResetDisplayPosition;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueBegin -= OffsetDisplayPosition;
        DialogueManager.OnDialogueEnd -= ResetDisplayPosition;
    }

    public void InitializeItemPreview(Quest quest)
    {
        if (quest != currentlyDisplayedQuest)
        {
            currentlyDisplayedQuest = quest;

            ResetPreviewItems();

            if(itemInitRoutine != null)
            {
                StopCoroutine(itemInitRoutine);
            }

            itemInitRoutine = ItemPreviewInit(quest);
            StartCoroutine(itemInitRoutine);
        }
    }

    private IEnumerator ItemPreviewInit(Quest quest)
    {
        List<CollectionObjective> itemstToCollect = new List<CollectionObjective>();
        for (int i = 0; i < quest.Objectives.Count; i++)
        {
            if (quest.Objectives[i] is CollectionObjective)
            {
                itemstToCollect.Add(quest.Objectives[i] as CollectionObjective);
            }
        }

        bool dialogueFinished = false;
        UnityEngine.Events.UnityAction setDialogueFinished = () => dialogueFinished = true;
        quest.OnDialogueComplete.AddListener(setDialogueFinished);
        yield return new WaitUntil(() => dialogueFinished);
        quest.OnDialogueComplete.RemoveListener(setDialogueFinished);

        for (int i = 0; i < itemstToCollect.Count; i++)
        {
            var currentItem = itemstToCollect[i];

            GameObject itemPreview = Instantiate(ItemPreviewTemplate) as GameObject;
            itemPreview.transform.SetParent(transform);
            itemPreview.transform.localPosition = Vector3.zero;
            itemPreview.transform.localScale = Vector3.one;
            itemPreview.SetActive(true);

            QuestItemPreviewItem previewScript = itemPreview.GetComponent<QuestItemPreviewItem>();
            previewScript.itemType = currentItem.ItemToCollect;
            previewScript.required = currentItem.requiredAmount;
            previewScript.progress = 0;

            var itemPreviewObj = itemTemplates.First((go) => go.GetComponent<QuestItem>().itemType == currentItem.ItemToCollect);
            GameObject itemGraphics = Instantiate(itemPreviewObj) as GameObject;
            itemGraphics.AddComponent<RotateObjectAroundAxis>();
            itemGraphics.SetActive(true);

            Utility.StripGameObjectFromComponents(itemGraphics, new System.Type[] { typeof(QuestItem), typeof(Collider), typeof(ShowInInventory), typeof(Pickup) });
            previewScript.SetItemPreview(itemGraphics, currentItem);
            previewScript.RefreshItemCount();

            currentPreviewItems.Add(previewScript);

        }

        quest.OnQuestComplete += HideItemPreview;

        StartCoroutine(ShowItemsWithDelay(0.2f));
    }

    IEnumerator ShowItemsWithDelay(float delay)
    {


        for (int i = 0; i < currentPreviewItems.Count; i++)
        {
            currentPreviewItems[i].animationHandler.EnableObject(1f);
            yield return new WaitForSecondsRealtime(delay);
        }

    }

    private void HideItemPreview(QuestSystem.Quest quest)
    {
        quest.OnQuestComplete -= HideItemPreview;
        currentlyDisplayedQuest = null;
        ResetPreviewItems();
    }

    private void ResetPreviewItems()
    {
        for (int i = 0; i < currentPreviewItems.Count; i++)
        {
            Destroy(currentPreviewItems[i].gameObject);
        }
        currentPreviewItems = new List<QuestItemPreviewItem>();
    }

    private void OffsetDisplayPosition(int questgiverID)
    {
        lerpRoutine = OffsetRect(DialogueOpenYPosition);
        StartCoroutine(lerpRoutine);
    }

    private void ResetDisplayPosition(int questgiverID)
    {
        lerpRoutine = OffsetRect(originalPosition.y);
        StartCoroutine(lerpRoutine);
    }

    IEnumerator OffsetRect(float newY)
    {
        float prevY = rectTransform.anchoredPosition.y;
        for (float t = 0; t < 1f; t += Time.unscaledDeltaTime / DialogueOpenTransitionTime)
        {
            float v = Curves.Instance.SmoothOut.Evaluate(t);
            rectTransform.anchoredPosition = new Vector2(originalPosition.x, Mathf.Lerp(prevY, newY, v));
            yield return null;
        }
    }
}

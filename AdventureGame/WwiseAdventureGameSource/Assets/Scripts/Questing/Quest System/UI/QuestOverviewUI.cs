////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;
using UnityEngine.UI;

namespace QuestSystem
{
    public enum QuestElementStatus
    {
        Inactive, InProgress, Complete
    }

    public class QuestOverviewUI : MonoBehaviour
    {
        public static QuestOverviewUI instance;

        public QuestSystem.QuestGiver QuestGiver;
        public GameObject QuestOverviewTemplate;

        public ScrollRect scrollView;

        [Header("Icons")]
        public Sprite CompleteIcon;
        public Sprite InProgressIcon;
        public Sprite InactiveIcon;

        #region private variables
        private Dictionary<Quest, QuestUITemplate> UIQuestObjects = new Dictionary<Quest, QuestUITemplate>();
        private ContentSizeFitter[] contentSizeFitters;
        #endregion

        private void Awake()
        {
            instance = this;
            Menu.OnMenuStateChange += CheckQuestProgress;
            LanguageManager.OnLanguageChange += ToggleContentSizeFitters;
        }

        private void OnDestroy()
        {
            LanguageManager.OnLanguageChange -= ToggleContentSizeFitters;
            Menu.OnMenuStateChange -= CheckQuestProgress;
        }

        private void CheckQuestProgress(bool open)
        {
            if (open)
            { 
                StartCoroutine(SetScrollbarPosition());
                ToggleContentSizeFitters();
            }
        }

        private IEnumerator SetScrollbarPosition()
        {
            yield return null;
            scrollView.verticalNormalizedPosition = (1f - QuestGiver.GetNormalizedQuestlineProgress());
        }

        private void OnEnable()
        {
            for (int i = 0; i < QuestGiver.Quests.Count; i++)
            {
                var currentQuest = QuestGiver.Quests[i];

                GameObject UIQuestElement = Instantiate(QuestOverviewTemplate, QuestOverviewTemplate.transform.parent) as GameObject;
                var template = UIQuestElement.GetComponent<QuestUITemplate>();
                template.SetQuest(currentQuest, (i + 1).ToString());

                UIQuestObjects.Add(currentQuest, template);
            }
            QuestOverviewTemplate.SetActive(false);

            contentSizeFitters = GetComponentsInChildren<ContentSizeFitter>();
            ToggleContentSizeFitters();
        }

        public static Sprite GetCompleteIcon()
        {
            return instance.CompleteIcon;
        }

        public static Sprite GetInProgressIcon()
        {
            return instance.InProgressIcon;
        }

        public static Sprite GetInactiveIcon()
        {
            return instance.InactiveIcon;
        }

        public static void SetCurrentQuest(Quest quest)
        {
            int questIdx = instance.QuestGiver.GetQuestIndex(quest);
            instance.QuestGiver.SetQuestLineProgress(questIdx);

            ToggleContentSizeFitters();
        }

        public static void ToggleContentSizeFitters()
        {
            instance.StartCoroutine(instance.ToggleFitters());
        }

        private IEnumerator ToggleFitters()
        {
            yield return null;

            foreach(var c in contentSizeFitters)
            {
                c.enabled = false;
                yield return null;
            }

            yield return null;

            foreach (var c in contentSizeFitters)
            {
                c.enabled = true;
                yield return null;
            }
        }
    }
}
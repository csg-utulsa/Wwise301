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

namespace QuestSystem
{
    public class QuestUITemplate : MonoBehaviour
    {
        public GameObject ObjectiveTemplate;

        public Image Highlighter;
        public Image QuestStatusIcon;
        public GameObject QuestStatusIconParent;
        public LocalisedText QuestTitle;
        public LocalisedText QuestDescription;
        public Text NumberText;

        #region private variables
#pragma warning disable 0414
        private QuestElementStatus status = QuestElementStatus.Inactive;
#pragma warning restore 0414

        private string questNumber;
        private Quest quest;
        private Dictionary<QuestObjective, GameObject> UIObjectiveObjects = new Dictionary<QuestObjective, GameObject>();
        #endregion

        private void Start()
        {
            SetInactive(null);
            QuestStatusIcon.GetComponent<Button>().onClick.AddListener(() => QuestOverviewUI.SetCurrentQuest(quest));
            QuestGiver.OnQuestlineForcedChangeStarted += SetInteractiveFalse;
            QuestGiver.OnQuestlineForcedChangeEnded += SetInteractiveTrue;
        }

        private void OnDestroy()
        {
            QuestGiver.OnQuestlineForcedChangeStarted -= SetInteractiveFalse;
            QuestGiver.OnQuestlineForcedChangeEnded -= SetInteractiveTrue;

            if (quest != null) {
                quest.OnQuestReset += SetInactive;
                quest.OnQuestReady += SetInProgress;
                quest.OnQuestComplete += SetCompleted;
                quest.OnQuestStatusUpdated += UpdateQuestText;
            }
            
        }

        private void SetInteractiveTrue(QuestGiver questgiver)
        {
            QuestStatusIcon.GetComponent<Button>().interactable = true;
        }

        private void SetInteractiveFalse(QuestGiver questgiver)
        {
            QuestStatusIcon.GetComponent<Button>().interactable = false;
        }

        public void SetInProgress(Quest quest)
        {
            status = QuestElementStatus.InProgress;
            Highlighter.enabled = true;
            QuestStatusIcon.sprite = QuestOverviewUI.GetInProgressIcon();
            ShowObjectives();
            ToggleIconActive();
            UpdateDisplay();
        }

        public void SetInactive(Quest quest)
        {
            status = QuestElementStatus.Inactive;
            Highlighter.enabled = false;
            QuestStatusIcon.sprite = QuestOverviewUI.GetInactiveIcon();
            HideObjectives();
            ToggleIconActive();
        }

        public void SetCompleted(Quest quest)
        {
            status = QuestElementStatus.Complete;
            Highlighter.enabled = false;
            QuestStatusIcon.sprite = QuestOverviewUI.GetCompleteIcon();
            HideObjectives();
            ToggleIconActive();
        }

        public void SetQuest(Quest quest, string number)
        {
            this.quest = quest;
            gameObject.name = number + LanguageManager.GetText(quest.TitleKey);
            questNumber = number;

            quest.OnQuestReset += SetInactive;
            quest.OnQuestReady += SetInProgress;
            quest.OnQuestComplete += SetCompleted;
            quest.OnQuestStatusUpdated += UpdateQuestText;
            UpdateQuestText(quest);

            //Objectives
            for (int i = 0; i < quest.Objectives.Count; i++)
            {
                var currentObjective = quest.Objectives[i];

                GameObject UIObjectiveElement = Instantiate(ObjectiveTemplate, ObjectiveTemplate.transform.parent) as GameObject;
                var template = UIObjectiveElement.GetComponent<ObjectiveUITemplate>();
                template.SetObjective(currentObjective, string.Format("{0}.{1}", questNumber, i + 1 + "."));

                UIObjectiveObjects.Add(currentObjective, UIObjectiveElement);
            }
            ObjectiveTemplate.SetActive(false);
            UpdateDisplay();
        }

        private void ShowObjectives()
        {
            foreach (GameObject go in UIObjectiveObjects.Values)
            {
                go.SetActive(true);
            }
            UpdateDisplay();
        }

        private void HideObjectives()
        {
            foreach (GameObject go in UIObjectiveObjects.Values)
            {
                go.SetActive(false);
            }
            UpdateDisplay();
        }

        private void UpdateQuestText(Quest quest)
        {
            QuestTitle.SetKey(quest.TitleKey);
            QuestDescription.SetKey(quest.DescriptionKey);
            NumberText.text = questNumber;
        }

        private void UpdateDisplay()
        {
            QuestOverviewUI.ToggleContentSizeFitters();
        }

        private void ToggleIconActive()
        {
            StartCoroutine(ToggleActive());
        }

        private IEnumerator ToggleActive()
        {
            QuestStatusIconParent.transform.localScale = Vector3.zero;

            for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / 0.75f)
            {
                float v = Curves.Instance.Overshoot.Evaluate(t);
                QuestStatusIconParent.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, v);
                yield return null;
            }
            QuestStatusIconParent.transform.localScale = Vector3.one;
        }
    }
}
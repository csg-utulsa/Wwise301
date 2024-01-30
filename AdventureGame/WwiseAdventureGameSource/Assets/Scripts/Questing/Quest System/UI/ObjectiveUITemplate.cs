////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace QuestSystem
{
    public class ObjectiveUITemplate : MonoBehaviour
    {
        public Image StatusIcon;
        public GameObject StatusIconParent;
        public LocalisedText ObjectiveTitle;
        public Text ObjectiveStatus;
        public Text NumberText;

        #region private variables
#pragma warning disable 0414
        private QuestElementStatus status = QuestElementStatus.Inactive;
#pragma warning restore 0414

        private QuestObjective objective;
        private string objectiveNumber;
        private bool listenerAdded = false;
        #endregion

        private void Start()
        {
            SetInactive(objective);
            ToggleIconActive();
        }

        public void SetObjective(QuestObjective objective, string number)
        {
            this.objective = objective;
            objectiveNumber = number;
            SetInactive(objective);

            SubscribeToEvents(objective);

            UpdateObjectiveText(objective);
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents(objective);
        }

        private void SetInteractiveTrue(QuestGiver questGiver)
        {
            StatusIcon.GetComponent<Button>().interactable = true;
        }

        private void SetInteractiveFalse(QuestGiver questGiver)
        {
            StatusIcon.GetComponent<Button>().interactable = false;
        }

        public void CompleteObjective()
        {
            objective.ForceComplete();
        }

        private void SubscribeToEvents(QuestObjective objective)
        {
            objective.OnReset += SetInactive;
            objective.OnObjectiveReady += SetInProgress;
            objective.OnComplete += SetComplete;
            objective.OnUpdate += UpdateObjectiveText;

            QuestGiver.OnQuestlineForcedChangeStarted += SetInteractiveTrue;
            QuestGiver.OnQuestlineForcedChangeEnded += SetInteractiveFalse;
        }

        private void UnsubscribeFromEvents(QuestObjective objective)
        {
            if (objective != null)
            {
                objective.OnReset -= SetInactive;
                objective.OnObjectiveReady -= SetInProgress;
                objective.OnComplete -= SetComplete;
                objective.OnUpdate -= UpdateObjectiveText;
            }

            QuestGiver.OnQuestlineForcedChangeStarted -= SetInteractiveTrue;
            QuestGiver.OnQuestlineForcedChangeEnded -= SetInteractiveFalse;
        }

        public void SetInactive(QuestObjective objective)
        {
            status = QuestElementStatus.Inactive;
            StatusIcon.sprite = QuestOverviewUI.GetInactiveIcon();

            if (listenerAdded)
            {
                StatusIcon.GetComponent<Button>().onClick.RemoveListener(CompleteObjective);
                listenerAdded = false;
            }

            ToggleIconActive();
            QuestOverviewUI.ToggleContentSizeFitters();
        }

        public void SetInProgress(QuestObjective objective)
        {
            status = QuestElementStatus.InProgress;
            StatusIcon.sprite = QuestOverviewUI.GetInProgressIcon();

            if (!listenerAdded)
            {
                StatusIcon.GetComponent<Button>().onClick.AddListener(CompleteObjective);
                listenerAdded = true;
            }

            ToggleIconActive();
            SetInteractiveTrue(null);
        }

        public void SetComplete(QuestObjective objective)
        {
            status = QuestElementStatus.Complete;
            StatusIcon.sprite = QuestOverviewUI.GetCompleteIcon();

            if (listenerAdded)
            {
                StatusIcon.GetComponent<Button>().onClick.RemoveListener(CompleteObjective);
                listenerAdded = false;
            }

            ToggleIconActive();
        }

        private void UpdateObjectiveText(QuestObjective objective)
        {
            if (ObjectiveStatus != null)
            {
                ObjectiveTitle.SetKey(objective.TitleKey);
                ObjectiveStatus.text = objective.GetObjectiveStatusString();
                NumberText.text = objectiveNumber;
            }
        }

        private void ToggleIconActive()
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(ToggleActive());
            }
        }

        private IEnumerator ToggleActive()
        {
            StatusIconParent.transform.localScale = Vector3.zero;

            for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / 0.75f)
            {
                float v = Curves.Instance.Overshoot.Evaluate(t);
                StatusIconParent.transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one, v);
                yield return null;
            }
            StatusIconParent.transform.localScale = Vector3.one;
            QuestOverviewUI.ToggleContentSizeFitters();
        }
    }
}

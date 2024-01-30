////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public enum QuestObjectiveState { Inactive, InProgress, Completed }

    public delegate void QuestObjectiveEvent(QuestObjective objective);
    public abstract class QuestObjective : MonoBehaviour
    {
        public QuestObjectiveEvent OnComplete;
        public QuestObjectiveEvent OnUpdate;
        public QuestObjectiveEvent OnObjectiveReady;
        public QuestObjectiveEvent OnRestarted;
        public QuestObjectiveEvent OnReset;

        protected int progress = 0;

        public string TitleKey;

        public QuestObjectiveState state = QuestObjectiveState.Inactive;

        [Tooltip("These objectives must be completed before this objective can be completed")]
        public List<QuestObjective> PrerequisiteObjectives = new List<QuestObjective>();

        [Tooltip("Preparations to be made before the objective is ready.")]
        public List<QuestObjectivePreparer> QuestPreparations = new List<QuestObjectivePreparer>();

        [Tooltip("Actions that should be performed after the objective is completed.")]
        public List<QuestCompleteAction> QuestCompletionActions = new List<QuestCompleteAction>();

        public virtual Coroutine Initialize()
        {
            return StartCoroutine(PrepareObjective());
        }

        public virtual Coroutine ResetObjective()
        {
            return StartCoroutine(ObjectiveReset());
        }

        public Coroutine ForceComplete()
        {
            return StartCoroutine(ForceCompleteObjective());
        }

        protected virtual void Complete()
        {
            StartCoroutine(ObjectiveCompletion());
        }

        private IEnumerator ObjectiveReset()
        {
            state = QuestObjectiveState.Inactive;
            progress = 0;

            for (int i = 0; i < QuestCompletionActions.Count; i++)
            {
                QuestCompletionActions[i].Reverse();
            }

            yield return StartCoroutine(ReversePreparations());

            if (OnUpdate != null)
            {
                OnUpdate(this);
            }
        }

        private IEnumerator ObjectiveRestart()
        {
            yield return ResetObjective();

            yield return Initialize();

            if (OnRestarted != null)
            {
                OnRestarted(this);
            }
        }

        public abstract string GetObjectiveStatusString();

        public virtual void UpdateProgress()
        {
            progress++;

            if (OnUpdate != null)
            {
                OnUpdate(this);
            }
        }

        private IEnumerator ObjectiveCompletion()
        {
            state = QuestObjectiveState.Completed;

            for (int i = 0; i < QuestCompletionActions.Count; i++)
            {
                QuestCompletionActions[i].Execute();
            }
            yield return StartCoroutine(ReversePreparations());

            if (OnUpdate != null)
            {
                OnUpdate(this);
            }

            if (OnComplete != null)
            {
                OnComplete(this);
            }
        }

        public virtual string GetTitle()
        {
            return LanguageManager.GetText(TitleKey);
        }

        private IEnumerator PrepareObjective()
        {
            state = QuestObjectiveState.InProgress;

            if (PrerequisiteObjectives.Count > 0)
            {
                //Subscribe to prerequisite completion
                for (int i = 0; i < PrerequisiteObjectives.Count; i++)
                {
                    QuestObjective prereq = PrerequisiteObjectives[i];
                    yield return new WaitUntil(() => prereq.state == QuestObjectiveState.Completed);
                }
            }

            for (int i = 0; i < QuestPreparations.Count; i++)
            {
                QuestObjectivePreparer p = QuestPreparations[i];

                bool prepFinished = false;

                if (p != null) {
                    QuestPreparationEvent prepIncrement = () => prepFinished = true;
                    p.OnPreparationDone += prepIncrement;
                    p.PrepareObjective();
                    yield return new WaitUntil(() => prepFinished);
                    p.OnPreparationDone -= prepIncrement;
                }
            }

            QuestObject.OnCollect += QuestObjectUpdate;

            if (OnObjectiveReady != null)
            {
                OnObjectiveReady(this);
            }
        }

        private IEnumerator ReversePreparations()
        {
            for (int i = QuestPreparations.Count - 1; i >= 0; i--)
            {
                QuestObjectivePreparer p = QuestPreparations[i];

                bool prepReversed = false;
                if (p != null)
                {
                    QuestPreparationEvent prepDecrement = () => prepReversed = true;
                    p.OnPreparationReversed += prepDecrement;
                    p.ReversePreparations();
                    yield return new WaitUntil(() => prepReversed);
                    p.OnPreparationDone -= prepDecrement;
                }
            }

            QuestObject.OnCollect -= QuestObjectUpdate;

            if (OnReset != null)
            {
                OnReset(this);
            }
        }

        private void ForceUpdate()
        {
            UpdateProgress();
        }

        private IEnumerator ForceCompleteObjective()
        {
            while (state != QuestObjectiveState.Completed)
            {
                ForceUpdate();
                yield return null;
            }
        }

        protected abstract void QuestObjectUpdate(object sender);
    }
}

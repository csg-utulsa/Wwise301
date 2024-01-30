////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    public delegate void QuestEvent(Quest quest);
    public delegate void QuestProgressUpdate(QuestObjective QO);
    public delegate void QuestReset();

    public class Quest : MonoBehaviour
    {
        public QuestEvent OnQuestReady;
        public QuestEvent OnQuestStatusUpdated;
        public QuestEvent OnQuestComplete;
        public QuestEvent OnQuestReset;
        public QuestEvent OnQuestRestarted;
        public static QuestProgressUpdate OnObjectiveStepProgress;
        public static QuestReset ResetListener;

        [Header("Quest Info")]
        public string TitleKey;
        public string DescriptionKey;
        public AK.Wwise.Bank QuestBank;
        public bool UnloadBankOnComplete = true;
        public bool ClearDialogueOnCompletion = true;

        [Header("Objectives")]
        public List<QuestObjective> Objectives;

        [Header("Dialogue")]
        [SerializeField]
        private DialogueSet Dialogue;

        [Header("UnityEvents")]
        public UnityEvent OnQuestStarted;
        public UnityEvent OnDialogueStart;
        public UnityEvent OnDialogueComplete;
        public UnityEvent OnQuestCompleted;

        #region private variables
        private int progress = 0;

        private bool subscribedToDialogueCallback = false;
        #endregion

        public Coroutine InitializeQuest()
        {
            QuestBank.Load();
            return StartCoroutine(QuestInitialization());
        }

        public Coroutine ResetQuest()
        {
            return StartCoroutine(QuestReset());
        }

        public void RestartQuest()
        {
            StartCoroutine(QuestRestart());
        }

        public Coroutine CompleteQuest()
        {
            if (UnloadBankOnComplete)
            {
            QuestBank.Unload();
            }
            if (ClearDialogueOnCompletion)
            {
                DialogueManager.Instance.ClearAllDialogues();
            }
            return StartCoroutine(QuestCompletion());
        }

        public Coroutine ForceCompleteQuest()
        {
            return StartCoroutine(ForceCompletion());
        }

        public bool GetQuestCompleted()
        {
            for (int i = 0; i < Objectives.Count; i++)
            {
                if (Objectives[i].state != QuestObjectiveState.Completed)
                {
                    return false;
                }
            }

            return true;
        }

        private IEnumerator QuestInitialization()
        {
            progress = 0;
            yield return StartCoroutine(InitializeObjectives());

            if (OnQuestReady != null)
            {
                OnQuestReady(this);
            }

            OnQuestStarted.Invoke();

            if (OnQuestStatusUpdated != null)
            {
                OnQuestStatusUpdated(this);
            }

            if (Dialogue != null && !subscribedToDialogueCallback)
            {
                Dialogue.OnPrimaryDialogueStarted += DialogueStarted;
                Dialogue.OnPrimaryDialogueEnded += DialogueEnded;
                subscribedToDialogueCallback = true;
            }
        }

        private IEnumerator QuestRestart()
        {
            yield return ResetQuest();

            yield return InitializeQuest();

            if (OnQuestRestarted != null)
            {
                OnQuestRestarted(this);
            }
        }

        private IEnumerator QuestReset()
        {
            progress = 0;
            ResetListener();

            if (Dialogue != null && subscribedToDialogueCallback)
            {
                Dialogue.OnPrimaryDialogueStarted -= DialogueStarted;
                Dialogue.OnPrimaryDialogueEnded -= DialogueEnded;
                subscribedToDialogueCallback = false;
            }
            ResetDialogue();

            for (int i = 0; i < Objectives.Count; i++)
            {
                QuestObjective currentObjective = Objectives[i];
                currentObjective.OnComplete -= UpdateQuestProgress;
                yield return currentObjective.ResetObjective();
            }

            if (OnQuestReset != null)
            {
                OnQuestReset(this);
            }
        }

        private IEnumerator ForceCompletion()
        {
            for (int i = 0; i < Objectives.Count; i++)
            {
                QuestObjective currentObjective = Objectives[i];

                if (currentObjective.state != QuestObjectiveState.Completed)
                {
                    yield return currentObjective.ForceComplete();
                }
            }
        }

        private IEnumerator QuestCompletion()
        {
            if (OnQuestComplete != null)
            {
                OnQuestComplete(this);
            }

            if (subscribedToDialogueCallback)
            {
                Dialogue.OnPrimaryDialogueStarted -= DialogueStarted;
                Dialogue.OnPrimaryDialogueEnded -= DialogueEnded;
            }

            //Invoke UnityEvent (for scene hookups)
            OnQuestCompleted.Invoke();
            yield return null;
        }

        public string GetProgressString()
        {
            return string.Format("{0}/{1} objectives completed", progress, Objectives.Count);
        }

        private IEnumerator InitializeObjectives()
        {
            for (int i = 0; i < Objectives.Count; i++)
            {
                QuestObjective currentObjective = Objectives[i];
                //yield return currentObjective.Initialize();
                currentObjective.OnComplete += UpdateQuestProgress;
                currentObjective.Initialize();

                //yield return new WaitUntil(() => currentObjective.state == QuestObjectiveState.InProgress);
            }
            yield return null;
        }

        private void UpdateQuestProgress(QuestObjective objective)
        {
            progress++;
            
            OnObjectiveStepProgress.Invoke(objective);

            objective.OnComplete -= UpdateQuestProgress;

            if (OnQuestStatusUpdated != null)
            {
                OnQuestStatusUpdated(this);
            }

            if (progress == Objectives.Count)
            {
                CompleteQuest();
            }
        }

        public DialogueSet GetDialogue()
        {
            return Dialogue;
        }

        private void ResetDialogue()
        {
            if (Dialogue != null)
            {
                Dialogue.Reset();
            }
        }

        private void DialogueEnded()
        {
            OnDialogueComplete.Invoke();
        }

        private void DialogueStarted()
        {
            OnDialogueStart.Invoke();
        }
    }
}

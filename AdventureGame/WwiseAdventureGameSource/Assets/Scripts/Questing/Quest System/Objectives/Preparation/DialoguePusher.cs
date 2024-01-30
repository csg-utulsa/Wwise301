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
    public class DialoguePusher : QuestObjectivePreparer
    {
        public List<DialogueLine> Dialogue;
        public bool AutoStartDialogue = false;
        public bool FreezePlayerDuringDialogue = true;

        public bool UseOther = false;
        [ShowIf("UseOther", true)]
        public GameObject ObjectToInteractWith;

        public UnityEvent OnDialogueFinished;

        public override void PrepareObjective()
        {
            SendDialogue();
            PreparationDone();
        }

        public override void ReversePreparations()
        {
            PreparationReversed();
        }

        public void SendDialogue()
        {
            DialogueManager.Instance.TransferDialogue(Dialogue, UseOther ? ObjectToInteractWith : gameObject);
            DialogueManager.OnDialogueEnd += TriggerEvent;
        }

        private void TriggerEvent(int dialogueID)
        {
            DialogueManager.OnDialogueEnd -= TriggerEvent;
            OnDialogueFinished.Invoke();
        }
    }
}

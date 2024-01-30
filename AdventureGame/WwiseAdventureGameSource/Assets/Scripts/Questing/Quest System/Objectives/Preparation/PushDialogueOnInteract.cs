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
    public class PushDialogueOnInteract : QuestObjectivePreparer
    {
        public Pickup InteractableScript;
        public DialoguePusher DialoguePusher;

        #region private variables
        private bool listenerAdded = false;
        #endregion

        public override void PrepareObjective()
        {
            if (!listenerAdded)
            {
                InteractableScript.OnInteraction.AddListener(PushDialogue);
            }
            listenerAdded = true;
            PreparationDone();
        }

        public override void ReversePreparations()
        {
            if (listenerAdded)
            {
                InteractableScript.OnInteraction.RemoveListener(PushDialogue);
                listenerAdded = false;
            }
            PreparationReversed();
        }

        private void PushDialogue()
        {
            if (listenerAdded)
            {
                InteractableScript.OnInteraction.RemoveListener(PushDialogue);
                listenerAdded = false;
            }

            DialoguePusher.SendDialogue();
        }
    }
}

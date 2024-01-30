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
    public class DialogueOverrider : QuestObjectivePreparer
    {
        public InteractableNPC NPC;
        public DialogueSet Dialogue;

        #region private variables
        private DialogueSet originalDialogue;
        #endregion

        public override void PrepareObjective()
        {
            originalDialogue = NPC.GetDialogue();
            NPC.SetDialogue(Dialogue);

            PreparationDone();
        }

        public override void ReversePreparations()
        {
            Dialogue.Reset();
            NPC.SetDialogue(originalDialogue);

            PreparationReversed();
        }
    }
}

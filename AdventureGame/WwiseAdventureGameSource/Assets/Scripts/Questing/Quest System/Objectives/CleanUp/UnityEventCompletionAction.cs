////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine.Events;

namespace QuestSystem
{
    public class UnityEventCompletionAction : QuestCompleteAction
    {
        public UnityEvent OnComplete;
        public UnityEvent OnReverse;

        public override void Execute()
        {
            OnComplete.Invoke();
        }

        public override void Reverse()
        {
            OnReverse.Invoke();
        }
    }
}
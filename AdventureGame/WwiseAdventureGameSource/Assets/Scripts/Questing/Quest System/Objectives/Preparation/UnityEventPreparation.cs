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
    public class UnityEventPreparation : QuestObjectivePreparer
    {
        public UnityEvent OnPreparation;
        public UnityEvent OnReversePreparation;

        public override void PrepareObjective()
        {
            OnPreparation.Invoke();
            PreparationDone();
        }

        public override void ReversePreparations()
        {
            OnReversePreparation.Invoke();
            PreparationReversed();
        }
    }
}
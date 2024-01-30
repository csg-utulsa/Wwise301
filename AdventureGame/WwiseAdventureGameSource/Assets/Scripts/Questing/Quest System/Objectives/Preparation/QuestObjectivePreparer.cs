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
    public delegate void QuestPreparationEvent();
    public abstract class QuestObjectivePreparer : MonoBehaviour
    {
        public event QuestPreparationEvent OnPreparationDone;
        public event QuestPreparationEvent OnPreparationReversed;

        public abstract void PrepareObjective();

        public abstract void ReversePreparations();

        public virtual void PreparationDone()
        {
            if (OnPreparationDone != null)
            {
                OnPreparationDone();
            }
        }

        public virtual void PreparationReversed()
        {
            if (OnPreparationReversed != null)
            {
                OnPreparationReversed();
            }
        }
    }
}

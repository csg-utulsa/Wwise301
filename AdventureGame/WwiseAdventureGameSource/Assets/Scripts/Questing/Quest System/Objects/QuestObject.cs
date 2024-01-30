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
    public delegate void QuestObjectEvent(object sender);
    public class QuestObject : MonoBehaviour
    {
        public static event QuestObjectEvent OnCollect;

        public virtual void Collect()
        {
            if (OnCollect != null)
            {
                OnCollect(this);
            }
        }
    }
}

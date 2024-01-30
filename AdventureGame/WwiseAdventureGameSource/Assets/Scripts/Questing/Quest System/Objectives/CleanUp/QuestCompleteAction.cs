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
    public abstract class QuestCompleteAction : MonoBehaviour
    {
        public abstract void Execute();

        public abstract void Reverse();
    }
}
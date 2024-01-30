////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestTriggerAttacher : QuestObjectAttacher
    {
        protected override Type QuestObjectType
        {
            get
            {
                return typeof(QuestTrigger);
            }
        }
    }
}
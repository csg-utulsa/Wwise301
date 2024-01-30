////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class Quest_Forge_Preparation : QuestObjectivePreparer
{
    public float newRTPCValues = 100f;

    public override void PrepareObjective()
    {
        GameManager.Instance.OverrideEnemyRTPCValues(true, newRTPCValues);

        PreparationDone();
    }

    public override void ReversePreparations()
    {
        GameManager.Instance.OverrideEnemyRTPCValues(false);

        PreparationReversed();
    }
}

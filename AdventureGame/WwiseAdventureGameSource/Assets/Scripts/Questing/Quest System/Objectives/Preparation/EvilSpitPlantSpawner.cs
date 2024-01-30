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
    public class EvilSpitPlantSpawner : QuestObjectSpawner
    {
        protected override bool ObjectIntegrityIntact(GameObject obj)
        {
            var creature = obj.GetComponent<Creature>();
            return creature.isAlive;;
        }

        protected override void PerformObjectIntegrityReset(GameObject obj, int idx)
        {
            Destroy(obj);
            base.PerformObjectIntegrityReset(obj, idx);
        }
     
    }
}

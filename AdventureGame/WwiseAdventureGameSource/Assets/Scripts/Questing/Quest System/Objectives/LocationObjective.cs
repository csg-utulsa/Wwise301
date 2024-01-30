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
    public class LocationObjective : QuestObjective
    {
        public GameObject TriggerObject;

        public override void UpdateProgress()
        {
            base.UpdateProgress();

            if (progress == 1)
            {
                Complete();
            }
        }

        protected override void Complete()
        {
            progress = 1;
            base.Complete();
        }

        protected override void QuestObjectUpdate(object sender)
        {
            if (sender is QuestTrigger)
            {
                QuestTrigger questTrigger = sender as QuestTrigger;
                int triggerID = questTrigger.gameObject.GetInstanceID();

                if (triggerID == TriggerObject.GetInstanceID())
                {
                    UpdateProgress();
                }
            }
        }

        public override string GetObjectiveStatusString()
        {
            return (progress == 0 ? "Not Done" : "Done");
        }
    }
}
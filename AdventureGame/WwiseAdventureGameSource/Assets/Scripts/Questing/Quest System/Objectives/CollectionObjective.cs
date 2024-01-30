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
    public class CollectionObjective : QuestObjective
    {
        public ItemType ItemToCollect;
        public int requiredAmount;
        public AK.Wwise.Event CollectionCompleteEvent;

        public override void UpdateProgress()
        {
			base.UpdateProgress();

            if (progress == requiredAmount)
            {
                Complete();
            }
        }

        protected override void Complete()
        {
            CollectionCompleteEvent.Post(gameObject);
            progress = requiredAmount;
            base.Complete();
        }

        protected override void QuestObjectUpdate(object sender)
        {
            if (sender is QuestItem)
            {
                QuestItem questItem = sender as QuestItem;

                if (questItem.itemType == ItemToCollect)
                {
                    UpdateProgress();
                }
            }
        }

        public override string GetObjectiveStatusString()
        {
            return string.Format("{0}/{1}", progress, requiredAmount);
        }
    }
}
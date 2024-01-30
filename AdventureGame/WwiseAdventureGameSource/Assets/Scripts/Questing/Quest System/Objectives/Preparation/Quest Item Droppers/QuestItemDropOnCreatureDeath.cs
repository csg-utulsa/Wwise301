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
    public class QuestItemDropOnCreatureDeath : QuestItemDropper
    {
        private Creature creature;

        #region private variables
        private bool subscribed = false;
        #endregion

        public void SetTarget(Creature c)
        {
            creature = c;
            if (!subscribed)
            {
                creature.OnCreatureDeath += DropItem;
                subscribed = true;
            }
        }

        public override void DropItem()
        {
            base.DropItem();
            if (subscribed)
            {
                creature.OnCreatureDeath -= DropItem;
            }
        }

        private void OnDestroy()
        {
            if(subscribed)
            {
                creature.OnCreatureDeath -= DropItem;
            }
        }
    }
}

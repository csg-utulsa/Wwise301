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
    public abstract class QuestItemDropper : MonoBehaviour
    {
        public GameObject ItemToDrop;
        public Vector3 DropPositionOffset = Vector3.zero;

        public virtual void DropItem()
        {
            Instantiate(ItemToDrop, transform.position + DropPositionOffset, Quaternion.identity);
        }

        public virtual void PerformDropperReverse()
        {
            Destroy(this);
        }
    }
}

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
    public enum ItemType
    {
        Book = 100,
        EvilEssence = 200,
        Mushroom = 300,
        CrystalShard = 400,
        Pinecone = 500,
        Key = 600,
        Scroll = 650,
        Sword = 700,
        Hammer = 800,
        Axe = 900,
        Pickaxe = 1000
    }

    public class QuestItem : QuestObject, IInteractable
    {
        public ItemType itemType;

        public void OnInteract()
        {
            Collect();
        }
    }
}

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
    public class EquipWeapon : QuestCompleteAction
    {
        public GameObject WeaponToEquip;

        #region private variables
        private int objectID;
        private Weapon weaponScript;
        #endregion

        private void Awake()
        {
            objectID = WeaponToEquip.GetInstanceID();
            weaponScript = WeaponToEquip.GetComponent<Weapon>();

            QuestObjectSpawner.OnReplacedUnintactObject += CheckWeaponIntegrity;
        }

        private void CheckWeaponIntegrity(int oldInstanceID, GameObject newObject)
        {
            if (oldInstanceID == objectID)
            {
                WeaponToEquip = newObject;
                objectID = WeaponToEquip.GetInstanceID();
                weaponScript = WeaponToEquip.GetComponent<Weapon>();
            }
        }

        public override void Execute()
        {
            if (!weaponScript.equipped)
            {
                weaponScript.EquipWeapon();
            }
        }

        public override void Reverse()
        {
            if (PlayerManager.Instance.EquippedInventory.Weapons.Contains(WeaponToEquip))
            {
                PlayerManager.Instance.Inventory_DestroyWeapon(WeaponToEquip);
            }
        }
    }
}

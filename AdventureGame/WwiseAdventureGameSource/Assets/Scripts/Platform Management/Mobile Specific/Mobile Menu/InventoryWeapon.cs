////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Linq;

public class InventoryWeapon : MonoBehaviour
{
    public WeaponTypes weaponType;
    public GameObject outlineObj;

    [Header("VFX")]
    public GameObject selectionParticles;

    public void Equip()
    {
        if (PlayerManager.Instance.equippedWeaponInfo.weaponType != weaponType)
        {
            for(int i=0; i<PlayerManager.Instance.pickedUpWeaponObjects.Count; i++)
            {
                Weapon currentWeapon = PlayerManager.Instance.pickedUpWeaponObjects[i].GetComponent<Weapon>();
                if(currentWeapon.weaponType == weaponType)
                {
                    Debug.Log("Equipping " + currentWeapon.gameObject + " ("+currentWeapon.weaponType+")");
                    currentWeapon.EquipWeapon();
                    return;
                }
            }
            Debug.Log("Couldn't find a " + weaponType + " in pickedUpWeaponObjects...");
        }

        if (selectionParticles != null)
        {
            GameObject go = Instantiate(selectionParticles, transform.GetChild(0).transform.position, Quaternion.identity) as GameObject;
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.localScale = new Vector3(18f, 18f, 18f);
            Destroy(go, 2f);
        }
    }

}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public enum SwingTypes{ None, Left, Right, Top }
public enum WeaponTypes{ None, Dagger, Sword, Axe, PickAxe, Hammer, EvilSpitPlant, EvilCrawler, EvilHead }
public enum WeaponAnimationTypes{ OneHanded, BigAndHeavy }

public class Attack {

    public float damage;
    public Vector3 impactPoint;
    public Vector3 attackDir;
    public float knockbackStrength;

    public SwingTypes swingType;

    public WeaponTypes weaponType;

    public Attack(float damage, Vector3 attackDir = default(Vector3), float knockbackStrength = 0, SwingTypes swingType = SwingTypes.None, WeaponTypes weaponType = WeaponTypes.None, Vector3 impactPoint = default(Vector3)){
		this.damage = damage;
        this.attackDir = attackDir;
        this.knockbackStrength = knockbackStrength;
        this.swingType = swingType;
        this.weaponType = weaponType;
        this.impactPoint = impactPoint;
	}		
}

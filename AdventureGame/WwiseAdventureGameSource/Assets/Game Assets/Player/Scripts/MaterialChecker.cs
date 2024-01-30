////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class MaterialChecker : MonoBehaviour
{
    public LayerMask layermask;

    #region private variables
    private RaycastHit hit;
    private Vector3 direction = Vector3.down;
    private Transform trn;
    private Vector3 checkOffset = Vector3.up * 0.1f;
    #endregion

    void Awake()
    {
        trn = transform;

        if (gameObject.name == "toe_right")
        {
            PlayerManager.foot_R = this;
            //print(gameObject.name+" is Right.");
        }
        else if (gameObject.name == "toe_left")
        {
            PlayerManager.foot_L = this;
            //print(gameObject.name+" is Left.");
        }
    }

    public void CheckMaterial(GameObject go)
    {
        if (Physics.Raycast(trn.position + checkOffset, direction, out hit, layermask))
        {
            SoundMaterial sm = hit.collider.gameObject.GetComponent<SoundMaterial>();

            if (sm != null)
            {
                sm.material.SetValue(go);
            }
        }
    }

    public AK.Wwise.Switch GetMaterial()
    {
        if (Physics.Raycast(trn.position + checkOffset, direction, out hit, layermask))
        {
            SoundMaterial sm = hit.collider.gameObject.GetComponent<SoundMaterial>();

            if (sm != null)
            {
                return sm.material;
            }
        }
        return null;
    }

}

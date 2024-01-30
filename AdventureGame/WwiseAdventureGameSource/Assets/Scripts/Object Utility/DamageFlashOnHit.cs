////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageFlashOnHit : MonoBehaviour, IDamageable
{
	public float flashDuration = 0.1f;

    #region private variables
    private List<Material> materials;
    private List<Color> originalColors;
    private bool isFlashing;
    #endregion

    void OnEnable()
    {
        materials = new List<Material>();
        originalColors = new List<Color>();

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].gameObject.layer != LayerMask.NameToLayer("Weapon"))
            {
                Material[] mats = renderers[i].materials;
                for (int j = 0; j < mats.Length; j++)
                {
                    if (mats[j].HasProperty("_EmissionColor"))
                    {
                        materials.Add(mats[j]);
                        originalColors.Add(mats[j].GetColor("_EmissionColor"));
                    }
                }
            }
        }
    }


    public void OnDamage(Attack attack)
    {
        if (!isFlashing)
        {
            StartCoroutine(DamageFlash());
        }
    }

    IEnumerator DamageFlash()
    {
        if (materials.Count > 0)
        {
            isFlashing = true;

            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetColor("_EmissionColor", Color.white);
            }

            yield return new WaitForSeconds(flashDuration);

            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetColor("_EmissionColor", originalColors[i]);
            }

            isFlashing = false;
        }
    }
}

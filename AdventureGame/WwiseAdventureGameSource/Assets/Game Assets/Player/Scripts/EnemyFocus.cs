////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyFocus : MonoBehaviour
{
    [Header("Object Links")]
    public GameObject targetHighlightGraphics;

    [Header("Detection Settings")]
    public float detectionRange = 10f;

    #region private variables
    private List<Creature> nearbyEnemies;
    private Transform currentFocus;

    private AnimatedObjectActiveHandler targetAnimation;
    private int currentFocusIndex = 0;

    private SphereCollider detectionTrigger;
    private Creature closestCreature = null;
    #endregion

    void Start()
    {
        targetHighlightGraphics = Instantiate(targetHighlightGraphics, transform) as GameObject;
        targetAnimation = targetHighlightGraphics.GetComponent<AnimatedObjectActiveHandler>();
        //InputManager.OnTabDown += OnTab;

        //create sphere collider
        detectionTrigger = gameObject.AddComponent<SphereCollider>();
        detectionTrigger.isTrigger = true;
        detectionTrigger.radius = detectionRange;

        //initialize nearbyEnemies list
        nearbyEnemies = new List<Creature>();
    }

    private void LateUpdate()
    {
        if (currentFocus != null)
        {
            targetHighlightGraphics.transform.position = currentFocus.position;
        }
    }

    private void Update()
    {
        if (nearbyEnemies.Count > 0)
        {
            if (!nearbyEnemies[currentFocusIndex].isAlive)
            {
                nearbyEnemies.Remove(nearbyEnemies[currentFocusIndex]);
                return;
            }
            UpdateFocus();

            //Enable graphics
            if (!targetAnimation.isActive)
            {
                targetAnimation.EnableObject(0.4f);
            }
        }
        else
        {
            if (targetAnimation.isActive)
            {
                targetAnimation.DisableObject(0.5f);
            }
        }

    }

    void OnTriggerEnter(Collider col)
    {
        Creature e = col.GetComponent<Creature>();

        if (e != null)
        { //Agent has a Creature or Creature derived script
            if (!e.isFriendly)
            { //Only hostile enemies? 
                nearbyEnemies.Add(e);
            }
        }
        UpdateFocus();
    }

    void OnTriggerExit(Collider col)
    {
        Creature e = col.GetComponent<Creature>();

        if (e != null)
        {
            if (nearbyEnemies.Contains(e))
            {
                nearbyEnemies.Remove(e);
            }
        }
        UpdateFocus();
    }

    void UpdateFocus()
    { 
        if (nearbyEnemies.Count > 0)
        {
            nearbyEnemies.RemoveAll(x => x == null);
            closestCreature = nearbyEnemies.OrderBy(x => Vector3.SqrMagnitude(x.transform.position - transform.position)).FirstOrDefault();
            if (closestCreature.gameObject.GetComponent<Creature>().CanBeAttacked())
            {
                currentFocus = closestCreature.transform;
                PlayerManager.Instance.targetEnemy = currentFocus.gameObject;
                return;
            }
            
        }
        closestCreature = null;
        PlayerManager.Instance.targetEnemy = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

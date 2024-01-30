////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace QuestSystem
{
    public class GameObjectMover : QuestObjectivePreparer
    {
        [Header("Movement Settings")]
        public GameObject GameObjectToMove;
        public Transform MovePosition;
        public bool MoveBackOnReset = true;
        public bool AwaitExternalCall = false;

        [Space(10f)]
        [Header("VFX")]
        public bool UseTeleportParticles = true;
        [ShowIf("UseTeleportParticles", true)]
        public GameObject TeleportParticles;

        #region private variables
        private Vector3 originalPosition;

        private NavMeshAgent navAgent;
        #endregion

        private void Awake()
        {
            originalPosition = GameObjectToMove.transform.position;

            navAgent = GameObjectToMove.GetComponent<NavMeshAgent>();
        }

        public override void PrepareObjective()
        {
            originalPosition = GameObjectToMove.transform.position;

            if (!AwaitExternalCall)
            {
                MoveObject(GameObjectToMove.transform.position, MovePosition.position);
            }

            PreparationDone();
        }

        public override void ReversePreparations()
        {
            if (MoveBackOnReset)
            {
                MoveBack();
            }

            PreparationReversed();
        }

        //For editor hook-ups
        public void Move()
        {
            MoveObject(GameObjectToMove.transform.position, MovePosition.position);
        }

        public void MoveBack()
        {
            MoveObject(GameObjectToMove.transform.position, originalPosition);
        }

        private void MoveObject(Vector3 currentPosition, Vector3 targetPosition)
        {
            if (currentPosition != targetPosition)
            {
                if (navAgent != null)
                {
                    navAgent.updatePosition = false;
                    navAgent.updateRotation = false;

                    navAgent.transform.position = targetPosition;

                    navAgent.Warp(targetPosition);
             
                    navAgent.updatePosition = true;
                    navAgent.updateRotation = true;
                }
                else
                {
                    GameObjectToMove.transform.position = targetPosition;
                }

                if (UseTeleportParticles)
                {
                    GameObject startVfx = Instantiate(TeleportParticles, currentPosition, Quaternion.identity) as GameObject;
                    Destroy(startVfx, 5f);

                    GameObject endVfx = Instantiate(TeleportParticles, targetPosition, Quaternion.identity) as GameObject;
                    Destroy(endVfx, 5f);
                }
            }
        }
    }
}

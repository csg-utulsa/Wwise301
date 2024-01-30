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
    public class GameObjectActiveSetter : QuestObjectivePreparer
    {
        public GameObject ObjectToSetActive;
        public bool ActiveStatus = true;

        [Space(10f), Tooltip("Toggles whether the object should revert back to its original active status on prep reverse.")]
        public bool KeepNewStatusOnReversePreparation = false;
        public bool AwaitExternalCall = false;

        #region private variables
        private int objectID;

        private bool originalStatus;
        #endregion

        private void Awake()
        {
            originalStatus = ObjectToSetActive.activeInHierarchy;

            objectID = ObjectToSetActive.GetInstanceID();
            QuestObjectSpawner.OnReplacedUnintactObject += CheckObjectIntegrity;
        }

        private void CheckObjectIntegrity(int oldInstanceID, GameObject newObject)
        {
            if (oldInstanceID == objectID)
            {
                ObjectToSetActive = newObject;
                objectID = ObjectToSetActive.GetInstanceID();
            }
        }

        public override void PrepareObjective()
        {
            if (!AwaitExternalCall)
            {
                SetObjectStatus(ActiveStatus);
            }

            PreparationDone();
        }

        public void SetObjectStatus(bool status)
        {
            if (ObjectToSetActive != null)
            {
                ObjectToSetActive.SetActive(ActiveStatus);
            }
        }

        public override void ReversePreparations()
        {
            if (!KeepNewStatusOnReversePreparation)
            {
                if (ObjectToSetActive != null)
                {
                    ObjectToSetActive.SetActive(originalStatus);
                }
            }

            PreparationReversed();
        }
    }
}
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
    public class MonoBehaviourEnabledSetter : QuestObjectivePreparer
    {
        public List<MonoBehaviour> ScriptsToSet = new List<MonoBehaviour>();
        public bool EnabledStatus = true;

        [Space(10f), Tooltip("Toggles whether the object should revert back to its original active status on prep reverse.")]
        public bool KeepNewStatusOnReversePreparation = false;
        public bool AwaitExternalCall = false;

        #region private variables
        private List<int> objectIDs = new List<int>();
        private List<bool> originalStatuses = new List<bool>();
        #endregion

        private void Awake()
        {
            for (int i = 0; i < ScriptsToSet.Count; i++)
            {
                objectIDs.Add(ScriptsToSet[i].gameObject.GetInstanceID());
                originalStatuses.Add(ScriptsToSet[i].enabled);
            }
            QuestObjectSpawner.OnReplacedUnintactObject += CheckObjectIntegrity;
        }

        private void CheckObjectIntegrity(int oldInstanceID, GameObject newObject)
        {
            for (int i = 0; i < objectIDs.Count; i++)
            {
                if(oldInstanceID == objectIDs[i])
                {
                    System.Type type = (ScriptsToSet[i] as Component).GetType();
                    ScriptsToSet[i] = newObject.GetComponent(type) as MonoBehaviour;
                    objectIDs[i] = ScriptsToSet[i].gameObject.GetInstanceID();
                }
            }
        }

        public override void PrepareObjective()
        {
            if (!AwaitExternalCall)
            {
                for (int i = 0; i < ScriptsToSet.Count; i++)
                {
                    if (ScriptsToSet[i] != null)
                    {
                        SetScriptEnabled(ScriptsToSet[i], EnabledStatus);
                    }
                }
            }

            PreparationDone();
        }

        public void SetScriptEnabled(MonoBehaviour behaviour, bool status)
        {
            if (behaviour != null)
            {
                behaviour.enabled = status;
            }
        }

        public override void ReversePreparations()
        {
            if (!KeepNewStatusOnReversePreparation)
            {
                for (int i = 0; i < ScriptsToSet.Count; i++)
                {
                    if (ScriptsToSet[i] != null)
                    {
                        SetScriptEnabled(ScriptsToSet[i], originalStatuses[i]);
                    }
                }
            }

            PreparationReversed();
        }
    }
}
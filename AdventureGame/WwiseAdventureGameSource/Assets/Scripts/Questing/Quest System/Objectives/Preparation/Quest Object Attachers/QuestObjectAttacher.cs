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
    public abstract class QuestObjectAttacher : QuestObjectivePreparer
    {
        public List<GameObject> ObjectsToAttachTo;
        public bool AwaitExternalCall = false;

        #region private variables
        private List<int> objectIDs = new List<int>();
        #endregion

        private void Awake()
        {
            for (int i = 0; i < ObjectsToAttachTo.Count; i++)
            {
                objectIDs.Add(ObjectsToAttachTo[i].GetInstanceID());
            }

            QuestObjectSpawner.OnReplacedUnintactObject += CheckObjectsIntegrity;
        }

        private void CheckObjectsIntegrity(int oldInstanceID, GameObject newObject)
        {
            for (int i = 0; i < ObjectsToAttachTo.Count; i++)
            {
                if (objectIDs[i] == oldInstanceID)
                {
                    ObjectsToAttachTo[i] = newObject;
                }
            }
        }

        protected abstract System.Type QuestObjectType
        {
            get;
        }

        public override void PrepareObjective()
        {
            if (!AwaitExternalCall)
            {
                Prepare();
            }

            PreparationDone();
        }

        public void Prepare()
        {
            for (int i = 0; i < ObjectsToAttachTo.Count; i++)
            {
                GameObject currentObject = ObjectsToAttachTo[i];
                if (currentObject != null)
                {
                    var existingBehaviour = currentObject.GetComponent(QuestObjectType);

                    if (existingBehaviour == null)
                    {
                        PerformAttachment(currentObject);
                    }
                }
            }
        }

        public override void ReversePreparations()
        {
            for (int i = 0; i < ObjectsToAttachTo.Count; i++)
            {
                GameObject currentObject = ObjectsToAttachTo[i];

                if (currentObject != null)
                {
                    var existingBehaviour = currentObject.GetComponent(QuestObjectType);

                    if (existingBehaviour != null)
                    {
                        Destroy(existingBehaviour);
                    }
                }
            }

            PreparationReversed();
        }

        protected virtual void PerformAttachment(GameObject obj)
        {
            obj.AddComponent(QuestObjectType);
        }
    }
}
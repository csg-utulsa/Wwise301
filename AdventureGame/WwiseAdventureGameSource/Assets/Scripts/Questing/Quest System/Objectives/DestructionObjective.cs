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
    public class DestructionObjective : QuestObjective
    {
        public List<GameObject> ObjectsToDestroy = new List<GameObject>();

        #region private variables
        private List<int> destructionObjectIDs = new List<int>();
        #endregion

        private void Awake()
        {
            for (int i = 0; i < ObjectsToDestroy.Count; i++)
            {
                destructionObjectIDs.Add(ObjectsToDestroy[i].GetInstanceID());
            }

            QuestObjectSpawner.OnReplacedUnintactObject += CheckIfObjectIsIntact;
        }

        private void CheckIfObjectIsIntact(int oldInstanceID, GameObject newObject)
        {
            for (int i = 0; i < destructionObjectIDs.Count; i++)
            {
                if (oldInstanceID == destructionObjectIDs[i])
                {
                    ObjectsToDestroy[i] = newObject;
                    destructionObjectIDs[i] = ObjectsToDestroy[i].GetInstanceID();
                }
            }
        }

        public override string GetObjectiveStatusString()
        {
            string statusString;
            int destroyCount = ObjectsToDestroy.Count;
            if (destroyCount > 1)
            {
                statusString = string.Format("{0}/{1}", progress, ObjectsToDestroy.Count);
            }
            else
            {
                statusString = progress == ObjectsToDestroy.Count ? "Done" : "Not Done";
            }

            return statusString;
        }

        protected override void Complete()
        {
            progress = ObjectsToDestroy.Count;
            base.Complete();
        }

        public override void UpdateProgress()
        {
            base.UpdateProgress();

            if (progress == ObjectsToDestroy.Count)
            {
                Complete();
            }
        }

        protected override void QuestObjectUpdate(object sender)
        {
            if (sender is QuestDestroyable)
            {
                QuestDestroyable questDestroyable = sender as QuestDestroyable;
                int objectID = questDestroyable.gameObject.GetInstanceID();
                if (destructionObjectIDs.Contains(objectID))
                {
                    UpdateProgress();
                }
            }
        }
    }
}

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
    public delegate void ObjectSpawnerEvent(List<GameObject> objects);
    public delegate void GlobalObjectSpawnerEventU(int oldInstanceID, GameObject newObject);
    public class QuestObjectSpawner : QuestObjectivePreparer
    {
        public event ObjectSpawnerEvent OnFinishedSpawning;
        public static event GlobalObjectSpawnerEventU OnReplacedUnintactObject;
        //public static 

        public List<GameObject> ExistingObjects = new List<GameObject>();
        public bool LeaveObjectsOnReset = true;

        #region private variables
        private List<GameObject> originalObjects = new List<GameObject>();
        private List<int> originalObjectIDs = new List<int>();
        #endregion

        private void Awake()
        {
            for (int i = 0; i < ExistingObjects.Count; i++)
            {
                //Copy original objects 
                GameObject currentObject = ExistingObjects[i];
                originalObjects.Add(Instantiate(currentObject, currentObject.transform.position, currentObject.transform.rotation, currentObject.transform.parent));
                originalObjects[i].name = currentObject.name;
                originalObjects[i].SetActive(false);

                originalObjectIDs.Add(currentObject.GetInstanceID());
            }
        }

        public override void PrepareObjective()
        {
            for (int i = 0; i < ExistingObjects.Count; i++)
            {
                GameObject currentObject = ExistingObjects[i];

                if (!ObjectIntegrityIntact(currentObject))
                {
                    PerformObjectIntegrityReset(currentObject, i);
                }
            }

            if (OnFinishedSpawning != null)
            {
                OnFinishedSpawning(ExistingObjects);
            }

            PreparationDone();
        }

        public override void ReversePreparations()
        {
            if (!LeaveObjectsOnReset)
            {
                for (int i = ExistingObjects.Count-1; i >= 0; i--)
                {
                    if (ExistingObjects[i] != null)
                    {
                        Destroy(ExistingObjects[i]);
                    }
                }
            }
            PreparationReversed();
        }

        protected virtual bool ObjectIntegrityIntact(GameObject obj)
        {
            return obj != null;
        }

        protected virtual void PerformObjectIntegrityReset(GameObject obj, int idx)
        {
            GameObject originalObject = originalObjects[idx];
            ExistingObjects[idx] = Instantiate(originalObject, originalObject.transform.position, originalObject.transform.rotation, originalObject.transform.parent);
            ExistingObjects[idx].name = originalObject.name;
            ExistingObjects[idx].SetActive(true);

            if (OnReplacedUnintactObject != null)
            {
                OnReplacedUnintactObject(originalObjectIDs[idx], ExistingObjects[idx]);
                originalObjectIDs[idx] = ExistingObjects[idx].GetInstanceID();
            }
        }
    }
}

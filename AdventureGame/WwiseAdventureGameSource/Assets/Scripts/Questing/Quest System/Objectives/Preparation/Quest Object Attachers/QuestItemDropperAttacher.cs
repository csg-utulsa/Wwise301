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
    public class QuestItemDropperAttacher : QuestObjectivePreparer
    {
        public QuestObjectSpawner ObjectSpawner;
        public QuestItemDropper DropperTemplate;

        #region private variables
        private bool readyToAttach = false;
        private List<GameObject> objectsToAttachTo;
        #endregion

        private void Awake()
        {
            ObjectSpawner.OnFinishedSpawning += SetReady;
        }

        public override void PrepareObjective()
        {
            StartCoroutine(PrepareObjectAttachment());
        }

        public override void ReversePreparations()
        {
            if (objectsToAttachTo != null)
            {
                for (int i = 0; i < objectsToAttachTo.Count; i++)
                {
                    var currentObject = objectsToAttachTo[i];

                    if (currentObject != null)
                    {
                        QuestItemDropper dropper = currentObject.GetComponent<QuestItemDropper>();

                        if (dropper != null)
                        {
                            dropper.PerformDropperReverse();
                        }
                    }
                }
            }

            PreparationReversed();
        }

        private IEnumerator PrepareObjectAttachment()
        {
            yield return new WaitUntil(() => readyToAttach);

            yield return StartCoroutine(AttachComponent(objectsToAttachTo));

            PreparationDone();
        }

        private IEnumerator AttachComponent(List<GameObject> gameObjects)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                //TODO: Attach objects here
                GameObject currentObject = gameObjects[i];

                System.Type type = DropperTemplate.GetType();
                var dropper = currentObject.GetComponent(type);

                if (dropper == null)
                {
                    var component = currentObject.AddComponent(type) as QuestItemDropper;
                    component.ItemToDrop = DropperTemplate.ItemToDrop;
                    component.DropPositionOffset = DropperTemplate.DropPositionOffset;

                    //Special case for Creature deaths
                    if (component is QuestItemDropOnCreatureDeath)
                    {
                        var creatureDeathDropper = component as QuestItemDropOnCreatureDeath;
                        var creature = currentObject.GetComponent<Creature>();

                        if (creature != null)
                        {
                            creatureDeathDropper.SetTarget(creature);
                        }
                    }
                }
            }
            yield return null; //TODO: Maybe not hold off until next frame?
        }

        private void SetReady(List<GameObject> gameObjects)
        {
            ObjectSpawner.OnFinishedSpawning -= SetReady;

            objectsToAttachTo = gameObjects;
            readyToAttach = true;
        }
    }
}

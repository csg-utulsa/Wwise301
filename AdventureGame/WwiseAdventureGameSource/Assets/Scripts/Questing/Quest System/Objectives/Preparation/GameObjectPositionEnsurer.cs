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
    public class GameObjectPositionEnsurer : QuestCompleteAction
    {
        public GameObjectMover mover;
        public float DistanceThreshold = 5f;

        #region private variables
        private Vector3 origPosition;
        #endregion

        public override void Execute()
        {
            if(Vector3.Distance(mover.GameObjectToMove.transform.position, mover.MovePosition.position) > DistanceThreshold)
            {
                mover.Move();
            }
        }

        public override void Reverse()
        {
            mover.MoveBack();
        }
    }
}
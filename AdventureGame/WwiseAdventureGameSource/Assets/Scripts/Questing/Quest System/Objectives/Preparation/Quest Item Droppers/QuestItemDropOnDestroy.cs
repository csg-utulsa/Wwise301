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
    public class QuestItemDropOnDestroy : QuestItemDropper
    {
        #region private variables
        private bool enableDrop = true;
		#endregion

		private void OnApplicationQuit()
		{
			enableDrop = false;
		}

		private void OnDisable()
        {
            if (enableDrop)
            {
				DropItem();
            }
        }

        public override void PerformDropperReverse()
        {
            enableDrop = false;
            base.PerformDropperReverse();
        }
    }
}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public delegate void PlayerEvent();
public delegate void QuestEvent(int ID);

//TODO: This seems to actually only be something that the spell system uses. Refactor into the appropriate classes and delete.
public class EventManager : MonoBehaviour
{
    public static event PlayerEvent OnSpellReady;
    public static event PlayerEvent OnSpellNotReady;

    public static void SpellReady()
    {
        if (OnSpellReady != null)
        {
            OnSpellReady();
        }
    }

    public static void SpellNotReady()
    {
        if (OnSpellNotReady != null)
        {
            OnSpellNotReady();
        }
    }
}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public delegate void DialogueSetEvent();
public class DialogueSet : MonoBehaviour
{
    public event DialogueSetEvent OnPrimaryDialogueStarted;
    public event DialogueSetEvent OnPrimaryDialogueEnded;
    public event DialogueSetEvent OnDialogueEnded;

    public bool AutoStart = false;
    public bool FreezePlayerInDialogue = true;

    public List<DialogueLine> PrimaryDialogue = new List<DialogueLine>();
    public List<DialogueLine> SecondaryDialogue = new List<DialogueLine>();

    #region private variables 
    private bool primaryDialogueSeen = false;
    #endregion

    public void PushDialogue(GameObject sender)
    {
        if (!primaryDialogueSeen)
        {
            DialogueManager.Instance.TransferDialogue(PrimaryDialogue, sender);
            primaryDialogueSeen = true;

            DialogueManager.OnDialogueEnd += PrimaryDialogueFinished;

            if (OnPrimaryDialogueStarted != null)
            {
                OnPrimaryDialogueStarted();
            }
        }
        else
        {
            DialogueManager.Instance.TransferDialogue(SecondaryDialogue, gameObject);
        }

        DialogueManager.OnDialogueEnd += DialogueFinished;
    }

    private void PrimaryDialogueFinished(int id)
    {
        DialogueManager.OnDialogueEnd -= PrimaryDialogueFinished;

        if (OnPrimaryDialogueEnded != null)
        {
            OnPrimaryDialogueEnded();
        }
    }

    private void DialogueFinished(int id)
    {
        DialogueManager.OnDialogueEnd -= DialogueFinished;

        if (OnDialogueEnded != null)
        {
            OnDialogueEnded();
        }
    }

    public void Reset()
    {
        primaryDialogueSeen = false;
    }

    public bool HasPrimary()
    {
        return PrimaryDialogue.Count > 0;
    }

    public bool HasSecondary()
    {
        return SecondaryDialogue.Count > 0;
    }

    public bool PrimaryDialogueSeen()
    {
        return primaryDialogueSeen;
    }

    public DialoguePusher origDialogue;
    [ContextMenu("EXECUTE")]
    private void UpdateDialogueSet()
    {
        PrimaryDialogue = origDialogue.Dialogue;
    }
}

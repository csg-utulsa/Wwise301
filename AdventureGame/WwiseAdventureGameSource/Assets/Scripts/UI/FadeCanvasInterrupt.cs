////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class FadeCanvasInterrupt : MonoBehaviour
{
    #region private variables
    private Animator Anim;
    private bool canInterrupt = true;
    private readonly int fadeFastHash = Animator.StringToHash("FadeFast");
    #endregion

    void Start()
    {
        Anim = GetComponent<Animator>();
        InputManager.OnMenuDown += InterruptFromMenuDown;
        DialogueManager.OnDialogueEnd += InterruptFromEndOfDialogue;
    }

    void InterruptFromEndOfDialogue(int id)
    {
        InterruptAndRemove();
    }

    void InterruptFromMenuDown()
    {
        InterruptAndRemove();
    }

    void InterruptAndRemove()
    {
        if (canInterrupt)
        {
            Anim.SetTrigger(fadeFastHash);
            canInterrupt = false;
        }
    }

    void OnDestroy()
    {
        InputManager.OnMenuDown -= InterruptFromMenuDown;
        DialogueManager.OnDialogueEnd -= InterruptFromEndOfDialogue;
    }
}

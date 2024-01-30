////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;

public class InteractableNPC : MonoBehaviour, IInteractable
{
    public Animator QuestMarkAnimator;

    #region private variables
    private DialogueSet Dialogue;

    private QuestGiver questGiver;
    private Creature creature;
    protected bool playerIsInteracting = false;
    private bool pushedDialogue = false;

    //Guide visuals
    private bool showingQuestMark = false;
    private readonly int questMarkActiveHash = Animator.StringToHash("Active");

    private IEnumerator awarenessRoutine;

    private bool hasSomethingToSay;
    private Pickup pickupScript;
    #endregion

    private void OnEnable()
    {
        questGiver = GetComponent<QuestGiver>();
        creature = GetComponent<Creature>();
        pickupScript = GetComponent<Pickup>();

        awarenessRoutine = NPCAwareness();
        StartCoroutine(awarenessRoutine);
    }

    private void OnDisable()
    {
        if(awarenessRoutine != null)
        {
            StopCoroutine(awarenessRoutine);
        }
    }

    private IEnumerator NPCAwareness()
    {
        while (true)
        {
            hasSomethingToSay = Dialogue == null ? false : pickupScript.InteractionEnabled;
            
            if (hasSomethingToSay && !showingQuestMark)
            {
                SetQuestMarkActive(true);
            }
            else if(!hasSomethingToSay && showingQuestMark)
            {
                SetQuestMarkActive(false);
            }
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }

    private void SetQuestMarkActive(bool active)
    {
        showingQuestMark = active;
        QuestMarkAnimator.SetBool(questMarkActiveHash, active);
    }

    public void OnInteract()
    {
        if (!playerIsInteracting)
        {
            EnterInteraction();
        }
    }

    protected void EnterInteraction()
    {
        playerIsInteracting = true;
        StartCoroutine(InteractionEnter());
    }

    private IEnumerator InteractionEnter()
    {
        yield return new WaitForSecondsRealtime(0.2f); // Wait a bit to make time for any potential quest change

        if (questGiver != null)
        {
            yield return new WaitUntil(() => !questGiver.IsInitializingQuest());
        }

        if (Dialogue != null)
        {
            if (!Dialogue.PrimaryDialogueSeen() || Dialogue.HasSecondary())
            {
                TransferDialogue(Dialogue, gameObject);
                if (creature != null)
                {
                    creature.TalkMode(true);
                }
            }
            else
            {
                ExitInteraction();
            }
        }
        else
        {
            ExitInteraction();
        }

    }

    protected void ExitInteraction()
    {
        playerIsInteracting = false;
        SetPlayerRestriction(false);

        pickupScript.SetInteractionEnabled(false);

        if (pushedDialogue)
        {
            Dialogue.OnDialogueEnded -= ExitInteraction;
            if (creature != null)
            {
                pushedDialogue = false;
            }
        }

        if (creature != null)
        {
            creature.TalkMode(false);
        }
    }

    private void SetPlayerRestriction(bool restrict)
    {
        if (restrict)
        {
            PlayerManager.Instance.PauseMovement(this.gameObject);
            PlayerManager.Instance.PauseAttacking(this.gameObject);
        }
        else
        {
            PlayerManager.Instance.ResumeMovement(this.gameObject);
            PlayerManager.Instance.ResumeAttacking(this.gameObject);
        }
    }

    private void TransferDialogue(DialogueSet dialogue, GameObject sender)
    {
        if (Dialogue != null)
        {
            if (Dialogue.HasPrimary() || dialogue.HasSecondary())
            {
                Dialogue.OnDialogueEnded += ExitInteraction;

                if (Dialogue.FreezePlayerInDialogue)
                {
                    SetPlayerRestriction(true);
                }

                Dialogue.PushDialogue(sender);
                pushedDialogue = true;
            }
        }
    }

    public DialogueSet GetDialogue()
    {
        return Dialogue;
    }

    public void SetDialogue(DialogueSet dialogue)
    {
        if (dialogue != null)
        {
            pickupScript.SetInteractionEnabled(true);

            Dialogue = dialogue;

            if (dialogue.AutoStart)
            {
                EnterInteraction();
                //dialogue.PushDialogue(gameObject);
            }
        }
    }

    public void SetFollow()
    {
        if(creature != null)
        {
            creature.isFollowing = true;
        }
    }

    public void SetUnfollow()
    {
        if (creature != null)
        {
            creature.isFollowing = false;
        }
    }
}

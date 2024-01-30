////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public delegate void DialogueEvent(int questGiverInstanceID);
public class DialogueManager : Singleton<DialogueManager>
{
    protected DialogueManager() { }

    public static event DialogueEvent OnDialogueEnd;
    public static event DialogueEvent OnDialogueBegin;

    public static bool DialogueActive = false;

    public List<DialogueLine> Dialogue = new List<DialogueLine>();

    [Range(0f, 1f)]
    public float BarVisibility = 0f;
    public bool isVisible = false;
    IEnumerator Lerper;

    [Header("Object Links")]
    public Text dialogueText;
    public Image dialogueBorder;
    public Text SpeakerText;
    public Animator continueIconAnimator;

    #region private variables
    private bool NextMessage = false;
    private bool continueIconVisible = false;
    private bool FinishedCameraEvent = false;
    private bool dialogueOn = false;
    private readonly int showHash = Animator.StringToHash("Show");

    private DialogueLine currentDialogueLine;
    #endregion

    void Start()
    {
        dialogueText.gameObject.SetActive(true);
        dialogueBorder.gameObject.SetActive(true);
        continueIconAnimator.gameObject.SetActive(true);

        StartCoroutine(DialogueControl());
    }

    IEnumerator DialogueControl()
    {
        StartCoroutine(DialogueWindow());
        LanguageManager.OnLanguageChange += DisplayDialogue;
        dialogueText.text = "";

        dialogueBorder.fillAmount = BarVisibility;

        QuestManager.DialogueReady = true;

        while (true)
        {
            if (Dialogue.Count > 0)
            {
                DialogueActive = true;

                bool ConOn = false;
                if (Dialogue[0].unSkippable)
                {

                }
                else
                {
                    InputManager.OnUseDown += OnContinue;
                    ConOn = true;
                }

                InteractionManager.inConversation = true;

                OnDialogueBegin(Dialogue[0].DialogueID.GetInstanceID());

                if (Dialogue.Count > 0 && Dialogue[0].Action.Count > 0)
                {
                    for (int d = 0; d < Dialogue[0].Action.Count; d++)
                    {
                        ExecuteActionOnDialogue(Dialogue[0], d);
                    }
                }

                if (!isVisible)
                {
                    yield return new WaitUntil(() => isVisible);
                }

                if (Dialogue.Count > 0)
                {
                    yield return new WaitForSeconds(Dialogue[0].Delay);

                    DisplayDialogue();
                    Dialogue[0].DialogueEvent.Post(Dialogue[0].DialogueID);
                }

                if (Dialogue.Count > 0 && !Dialogue[0].autoSkip)
                {
                    yield return new WaitUntil(() => NextMessage);
                }
                else if (Dialogue.Count > 0 && Dialogue[0].autoSkip)
                {
                    bool hasCameraEvent = false;
                    for (int d = 0; d < Dialogue[0].Action.Count; d++)
                    {
                        if (Dialogue[0].Action[d].DialogueActionType == DialogueActionType.CameraChange)
                        {
                            hasCameraEvent = true;
                        }
                    }

                    if (Dialogue[0].Action.Count > 0 && hasCameraEvent)
                    {
                        FinishedCameraEvent = false;
                        PlayerCamera.OnCameraEventEnd += OnCameraEventBeingFinished;
                        while (!FinishedCameraEvent && !NextMessage)
                        {
                            yield return null;
                        }
                        PlayerCamera.OnCameraEventEnd -= OnCameraEventBeingFinished;
                    }
                    else
                    {
                        float timeLeft = Dialogue[0].skipTime;
                        for (float t = 0; t < 1 && !NextMessage; t += Time.deltaTime / timeLeft)
                        {
                            yield return null;
                        }
                    }
                }

                int nexID = 0;
                if (Dialogue.Count > 0 && Dialogue.Count > 1)
                {
                    nexID = Dialogue[1].DialogueID.GetInstanceID();
                }

                if (Dialogue.Count > 0)
                {
                    Dialogue[0].DialogueEvent.Stop(Dialogue[0].DialogueID, 0, AkCurveInterpolation.AkCurveInterpolation_Linear);
                }

                if (Dialogue.Count > 0)
                {
                    if (Dialogue[0].DialogueID.GetInstanceID() != nexID)
                    {
                        OnDialogueEnd(Dialogue[0].DialogueID.GetInstanceID());
                        Dialogue[0].OnExitDialogue.Invoke();
                    }

                    currentDialogueLine = null;
                    Dialogue.RemoveAt(0);
                }

                NextMessage = false;
                InteractionManager.inConversation = false;
                if (ConOn)
                {
                    InputManager.OnUseDown -= OnContinue;
                }

                dialogueText.text = "";
            }
            else
            {
                DialogueActive = false;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    void DisplayDialogue()
    {
        if (Dialogue.Count > 0 && Dialogue[0] != null)
        {
            currentDialogueLine = Dialogue[0];
			Dialogue[0].OnEnterDialogue.Invoke();
            string Dia = InputManager.ReplaceControllerStrings(LanguageManager.GetText(Dialogue[0].DialogueKey));
            dialogueText.text = Dia;
        }
    }

    void ExecuteActionOnDialogue(DialogueLine dialogueSegment, int actionNum)
    {
        DialogueAction currentAction = dialogueSegment.Action[actionNum];

        switch (currentAction.DialogueActionType)
        {

            case DialogueActionType.SetActive:
                currentAction.Item.SetActive(true);
                break;
            case DialogueActionType.SetInactive:
                currentAction.Item.SetActive(false);
                break;
            case DialogueActionType.CameraChange:
                PlayerCamera.CameraEvent C = new PlayerCamera.CameraEvent(currentAction.Item.GetComponent<Camera>(), dialogueSegment.transitionTime, dialogueSegment.skipTime, dialogueSegment.returnCam2Player);
                C.unSkippable = dialogueSegment.unSkippable;
                PlayerManager.Instance.cameraScript.ChangeCamera(C);
                break;
            case DialogueActionType.PlaySound: //TODO: Remove from enum (means breaking all the other DialogueActions) 
                //No action
                break;
            case DialogueActionType.PausePlayer:
                PlayerManager.Instance.PauseMovement(this.gameObject);
                PlayerManager.Instance.PauseAttacking(this.gameObject);
                break;
            case DialogueActionType.ResumePlayer:
                PlayerManager.Instance.ResumeMovement(this.gameObject);
                PlayerManager.Instance.ResumeAttacking(this.gameObject);
                break;
            case DialogueActionType.SetPlayerPosition:
                PlayerManager.Instance.player.transform.position = currentAction.Item.transform.position;
                break;
            case DialogueActionType.Wwizard_ChangeAnimation:
                //QuestManager.MainQuestGiverReference.GetComponent<WwizardAI>().TriggerAnimation_Charge();
                break;
            case DialogueActionType.Wwizard_CompleteChargeAnimation:
                //QuestManager.MainQuestGiverReference.GetComponent<WwizardAI>().TriggerAnimation_ChargeEnd();
                break;
            case DialogueActionType.SendDialoguePushMessage:

                break;
            case DialogueActionType.SetCameraInstantly:
                Vector3 pos = currentAction.Item.transform.position;
                Quaternion rot = currentAction.Item.transform.rotation;
                float fov = currentAction.Item.GetComponent<Camera>().fieldOfView;
                PlayerManager.Instance.cameraScript.SetCameraInstantly(pos, rot, fov);

                break;
            case DialogueActionType.FadeToBlack:
                StartCoroutine(FadeToBlackAfterSeconds(dialogueSegment));
                break;
            case DialogueActionType.FadeFromBlack:
                UI_Overlayers.Instance.FadeLayer(false, dialogueSegment.transitionTime);
                break;
            case DialogueActionType.LockCamera:
                PlayerManager.Instance.cameraScript.FreezeAndShowCursor(true, gameObject);
                PlayerManager.Instance.cameraScript.cameraMode = PlayerCamera.CameraMode.lockCameraCompletely;
                break;
        }
    }

    IEnumerator FadeToBlackAfterSeconds(DialogueLine l)
    {
        float waitTime = l.skipTime - l.transitionTime;
        for (float t = 0; t < 1 && !NextMessage; t += Time.deltaTime / waitTime)
        {
            yield return null;
        }
        UI_Overlayers.Instance.FadeLayer(true, l.transitionTime);
    }

    IEnumerator DialogueWindow()
    {
        while (true)
        {
            // DIALOGUE SHOW
            if (Dialogue.Count > 0)
            {
                if (!dialogueOn)
                {
                    dialogueText.enabled = true;
                    dialogueOn = true;

                    // Instantiate new lerp
                    if (Lerper != null)
                    {
                        StopCoroutine(Lerper);
                    }
                    Lerper = LerpTo(1.0f);
                    StartCoroutine(Lerper);
                }

                if (Dialogue[0].unSkippable)
                {
                    ClearContinueButton();
                }
                else
                {
                    ShowContinueButton();
                }

            }
            else
            {
                if (dialogueOn)
                {
                    dialogueText.enabled = false;
                    //dialogueBorder.enabled = false;
                    dialogueOn = false;
                    InteractionManager.BreakConversation();

                    // Instantiate new lerp
                    if (Lerper != null)
                    {
                        StopCoroutine(Lerper);
                    }
                    Lerper = LerpTo(0.0f);
                    StartCoroutine(Lerper);
                }
                ClearContinueButton();
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public void ShowContinueButton()
    {
        if (!continueIconVisible)
        {
            continueIconAnimator.SetBool(showHash, true);
            continueIconVisible = true;
        }
    }

    public void ClearContinueButton()
    {
        if (continueIconVisible)
        {
            continueIconAnimator.SetBool(showHash, false);
            continueIconVisible = false;
        }
    }

    IEnumerator LerpTo(float targetVisibility)
    {
        float seconds = 0.2f;
        float currentVisibility = dialogueBorder.fillAmount;
        dialogueBorder.gameObject.SetActive(true);

        for (float t = 0; t < 1; t += Time.unscaledDeltaTime / seconds)
        {
            dialogueBorder.fillAmount = Mathf.Lerp(currentVisibility, targetVisibility, t);
            yield return null;
        }
        dialogueBorder.fillAmount = targetVisibility;

        if (targetVisibility == 0f)
        {
            dialogueBorder.gameObject.SetActive(false);
        }

        isVisible = dialogueBorder.gameObject.activeInHierarchy;
    }

    public void TransferDialogue(List<DialogueLine> receivedDialogue, GameObject senderID)
    {
        if (receivedDialogue.Count > 0)
        {
            for (int i = 0; i < receivedDialogue.Count; i++)
            {
                receivedDialogue[i].DialogueID = senderID;
                Dialogue.Add(receivedDialogue[i]);
            }
        }
    }

    void OnContinue()
    {
        NextMessage = true;
    }

    public void ClearAllDialogues()
    {
        if (Dialogue.Count > 0)
        {
            if(currentDialogueLine != null)
            {
                currentDialogueLine.OnExitDialogue.Invoke();
                currentDialogueLine.DialogueEvent.Stop(currentDialogueLine.DialogueID);
            }
            Dialogue.Clear();

            if(OnDialogueEnd != null)
            {
                OnDialogueEnd(0);
            }
        }
    }

    void OnCameraEventBeingFinished()
    {
        FinishedCameraEvent = true;
    }
}

[System.Serializable]
public class DialogueLine
{
    public string DialogueKey;
    public AK.Wwise.Event DialogueEvent = new AK.Wwise.Event();
    public float Delay = 0f;
    public List<DialogueAction> Action;
    [HideInInspector]
    public GameObject DialogueID;
    public bool unSkippable = false;
    public bool autoSkip;
    public float transitionTime;
    public float skipTime;
    public bool returnCam2Player = true;
    public UnityEngine.Events.UnityEvent OnEnterDialogue;
    public UnityEngine.Events.UnityEvent OnExitDialogue;
}

[System.Serializable]
public class DialogueAction
{
    public GameObject Item;
    public DialogueActionType DialogueActionType;
}

public enum DialogueActionType
{
    SetActive,
    SetInactive,
    SetFollow,
    SetUnfollow,
    CameraChange,
    PlaySound,
    PausePlayer,
    SetPlayerPosition,
    Wwizard_ChangeAnimation,
    Wwizard_CompleteChargeAnimation,
    ResumePlayer,
    SendDialoguePushMessage,
    SetCameraInstantly,
    FadeToBlack,
    FadeFromBlack,
    LockCamera
};
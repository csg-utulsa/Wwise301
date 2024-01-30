////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameSpeedEvent(GameSpeedCommand gsc);
public class GameSpeedHandler : MonoBehaviour
{
    public GameSpeedEvent OnSpeedTransitionDone;

    #region private variables
    private List<GameSpeedCommand> gameSpeedModifiers = new List<GameSpeedCommand>();
    private float currentGameSpeed;
    private IEnumerator gameSpeedRoutine;
    #endregion

    public void SetGameSpeed(int callerID,
                             float multiplier,
                             float inTransitionTime = 0.1f,
                             float outTransitionTime = 0.3f,
                             float holdTime = 0.3f,
                             AnimationCurve inCurve = default(AnimationCurve),
                             AnimationCurve outCurve = default(AnimationCurve)
                            )
    {
        GameSpeedCommand newGameSpeed = new GameSpeedCommand()
        {
            instanceID = callerID,
            NewGameSpeed = Mathf.Clamp01(multiplier),
            InTransitionTime = inTransitionTime,
            OutTransitionTime = outTransitionTime,
            HoldTime = holdTime,
            InCurve = inCurve,
            OutCurve = outCurve
        };
        SetGameSpeed(newGameSpeed);
    }

    public void SetGameSpeed(GameSpeedCommand newGameSpeed)
    {
        StopGameSpeedTransition();

        if (!gameSpeedModifiers.Contains(newGameSpeed))
        {
            gameSpeedModifiers.Add(newGameSpeed);
        }

        gameSpeedRoutine = GameSpeedTransition(newGameSpeed);
        StartCoroutine(gameSpeedRoutine);

        OnSpeedTransitionDone += DisposeGameSpeedCommand;
    }

    public void ReleaseGameSpeed(int callerID)
    {
        StopGameSpeedTransition();
        var toRemove = gameSpeedModifiers.Find(x => x.instanceID == callerID);
        gameSpeedModifiers.Remove(toRemove);

        if (gameSpeedModifiers.Count > 0)
        {
            SetGameSpeed(gameSpeedModifiers[gameSpeedModifiers.Count-1]);
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void PauseGameSpeed(int callerID)
    {
        SetGameSpeed(callerID, 0.01f, 0.1f, 0.5f, Mathf.Infinity);
    }

    public void UnPauseGameSpeed(int callerID)
    {
        ReleaseGameSpeed(callerID);
    }

    private void DisposeGameSpeedCommand(GameSpeedCommand gsc)
    {
        ReleaseGameSpeed(gsc.instanceID);
    }

    private IEnumerator GameSpeedTransition(GameSpeedCommand gsc)
    {
        currentGameSpeed = Time.timeScale;

        if (gsc.InCurve == null)
        {
            gsc.InCurve = Curves.Instance.SmoothOut;
        }
        if (gsc.OutCurve == null)
        {
            gsc.OutCurve = Curves.Instance.SmoothOut;
        }

        // Transition in
        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / gsc.InTransitionTime)
        {
            float value = gsc.InCurve.Evaluate(t);
            Time.timeScale = Mathf.Lerp(currentGameSpeed, GetTargetGameSpeed(), value);
            yield return null;
        }
        Time.timeScale = GetTargetGameSpeed();
		currentGameSpeed = Time.timeScale;

        // Hold
        for (float h = 0f; h < 1f; h += Time.unscaledDeltaTime / gsc.HoldTime)
        {
            yield return null;
        }

        // Transition out
        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / gsc.OutTransitionTime)
        {
            float value = gsc.InCurve.Evaluate(t);
            Time.timeScale = Mathf.Lerp(currentGameSpeed, GetTargetGameSpeed(), value);
            yield return null;
        }
        Time.timeScale = GetTargetGameSpeed();
        currentGameSpeed = Time.timeScale;

        if (OnSpeedTransitionDone != null)
        {
            OnSpeedTransitionDone(gsc);
        }

    }

    private void StopGameSpeedTransition()
    {
        if (gameSpeedRoutine != null)
        {
            StopCoroutine(gameSpeedRoutine);
        }
    }

    private float GetTargetGameSpeed()
    {
        float modifiersResult = 1f;
        for (int i = 0; i < gameSpeedModifiers.Count; i++)
        {
            modifiersResult *= gameSpeedModifiers[i].NewGameSpeed;
            modifiersResult = Mathf.Clamp(modifiersResult, 0.01f, 1f);
        }

        return modifiersResult;
    }

}

[System.Serializable]
public class GameSpeedCommand
{
    public int instanceID;

    public float NewGameSpeed;
    public float InTransitionTime;
    public float OutTransitionTime;
    public AnimationCurve InCurve;
    public AnimationCurve OutCurve;
    public float HoldTime;
}

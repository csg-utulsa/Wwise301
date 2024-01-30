////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

public class ExitGameOnClick : MonoBehaviour
{
    /// <summary>
    /// Useful for the exit button OnClicked UnityEvent in the UI.
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif

    }
}

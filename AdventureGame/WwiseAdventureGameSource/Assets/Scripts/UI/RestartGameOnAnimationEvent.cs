////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGameOnAnimationEvent : MonoBehaviour
{
    /// <summary>
    /// Hooked up to an Animation Event
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("Title Screen");
    }
}

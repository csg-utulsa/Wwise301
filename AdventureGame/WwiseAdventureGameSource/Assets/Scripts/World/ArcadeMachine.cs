////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;

public class ArcadeMachine : MonoBehaviour, IInteractable
{
    public bool isPlayingMusic = false;
    public ParticleSystem NoteParticles;

    public AK.Wwise.Event ArcadeMusicStart;
    public AK.Wwise.Event ArcadeMusicStop;

    public GameObject SoundPosition; 

    public void Start()
    {
        if (isPlayingMusic) {
            Play();
        }

        if (SoundPosition == null) {
            SoundPosition = gameObject;
        }
    }

    public void Play() {
        isPlayingMusic = true;
        NoteParticles.Play();
        if (SoundPosition != null)
        {
            ArcadeMusicStart.Post(SoundPosition);
        }
    }

    public void Stop()
    {
        isPlayingMusic = false;
        NoteParticles.Stop();
        if (SoundPosition != null)
        {
            ArcadeMusicStop.Post(SoundPosition);
        }
    }

    public void OnInteract()
    {
        if (!isPlayingMusic)
        {
            Play();
        }
        else
        {
            Stop();
        }

    }

    public void OnDestroy()
    {
        if (SoundPosition != null && isPlayingMusic)
        {
            ArcadeMusicStop.Post(SoundPosition);
        }
    }
}

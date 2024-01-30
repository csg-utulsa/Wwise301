////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LoadSceneAsyncOnEnter))]
[CanEditMultipleObjects]
public class LoadSceneAsyncOnEnterInspector : Editor
{
    private bool isValid;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LoadSceneAsyncOnEnter sceneLoader = (LoadSceneAsyncOnEnter)target;

        isValid = ValidateScene(sceneLoader.sceneToLoad);

        if (sceneLoader.sceneToLoad != null && sceneLoader.sceneToLoad != "")
        {
            if (!isValid)
            {
                EditorGUILayout.HelpBox("It seems that such a scene does not exist. \n" +
                "If it exists, make sure to add it to the scenes in the build settings!",
                    MessageType.Error);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("WARNING: No Scene name!", MessageType.Error);
        }
    }

    bool ValidateScene(string sceneName)
    {
        //This is ridiculous, but it's the only way of getting the scenes in the build settings ...
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        string[] sceneNames = EditorBuildSettingsScene.GetActiveSceneList(scenes);

        for (int i = 0; i < sceneNames.Length; i++)
        {
            SceneAsset SA = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneNames[i]);
            if (SA != null) {
                string s = SA.name;
                if (sceneName == s)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
#endif

public class LoadSceneAsyncOnEnter : MonoBehaviour
{
    public string TriggerObjectTag = "MainCamera";
    public UnityEvent OnSceneLoadStarted;
    public UnityEvent OnSceneLoaded;
    public UnityEvent OnSceneUnloadedWaitStarted;
    public UnityEvent OnSceneUnloaded;

	public float waitTimeBeforeUnload = 3f;
    public string sceneToLoad;

    #region private variables
    private bool sceneIsLoaded = false;
    private bool sceneIsLoading = false;
    private bool playerIsInside = false;

    private IEnumerator loadRoutine;
    private IEnumerator unloadRoutine;
    private IEnumerator unloadWaiterRoutine;
    #endregion

    void OnTriggerEnter(Collider col)
    {
        if (this.enabled && col.gameObject.CompareTag(TriggerObjectTag))
        {
            playerIsInside = true;

            LoadScene();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (this.enabled && col.gameObject.CompareTag(TriggerObjectTag))
        {
            playerIsInside = false;

            UnloadScene();
        }
    }

    public bool SetSceneToLoad(string Name) {
        if (!sceneIsLoaded && !sceneIsLoading)
        {
            print(sceneToLoad + " loads no longer on entry. " + Name + " will be loaded instead.");
            sceneToLoad = Name;
            return true;
        }
        else {
            print("Cannot change name of scene to load, as it is not unloaded yet.");
            return false;
        }
    }

    private void LoadScene()
    {
        if (!sceneIsLoaded && !sceneIsLoading)
        {
            OnSceneLoadStarted.Invoke();

            loadRoutine = LoadSceneAsync(sceneToLoad);
            StartCoroutine(loadRoutine);
        }
    }

    private void UnloadScene()
    {
        if (sceneIsLoaded)
        {
            OnSceneUnloadedWaitStarted.Invoke();

            if (unloadWaiterRoutine != null)
            {
                StopCoroutine(unloadWaiterRoutine);
            }
            unloadWaiterRoutine = WaitAndUnloadScene(sceneToLoad);
            StartCoroutine(unloadWaiterRoutine);
        }
    }

    private void UnloadSceneImmediately()
    {
        if (sceneIsLoaded)
        {
            if (unloadWaiterRoutine != null)
            {
                StopCoroutine(unloadWaiterRoutine);
            }
            SceneManager.UnloadSceneAsync(sceneToLoad);
            sceneIsLoaded = false;
            OnSceneUnloaded.Invoke();
        }
    }

    private void OnDisable()
    {
        UnloadSceneImmediately();
    }

    private void OnDestroy()
    {
        UnloadSceneImmediately();
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            sceneIsLoading = true;
            yield return null;
        }
        sceneIsLoading = false;
        sceneIsLoaded = true;

        yield return null;

        OnSceneLoaded.Invoke();
    }

    IEnumerator UnloadSceneAsync(string sceneName)
    {
        yield return null;

        AsyncOperation async = SceneManager.UnloadSceneAsync(sceneName);

        while (!async.isDone)
        {
            yield return null;
        }

        sceneIsLoaded = false;

        yield return null;

        OnSceneUnloaded.Invoke();
    }

    IEnumerator WaitAndUnloadScene(string sceneName)
    {
        yield return new WaitForSecondsRealtime(waitTimeBeforeUnload);

        if (!playerIsInside)
        {
            unloadRoutine = UnloadSceneAsync(sceneName);
            StartCoroutine(unloadRoutine);
        }
    }
}

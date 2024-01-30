////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(LoadScene))]
public class LoadSceneInspector : Editor
{
    private bool isValid;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LoadScene sceneLoader = (LoadScene)target;

        isValid = ValidateScene(sceneLoader.sceneName);

        if (sceneLoader.sceneName != null && sceneLoader.sceneName != "")
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

        if (GUILayout.Button("Validate Scene"))
        {
            ValidateScene(sceneLoader.sceneName);
        }
    }

    bool ValidateScene(string sceneName)
    {
        //This is ridiculous, but it's the only way of getting the scenes in the build settings ...
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        string[] sceneNames = EditorBuildSettingsScene.GetActiveSceneList(scenes);

        for (int i = 0; i < sceneNames.Length; i++)
        {
            string s = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneNames[i]).name;
            if (sceneName == s)
            {
                return true;
            }
        }
        return false;
    }
}
#endif

public class LoadScene : MonoBehaviour
{
	public bool async = true;
	public bool additive = true;
    public string sceneName;

    /// <summary>
    /// Useful for hooking up to UnitEvents in the editor
    /// </summary>
	public void LoadSceneNow()
    {
        if (async)
        {
            SceneManager.LoadSceneAsync(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(sceneName, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        }
    }

    public void UnloadSceneNow()
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}

////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(DLC))]
public class DLCInspector : Editor
{
    private bool isValid;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DLC dlc = (DLC)target;

        isValid = ValidateScene(dlc.sceneName);

        if (dlc.sceneName != null && dlc.sceneName != "")
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
            ValidateScene(dlc.sceneName);
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

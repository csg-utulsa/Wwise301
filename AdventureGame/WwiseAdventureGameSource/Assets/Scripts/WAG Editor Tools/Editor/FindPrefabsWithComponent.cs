////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class FindPrefabsWithComponent : EditorWindow
{

    private MonoScript component;
    private bool manualComponentSelection;

    string[] projectResults;
    List<Object> objRefs;
    string[] scenesResults;

    bool hasCheckedForReferences = false;
    string lastName;

    [MenuItem("Audiokinetic/Utilities/Development Utilities/Find Prefabs With Component")]
    public static void Init()
    {
        FindPrefabsWithComponent window = (FindPrefabsWithComponent)EditorWindow.GetWindow(typeof(FindPrefabsWithComponent));
        window.Show();
    }

    void OnGUI()
    {
        minSize = new Vector2(200, 50);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Component: ");
        component = (MonoScript)EditorGUILayout.ObjectField(component, typeof(MonoScript), false);

        projectResults = null;
        if (manualComponentSelection)
        {
            if (GUILayout.Button((component == null ? "N/A" : "Find Prefabs")))
            {
                projectResults = FindPrefabsWith(component);
                scenesResults = FindSceneObjectsWith(component);
            }
        }
        else
        {
            Object obj = Selection.activeObject;
            if (obj != null && obj.GetType() == typeof(MonoScript) && obj.GetType() != typeof(EditorWindow))
            {
                component = obj as MonoScript;
                Repaint();
                if (lastName != obj.name)
                {
                    hasCheckedForReferences = false;
                }

                lastName = obj.name;
            }

            if (component != null && !hasCheckedForReferences)
            {
                projectResults = FindPrefabsWith(component);
                scenesResults = FindSceneObjectsWith(component);
                hasCheckedForReferences = true;
            }
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        manualComponentSelection = EditorGUILayout.ToggleLeft("Manual Component Selection", manualComponentSelection);

        EditorGUILayout.EndHorizontal();

        GUILayout.Box("In Project: ");
        if (projectResults != null)
        {
            for (int i = 0; i < projectResults.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(">", GUILayout.MaxWidth(18f)))
                {
                    EditorGUIUtility.PingObject(objRefs[i]);
                }
                EditorGUILayout.LabelField(projectResults[i]);

                EditorGUILayout.EndHorizontal();
            }
        }

        GUILayout.Box("In Open Scenes: ");
        if (scenesResults != null)
        {
            for (int i = 0; i < scenesResults.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(scenesResults[i]);

                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndVertical();

    }

    string[] FindPrefabsWith(MonoScript component)
    {
        string[] allAssets = AssetDatabase.GetAllAssetPaths();

        System.Type componentType = component.GetClass();

        List<string> searchResult = new List<string>();
        objRefs = new List<Object>();

        for (int i = 0; i < allAssets.Length; i++)
        {
            //Project view
            string currentPath = allAssets[i];
            if (currentPath.StartsWith("Assets")) //Remove annoying warning in editor for incorrect paths
            {
                System.Type assetType = AssetDatabase.GetMainAssetTypeAtPath(currentPath);

                if (assetType == typeof(GameObject))
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(currentPath);

                    var scripts = prefab.GetComponentsInChildren(componentType);
                    if (scripts.Length > 0)
                    {
                        for (int j = 0; j < scripts.Length; j++)
                        {
                            GameObject currentObject = scripts[j].gameObject;
                            string r = string.Format("'{0}' {1} has '{2}' attached, at path '{3}'",
                                                     currentObject.name,
                                                     (currentObject.GetInstanceID() == prefab.GetInstanceID() ? "" : " (parent: " + prefab.name + ")"),
                                                     component.GetClass().ToString(),
                                                     currentPath);
                            searchResult.Add(r);
                            objRefs.Add(currentObject);
                        }
                    }
                }
            }
        }
        return searchResult.ToArray();
    }

    string[] FindSceneObjectsWith(MonoScript component)
    {
        string[] allAssets = AssetDatabase.GetAllAssetPaths();

        System.Type componentType = component.GetClass();

        List<string> searchResult = new List<string>();
        objRefs = new List<Object>();

        for (int i = 0; i < allAssets.Length; i++)
        {
            string currentPath = allAssets[i];
            if (currentPath.StartsWith("Assets")) //Remove annoying warning in editor for incorrect paths
            {
                System.Type assetType = AssetDatabase.GetMainAssetTypeAtPath(currentPath);

                // In open Scenes
                if (assetType == typeof(SceneAsset))
                {
                    SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(currentPath);
                    Scene s = SceneManager.GetSceneByName(scene.name);
                    if (s.IsValid())
                    {
                        GameObject[] rootObjects = s.GetRootGameObjects();

                        for (int j = 0; j < rootObjects.Length; j++)
                        {
                            GameObject currentObject = rootObjects[j];

                            var scripts = currentObject.GetComponentsInChildren(componentType, true);
                            if (scripts.Length > 0)
                            {
                                for (int k = 0; k < scripts.Length; k++)
                                {
                                    GameObject currentSubObject = scripts[k].gameObject;
                                    string r = string.Format("'{0}' {1} has '{2}' attached, in '{3}'",
                                                             currentSubObject.name,
                                                             (currentSubObject.GetInstanceID() == currentObject.GetInstanceID() ? "" : " (parent: " + currentObject.name + ")"),
                                                             component.GetClass().ToString(),
                                                             s.name);
                                    searchResult.Add(r);
                                }
                            }
                        }
                    }
                }
            }
        }
        return searchResult.ToArray();
    }
}

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

public class FindObjectsWithMaterial : EditorWindow
{

    private Material searchMaterial;

    private List<string> projectResults;
    private List<Object> projectObjRefs;

    private List<string> sceneResults;
    private List<Object> sceneObjRefs;

    private Vector2 sceneResultsScrollPosition = Vector2.zero;
    private Vector2 projectResultsScrollPosition = Vector2.zero;

    [MenuItem("Audiokinetic/Utilities/Development Utilities/Find Objects With Material")]
    public static void Init()
    {
        FindObjectsWithMaterial window = (FindObjectsWithMaterial)EditorWindow.GetWindow(typeof(FindObjectsWithMaterial));
        window.Show();
    }

    void OnGUI()
    {
        minSize = new Vector2(200, 50);

        //GUI
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Material: ", GUILayout.MaxWidth(position.width / 3f));
        searchMaterial = EditorGUILayout.ObjectField(searchMaterial, typeof(Material), false) as Material;

        if (searchMaterial != null)
        {
            if (GUILayout.Button(projectResults == null ? "FIND" : "REFRESH"))
            {
                FindObjects();
            }
        }

        EditorGUILayout.EndHorizontal();

        //Draw results
        GUIStyle labelStyle = new GUIStyle(GUI.skin.box);
        labelStyle.richText = true;

        if (sceneResults != null)
        {
            GUILayout.Box("In Loaded Scene(s) (<color=red>"+sceneResults.Count+"</color>): ", labelStyle);
            sceneResultsScrollPosition = EditorGUILayout.BeginScrollView(sceneResultsScrollPosition, GUILayout.MaxHeight(position.height / 2f));
            for (int i = 0; i < sceneResults.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(">", GUILayout.MaxWidth(18f)))
                {
                    Selection.activeObject = sceneObjRefs[i];
                    SceneView.FrameLastActiveSceneView();
                    EditorGUIUtility.PingObject(sceneObjRefs[i]);
                }
                EditorGUILayout.LabelField(sceneResults[i]);

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        if (projectResults != null)
        {
            GUILayout.Box("In Project (<color=red>"+projectResults.Count+"</color>): ", labelStyle);
            projectResultsScrollPosition = EditorGUILayout.BeginScrollView(projectResultsScrollPosition, GUILayout.MaxHeight(position.height / 2f));
            for (int i = 0; i < projectResults.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(">", GUILayout.MaxWidth(18f)))
                {
                    EditorGUIUtility.PingObject(projectObjRefs[i]);
                }
                EditorGUILayout.LabelField(projectResults[i]);

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndVertical();
    }

    void FindObjects()
    {
        sceneResults = new List<string>();
        sceneObjRefs = new List<Object>();
        projectResults = new List<string>();
        projectObjRefs = new List<Object>();

        string[] allAssets = AssetDatabase.GetAllAssetPaths();

        for (int i = 0; i < allAssets.Length; i++)
        {
            string currentPath = allAssets[i];
            if (currentPath.StartsWith("Assets"))
            {
                System.Type assetType = AssetDatabase.GetMainAssetTypeAtPath(currentPath);

                if (assetType == typeof(GameObject))
                {
                    //In project
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(currentPath);

                    Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
                    if (renderers != null)
                    {
                        foreach (Renderer r in renderers)
                        {
                            foreach (Material m in r.sharedMaterials)
                            {
                                if (m == searchMaterial)
                                {
                                    string result = string.Format("'{0}' {1} has Renderer with material {2}",
                                                                  r.transform.name,
                                                                  r.transform.parent == null ? "" : (" (parent: " + r.transform.parent.name + ")"),
                                                                  searchMaterial.name
                                                                 );
                                    projectResults.Add(result);
                                    projectObjRefs.Add(r.gameObject as Object);
                                    break;

                                }
                            }
                        }
                    }

                }
                else if (assetType == typeof(SceneAsset))
                {
                    //In scenes
                    SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(currentPath);
                    Scene s = SceneManager.GetSceneByName(scene.name);

                    if (s.IsValid())
                    {
                        GameObject[] rootObjects = s.GetRootGameObjects();

                        for (int j = 0; j < rootObjects.Length; j++)
                        {
                            GameObject currentObject = rootObjects[j];

                            Renderer[] renderers = currentObject.GetComponentsInChildren<Renderer>();
                            if (renderers != null)
                            {
                                foreach (Renderer r in renderers)
                                {
                                    foreach (Material m in r.sharedMaterials)
                                    {
                                        if (m == searchMaterial)
                                        {
                                            string result = string.Format("'{0}' {1} has Renderer with material {2}",
                                                                          r.transform.name,
                                                                          r.transform.parent == null ? "" : (" (parent: " + r.transform.parent.name + ")"),
                                                                          searchMaterial.name
                                                                         );
                                            sceneResults.Add(result);
                                            sceneObjRefs.Add(r.gameObject as Object);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}

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

public class FindObjectsWithMesh : EditorWindow
{

    private Mesh searchMesh;

    private List<string> projectResults;
    private List<Object> projectObjRefs;

    private List<string> sceneResults;
    private List<Object> sceneObjRefs;

    private Vector2 sceneResultsScrollPosition = Vector2.zero;
    private Vector2 projectResultsScrollPosition = Vector2.zero;

    [MenuItem("Audiokinetic/Utilities/Development Utilities/Find Objects With Mesh")]
    public static void Init()
    {
        FindObjectsWithMesh window = (FindObjectsWithMesh)EditorWindow.GetWindow(typeof(FindObjectsWithMesh));
        window.Show();
    }

    void OnGUI()
    {
        minSize = new Vector2(200, 50);

        //GUI
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Mesh: ", GUILayout.MaxWidth(position.width / 3f));
        searchMesh = EditorGUILayout.ObjectField(searchMesh, typeof(Mesh), false) as Mesh;

        if (searchMesh != null)
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
            GUILayout.Box("In Loaded Scene(s) (<color=red>" + sceneResults.Count + "</color>): ", labelStyle);
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
            GUILayout.Box("In Project (<color=red>" + projectResults.Count + "</color>): ", labelStyle);
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

                    MeshFilter[] meshFilters = prefab.GetComponentsInChildren<MeshFilter>();
                    if (meshFilters != null)
                    {
                        foreach (MeshFilter m in meshFilters)
                        {
                            if (m.sharedMesh == searchMesh)
                            {
                                string result = string.Format("'{0}' {1} has MeshFilter with mesh {2}",
                                                              m.transform.name,
                                                              m.transform.parent == null ? "" : (" (parent: " + m.transform.parent.name + ")"),
                                                              searchMesh.name
                                                             );
                                projectResults.Add(result);
                                projectObjRefs.Add(m.gameObject as Object);
                                break;

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

                            MeshFilter[] meshFilters = currentObject.GetComponentsInChildren<MeshFilter>();
                            if (meshFilters != null)
                            {
                                foreach (MeshFilter m in meshFilters)
                                {
                                    if (m.sharedMesh == searchMesh) 
                                    {
                                        string result = string.Format("'{0}' {1} has Renderer with material {2}",
                                                                      m.transform.name,
                                                                      m.transform.parent == null ? "" : (" (parent: " + m.transform.parent.name + ")"),
                                                                      searchMesh.name
                                                                     );
                                        sceneResults.Add(result);
                                        sceneObjRefs.Add(m.gameObject as Object);
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

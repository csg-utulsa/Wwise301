////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ReplaceObjectWithPrefab : EditorWindow
{

    private GameObject prefab;
    private Vector2 scrollPos;
    private bool keepChildren = true;
    private bool keepParent = true;

    [MenuItem("Audiokinetic/Utilities/Development Utilities/Replace Object(s) With Prefab")]
    public static void Init()
    {
        ReplaceObjectWithPrefab window = (ReplaceObjectWithPrefab)EditorWindow.GetWindow(typeof(ReplaceObjectWithPrefab));
        window.Show();
    }

    void OnGUI()
    {
        minSize = new Vector2(200, 50);

        EditorGUILayout.BeginVertical();

        //the prefab
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Replace objects with:");
        prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), false) as GameObject;
        keepChildren = EditorGUILayout.Toggle("Preserve children", keepChildren);
        keepParent = EditorGUILayout.Toggle("Preserve parent", keepParent);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        //What to replace
        EditorGUILayout.BeginVertical();

        GameObject[] selectedObjects = Selection.gameObjects;
        EditorGUILayout.LabelField("Following objects (" + selectedObjects.Length + ") will be replaced: ");

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxHeight(position.height / 2));
        for (int i = 0; i < selectedObjects.Length; i++)
        {
            EditorGUILayout.LabelField(selectedObjects[i].name + (keepChildren && selectedObjects[i].transform.childCount > 0 ? " (+ " + selectedObjects[i].transform.childCount + " children)" : ""));
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        //Button
        if (GUILayout.Button("REPLACE"))
        {
            ReplacePrefabs(selectedObjects);
        }

        EditorGUILayout.EndVertical();

    }

    void ReplacePrefabs(GameObject[] objects)
    {

        Undo.RecordObjects(objects, "Deleted stuff.");

        GameObject[] newSelection = new GameObject[objects.Length];

        for (int i = 0; i < objects.Length; i++)
        {
            GameObject currentObject = objects[i];
            newSelection[i] = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            newSelection[i].name = prefab.name;

			if (keepParent)
			{
				newSelection[i].transform.SetParent(currentObject.transform.parent);
			}

            newSelection[i].transform.position = currentObject.transform.position;
            newSelection[i].transform.rotation = currentObject.transform.rotation;
            newSelection[i].transform.localScale = currentObject.transform.localScale;


            if (keepChildren)
            {
                while (currentObject.transform.childCount > 0)
                {
                    currentObject.transform.GetChild(0).parent = newSelection[i].transform;
                }
            }
        }

        Selection.objects = newSelection;

        for (int i = objects.Length - 1; i >= 0; i--)
        {
            DestroyImmediate(objects[i]);
        }

        EditorSceneManager.MarkAllScenesDirty();

    }

}
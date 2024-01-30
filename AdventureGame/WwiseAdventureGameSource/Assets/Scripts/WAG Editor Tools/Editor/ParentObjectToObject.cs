////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class ParentObjectToObject : EditorWindow {

	private GameObject newParent;
	private Vector2 scrollPos;

    [MenuItem("Audiokinetic/Utilities/Development Utilities/Parent Object(s) to Object")]
	public static void Init(){
		ParentObjectToObject window = (ParentObjectToObject)EditorWindow.GetWindow (typeof(ParentObjectToObject));
		window.Show ();
	}

	void OnGUI(){
		minSize = new Vector2 (200, 50);

		EditorGUILayout.BeginVertical ();

		//the prefab
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField ("Parent objects to:");
		newParent = EditorGUILayout.ObjectField (newParent, typeof(GameObject), true) as GameObject;
		EditorGUILayout.EndVertical ();

		EditorGUILayout.Space ();

		//What to replace
		EditorGUILayout.BeginVertical();

		GameObject[] selectedObjects = Selection.gameObjects;
		EditorGUILayout.LabelField ("Following objects ("+selectedObjects.Length+") will be parented: ");

		scrollPos = EditorGUILayout.BeginScrollView (scrollPos, GUILayout.MaxHeight(position.height/2));
		for (int i = 0; i < selectedObjects.Length; i++) {
			EditorGUILayout.LabelField (selectedObjects [i].name);
		}
		EditorGUILayout.EndScrollView ();

		EditorGUILayout.EndVertical ();

		EditorGUILayout.Space ();

		//Button
		if (GUILayout.Button ("PARENT")) {
			ParentPrefabs (selectedObjects);
		}

		EditorGUILayout.EndVertical ();

	}

	void ParentPrefabs(GameObject[] objects){
		Undo.RecordObjects (objects, "Deleted stuff.");

		for (int i = 0; i < objects.Length; i++) {
			GameObject currentObject = objects [i];
			currentObject.transform.SetParent (newParent.transform);
		}
		EditorSceneManager.MarkAllScenesDirty ();

	}

}
#endif
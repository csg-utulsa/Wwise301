////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
#if UNITY_EDITOR

using UnityEditor;

public class WAG_Toolbar : EditorWindow {

    #region icon textures //TODO: Auto-load from some folder... References seem to not be retained after a restart of the editor...
    //Set these up in the inspector
    [SerializeField]
    private Texture wpagLogoTexture;
	[SerializeField]
	private Texture findPlayerTexture;
	[SerializeField]
	private Texture findWwizardTexture;
	[SerializeField]
	private Texture replaceObjectsTexture;
	[SerializeField]
	private Texture reparentObjectsTexture;
    [SerializeField]
    private Texture findSunTexture;
	#endregion

	private Vector2 scrollAmount;
	private float textureWidth = 50f; 

	private static GUIStyle tooltipLabelStyle;
    private static Color backgroundColor = new Color(0.25f, 0.25f, 0.25f);

	[MenuItem("Audiokinetic/Utilities/WAG Toolbar %#t", priority = 4)]
	public static void Init(){
		WAG_Toolbar window = (WAG_Toolbar)EditorWindow.GetWindow (typeof(WAG_Toolbar));
		window.Show (); 	
	}

	public void OnSceneGUI(SceneView view)
	{
		Handles.Label(Vector3.zero, "Instance Id = " + view.GetInstanceID());
	}

    void OnGUI()
    {
        Color origColor = GUI.color;

        tooltipLabelStyle = new GUIStyle(EditorStyles.miniBoldLabel);
        tooltipLabelStyle.wordWrap = true;
        tooltipLabelStyle.alignment = TextAnchor.MiddleCenter;
        tooltipLabelStyle.fontStyle = FontStyle.Bold;

        //set minimum sizes
        minSize = new Vector2(textureWidth, textureWidth);

        //Which mode?
        bool vertical = position.height > position.width;

        //Draw background
        EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), backgroundColor - new Color(0.06f, 0.06f, 0.06f, 0f));

        //Draw logo
        GUI.DrawTexture(new Rect(0, 0, textureWidth, textureWidth), wpagLogoTexture, ScaleMode.ScaleToFit);

        //Toolbar
        GUILayout.BeginArea(new Rect(vertical ? 0 : textureWidth, vertical ? textureWidth : 0, position.width, position.height));
        scrollAmount = GUILayout.BeginScrollView(scrollAmount, GUIStyle.none, GUIStyle.none);

        if (vertical)
        {
            EditorGUILayout.BeginVertical();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
        }

        GUIStyle buttonStyle = new GUIStyle(EditorStyles.toolbarButton);
        buttonStyle.fixedHeight = textureWidth;
        buttonStyle.fixedWidth = textureWidth;


        GUI.backgroundColor = backgroundColor;
        if (GUILayout.Button(new GUIContent(findPlayerTexture, "FIND PLAYER"), buttonStyle))
        {
            SelectObjectWithUniqueTag("Player");
        }

        if (GUILayout.Button(new GUIContent(findWwizardTexture, "FIND WWIZARD"), buttonStyle))
        {
            SelectWwizard();
        }

        if (GUILayout.Button(new GUIContent(findSunTexture, "FIND SUN"), buttonStyle))
        {
            SelectObjectWithUniqueTag("Sun");
        }

        if (GUILayout.Button(new GUIContent(replaceObjectsTexture, "REPLACE OBJECTS"), buttonStyle))
        {
            OpenWindow<ReplaceObjectWithPrefab>();
        }

        if (GUILayout.Button(new GUIContent(reparentObjectsTexture, "REPARENT OBJECTS"), buttonStyle))
        {
            OpenWindow<ParentObjectToObject>();
        }

        if (vertical)
        {
            EditorGUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();


        GUI.color = origColor;
        Repaint();
    }

    void SelectObjectWithUniqueTag(string tag){
        GameObject obj = GameObject.FindWithTag(tag);

		if (obj != null)
		{
			Selection.activeGameObject = obj;
			SceneView.FrameLastActiveSceneView();
			//Maybe also move camera
		}
		else
		{
			Debug.LogError("WPAG Toolbar: ERROR - no GameObject tagged with " + tag + " in the current scene.");
		}
    }

	void SelectWwizard(){
		GameObject wwizard = GameObject.Find ("Wwizard");

		if (wwizard != null) {
			Selection.activeGameObject = wwizard;
			SceneView.FrameLastActiveSceneView ();
			//Maybe also move camera
		} else {
			Debug.LogError ("WPAG Toolbar: ERROR - no GameObject named Wwizard in the current scene.");
		}
	}

	void OpenWindow<T>(){
		EditorWindow window = EditorWindow.GetWindow (typeof(T));
		window.Show ();
	}
}
#endif
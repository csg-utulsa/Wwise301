////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RotateAndScaleSelectionRandomly : EditorWindow
{
    private Vector3 scaleFrom;
    private Vector3 scaleTo;
    private Vector3 rotateFrom;
    private Vector3 rotateTo;

    bool link_xz;

    [MenuItem("Audiokinetic/Utilities/Development Utilities/Rotate and Scale Selection Randomly")]
    static void Init()
    {
        RotateAndScaleSelectionRandomly window = GetWindow<RotateAndScaleSelectionRandomly>() as RotateAndScaleSelectionRandomly;
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        link_xz = EditorGUILayout.Toggle("Link x/z", link_xz);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Scale between: ", GUILayout.MaxWidth(100f));
        scaleFrom = EditorGUILayout.Vector3Field("min", scaleFrom);
        scaleTo = EditorGUILayout.Vector3Field("max", scaleTo);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Rotate between: ", GUILayout.MaxWidth(100f));
        rotateFrom = EditorGUILayout.Vector3Field("min", rotateFrom);
        rotateTo = EditorGUILayout.Vector3Field("max", rotateTo);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("DO IT"))
        {
            Perform();
        }

        EditorGUILayout.EndVertical();
    }

    void Perform()
    {
        Object[] objs = Selection.objects;
        Undo.RegisterCompleteObjectUndo(objs, "Rotated/Scaled Selection");

        foreach (Object o in objs)
        {
            GameObject g = o as GameObject;
            if (g != null)
            {
                Transform t = g.transform;

                t.Rotate(RandomVector(rotateFrom, rotateTo));
                t.localScale = RandomVector(scaleFrom, scaleTo);
            }
        }
    }

    Vector3 RandomVector(Vector3 from, Vector3 to){
        float x = Random.Range(from.x, to.x);
        float y = Random.Range(from.y, to.y);
        float z = Random.Range(from.z, to.z);

        if(link_xz){
            z = x;
        }

        return new Vector3(x, y, z);
    }

    float GetRandomBetween(float a, float b){
        return Random.Range(a, b);
    }
}

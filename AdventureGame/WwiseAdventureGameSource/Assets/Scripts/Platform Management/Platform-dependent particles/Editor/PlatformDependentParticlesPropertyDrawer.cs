////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PlatformDependentParticles))]
public class PlatformDependentParticlesPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Top right button
        float size = 10;
        Rect topRight = position;
        topRight.xMin = topRight.xMax - size;
        topRight.yMax = topRight.yMin + size;

        EditorGUI.BeginProperty(position, label, property);
        position.height /= property.FindPropertyRelative("particleVariations").arraySize;
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        //Actual GUI goes here
        if (Event.current.type != EventType.ScrollWheel)
        {
            SerializedProperty list = property.FindPropertyRelative("particleVariations");

            for (int i = 0; i < list.arraySize; i++)
            {
                Rect rect = position;
                rect.y += position.height * i;

                Rect halfSpace = rect;
                halfSpace.width /= 2;
                EditorGUI.ObjectField(halfSpace, list.GetArrayElementAtIndex(i).FindPropertyRelative("particles"), GUIContent.none);

                halfSpace.x += halfSpace.width;
                EditorGUI.LabelField(halfSpace, "VFX on " + list.GetArrayElementAtIndex(i).FindPropertyRelative("platform").enumNames[i]);
            }
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * (1 + property.FindPropertyRelative("particleVariations").arraySize - 1);
    }
}

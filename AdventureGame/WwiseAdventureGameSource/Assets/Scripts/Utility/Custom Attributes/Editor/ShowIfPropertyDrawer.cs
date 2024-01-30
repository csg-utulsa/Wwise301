////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(ShowIf))]
public class ShowIfPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIf showIf = (ShowIf)attribute;
        bool enabled = GetAttributeResult(showIf, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;

        if (enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }

        GUI.enabled = wasEnabled;
    }

    private bool GetAttributeResult(ShowIf show, SerializedProperty prop)
    {
        bool enabled = true;
        SerializedProperty sourcePropertyValue = prop.serializedObject.FindProperty(show.Conditional);
        if (sourcePropertyValue != null)
        {
            if (show.isBool)
            {
                enabled = sourcePropertyValue.boolValue == show.HideInInspector ? true : false;
            }
            else if (show.isEnum)
            {
                string[] enumNames = sourcePropertyValue.enumNames;
                enabled = enumNames[sourcePropertyValue.enumValueIndex] == show.enumValue ? show.HideInInspector : !show.HideInInspector;
            }
            else if (show.isFloatHigherThan)
            {
                enabled = sourcePropertyValue.floatValue > show.floatValue ? show.HideInInspector : !show.HideInInspector;
            }
        }
        else
        {
            Debug.LogWarning("Attempting to use a ShowIfAttribute, but no matching SourcePropertyValue found in object: " + show.Conditional);
        }
        return enabled;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        ShowIf show = (ShowIf)attribute;
        bool enabled = GetAttributeResult(show, prop);

        if (enabled)
        {
            return EditorGUI.GetPropertyHeight(prop, label); //drawn
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing; //not drawn
        }
    }

}
#endif

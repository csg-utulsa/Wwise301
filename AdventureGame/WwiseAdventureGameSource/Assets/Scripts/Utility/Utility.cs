////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

﻿using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using Random = UnityEngine.Random;

public static class Utility
{

    public static Vector3 GetRandomVector(float size)
    {
        return new Vector3(Random.Range(-size, size), Random.Range(-size, size), Random.Range(-size, size));
    }

    public static float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static void RayVisionRange(int rays, Vector3 centerPos, float addHeight, float stayTime, float range, Color c)
    {
        Vector3 extraHeight = new Vector3(0f, addHeight, 0f);
        float degreeJump = 360 / rays;
        Vector3 angle = Vector3.forward;
        Vector3 newPos = extraHeight + centerPos + extraHeight + (angle * range);
        Vector3 oldPos = extraHeight + centerPos + extraHeight + (angle * range);

        for (int i = 0; i < rays; i++)
        {
            angle = Quaternion.Euler(0, -degreeJump, 0) * angle;
            newPos = extraHeight + centerPos + extraHeight + (angle * range);
            Vector3 oldDirection = oldPos - (extraHeight + centerPos + extraHeight + (angle * range));
            Debug.DrawRay(newPos, oldDirection, c, stayTime);
            oldPos = newPos;
        }
    }

    public static List<GameObject> GetAllChildren(Transform reference)
    {
        List<GameObject> listOfGameobjects = new List<GameObject>();
        for (int c = 0; c < reference.childCount; c++)
        {
            listOfGameobjects.Add(reference.GetChild(c).gameObject);
        }
        return listOfGameobjects;
    }

    public static Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);

        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

    public static void DestroyAllGameObjectsInList(List<GameObject> Clearlist)
    {
        for (int i = Clearlist.Count; i > 0; i--)
        {
            MonoBehaviour.Destroy(Clearlist[i - 1].gameObject);

        }
        Clearlist.Clear();
    }

    public static void DestroyFirstListElement(List<GameObject> ClearList)
    {
        if (ClearList.Count > 0)
        {
            MonoBehaviour.Destroy(ClearList[0].gameObject);
            ClearList.RemoveAt(0);
        }

    }

    public static void DestroyLastlistElement(List<GameObject> ClearList)
    {
        if (ClearList.Count > 0)
        {
            MonoBehaviour.Destroy(ClearList[ClearList.Count - 1].gameObject);
            ClearList.RemoveAt(ClearList.Count - 1);
        }
    }


    public static void RecalculateMesh(Mesh mesh, float ScaleMethod)
    {
        Vector3[] baseVertices = mesh.vertices;

        var vertices = new Vector3[baseVertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            var vertex = baseVertices[i];
            vertex.x = vertex.x * ScaleMethod;
            vertex.y = vertex.y * ScaleMethod;
            vertex.z = vertex.z * ScaleMethod;

            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
    }

    public static void StripGameObjectFromComponents(GameObject go, System.Type[] types)
    {
        for (int i = 0; i < types.Length; i++)
        {
            var c = go.GetComponents(types[i]);
            if (c != null)
            {
                foreach (Component comp in c)
                {
                    GameObject.Destroy(comp);
                }
            }
        }
    }

    public static void StripGameObjectFromComponents(GameObject go, System.Type type)
    {
        var c = go.GetComponents(type);
        if (c != null)
        {
            foreach (Component comp in c)
            {
                GameObject.Destroy(comp);
            }
        }
    }

    public static void PrintAUXChannelInfo(List<GameObject> G)
    {
        String finalString = "";

        for (int i = 0; i<G.Count;i++) {
            AkAuxSendArray AuxArray = new AkAuxSendArray();
            uint AuxNum = 99;
            AkSoundEngine.GetGameObjectAuxSendValues(G[i].gameObject, AuxArray, ref AuxNum);
            if (AuxNum != 99)
            {
                finalString = finalString + G[i].name + ": "+ AuxNum+", ";
            }
            else {
                Debug.Log("No AUX info on " + G[i].name);
            }
        }
        Debug.Log(finalString);
        // AkSoundEngine.SetGameObjectAuxSendValues(transform.parent.gameObject, asdas, sendval);
    }

}

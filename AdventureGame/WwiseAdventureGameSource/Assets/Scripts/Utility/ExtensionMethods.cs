////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2018 Audiokinetic Inc. / All Rights Reserved
//
////////////////////////////////////////////////////////////////////////

using UnityEngine;

public static class ExtensionMethods
{
    #region Vector3 extensions
    public static Vector3 WithY(this Vector3 vector, float newY)
    {
        return new Vector3(vector.x, newY, vector.z);
    }
    #endregion

    #region Color extensions
    public static Color WithAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
    #endregion
}

//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static Transform[] GetChildren(this Transform parent)
    {
        List<Transform> children = new List<Transform>();

        for(int i = 0; i < parent.childCount; i++)
        {
            children.Add(parent.GetChild(i));
        }

        return children.ToArray();
    }

    public static float Distance(this Vector3 position, Vector3 other)
    {
        return Mathf.Abs(position.x - other.x + position.y - other.y + position.z - other.z);
    }

    public static Vector3 Translate(this Vector3 position, Vector3 other, float amount)
    {
        Vector3 result = new Vector3(0, 0, 0);

        result.x = (position.x - other.x);
        result.y = (position.y - other.y);
        result.z = (position.z - other.z);

        result = result.normalized;

        return result * amount;
    }
}

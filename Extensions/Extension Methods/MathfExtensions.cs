//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;

public static class MathfExtensions
{
    public static Vector3 Lerp(this Vector3 a, Vector3 b, float alpha)
    {
        return new Vector3(Mathf.Lerp(a.x, b.x, alpha), Mathf.Lerp(a.y, b.y, alpha), Mathf.Lerp(a.z, b.z, alpha));
    }

    public static Vector3 Lerp(this Vector3 a, Vector3 b, Vector3 alpha)
    {
        return new Vector3(Mathf.Lerp(a.x, b.x, alpha.x), Mathf.Lerp(a.y, b.y, alpha.y), Mathf.Lerp(a.z, b.z, alpha.z));
    }

    public static Vector3 LerpAngle(this Vector3 a, Vector3 b, float alpha)
    {
        return new Vector3(Mathf.LerpAngle(a.x, b.x, alpha), Mathf.LerpAngle(a.y, b.y, alpha), Mathf.LerpAngle(a.z, b.z, alpha));
    }

    public static Vector3 Abs(this Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public static Vector3 Mul(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x*b.x, a.y*b.y, a.z*b.z);
    }

    public static float x(this Vector3 vector)
    {
        return vector.x;
    }
    
    public static float y(this Vector3 vector)
    {
        return vector.y;
    }
    
    public static float z(this Vector3 vector)
    {
        return vector.z;
    }
    
    public static Vector2 xy(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }   
    
    public static Vector2 xz(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
    
    public static Vector2 yz(this Vector3 vector)
    {
        return new Vector2(vector.y, vector.z);
    }
    
    public static Vector2 Lerp(this Vector2 a, Vector2 b, float alpha)
    {
        return new Vector2(Mathf.Lerp(a.x, b.x, alpha), Mathf.Lerp(a.y, b.y, alpha));
    }

}

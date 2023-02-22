using System;
using UnityEngine;

namespace UDT.Data
{
    [Serializable]
    public class Vec3Curve
    {
        public AnimationCurve[] curves = new AnimationCurve[3];
        
        public Vec3Curve()
        {
            for (int i = 0; i < 3; i++)
            {
                curves[i] = new AnimationCurve();
            }
        }
        
        public Vector3 Evaluate(float time)
        {
            return new Vector3(curves[0].Evaluate(time), curves[1].Evaluate(time), curves[2].Evaluate(time));
        }
    }
}
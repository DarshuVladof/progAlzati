using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace giobs
{
    public static class Bezier
    {
        public static Vector2 GetPoint(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1.0f - t;
            return
                oneMinusT * oneMinusT * p0 +
                2.0f * oneMinusT * t * p1 + t * t * p2;

            //return Vector2.Lerp(Vector2.Lerp(p0, p1, t), Vector2.Lerp(p1, p2, t), t);
        }

        public static Vector2 GetPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * oneMinusT * p0 +
                3f * oneMinusT * oneMinusT * t * p1 +
                3f * oneMinusT * t * t * p2 +
                t * t * t * p3;
        }

        //derivata della curva che rappresenta la velocità
        public static Vector2 GetFirstDerivative(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            return 2.0f * (1.0f - t) * (p1 - p0) +
                2.0f * t * (p2 - p1);
        }

        public static Vector2 GetFirstDerivative(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (p1 - p0) +
                6f * oneMinusT * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }
    }
}

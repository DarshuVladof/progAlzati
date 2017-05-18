using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace giobs
{
    public class BezierCurve : MonoBehaviour
    {
        public Vector2[] points;

        public Vector2 GetPoint(float t)
        {
            return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
        }

        //velocità --> sottrarre per la position per non ottenere un punto
        public Vector2 GetVelocity (float t)
        {
            return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - 
                transform.position;
        }

        public Vector2 GetDirection (float t)
        {
            return GetVelocity(t).normalized;
        }

        public void Reset()
        {
            points = new Vector2[] {
                new Vector2(1.0f, 0.0f), 
                new Vector2(2.0f, 0.0f),
                new Vector2(3.0f, 0.0f),
                new Vector2(4.0f, 0.0f)
            };
        }
    }
}

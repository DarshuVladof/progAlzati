using System;
using UnityEngine;

namespace giobs
{
    public class BezierSpline : MonoBehaviour
    {
        [SerializeField]
        private Vector2[] points;
        [SerializeField]
        private BezierControlPointMode[] modes;

        public Vector2 GetPoint(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return transform.TransformPoint(Bezier.GetPoint(
                points[i], points[i + 1], points[i + 2], points[i + 3], t));
        }

        public Vector2 GetVelocity(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                i = points.Length - 4;
            }
            else
            {
                t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
                i *= 3;
            }
            return transform.TransformPoint(Bezier.GetFirstDerivative(
                points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
        }

        public Vector2 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

        public void AddCurve ()
        {
            Vector2 point = points[points.Length - 1]; // primo punto della nuova curva coincide con l'ultimo della precedente
            Array.Resize(ref points, points.Length + 3);
            point.x += 1.0f;
            points[points.Length - 3] = point;
            point.x += 1.0f;
            points[points.Length - 2] = point;
            point.x += 1.0f;
            points[points.Length - 1] = point;

            Array.Resize(ref modes, modes.Length + 1);
            modes[modes.Length - 1] = modes[modes.Length - 2];
        }

        public int ControlPointCount ()
        {
            return points.Length;
        }

        public Vector2 GetControlPoint (int index)
        {
            return points[index];
        }

        public void SetControlPoint (int index, Vector2 controlPoint)
        {
            points[index] = controlPoint;
        }

        public int CurveCount
        {
            get{  return (points.Length - 1) / 3; }
        }

        public BezierControlPointMode GetControlPointMode(int index)
        {
            return modes[(index + 1) / 3];
        }

        public void SetControlPointMode(int index, BezierControlPointMode mode)
        {
            modes[(index + 1) / 3] = mode;
        }

        public void Reset()
        {
            points = new Vector2[] {
                new Vector2(1.0f, 0.0f), 
                new Vector2(2.0f, 0.0f),
                new Vector2(3.0f, 0.0f),
                new Vector2(4.0f, 0.0f)
            };

            modes = new BezierControlPointMode[] {
                BezierControlPointMode.Free,
                BezierControlPointMode.Free
            };
        }
    }
}

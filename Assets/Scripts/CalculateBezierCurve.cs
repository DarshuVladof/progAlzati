﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class CalculateBezierCurve
{
    public const int SEGMENTS_PER_CURVE = 100;
    public Generator gen = GameObject.FindGameObjectWithTag("Gen").GetComponent<Generator>();

    private List<Vector3> controlPoints;
    private int curveCount; //numero di curve di bezier di grado 5
    private Vector3 p0, p1, p2, p3, p4, p5;
    private List<Vector3> points;

    public CalculateBezierCurve()
    {
        controlPoints = new List<Vector3>();
        points = new List<Vector3>();
    }

    public void SetControlPoints(List<Vector3> newControlPoints)
    {
        controlPoints.Clear();
        points.Clear();
        controlPoints.AddRange(newControlPoints);
        curveCount = (controlPoints.Count - 1) / 6;
    }

    public List<Vector3> GetControlPoints()
    {
        return controlPoints;
    }

    public List<Vector3> GetDrawingPoints()
    {
        for (int i = 0; i < controlPoints.Count - 1; i += 3)
        {
            if (i == controlPoints.Count - 2)
            {
                p0 = controlPoints[i];
                p1 = controlPoints[i + 1];
                p2 = controlPoints[0];
                p3 = controlPoints[1];
            }
            else if (i == controlPoints.Count - 3)
            {
                p0 = controlPoints[i];
                p1 = controlPoints[i + 1];
                p2 = controlPoints[i + 2];
                p3 = controlPoints[0];
            }
            else
            {
                p0 = controlPoints[i];
                p1 = controlPoints[i + 1];
                p2 = controlPoints[i + 2];
                p3 = controlPoints[i + 3];
            }
            if (i == controlPoints.Count - 2)
            {
                for (int j = 1; j <= SEGMENTS_PER_CURVE / 2; j++)
                {
                    float t = j / (float)SEGMENTS_PER_CURVE;
                    points.Add(CalculateBezierPoint3(t, p0, p1, p2, p3));
                }
            }
            else
            {
                if (i == 0)
                {
                    points.Add(CalculateBezierPoint3(0, p0, p1, p2, p3));
                }
                for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                {
                    float t = j / (float)SEGMENTS_PER_CURVE;
                    points.Add(CalculateBezierPoint3(t, p0, p1, p2, p3));
                }
            }
        }
        return points;
    }

    public List<Vector3> GetDrawingPointsAlt()
    {
        int i = 0, k = 0;

        while (i <= controlPoints.Count - 1)
        {
            if (i > 0 && i < controlPoints.Count - 1)
            {
                if (gen.orderedTurns[k].GetComponent<TurningPoint>().wide)
                {
                    p0 = controlPoints[i];
                    p1 = controlPoints[i + 1];
                    p2 = controlPoints[i + 2];
                    p3 = controlPoints[i + 3];
                    p4 = controlPoints[i + 4];
                    for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                    {
                        float t = j / (float)SEGMENTS_PER_CURVE;
                        points.Add(CalculateBezierPoint4(t, p0, p1, p2, p3, p4));
                    }
                    i += 5;
                }
                else
                {
                    p0 = controlPoints[i];
                    p1 = controlPoints[i + 1];
                    p2 = controlPoints[i + 2];
                    for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                    {
                        float t = j / (float)SEGMENTS_PER_CURVE;
                        points.Add(CalculateBezierPoint2(t, p0, p1, p2));
                    }
                    i += 3;
                }
                k++;
            }
            else
            {
                if (i == 0)
                {
                    points.Add(controlPoints[i]);
                    i++;
                }
                else if (i == controlPoints.Count - 1)
                {
                    points.Add(controlPoints[i]);
                    i++;
                }
            }
        }
        return points;
    }

    public List<Vector3> GetDrawingPoints3or5Degree()
    {
        int i = 0, k = 0;

        while (i <= controlPoints.Count - 1)
        {
            if (i > 0 && i < controlPoints.Count - 1)
            {
                if (gen.orderedTurns[k].GetComponent<TurningPoint>().wide)
                {
                    p0 = controlPoints[i - 1];
                    p1 = controlPoints[i];
                    p2 = controlPoints[i + 1];
                    p3 = controlPoints[i + 2];
                    p4 = controlPoints[i + 3];
                    p5 = controlPoints[i + 4];
                    for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                    {
                        float t = j / (float)SEGMENTS_PER_CURVE;
                        points.Add(CalculateBezierPoint5(t, p0, p1, p2, p3, p4, p5));
                    }
                    i += 5;
                }
                else
                {
                    p0 = controlPoints[i - 1];
                    p1 = controlPoints[i];
                    p2 = controlPoints[i + 1];
                    p3 = controlPoints[i + 2];
                    for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                    {
                        float t = j / (float)SEGMENTS_PER_CURVE;
                        points.Add(CalculateBezierPoint3(t, p0, p1, p2, p3));
                    }
                    i += 3;
                }
                k++;
            }
            else
            {
                if (i == 0)
                {
                    points.Add(controlPoints[i]);
                    i++;
                }
                else if (i == controlPoints.Count - 1)
                {
                    points.Add(controlPoints[i]);
                    i++;
                }
            }
        }
        return points;
    }

    public Vector3[] GetPlayerDrawingPoints()
    {
        int i = 1;

        while (i < controlPoints.Count)
        {
            if (i > 0 && i < controlPoints.Count - 1)
            {
                p0 = controlPoints[i - 1];
                p1 = controlPoints[i];
                p2 = controlPoints[i + 1];
                p3 = controlPoints[i + 2];
                p4 = controlPoints[i + 3];
                p5 = controlPoints[i + 4];

                for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
                {
                    float t = j / (float)SEGMENTS_PER_CURVE;
                    points.Add(CalculateBezierPoint5(t, p0, p1, p2, p3, p4, p5));
                }
                i += 5;
            }
            else
            {
                if (i == 0)
                {
                    points.Add(controlPoints[i]);
                    i++;
                }
                else if (i == controlPoints.Count - 1)
                {
                    points.Add(controlPoints[i]);
                    i++;
                }
            }
        }
        return points.ToArray();
    }

    public Vector2 CalculateBezierPoint2(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * (oneMinusT * p0 + t * p1) +
            t * (oneMinusT * p1 + t * p2);
    }

    public Vector2 CalculateBezierPoint3(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }

    public Vector2 CalculateBezierPoint4(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            (oneMinusT * oneMinusT * oneMinusT * oneMinusT * p0) +
            (4 * oneMinusT * oneMinusT * oneMinusT * t * p1) +
            (6 * t * t * oneMinusT * oneMinusT * p2) +
            (4 * t * t * t * oneMinusT * p3) +
            (t * t * t * t * p4);
    }

    public Vector2 CalculateBezierPoint5(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector3 p5)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            (oneMinusT * oneMinusT * oneMinusT * oneMinusT * oneMinusT * p0) +
            (5 * oneMinusT * oneMinusT * oneMinusT * oneMinusT * t * p1) +
            (10 * t * t * oneMinusT * oneMinusT * oneMinusT * p2) +
            (10 * t * t * t * oneMinusT * oneMinusT * p3) +
            (5 * t * t * t * t * oneMinusT * p4) +
            (t * t * t * t * t * p5);
    }

    public Vector2 GetFirstDerivative(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector3 p5)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        Vector2 d1 = -5 * (oneMinusT * oneMinusT * oneMinusT * oneMinusT) * p0;
        Vector2 d2 = (5 * (oneMinusT * oneMinusT * oneMinusT * oneMinusT) -20 * t * (oneMinusT * oneMinusT * oneMinusT)) * p1;
        Vector2 d3 = (20 * t * (oneMinusT * oneMinusT * oneMinusT) -30 * t * t * (oneMinusT * oneMinusT))* p2;
        Vector2 d4 = (30 * t * t  * (oneMinusT * oneMinusT) -20 * t * t * t * (oneMinusT))* p3;
        Vector2 d5 = (20 * t * t * t * (oneMinusT) -5 * t * t * t * t) * p4;
        Vector2 d6 = (5 * t * t * t * t) * p5;

        return d1 + d2 + d3 + d4 + d5 + d6;
    }

    //public List<Vector3> GetDrawingPointsCR()
    //{
    //    int i = 0;

    //    while (i < controlPoints.Count)
    //    {
    //        if (i == controlPoints.Count - 2)
    //        {
    //            p0 = controlPoints[i - 1];
    //            p1 = controlPoints[i];
    //            p2 = controlPoints[i + 1];
    //            p3 = controlPoints[0];
    //            i += 3;
    //        }
    //        else if (i == 0)
    //        {
    //            p0 = controlPoints[controlPoints.Count - 1];
    //            p1 = controlPoints[i];
    //            p2 = controlPoints[i + 1];
    //            p3 = controlPoints[i + 2];

    //            i += 2;
    //        }
    //        else if (i == controlPoints.Count - 1)
    //        {
    //            p0 = controlPoints[i - 1];
    //            p1 = controlPoints[i];
    //            p2 = controlPoints[0];
    //            p3 = controlPoints[1];
    //            i += 3;
    //        }
    //        else
    //        {
    //            p0 = controlPoints[i - 1];
    //            p1 = controlPoints[i];
    //            p2 = controlPoints[i + 1];
    //            p3 = controlPoints[i + 2];
    //            i += 3;
    //        }
    //        for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
    //        {
    //            float t = j / (float)SEGMENTS_PER_CURVE;
    //            points.Add(CalculateCRomPoint(t, p0, p1, p2, p3));
    //        }
    //    }
    //    return points;
    //}

    //public Vector2 CalculateCRomPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    //{
    //    t = Mathf.Clamp01(t);
    //    return
    //       (2f * p1 +
    //       (p2 - p0) * t +
    //        (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
    //        (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t) * 0.5f;
    //}

    public Vector3 adjPoint(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float a, b, c, d, i, step;
        Vector3 result = new Vector3();
        i = Vector2.Distance(p1, p2);
        a = p1.y - p3.y;
        b = p3.x - p1.x;
        c = p1.x * p3.y - p1.y * p3.x;

        d = Mathf.Abs(a * p2.x + b * p2.y + c) / Mathf.Sqrt(a * a + b * b);
        step = Mathf.Sqrt(i * i - d * d);
        result = p1 + (p3 - p1).normalized * step;
        return result;
    }
}
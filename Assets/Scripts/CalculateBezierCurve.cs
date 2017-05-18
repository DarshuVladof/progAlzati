using UnityEngine;
using System.Collections.Generic;
using System;

public class CalculateBezierCurve
{
    private const int SEGMENTS_PER_CURVE = 200;
    private const float MINIMUM_SQR_DISTANCE = 0.01f;

    private List<Vector3> controlPoints;
    private int curveCount; //numero di curve di bezier di grado 4

    public CalculateBezierCurve ()
    {
        controlPoints = new List<Vector3>();
    }

    public void SetControlPoints(List<Vector3> newControlPoints)
    {
        controlPoints.Clear();
        controlPoints.AddRange(newControlPoints);
        curveCount = (controlPoints.Count - 1) / 3;
    }

    public List<Vector3> GetControlPoints()
    {
        return controlPoints;
    }

    public List<Vector3> GetDrawingPoints ()
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 p0;
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;

        for (int i = 0; i < controlPoints.Count; i += 3)
        {
            if (i == controlPoints.Count - 1)
            {
                p0 = controlPoints[i];
                p1 = controlPoints[0];
                p2 = controlPoints[1];
                p3 = controlPoints[2];
            }
           else if (i == controlPoints.Count - 2)
            {
                p0 = controlPoints[i];
                p1 = controlPoints[i + 1];
                p2 = controlPoints[0];
                p3 = controlPoints[1];
            }
            else if(i == controlPoints.Count - 3)
            {
                p0 = controlPoints[i];
                p1 = controlPoints[i + 1];
                p2 = controlPoints[i + 2];
                p3 = controlPoints[0];
            }
            else if(i % 3 == 0)
            {
                 p0 = controlPoints[i];
                 p1 = controlPoints[i + 1];
                 p2 = controlPoints[i + 1];
                 p3 = controlPoints[i + 2];
                i--;
            }
            else
            {
                 p0 = controlPoints[i];
                 p1 = controlPoints[i + 1];
                 p2 = controlPoints[i + 2];
                 p3 = controlPoints[i + 3];
            }

            if(i == 0)
            {
                points.Add(CalculateBezierPoint(0, p0, p1, p2, p3));
            }

            for (int j = 1; j <= SEGMENTS_PER_CURVE; j++)
            {
                float t = j / (float)SEGMENTS_PER_CURVE;
                points.Add(CalculateBezierPoint(t, p0, p1, p2, p3));
            }
        }

        return points;
    }

    public Vector2 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * p0 +
            3f * oneMinusT * oneMinusT * t * p1 +
            3f * oneMinusT * t * t * p2 +
            t * t * t * p3;
    }
}
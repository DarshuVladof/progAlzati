﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace giobs
{
    [CustomEditor(typeof(BezierCurve))]
    public class BezierCurveInspector : Editor
    {
        private const int lineSteps = 20;
        private const float directionScale = 0.5f;

        private BezierCurve curve;
        private Transform handleTransform;
        private Quaternion handleRotation;

        private void OnSceneGUI()
        {
            curve = target as BezierCurve;
            handleTransform = curve.transform;
            handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                handleTransform.rotation : Quaternion.identity;

            Vector2 p0 = ShowPoint(0);
            Vector2 p1 = ShowPoint(1);
            Vector2 p2 = ShowPoint(2);
            Vector2 p3 = ShowPoint(3);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p3);

            ShowDirections();
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
        }

        private Vector2 ShowPoint(int index)
        {
            Vector2 point = handleTransform.TransformPoint(curve.points[index]);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curve, "Move Point");
                EditorUtility.SetDirty(curve);
                curve.points[index] = handleTransform.InverseTransformPoint(point);
            }

            return point;
        }

        private void ShowDirections ()
        {
            Handles.color = Color.green;
            Vector2 point = curve.GetPoint(0f);
            Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
            for (int i = 1; i <= lineSteps; i++)
            {
                point = curve.GetPoint(i / (float)lineSteps);
                Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
            }
        }
    }
}

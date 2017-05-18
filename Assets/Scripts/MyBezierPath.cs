using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBezierPath : MonoBehaviour {

    private enum Mode
    {
        Line,
        Bezier,
    }

    private Mode mode;
    public List<Vector3> points;
    //private List<Vector2> gizmos;
    private LineRenderer lineRenderer;
    public GameObject point;
    private GameObject r;
    public Generator gen;
    public Vector3[] p;

    void Start () {
        r = GameObject.Find("Renderer");
        lineRenderer = GetComponent<LineRenderer>();
        points = new List<Vector3>();
        mode = Mode.Bezier;
        gen = GameObject.FindGameObjectWithTag("Gen").GetComponent<Generator>();
    }
	
	void Update () {

        ReceiveInput();
        Render();
    }

    private void ReceiveInput()
    {
        if (gen.done)
        {

            if (Input.GetMouseButtonDown(0))
            {
                //Vector2 position = Input.mousePosition;
                //Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
                //points.Add(worldPosition);
                int i = 0;
                p = new Vector3[gen.dots.Count];

                foreach (GameObject g in gen.dots)
                {
                    p[i] = g.transform.position;
                    i++;
                }
                points.AddRange(p);
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                mode = Mode.Line;
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                mode = Mode.Bezier;
            }

          /*  if (Input.GetKeyDown(KeyCode.X))
            {
                points.Clear();
            }*/
        }
    }

    private void Render ()
    {
        switch (mode)
        {
            case Mode.Line : RenderLine();
                break;
            case Mode.Bezier : RenderBezier();
                break;
            default: break;
        }
    }

    private void RenderLine()
    {
        //gizmos = points;
        SetLinePoints(points);
    }

    private void RenderBezier()
    {
        CalculateBezierCurve calculateBezier = new CalculateBezierCurve();

        calculateBezier.SetControlPoints(points);
        List<Vector3> drawingPoints = calculateBezier.GetDrawingPoints();

        //gizmos = drawingPoints;
     
        SetLinePoints(drawingPoints);
    }

    private void SetLinePoints(List<Vector3> drawingPoints)
    {
        lineRenderer.positionCount = drawingPoints.Count;

        //for (int i = 0; i < drawingPoints.Count; i++)
        //{
        //   lineRenderer.SetPosition(i, drawingPoints[i]);
        //}
       
        lineRenderer.SetPositions(drawingPoints.ToArray());
    }

    public void OnDrawGizmos ()
    {

    }

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        GUILayout.Label("F1 Line Segments (Click to add points)");
        GUILayout.Label("F2 Bezier curve (Click to add points)");
        GUILayout.Label("X  Clear");
        GUILayout.EndArea();
    }
}

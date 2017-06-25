using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBezierPath : MonoBehaviour
{
    private enum Mode
    {
        Line,
        Bezier,
        BezierVariousDegree,
        Quad,
    }

    public List<Vector3> points;
    public GameObject car;
    public Transform finish;

    private Generator gen;
    private Mode mode;
    private CalculateBezierCurve calculateBezier;
    private LineRenderer lineRenderer;
    private GameObject r;
    private Vector3[] p;
    private List<Vector3> drawingPoints;
    private int index = 0;

    public float carSpeed = 50.0f;

    void Start()
    {
        r = GameObject.Find("Renderer");
        gen = GameObject.FindGameObjectWithTag("Gen").GetComponent<Generator>();
        lineRenderer = GetComponent<LineRenderer>();
        points = new List<Vector3>();
        mode = Mode.BezierVariousDegree;
        calculateBezier = new CalculateBezierCurve();
        drawingPoints = new List<Vector3>();
        //if(car != null && finish != null)
        //    StartCoroutine(CarGo());
    }

    void Update()
    {
        //ReceiveInput();
        //Render();
    }

    public void GenerateSpline()
    {
        if (gen.done)
        {
            int i = 0;
            p = new Vector3[gen.dots.Count];

            for (i = 0; i < gen.dots.Count; i++)
            {
                p[i] = gen.dots[i].transform.position;
            }
            points.Clear();
            points.AddRange(p);

            RenderBezier3or5Degree();
        }
    }

    public void StartCar()
    {
        if (car != null && finish != null)
        {
            //car.SetActive(true);
            StartCoroutine(CarGo());
        }
    }

    private void ReceiveInput()
    {
        if (gen.done)
        {
            if (Input.GetMouseButtonDown(0))
            {
                int i = 0;
                p = new Vector3[gen.dots.Count];

                for (i = 0; i < gen.dots.Count; i++)
                {
                    p[i] = gen.dots[i].transform.position;
                }
                points.Clear();
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

            if (Input.GetKeyDown(KeyCode.F3))
            {
                mode = Mode.BezierVariousDegree;
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                mode = Mode.Quad;
            }

            /*  if (Input.GetKeyDown(KeyCode.X))
              {
                  points.Clear();
              }*/
        }
    }

    private void Render()
    {
        switch (mode)
        {
            case Mode.Line:
                RenderLine();
                break;
            case Mode.Bezier:
                RenderBezier();
                break;
            case Mode.BezierVariousDegree:
                RenderBezier3or5Degree();
                break;
            case Mode.Quad:
                RenderBezierAlt();
                break;
            default: break;
        }
    }

    private void RenderLine()
    {
        SetLinePoints(points);
    }

    private void RenderBezier()
    {
        calculateBezier.SetControlPoints(points);
        drawingPoints = calculateBezier.GetDrawingPoints();
        SetLinePoints(drawingPoints);
    }

    private void RenderBezierAlt()
    {
        calculateBezier.SetControlPoints(points);
        drawingPoints = calculateBezier.GetDrawingPointsAlt();
        SetLinePoints(drawingPoints);
    }

    private void RenderBezier3or5Degree()
    {
        calculateBezier.SetControlPoints(points);
        //drawingPoints = calculateBezier.GetDrawingPointsCR();

        drawingPoints = calculateBezier.GetDrawingPoints3or5Degree();
        SetLinePoints(drawingPoints);
    }

    private void SetLinePoints(List<Vector3> drawingPoints)
    {
        lineRenderer.positionCount = drawingPoints.Count;
        lineRenderer.SetPositions(drawingPoints.ToArray());
    }

    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        GUILayout.Label("F1 Line Segments (Click to add points)");
        GUILayout.Label("F2 Bezier curve (Click to add points)");
        GUILayout.Label("X  Clear");
        GUILayout.EndArea();
    }

    IEnumerator CarGo()
    {
        while (true)
        {
            if (drawingPoints.Count != 0)
            {
                car.transform.position = drawingPoints[index];
                index++;
                if (car.transform.position.Equals(finish.position))
                    break;
                //    index = 0;
            }
            yield return new WaitForSeconds(1 / carSpeed);

        }
    }
}

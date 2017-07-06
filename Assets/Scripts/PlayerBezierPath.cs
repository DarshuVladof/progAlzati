using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBezierPath : MonoBehaviour
{
    public List<GameObject> gamePoints;
    public GameObject controlPoint;

    private RaycastHit2D hit;
    private RaycastHit2D[] hits;
    private CalculateBezierCurve playerCalculatebezier;
    private LineRenderer lineRenderer;
    private List<Vector3> playersControlPoints;
    private Vector3[] drawingPoints;

    private int n;
    private PolygonCollider2D coll;
    private List<Vector2> edgePoints;
    private bool updateCollider = false;
    private CheckCollision checkCollision;
    private EventSystem eventSystem;
    private bool splineOutTrack = false;

    List<Vector2> puntiGiunzione = new List<Vector2>();

    // Use this for initialization
    void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        gamePoints = new List<GameObject>();
        playerCalculatebezier = new CalculateBezierCurve();
        lineRenderer = GetComponent<LineRenderer>();
        playersControlPoints = new List<Vector3>();
        edgePoints = new List<Vector2>();
        n = 1;
        gamePoints.Add(GameObject.FindGameObjectWithTag("Start"));
        //coll = new GameObject("Collider").AddComponent<PolygonCollider2D>();
        //coll.transform.parent = lineRenderer.transform;
        //coll = gameObject.AddComponent<PolygonCollider2D>();
        checkCollision = FindObjectOfType<CheckCollision>();
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        bool render = false;

        if (!eventSystem.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift)))
            {
                bool raycastOnControlPoint = false;

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.gameObject.name.StartsWith("Control"))
                    {
                        hit = hits[i];
                        raycastOnControlPoint = true;
                        break;
                    }
                }

                if (raycastOnControlPoint)
                {
                    gamePoints.Remove(hit.collider.gameObject);
                    hit.collider.gameObject.SetActive(false);
                    n--;
                    for (int i = 1; i < gamePoints.Count; i++)
                    {
                        gamePoints[i].GetComponentInChildren<TextMesh>().text = (i + 1).ToString();
                    }
                }
            }

            else if (Input.GetMouseButtonDown(0))
            {
                bool raycastOnControlPoint = false;

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.gameObject.name.StartsWith("Control"))
                    {
                        hit = hits[i];
                        raycastOnControlPoint = true;
                        break;
                    }
                }

                if (!raycastOnControlPoint)
                {
                    if (controlPoint != null)
                    {
                        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        GameObject g = ObjectPoolingManager.Instance.GetObject(controlPoint.name);
                        g.transform.position = worldPosition;
                        n++;
                        g.GetComponentInChildren<TextMesh>().text = n.ToString();
                        gamePoints.Add(g);
                        updateCollider = true;
                    }
                }
                else
                {
                    //lo forzo perché a volte la funzione OnMouseDown di MovePoint da problemi
                    gamePoints[gamePoints.IndexOf(hit.collider.gameObject)].GetComponent<MovePoint>().IsPicked = true;
                }
            }

            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
                render = true;

            if ((gamePoints.Count - 1) % 5 == 0 && gamePoints.Count - 1 != 0)
            {
                if(gamePoints.Count > 7)
                {
                    int pos = gamePoints.Count - 6;
                    while (pos > 0)
                    {
                        gamePoints[pos].transform.position = playerCalculatebezier.adjPoint(gamePoints[pos - 1].transform.position,
                       gamePoints[pos].transform.position, gamePoints[pos + 1].transform.position);
                        pos = pos - 5;
                    }
                   
                }

                if (render)
                    Render();
            }
        }
    }

    private void Render()
    {
        GameObject[] splinePoints = GameObject.FindGameObjectsWithTag("SplinePoint");
        for(int i = 0; i < splinePoints.Length; i++)
        {
            splinePoints[i].SetActive(false);
        }

        playersControlPoints.Clear();
        for (int i = 0; i < gamePoints.Count; i++)
        {
            playersControlPoints.Add(gamePoints[i].transform.position);

        }
        playerCalculatebezier.SetControlPoints(playersControlPoints);
        drawingPoints = playerCalculatebezier.GetPlayerDrawingPoints();

        splineOutTrack = false;
        for (int i = 0; i < drawingPoints.Length; i++)
        {
            if (!checkCollision.PointIn(drawingPoints[i]))
            {
                GameObject g = ObjectPoolingManager.Instance.GetObject("SplinePoint");
                g.transform.position = drawingPoints[i];
                splineOutTrack = true;
            }
        }

        SetLinePoints(drawingPoints);
    }

    private void SetLinePoints(Vector3[] drawingPoints)
    {
        lineRenderer.positionCount = drawingPoints.Length;
        lineRenderer.SetPositions(drawingPoints);
    }

    public void EndSpline()
    {
        Vector3[] v = new Vector3[drawingPoints.Length + 1];
        int i = 0;
        while(i < v.Length - 1)
        {
            v[i] = drawingPoints[i];
            i++;
        }
        v[i] = GameObject.FindGameObjectWithTag("End").transform.position;

        lineRenderer.positionCount = 0;
        splineOutTrack = false;
        for (int j = 0; j < v.Length; j++)
        {
            if (!checkCollision.PointIn(v[j]))
            {
                GameObject g = ObjectPoolingManager.Instance.GetObject("SplinePoint");
                g.transform.position = v[j];
                splineOutTrack = true;
            }
        }

        lineRenderer.positionCount = v.Length;
        lineRenderer.SetPositions(v);
    }

    public bool CheckLastControlPoint()
    {
        

        return false;
    }

    public bool SplineOutTrack
    {
        get { return splineOutTrack; }
    }

    private void AddCollider()
    {
        edgePoints = new List<Vector2>();
        Vector2[] v = new Vector2[drawingPoints.Length];
        v = drawingPoints.toVector2Array();

        for (int j = 1; j < v.Length; j++)
        {
            Vector2 distanceBetweenPoints = v[j - 1] - v[j];
            Vector3 crossProduct = Vector3.Cross(distanceBetweenPoints, Vector3.forward);

            Vector2 up = (lineRenderer.startWidth / 2) * new Vector2(crossProduct.normalized.x, crossProduct.normalized.y) + v[j - 1];
            Vector2 down = -(lineRenderer.startWidth / 2) * new Vector2(crossProduct.normalized.x, crossProduct.normalized.y) + v[j - 1];

            edgePoints.Insert(0, down);
            edgePoints.Add(up);

            if (j == v.Length - 1)
            {
                // Compute the values for the last point on the Bezier curve
                up = (lineRenderer.startWidth / 2) * new Vector2(crossProduct.normalized.x, crossProduct.normalized.y) + v[j];
                down = -(lineRenderer.startWidth / 2) * new Vector2(crossProduct.normalized.x, crossProduct.normalized.y) + v[j];

                edgePoints.Insert(0, down);
                edgePoints.Add(up);
            }
        }

        coll.points = edgePoints.ToArray();
    }
}

public static class MyVector3Extension
{
    public static Vector2[] toVector2Array(this Vector3[] v3)
    {
        return Array.ConvertAll(v3, getV3fromV2);
    }

    public static Vector2 getV3fromV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }
}

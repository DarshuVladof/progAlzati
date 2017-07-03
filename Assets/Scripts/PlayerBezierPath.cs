using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Use this for initialization
    void Start()
    {
        gamePoints = new List<GameObject>();
        playerCalculatebezier = new CalculateBezierCurve();
        lineRenderer = GetComponent<LineRenderer>();
        playersControlPoints = new List<Vector3>();
        edgePoints = new List<Vector2>();
        n = 0;
        //gamePoints.Add(GameObject.FindGameObjectWithTag("Start").transform.position);
        //coll = new GameObject("Collider").AddComponent<PolygonCollider2D>();
        //coll.transform.parent = lineRenderer.transform;
        coll = gameObject.AddComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        bool rendered = false;

        if (Input.GetMouseButtonDown(1))
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
                //Destroy(hit.collider.gameObject);
                gamePoints.Remove(hit.collider.gameObject);
                hit.collider.gameObject.SetActive(false);
                n--;
                for (int i = 0; i < gamePoints.Count; i++)
                {
                    gamePoints[i].GetComponentInChildren<TextMesh>().text = (i + 1).ToString();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
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
                    //GameObject g = Instantiate(controlPoint, worldPosition, Quaternion.identity);
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

        if (Input.GetMouseButtonUp(0))
            updateCollider = true;

        if ((gamePoints.Count - 1) % 5 == 0 && gamePoints.Count - 1 != 0)
        {
            Render();
            if (updateCollider)
            {
                updateCollider = false;
                AddCollider();
                rendered = true;
                print("x");
            }
        }

        if(rendered)
        {
            Check();
            rendered = false;
        }
    }

    private void Render()
    {
        playersControlPoints.Clear();
        for (int i = 0; i < gamePoints.Count; i++)
        {
            playersControlPoints.Add(gamePoints[i].transform.position);

        }
        playerCalculatebezier.SetControlPoints(playersControlPoints);
        drawingPoints = playerCalculatebezier.GetPlayerDrawingPoints();
        SetLinePoints(drawingPoints);
    }

    private void SetLinePoints(Vector3[] drawingPoints)
    {
        lineRenderer.positionCount = drawingPoints.Length;
        lineRenderer.SetPositions(drawingPoints);
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

    void Check()
    {
        if(lineRenderer.positionCount != 0)
        {
            RaycastHit2D[] hs = Physics2D.RaycastAll(lineRenderer.GetPosition(0), Vector2.zero);

            //print(hs.Length);

            //for(int i = 0; i < hs.Length; i++)
            //{
            //    print(hs[i].collider.name);
            //}
            
            //for(int i = 0; i < lineRenderer.positionCount; i++)
            //{
            //    RaycastHit2D[] hitt = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(lineRenderer.GetPosition(i)), Vector2.zero);
            //    RaycastHit2D h = Physics2D.Raycast(lineRenderer.GetPosition(i), Vector2.zero);
            //    print(hitt.Length);
            //}
        }
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

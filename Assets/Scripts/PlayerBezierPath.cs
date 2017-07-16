using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBezierPath : MonoBehaviour
{
    public List<GameObject> gamePoints;
    public GameObject controlPoint;
    public GameObject car;
    public Transform finish;
    public bool carmove = false;
    public float carSpeed = 10.0f;

    private RaycastHit2D hit;
    private RaycastHit2D[] hits;
    private CalculateBezierCurve playerCalculatebezier;
    private LineRenderer lineRenderer;
    public List<Vector3> playersControlPoints;
    private Vector3[] drawingPoints;

    private int n;
    private PolygonCollider2D coll;
    private List<Vector2> edgePoints;
    private bool updateCollider = false;
    private CheckCollision checkCollision;
    private EventSystem eventSystem;
    private bool splineOutTrack = false;

    private List<Vector2> puntiGiunzione = new List<Vector2>();

    private double soglia;
    private int count = 0;
    private float timer = 0.0f;
    private bool carArrived = false;
    public float threshold = 0.5f;

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
        finish = GameObject.FindGameObjectWithTag("End").transform;
        //car = GameObject.FindGameObjectWithTag("PlayerCar");
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
                        if (GameObject.FindGameObjectWithTag("GUI").GetComponent<InGameGui>().cpHidden)
                            GameObject.FindGameObjectWithTag("GUI").GetComponent<InGameGui>().HideControlPoints();
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
                if (gamePoints.Count > 7)
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

        if (carmove == true)
        {
            timer += Time.deltaTime;
            Vector3 a = car.transform.position;
            Vector3 b = drawingPoints[count + 1];

            car.transform.position += (b - a).normalized * carSpeed * Time.deltaTime;

            while (Vector3.Distance(car.transform.position, b) <= .1f)
            {
                if (count < drawingPoints.Length - 2)
                {
                    count++;
                    b = drawingPoints[count + 1];
                }
                else
                {
                    carmove = false;
                    carArrived = true;
                    break;
                }
            }
        }

        if (carArrived)
        {
            StartCoroutine(ResetCarPosition());
        }
    }

    private void Render()
    {
        GameObject[] splinePoints = GameObject.FindGameObjectsWithTag("SplinePoint");
        for (int i = 0; i < splinePoints.Length; i++)
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

    public bool CheckLastControlPoint()
    {
        if (Vector3.Distance(gamePoints[gamePoints.Count - 1].transform.position, finish.position) <= threshold)
        {
            return true;
        }
        return false;
    }

    public bool SplineOutTrack
    {
        get { return splineOutTrack; }
    }

    public float Timer
    {
        get { return timer; }
        set { timer = value; }
    }

    public bool CarArrived
    {
        get { return carArrived; }
    }

    IEnumerator ResetCarPosition()
    {
        count = 0;
        yield return new WaitForSeconds(0.5f);
        car.transform.position = GameObject.FindGameObjectWithTag("Start").transform.position;
        carArrived = false;
    }

    public void clear()
    {
        gamePoints.Clear();
        drawingPoints = new Vector3[0];
        n = 1;
        gamePoints.Add(GameObject.FindGameObjectWithTag("Start"));
        lineRenderer.SetPositions(drawingPoints);
        Render();
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

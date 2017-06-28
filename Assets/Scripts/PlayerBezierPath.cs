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

    // Use this for initialization
    void Start()
    {
        gamePoints = new List<GameObject>();
        playerCalculatebezier = new CalculateBezierCurve();
        lineRenderer = GetComponent<LineRenderer>();
        playersControlPoints = new List<Vector3>();
        n = 0;
        //gamePoints.Add(GameObject.FindGameObjectWithTag("Start").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

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
                }
            }
            else
            {
                //lo forzo perché a volte la funzione OnMouseDown di MovePoint da problemi
                gamePoints[gamePoints.IndexOf(hit.collider.gameObject)].GetComponent<MovePoint>().IsPicked = true;
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((gamePoints.Count - 1) % 5 == 0)
            {
                Render();
            }
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
}

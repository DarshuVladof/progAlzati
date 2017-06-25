using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBezierPath : MonoBehaviour {

    public List<Vector3> playersPoints;
    public GameObject controlPoint;

    private RaycastHit2D hit;
    private CalculateBezierCurve playerCalculatebezier;
    private LineRenderer lineRenderer;
    private List<Vector3> drawingPoints;

    // Use this for initialization
    void Start () {
        playerCalculatebezier = new CalculateBezierCurve();
        lineRenderer = GetComponent<LineRenderer>();
        playersPoints = new List<Vector3>();
        drawingPoints = new List<Vector3>();
        //playersPoints.Add(GameObject.FindGameObjectWithTag("Start").transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (Input.GetMouseButtonDown(1))
        {
            if (hit.collider != null && hit.collider.gameObject.name.StartsWith("Control"))
            {
                Destroy(hit.collider.gameObject);
                playersPoints.Remove(hit.collider.gameObject.transform.position);
                print(playersPoints.Count);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {

            if (hit.collider == null || (hit.collider != null && !hit.collider.gameObject.name.StartsWith("Control")))
            {
                if (controlPoint != null)
                {
                    Vector2 position = Input.mousePosition;
                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
                    playersPoints.Add(worldPosition);
                    Instantiate(controlPoint, worldPosition, Quaternion.identity);
                    controlPoint.SetActive(true);
                }
            }
            //else if (hit.collider.gameObject.name.StartsWith("Control"))
            //{
            //    Vector3 old = hit.collider.gameObject.transform.position;
            //    if (hit.collider.gameObject.GetComponent<MovePoint>().moved)
            //    {
            //        int i = 0;
            //        for (i = 0; i < playersPoints.Count; i++)
            //        {
            //            if (playersPoints[i].Equals(old))
            //                break;
            //        }
            //        playersPoints.Remove(old);
            //        playersPoints.Insert(i, hit.collider.gameObject.transform.position);
            //    }
            //}
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((playersPoints.Count - 1) % 5 == 0)
            {
                Render();
            }
        }
    }

    private void Render()
    {
        playerCalculatebezier.SetControlPoints(playersPoints);
        drawingPoints = playerCalculatebezier.GetPlayerDrawingPoints();
        SetLinePoints(drawingPoints);
    }

    private void SetLinePoints(List<Vector3> drawingPoints)
    {
        lineRenderer.positionCount = drawingPoints.Count;
        lineRenderer.SetPositions(drawingPoints.ToArray());
    }
}

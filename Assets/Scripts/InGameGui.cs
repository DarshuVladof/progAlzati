using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameGui : MonoBehaviour
{
    public Button drawButton, startButton;

    private List<GameObject> controlPoints;
    private MyBezierPath bezierPath;

    void Start()
    {
        controlPoints = new List<GameObject>();
        bezierPath = FindObjectOfType<MyBezierPath>();
        startButton.enabled = false;
    }

    void Update()
    {

    }

    public void DrawSpline()
    {
        if (bezierPath != null)
            bezierPath.GenerateSpline();
        startButton.enabled = true;
    }

    public void CarGo()
    {
        if (bezierPath != null)
            bezierPath.StartCar();
    }
}

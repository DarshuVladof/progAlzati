using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameGui : MonoBehaviour
{
    public Button drawButton, startButton;
    public Text ourScore, playerScore;

    private List<GameObject> controlPoints;
    private MyBezierPath bezierPath;
    private PlayerBezierPath playerBezierPath;

    void Start()
    {
        //controlPoints = new List<GameObject>();
        bezierPath = FindObjectOfType<MyBezierPath>();
        playerBezierPath = FindObjectOfType<PlayerBezierPath>();
        if (playerBezierPath.gameObject != null)
            playerBezierPath.gameObject.SetActive(false);

        startButton.enabled = false;
        
    }

    void Update()
    {
        if(bezierPath != null)
            ourScore.text = bezierPath.timer.ToString();
    }

    public void DrawSpline()
    {
        if (bezierPath != null)
        {
            bezierPath.GenerateSpline();
            startButton.enabled = true;
           
        }
    }

    public void CarGo()
    {
        if (bezierPath != null) 
            bezierPath.StartCar();
    }

    public void GenerateSpline()
    {
        GameObject[] turningPoints = GameObject.FindGameObjectsWithTag("Tp");
        for(int i = 0; i < turningPoints.Length; i++)
        {
            if (turningPoints[i].activeSelf)
                turningPoints[i].SetActive(false);
        }
        if (playerBezierPath.gameObject != null)
            playerBezierPath.gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameGui : MonoBehaviour
{
    public Button drawButton, runButton, createSpline, backButton;
    public Text ourScore, playerScore;
    public GameObject pausePanel, generalPanel, endAndRun;

    private List<GameObject> controlPoints;
    private MyBezierPath bezierPath;
    private PlayerBezierPath playerBezierPath;
    private bool pause = false;

    private enum Circuit {
        barahe,
        dsa
    }

    void Start()
    {
        //controlPoints = new List<GameObject>();
        bezierPath = FindObjectOfType<MyBezierPath>();
        playerBezierPath = FindObjectOfType<PlayerBezierPath>();
        if (playerBezierPath.gameObject != null)
            playerBezierPath.gameObject.SetActive(false);

        runButton.gameObject.SetActive(false);
        endAndRun.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause)
            {
                generalPanel.GetComponent<Image>().raycastTarget = true;
                pausePanel.SetActive(true);
                pause = true;
                Time.timeScale = 0;
            }
            else
            {
                generalPanel.GetComponent<Image>().raycastTarget = false;
                pausePanel.SetActive(false);
                pause = false;
                Time.timeScale = 1;
            }
        }

        ourScore.text = "Nostro Tempo: ";
        playerScore.text = "-    Tuo Tempo: ";
        if (bezierPath != null)
        {
            ourScore.text += bezierPath.Timer.ToString("0.000");
            if (bezierPath.CarArrived)
                createSpline.gameObject.SetActive(true);
        }
    }

    public void DrawSpline()
    {
        if (bezierPath != null)
        {
            bezierPath.GenerateSpline();
            runButton.gameObject.SetActive(true);
        }
    }

    public void CarGo()
    {
        if (bezierPath != null) 
            bezierPath.StartCar();
        createSpline.gameObject.SetActive(false);
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
        if (bezierPath.gameObject != null)
            bezierPath.gameObject.SetActive(false);

        drawButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    public void GoToSelectionMenu()
    {
        SceneManager.LoadScene("SelectionMenu");
    }

    public void ClosePauseMenu()
    {
        generalPanel.GetComponent<Image>().raycastTarget = false;
        pausePanel.SetActive(false);
        pause = false;
        Time.timeScale = 1;
    }

    public void BackButton()
    {
        backButton.gameObject.SetActive(false);
        if (playerBezierPath.gameObject != null)
            playerBezierPath.gameObject.SetActive(false);
        if (bezierPath.gameObject != null)
            bezierPath.gameObject.SetActive(true);
    }

    
}

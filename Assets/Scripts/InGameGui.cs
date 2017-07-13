using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameGui : MonoBehaviour
{
    public Button drawButton, runButton, createSpline, backButton;
    public Text ourTime, playerTime, ourScore, playerScore;
    public GameObject pausePanel, generalPanel, errorPanel, endAndRun;

    private MyBezierPath bezierPath;
    private PlayerBezierPath playerBezierPath;
    private bool pause = false;
    private bool ourCarGo = false, playerCarGo = false;
    private bool ourSplineGenerated = false;
    private List<GameObject> playerControlPoints;
    private float ourTimer, playerTimer;
    private int ourPoints, ourCurves, playerPoints, PlayerCurves;

    void Start()
    {
        SetCorrectScore();
        SetPlayerHighScore();

        playerControlPoints = new List<GameObject>();
        playerControlPoints.Add(GameObject.FindGameObjectWithTag("Start"));

        bezierPath = FindObjectOfType<MyBezierPath>();
        playerBezierPath = FindObjectOfType<PlayerBezierPath>();
        playerBezierPath.car.SetActive(false);
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
                Pause();
            }
            else
            {
                Resume();
            }
        }

        if (ourCarGo)
        {
            ourTimer = 0.0f;
            ourTime.text = "Our Time: ";
            
            if (bezierPath != null)
            {
                ourTimer = bezierPath.Timer;
                ourTime.text += bezierPath.Timer.ToString("0.000");
                if (bezierPath.CarArrived)
                {
                    Debug.Log(ourTimer);
                    Debug.Log((10 * ((ourTimer / ourPoints) + (ourTimer /ourCurves)) / 2));
                    ourScore.text = "Our score: "+( (((int)(1000*ourTimer) / bezierPath.dots)+((int)(1000* ourTimer) / bezierPath.curves))/2).ToString();
                    createSpline.gameObject.SetActive(true);
                    runButton.enabled = true;
                }
            }
        }

        if(playerCarGo)
        {
            playerTime.text = "-    Your Time: ";

            if(playerBezierPath != null)
            {   
                 playerTime.text += playerBezierPath.Timer.ToString("0.000");
                if(playerBezierPath.CarArrived)
                {
                    playerScore.text = "-    Your Score: " + ((((int)(1000 * playerBezierPath.Timer) / playerBezierPath.playersControlPoints.Count) + ((int)(1000 * playerBezierPath.Timer) / ((playerBezierPath.playersControlPoints.Count + 1) / 6))) / 2).ToString();
                    playerCarGo = false;
                    endAndRun.GetComponent<Button>().enabled = true;
                }
            }
        }
    }

    public void DrawSpline()
    {
        backButton.gameObject.SetActive(true);
        if (bezierPath != null)
        {
            bezierPath.car.SetActive(true);
            bezierPath.gameObject.SetActive(true);
            bezierPath.GenerateSpline();
            ourPoints = bezierPath.dots;
            ourCurves = bezierPath.curves;
            runButton.gameObject.SetActive(true);
            drawButton.gameObject.SetActive(false);
            ourSplineGenerated = true;
            createSpline.gameObject.SetActive(false);
        }
    }

    public void CarGo()
    {
        if (bezierPath != null)
            bezierPath.StartCar();
        createSpline.gameObject.SetActive(false);
        ourCarGo = true;
        runButton.enabled = false;
    }

    public void GenerateSpline()
    {
        GameObject[] turningPoints = GameObject.FindGameObjectsWithTag("Tp");
        for (int i = 0; i < turningPoints.Length; i++)
        {
            if (turningPoints[i].activeSelf)
                turningPoints[i].SetActive(false);
        }

        if (playerBezierPath.gameObject != null)
            playerBezierPath.gameObject.SetActive(true);
        if (bezierPath.gameObject != null)
            bezierPath.gameObject.SetActive(false);

        playerBezierPath.car.SetActive(true);
        bezierPath.car.SetActive(false);

        if (drawButton.gameObject.activeSelf)
            drawButton.gameObject.SetActive(false);
        if (runButton.gameObject.activeSelf)
            runButton.gameObject.SetActive(false);

        backButton.gameObject.SetActive(true);
        endAndRun.gameObject.SetActive(true);
        createSpline.gameObject.SetActive(false);

        playerBezierPath.gamePoints = playerControlPoints;
        for(int i = 1; i < playerControlPoints.Count; i++)
        {
            if (!playerControlPoints[i].activeSelf)
                playerControlPoints[i].SetActive(true);
        }
    }

    public void EndAndRun()
    {
        if(playerBezierPath.SplineOutTrack)
        {
            StartCoroutine(ErrorPanel("Spline is out of track"));
        }
        else
        {
            if(playerBezierPath.CheckLastControlPoint())
            {
                playerBezierPath.carmove = true;
                endAndRun.GetComponent<Button>().enabled = false;
                playerCarGo = true;
                playerBezierPath.Timer = 0.0f;
            }
            else
            {
                StartCoroutine(ErrorPanel("Last point is not near end"));
            }
        }
    }

    public void GoToSelectionMenu()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Dot"))
            g.SetActive(false);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("CP"))
            g.SetActive(false);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("SplinePoint"))
            g.SetActive(false);

        SceneManager.LoadScene("SelectionMenu");
        Time.timeScale = 1;
    }

    public void ClosePauseMenu()
    {
        Resume();
    }

    public void BackButton()
    {
        if (ourSplineGenerated == true && !ourCarGo)
        {
            backButton.gameObject.SetActive(false);
            if (bezierPath.gameObject != null)
                bezierPath.gameObject.SetActive(false);
            runButton.gameObject.SetActive(false);
            drawButton.gameObject.SetActive(true);
            bezierPath.car.SetActive(false);
            ourSplineGenerated = false;
            createSpline.gameObject.SetActive(true);

        }
        else if(!playerCarGo)
        {
            backButton.gameObject.SetActive(false);
            if (playerBezierPath.gameObject != null)
                playerBezierPath.gameObject.SetActive(false);
            if (bezierPath.gameObject != null)
                bezierPath.gameObject.SetActive(true);

            playerBezierPath.car.SetActive(false);
            bezierPath.car.SetActive(true);

            if (ourSplineGenerated)
                runButton.gameObject.SetActive(true);
            else
                drawButton.gameObject.SetActive(true);

            createSpline.gameObject.SetActive(true);
            endAndRun.gameObject.SetActive(false);

            playerControlPoints = playerBezierPath.gamePoints;
            for (int i = 1; i < playerControlPoints.Count; i++)
            {
                if (playerControlPoints[i].activeSelf)
                    playerControlPoints[i].SetActive(false);
            }
            GameObject[] g = GameObject.FindGameObjectsWithTag("SplinePoint");
            foreach (GameObject p in g)
                p.gameObject.SetActive(false);
        }
        
    }

    private void SetCorrectScore()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Bahrain": ourTime.text = "Nostro Tempo: " + OurScores.BahrainScore; break;
            case "Interlagos": ourTime.text = "Nostro Tempo: " + OurScores.IntelagosScore; break;
            case "MonteCarlo": ourTime.text = "Nostro Tempo: " + OurScores.MonteCarloScore; break;
            case "Monza": ourTime.text = "Nostro Tempo: " + OurScores.MonzaScore; break;
            case "Singapore": ourTime.text = "Nostro Tempo: " + OurScores.SingaporeScore; break;
            default: break;
        }
    }

    private void SetPlayerHighScore()
    {

    }

    private void Pause()
    {
        generalPanel.GetComponent<Image>().raycastTarget = true;
        pausePanel.SetActive(true);
        pause = true;
        Time.timeScale = 0;
        backButton.enabled = false;
        endAndRun.GetComponent<Button>().enabled = false;
        drawButton.enabled = false;
        runButton.enabled = false;
        createSpline.enabled = false;
    }

    private void Resume()
    {
        generalPanel.GetComponent<Image>().raycastTarget = false;
        pausePanel.SetActive(false);
        pause = false;
        Time.timeScale = 1;
        backButton.enabled = true;
        endAndRun.GetComponent<Button>().enabled = true;
        drawButton.enabled = true;
        runButton.enabled = true;
        createSpline.enabled = true;
    }

    IEnumerator ErrorPanel(string message)
    {
        errorPanel.SetActive(true);
        errorPanel.GetComponentInChildren<Text>().text = message;
        GameObject.Find("GUI").GetComponent<GraphicRaycaster>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        errorPanel.SetActive(false);
        GameObject.Find("GUI").GetComponent<GraphicRaycaster>().enabled = true;
    }
}

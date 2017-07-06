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

    private MyBezierPath bezierPath;
    private PlayerBezierPath playerBezierPath;
    private bool pause = false;
    private bool carGo = false;

    void Start()
    {
        SetCorrectScore();

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
                Pause();
            }
            else
            {
                Resume();
            }
        }

        if (carGo)
        {
            ourScore.text = "Nostro Tempo: ";
            playerScore.text = "-    Tuo Tempo: ";
            if (bezierPath != null)
            {
                ourScore.text += bezierPath.Timer.ToString("0.000");
                if (bezierPath.CarArrived)
                {
                    createSpline.gameObject.SetActive(true);
                    runButton.enabled = true;
                }
            }
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
        carGo = true;
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

        drawButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        endAndRun.gameObject.SetActive(true);
    }

    public void EndAndRun()
    {

    }

    public void GoToSelectionMenu()
    {
        SceneManager.LoadScene("SelectionMenu");
        Time.timeScale = 1;
    }

    public void ClosePauseMenu()
    {
        Resume();
    }

    public void BackButton()
    {
        backButton.gameObject.SetActive(false);
        if (playerBezierPath.gameObject != null)
            playerBezierPath.gameObject.SetActive(false);
        if (bezierPath.gameObject != null)
            bezierPath.gameObject.SetActive(true);
    }

    private void SetCorrectScore()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Bahrain": ourScore.text = "Nostro Tempo: " + OurScores.BahrainScore; break;
            case "Interlagos": ourScore.text = "Nostro Tempo: " + OurScores.IntelagosScore; break;
            case "MonteCarlo": ourScore.text = "Nostro Tempo: " + OurScores.MonteCarloScore; break;
            case "Monza": ourScore.text = "Nostro Tempo: " + OurScores.MonzaScore; break;
            case "Singapore": ourScore.text = "Nostro Tempo: " + OurScores.SingaporeScore; break;
            default: break;
        }
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
}

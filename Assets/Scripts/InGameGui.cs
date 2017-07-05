using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameGui : MonoBehaviour
{
    public Button drawButton, startButton;
    public Text ourScore, playerScore;
    public GameObject pausePanel, generalPanel;

    private List<GameObject> controlPoints;
    private MyBezierPath bezierPath;
    private PlayerBezierPath playerBezierPath;
    private bool pause = false;

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
        if(bezierPath != null)
            ourScore.text += bezierPath.Timer.ToString("0.000");
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
}

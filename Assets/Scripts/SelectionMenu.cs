using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    public GameObject[] circuitsList;
    public GameObject buttonLeft, buttonRight;
    public GameObject actualName;

    private string[] circuitsName = { "Singapore", "Interlagos", "Bahrain", "Monza", "MonteCarlo" };
    private int currentIndex;

    void Start()
    {
        currentIndex = 0;
        SetActualCircuit(currentIndex, -1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Previous();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Next();
        }
    }

    public void GoToCircuit()
    {
        SceneManager.LoadScene(circuitsName[currentIndex]);
    }

    public void OnClickLeft()
    {
        Previous();
    }

    public void OnClickRight()
    {
        Next();
    }

    private void Next()
    {
        int previousIndex = currentIndex;
        currentIndex++;
        if (currentIndex == circuitsList.Length)
        {
            currentIndex = 0;
        }
        SetActualCircuit(currentIndex, previousIndex);
    }

    private void Previous()
    {
        int previousIndex = currentIndex;
        currentIndex--;
        if (currentIndex == -1)
        {
            currentIndex = circuitsList.Length - 1;
        }
        SetActualCircuit(currentIndex, previousIndex);
    }

    private void SetActualCircuit(int index, int previousIndex)
    {
        actualName.GetComponent<Text>().text = circuitsName[currentIndex];
        if (previousIndex != -1)
            circuitsList[previousIndex].SetActive(false);
        circuitsList[currentIndex].SetActive(true);
    }
}
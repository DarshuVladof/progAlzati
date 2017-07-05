using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLoader : MonoBehaviour {

    public void GoToSelectionMenu()
    {
        SceneManager.LoadScene("SelectionMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}

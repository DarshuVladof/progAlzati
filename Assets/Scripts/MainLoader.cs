using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLoader : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void GoToSelectionMenu()
    {
        SceneManager.LoadScene("SelectionMenu");
    }
}

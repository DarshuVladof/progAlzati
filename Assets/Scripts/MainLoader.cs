using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLoader : MonoBehaviour {

    //public static MainLoader Instance = null;

    void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void GoToSelectionMenu()
    {
        SceneManager.LoadScene("SelectionMenu");
    }
}

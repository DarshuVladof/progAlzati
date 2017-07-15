using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(!PlayerPrefs.HasKey("Bahrain"))
            PlayerPrefs.SetFloat("Bahrain", 0.00f);
        if (!PlayerPrefs.HasKey("Interlagos"))
            PlayerPrefs.SetFloat("Interlagos", 0.00f);
        if (!PlayerPrefs.HasKey("MonteCarlo"))
            PlayerPrefs.SetFloat("MonteCarlo", 0.00f);
        if (!PlayerPrefs.HasKey("Monza"))
            PlayerPrefs.SetFloat("Monza", 0.00f);
        if (!PlayerPrefs.HasKey("Singapore"))
            PlayerPrefs.SetFloat("Singapore", 0.00f);

        PlayerPrefs.SetFloat("Monza", 0.00f);
        PlayerPrefs.Save();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

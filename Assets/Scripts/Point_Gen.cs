using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Point_Gen : MonoBehaviour {
    public int distance;
    public Vector3[] pointList;
    public GameObject start, end;
    // Use this for initialization
    void Start () {
        start = GameObject.FindGameObjectWithTag("Start");
        end = GameObject.FindGameObjectWithTag("End");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

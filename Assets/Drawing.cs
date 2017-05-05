using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour {

    public Generator gen;
    public Vector3[] points;
    public bool havePoints = false;
    public LineRenderer r;
    // Use this for initialization
    void Start () {
        gen = GameObject.FindGameObjectWithTag("Gen").GetComponent<Generator>();
        r = gameObject.GetComponent<LineRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!havePoints)
        {
            if (gen.done)
            {
                int i = 0;
                r.positionCount=gen.dots.Count;
                points = new Vector3[gen.dots.Count];

                foreach(GameObject g in gen.dots)
                {
                    points[i] = g.transform.position;
                    i++;
                }
                r.SetPositions(points);
            }
        }
        if (Input.GetButtonDown("Jump"))
            draw();
		
	}

    public void draw()
    {
        
    }
}



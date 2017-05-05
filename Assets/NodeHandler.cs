using UnityEngine;
using System.Collections;
using System;

public class NodeHandler : MonoBehaviour {
    public bool debug;
    public GameObject[] g;
    public int ended;
    public GGraph pathGraph;
    public bool GraphCreated=false;
    public GameObject start, end;
    // Use this for initialization
    void Awake () {
	    
        
    }
	void Start()
    {
        
    }

    public void AddNode(Node node)
    {
        pathGraph.AddNode(node);
       /* if (pathGraph.GetKeys().Count == 20)
        {
           foreach(GameObject go in g)
            {
                go.GetComponent<Node>().SetTrigger(); 
            }
        }   */  
    }

	// Update is called once per frame
	void Update () {
        if(gameObject.GetComponent<Proj>().generated==true && !GraphCreated)
        {
            start = GameObject.FindGameObjectWithTag("Start");
            end = GameObject.FindGameObjectWithTag("End");
            g = GameObject.FindGameObjectsWithTag("Dot");
            ended = 0;
            GraphCreated = false;
            pathGraph = new GGraph();
            /*foreach (GameObject n in g)
            {
                n.SetActive(true);

            }*/
            start.GetComponent<Node>().CheckSiblings();
            foreach (GameObject n in g)
            {
                n.GetComponent<Node>().CheckSiblings();

            }
            end.GetComponent<Node>().CheckSiblings();
            GraphCreated = true;
        }
    
        
        
        
	
	}
}

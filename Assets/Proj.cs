using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj : MonoBehaviour {
    public GameObject Dot;
    public List<GameObject> dots;
    public int dot_num;
    public GameObject gameManager;
    public bool generated=false;


   
    private LinkedList<Vector3> path;
    // Use this for initialization
    void Start () {
        dots = new List<GameObject>();
        dot_num = 0;
        Debug.Log("Wakeup");
        gameManager = GameObject.FindGameObjectWithTag("Manager");


    }

    

    // Update is called once per frame
    void Update () {
        if (generated == false)
        {
            if (gameManager.gameObject.GetComponent<Manager>().managerInit)
            {
                for (float i = 0; i < this.gameObject.GetComponent<Camera>().pixelWidth; i += 1.5f)
                {
                    // Debug.Log(i);
                    for (float j = 0; j < this.gameObject.GetComponent<Camera>().pixelHeight; j += 1.5f)
                    {
                        //Debug.Log(" "+j);
                        RaycastHit2D hit;
                        Ray ray = this.GetComponent<Camera>().ScreenPointToRay(new Vector3(i, j, 0));
                        hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                        //Debug.DrawRay(ray.origin, ray.direction);
                        // Debug.Log(hit.collider.gameObject.name);
                        if (hit== true)
                        {
                            // Debug.Log("Hit");
                            if (hit.collider.gameObject.tag == "Track")
                            {


                                dots.Add(ObjectPoolingManager.Instance.GetObject(Dot.name));
                                dots[dot_num].transform.position = new Vector3(hit.point.x, hit.point.y, -0.01f);
                                dots[dot_num].SetActive(true);
                                //Debug.Log("Dot");
                                dot_num++;

                            }

                        }
                    }
                }
                generated = true;
               
            }
        }
        else
        {
            //StartCoroutine(Astar());
        }
        
    }









    /*public IEnumerator Astar()
    {
        bool opened;
        Debug.Log("ASTAR");
        if (!pathFinding)
        {
            pathFinding = true;
            open = new SortedDictionary<float, List<NodeRecord>>();
            closed = new List<NodeRecord>();
            start = new NodeRecord();
            current = new NodeRecord();
            endNode = new NodeRecord();
            oldNode = new NodeRecord();
            start.Node = mynode;
            start.CostSoFar = 0;
            start.EstimatedTotalCost = Heuristic(start.Node, goal);
            open.Add(start.EstimatedTotalCost, new List<NodeRecord>());
            open[start.EstimatedTotalCost].Add(new NodeRecord(start));

        }
        //inizializzazione

        int iterations = 0;

        //main loop
        while (open.Count > 0 && iterations < 200)
        {
            iterations++;
            current = open.Values.First().First();
            if (current.Node == goal)
                break;
            foreach (GLink link in graph.GetNode(current.Node))
            {
                endNode.Node = link.GetNode();
                endNode.CostSoFar = current.CostSoFar + link.GetCost();
                endNode.From = current.Node;
                opened = false;

                if (closed.Contains(endNode))
                {//se è in closed

                    oldNode = closed.Find(x => x.Node == endNode.Node);

                    if (endNode.CostSoFar < oldNode.CostSoFar)
                    {
                        closed.Remove(oldNode);
                        endNode.EstimatedTotalCost = oldNode.EstimatedTotalCost - oldNode.CostSoFar + endNode.CostSoFar;
                    }
                    if (open.ContainsKey(endNode.EstimatedTotalCost))
                        open[endNode.EstimatedTotalCost].Add(new NodeRecord(endNode));
                    else
                    {
                        open.Add(endNode.EstimatedTotalCost, new List<NodeRecord>());
                        open[endNode.EstimatedTotalCost].Add(new NodeRecord(endNode));
                    }
                }
                else
                { //è in open?
                    foreach (float i in open.Keys)
                    {
                        if (open[i].Contains(endNode))
                        {
                            oldNode = open[i].Find(x => x.Node == endNode.Node);
                            if (endNode.CostSoFar < oldNode.CostSoFar)
                            {

                                oldNode.EstimatedTotalCost = oldNode.EstimatedTotalCost - oldNode.CostSoFar + endNode.CostSoFar;
                            }
                            opened = true;
                            break;
                        }
                    }
                }
                if (opened == false)
                {//se è nuovo...
                    endNode.EstimatedTotalCost = endNode.CostSoFar + Heuristic(endNode.Node, goal);
                    if (open.ContainsKey(endNode.EstimatedTotalCost))
                        open[endNode.EstimatedTotalCost].Add(new NodeRecord(endNode));
                    else
                    {
                        open.Add(endNode.EstimatedTotalCost, new List<NodeRecord>());
                        open[endNode.EstimatedTotalCost].Add(new NodeRecord(endNode));
                    }
                }

            }
            closed.Add(current);
            if (open.Count != 0)
            {
                if (open.Values.First().Count == 1)
                {
                    open.Remove(open.Keys.First());
                }
                else
                    open[open.Keys.First()].RemoveAt(0);
            }


        }


        if (current.Node == goal)
        {//se tutto va bene, carica il percorso
            bool done = false;
            while (!done)
            {

                if (current.Node == start.Node)
                {
                    done = true;
                    path.AddFirst(current.Node);
                }
                else
                {
                    path.AddFirst(current.Node);
                    oldNode = current;
                    current = closed.Find(x => x.Node == oldNode.From);
                }
            }

            path.AddLast(trueGoal);

            foreach (Vector3 v in path)
            {
                Debug.Log(v);
            }
            Debug.Log(iterations);
            pos = transform.position;
            pathFound = true;
            pathFinding = false;
            moving = true;
            iterations = 0;

            yield return null;

        }
        else
        {
            yield return new WaitForSeconds(0.005f);
            Call();
        }

    }*/
}

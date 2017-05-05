using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GGraph  {

    private Dictionary<Vector3, LinkedList<GLink>> graph;

    public GGraph() {
        graph = new Dictionary<Vector3, LinkedList<GLink>>();
    }

    public GGraph(Node [] n)
    {
        graph = new Dictionary<Vector3, LinkedList<GLink>>();
        foreach (Node no in n)
        {
            this.AddNode(no);
        }
    }

    public List<Vector3> GetKeys()
    {
        
      
        return graph.Keys.ToList();
    }
    public void AddNode(Node n)
    {
        if (!graph.ContainsKey(n.gameObject.transform.position))
        {
            graph.Add(n.gameObject.transform.position, new LinkedList<GLink> ());
            foreach(Vector3 sibling in n.siblings)
            {
                graph[n.gameObject.transform.position].AddFirst(new GLink(sibling, n.transform.position));
            }
        }
    }
    public LinkedList<GLink> GetNode(Vector3 pos)
    {
        return graph[pos];
    }
    /*
    public void UpdateCost(Vector3 n1, Vector3 n2, float cost)
    {
        if(graph.ContainsKey(n1) && graph.ContainsKey(n2))
        {
            graph[n1].UpdateCost(n2, cost);
            graph[n2].UpdateCost(n1, cost);
        }
    }*/
	
}
    
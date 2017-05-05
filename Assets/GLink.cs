using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GLink
{

    private Vector3 target;
    private float cost;
    // Use this for initialization
    public GLink(Vector3 to,Vector3 from)
    {
        target = to;
        cost = Vector3.Distance(target, from);
    }

    private void SetCost(float c)
    {
        cost=c;
    }
    public Vector3 GetNode()
    {
        return target;
    }
    public float GetCost()
    {
        return cost;
    }

    override
    public string ToString()
    {
        return target.ToString(); 
    }
}

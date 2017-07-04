using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    public bool PointIn(Vector2 point)
    {
        return GetComponent<Collider2D>().OverlapPoint(point);
    }
}

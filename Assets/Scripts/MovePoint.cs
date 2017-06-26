using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    private bool isPicked = true;
    private Color normal;

    void Start()
    {
        normal = GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isPicked = false;
            GetComponent<SpriteRenderer>().color = normal;
        }

        if (isPicked)
        {
            Vector2 position = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
            transform.position = worldPosition;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void OnMouseDown()
    {
        isPicked = true;
    }

    public bool IsPicked
    {
        get { return isPicked; }
        set { isPicked = value; }
    }
}

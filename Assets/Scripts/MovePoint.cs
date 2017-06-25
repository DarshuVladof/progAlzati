using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour {

    private bool isPicked = true;
    private Color normal = new Color(255.0f, 237.0f, 0.0f, 255.0f);
    public bool moved = false;

	void Start () {
		
	}
	
	
	void Update () {

        if(Input.GetMouseButtonUp(0))
        {
            isPicked = false;
            GetComponent<SpriteRenderer>().color = normal;
        }

        if(isPicked)
        {
            Vector2 position = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
            transform.position = worldPosition;
            GetComponent<SpriteRenderer>().color = Color.white;
            moved = true;
        }
	}

    void OnMouseDown()
    {
        isPicked = true;
    }
}

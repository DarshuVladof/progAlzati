using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float zoomSpeed = 20.0f;
    public float targetOrtho;
    public float smoothSpeed = 10.0f;
    public float minOrtho = 5.0f;
    public float movementSpeed = 10.0f;

    private float maxOrtho;

    void Start () {
        targetOrtho = Camera.main.orthographicSize;
        maxOrtho = Camera.main.orthographicSize;
    }
	
	void Update () {

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(movementSpeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-movementSpeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -movementSpeed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, movementSpeed * Time.deltaTime, 0));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public GameObject next, prev;
    public GameObject gen;
    public GameObject track, nope, Projector;
    private bool check = false;
    // Use this for initialization
    void Start()
    {
        gen = GameObject.FindGameObjectWithTag("Gen");
        track = GameObject.FindGameObjectWithTag("Track");
        nope = GameObject.FindGameObjectWithTag("Nope");

        Projector = GameObject.FindGameObjectWithTag("Proj");
    }

    // Update is called once per frame
    void Update()
    {
        /*	if(check && Projector.GetComponent<Proj>().generated)
            {
                this.gameObject.GetComponent<CircleCollider2D>().radius = 60;
            }*/
    }

    void LateUpdate()
    {
        if (!check)
        {
            check = true;
            if (gameObject.tag == "Dot")
            {
                if (!track.gameObject.GetComponent<PolygonCollider2D>().bounds.Contains(new Vector3(transform.position.x, transform.position.y, 0)))
                {
                    this.gameObject.SetActive(false);
                }//nope.gameObject.GetComponent<BoxCollider2D>().bounds.Contains(transform.position)
                /*if (this.gameObject.GetComponent<CircleCollider2D>().IsTouching(nope.gameObject.GetComponent<BoxCollider2D>()))
                {
                    this.gameObject.SetActive(false);
                }*/
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /* if (this.gameObject.tag == "Start" || this.gameObject.tag == "End")
         {
             if (other.gameObject.tag == "Dot")
                 other.gameObject.SetActive(false);
         }*/
    }
}

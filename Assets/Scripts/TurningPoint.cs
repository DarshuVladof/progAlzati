using System; //This allows the IComparable Interface
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPoint : MonoBehaviour
{
    
    Transform pointer;
    public int id;
    public GameObject dot,turn, previous, next;
    public GameObject gameManager;
    public GameObject Generator;
    public bool generated = false;
    public bool trackup=true;
    public float angle;
    private float w, wnext, wprev;
    public bool cub;
    // Use this for initialization
    void Start()
    {
        pointer = this.transform.GetChild(0);
        gameManager = GameObject.FindGameObjectWithTag("Manager");
        Generator = GameObject.FindGameObjectWithTag("Gen");
        if (cub)
            cubic();
        else
            quadra();
    }

    public void quadra()
    {
        angle = 35;
        w = .4f;
        wnext = .70f;
        wprev = .2f;
    }

    public void cubic()
    {
        angle = 35;
        w = .3f;
        wnext = .60f;
        wprev = .88f;
    }
    // Update is called once per frame
    void Update()
    {
        if (generated == false)
        {
            if (gameManager.gameObject.GetComponent<Manager>().managerInit)
            {
                pointer = transform.GetChild(0);

                RaycastHit2D hit = Physics2D.Raycast(transform.position,  pointer.position- transform.position , 20);
                Debug.DrawRay(transform.position, transform.position - new Vector3(hit.point.x, hit.point.y, transform.position.z), UnityEngine.Color.cyan, 60f);
                if (hit.collider.gameObject.tag == "Track")
                {
                    turn = ObjectPoolingManager.Instance.GetObject(dot.name);
                    turn.transform.position = new Vector3(hit.point.x, hit.point.y, -0.01f) +w* (pointer.position - transform.position).normalized;
                    turn.SetActive(true);
                }


                // Vector3 dirNext = transform.rotation * Quaternion.AngleAxis(20, Vector3.up) * (pointer.position - transform.position);
                Vector3 dirNext;
                Vector3 dirprev;

                if ( trackup)
                {
                    dirNext = Quaternion.Euler(0, 0, angle) * (pointer.position - transform.position);
                    dirprev = Quaternion.Euler(0, 0, -angle) * (pointer.position - transform.position);
               }
                else
                {
                    dirNext = Quaternion.Euler(0, 0, -angle) * (pointer.position - transform.position);
                    dirprev = Quaternion.Euler(0, 0, angle) * (pointer.position - transform.position);
                }
                
                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, dirNext, 20);
                Debug.DrawRay(transform.position, dirNext, UnityEngine.Color.green, 60f);
                //Debug.DrawRay(transform.position, transform.position - new Vector3(hit.point.x, hit.point.y, transform.position.z), UnityEngine.Color.cyan, 60f);
                if (hit2.collider.gameObject.tag == "Track")
                {
                    next = ObjectPoolingManager.Instance.GetObject(dot.name);
                    next.transform.position = new Vector3(hit2.point.x, hit2.point.y, -0.01f);
                    next.transform.position = next.transform.position + (wnext* dirNext.normalized);
                    next.SetActive(true);
                }


                // Vector3 dirprev = transform.rotation * Quaternion.AngleAxis(-20, Vector3.up) * (pointer.position - transform.position);
              
                RaycastHit2D hit3 = Physics2D.Raycast(transform.position, dirprev, 20);
                Debug.DrawRay(transform.position, dirprev, UnityEngine.Color.red, 60f);
               // Debug.DrawRay(transform.position, transform.position - new Vector3(hit.point.x, hit.point.y, transform.position.z), UnityEngine.Color.cyan, 60f);
                if (hit3.collider.gameObject.tag == "Track")
                {
                    previous = ObjectPoolingManager.Instance.GetObject(dot.name);
                    previous.transform.position = new Vector3(hit3.point.x, hit3.point.y, -0.01f);
                    previous.transform.position = previous.transform.position + (wprev * dirprev.normalized);
                    previous.SetActive(true);
                }


                generated = true;
                Generator.GetComponent<Generator>().confirms++;
            }
        }

    }
}
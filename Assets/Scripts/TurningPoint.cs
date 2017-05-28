using System; //This allows the IComparable Interface
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPoint : MonoBehaviour
{

    Transform pointer;
    public int id;
    public GameObject dot, turn, previous, next, widePrevious, wideNext;
    public GameObject gameManager;
    public GameObject Generator;
    public bool generated = false;
    public bool trackup = true;
    public float angle = 35f;
    public float w = .3f, wnext = .60f, wprev = .88f, wideAngle = 70f, wideWnext = .60f, widwWprev = .88f;
    public bool cub, wide = false;
    // Use this for initialization
    void Start()
    {
        pointer = this.transform.GetChild(0);
        gameManager = GameObject.FindGameObjectWithTag("Manager");
        Generator = GameObject.FindGameObjectWithTag("Gen");
        /*if (cub)
            cubic();
        else
            quadra();*/
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

                RaycastHit2D hit = Physics2D.Raycast(transform.position, pointer.position - transform.position, 20);
                Debug.DrawRay(transform.position, transform.position - new Vector3(hit.point.x, hit.point.y, transform.position.z), UnityEngine.Color.cyan, 60f);
                if (hit.collider.gameObject.tag == "Track")
                {
                    turn = ObjectPoolingManager.Instance.GetObject(dot.name);
                    turn.transform.position = new Vector3(hit.point.x, hit.point.y, -0.01f) + w * (pointer.position - transform.position).normalized;
                    turn.SetActive(true);
                }


                // Vector3 dirNext = transform.rotation * Quaternion.AngleAxis(20, Vector3.up) * (pointer.position - transform.position);
                Vector3 dirNext, dirprev, dirWNext, dirWPrev;


                if (trackup)
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
                    next.transform.position = next.transform.position + (wnext * dirNext.normalized);
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

                if (wide)
                {
                    if (trackup)
                    {
                        dirWNext = Quaternion.Euler(0, 0, wideAngle) * (pointer.position - transform.position);
                        dirWPrev = Quaternion.Euler(0, 0, -wideAngle) * (pointer.position - transform.position);
                    }
                    else
                    {
                        dirWNext = Quaternion.Euler(0, 0, -wideAngle) * (pointer.position - transform.position);
                        dirWPrev = Quaternion.Euler(0, 0, wideAngle) * (pointer.position - transform.position);
                    }

                    RaycastHit2D hit4 = Physics2D.Raycast(transform.position, dirWNext, 20);
                    Debug.DrawRay(transform.position, dirWNext, UnityEngine.Color.blue, 60f);
                    //Debug.DrawRay(transform.position, transform.position - new Vector3(hit.point.x, hit.point.y, transform.position.z), UnityEngine.Color.cyan, 60f);
                    if (hit4.collider.gameObject.tag == "Track")
                    {
                        wideNext = ObjectPoolingManager.Instance.GetObject(dot.name);
                        wideNext.transform.position = new Vector3(hit4.point.x, hit4.point.y, -0.01f);
                        wideNext.transform.position = wideNext.transform.position + (wideWnext * dirWNext.normalized);
                        wideNext.SetActive(true);
                    }


                    // Vector3 dirprev = transform.rotation * Quaternion.AngleAxis(-20, Vector3.up) * (pointer.position - transform.position);

                    RaycastHit2D hit5 = Physics2D.Raycast(transform.position, dirWPrev, 20);
                    Debug.DrawRay(transform.position, dirWPrev, UnityEngine.Color.blue, 60f);
                    // Debug.DrawRay(transform.position, transform.position - new Vector3(hit.point.x, hit.point.y, transform.position.z), UnityEngine.Color.cyan, 60f);
                    if (hit5.collider.gameObject.tag == "Track")
                    {
                        widePrevious = ObjectPoolingManager.Instance.GetObject(dot.name);
                        widePrevious.transform.position = new Vector3(hit5.point.x, hit5.point.y, -0.01f);
                        widePrevious.transform.position = widePrevious.transform.position + (widwWprev * dirWPrev.normalized);
                        widePrevious.SetActive(true);
                    }
                }

                generated = true;
                Generator.GetComponent<Generator>().confirms++;
            }
        }

    }
}
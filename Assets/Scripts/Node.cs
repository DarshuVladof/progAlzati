using UnityEngine;
using System.Collections;



public class Node : MonoBehaviour
{
    public LayerMask mask;
    public ArrayList siblings;
    public ArrayList siblingsNames;
    public bool debug;
    private bool done;
 
    private RaycastHit2D hit;
    Vector3 direction;
    float dist;
    public GameObject[] g;
    private NodeHandler NH;
    public GameObject track;
    public Collider2D[] sibs = new Collider2D[13];

    // Use this for initialization
    void Start()
    {
        done = false;
        track = GameObject.FindGameObjectWithTag("Track");
    }
    public bool IsDone()
    {
        return done;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTrigger()
    {
       // this.gameObject.GetComponent<CircleCollider2D>().radius = 7;
    }
    
    void OnTriggerEnter2D(Collider2D coll)
    {
       
    }

    public void CheckSiblings()
    {
        NH = GameObject.FindGameObjectWithTag("Proj").GetComponent<NodeHandler>();
        siblings = new ArrayList();
        siblingsNames = new ArrayList();
       
        /*for (int i = 0; i < 9; i++)
        {
            sibs[i] = new Collider2D();
        }*/
        ContactFilter2D filter=new  ContactFilter2D();
        int count = 0;
        filter.SetLayerMask(mask);
        this.gameObject.GetComponent<CircleCollider2D>().OverlapCollider(filter,sibs);
        //Physics2D.OverlapCollider(gameObject.GetComponent<CircleCollider2D>(), filter, sibs);
       // if (count != 0)
       // {
       //     Debug.Log(sibs[0].gameObject.name);
       // }
       
        /*
        if (!done)
        {
            g =NH.g;
            foreach (Collider2D n in sibs)
            {
                if (n.gameObject.tag == "Dot") { 
                siblings.Add(n.transform.position);
                                siblingsNames.Add(n.name);
                                if (debug)
                                {
                                    Debug.Log("libero:");
                                    Debug.DrawLine(transform.position, n.transform.position, Color.green, 600000f);
                                }
                                
                               
                }

            }
            NH.AddNode(this); 
            done = true;

        }*/
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {
    public List<GameObject> dots;
    public GameObject[] turns;
    public List<GameObject> orderedTurns;
    public GameObject start,end;
    public int dot_num = 0;
    public int confirms = 0;
    private int index = 1;
    public bool done = false;
    // Use this for initialization
    void Start () {
        turns = GameObject.FindGameObjectsWithTag("Tp");
        start = GameObject.FindGameObjectWithTag("Start");
        end = GameObject.FindGameObjectWithTag("End");
        orderedTurns=new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
        if (index == 1)
        {
            if (confirms == turns.Length)
            {
                Debug.Log("conferme arrivate");
                dots.Add(start);
                while(index != turns.Length+1){
                    for(int i=0;i<turns.Length; i++)
                    {
                        if (turns[i].GetComponent<TurningPoint>().id == index)
                        {
                            orderedTurns.Add(turns[i]);
                            index++;
                            if((turns[i].GetComponent<TurningPoint>().wide))
                                dots.Add(turns[i].GetComponent<TurningPoint>().widePrevious);
                            dots.Add(turns[i].GetComponent<TurningPoint>().previous);
                            dots.Add(turns[i].GetComponent<TurningPoint>().turn);
                            dots.Add(turns[i].GetComponent<TurningPoint>().next);
                            if ((turns[i].GetComponent<TurningPoint>().wide))
                                dots.Add(turns[i].GetComponent<TurningPoint>().wideNext);

                        }
                    }
                }
                dots.Add(end);
                done = true;
            }
        }
		
	}
}

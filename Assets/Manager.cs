using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    [Header("Prefabs for Pooling")]
    public GameObject Dot;
    public bool managerInit=false;
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    // Use this for initialization
    void Start () {
        ObjectPoolingManager.Instance.CreatePool(Dot, 1000, 10000);
        managerInit = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

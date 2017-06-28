using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    [Header("Prefabs for Pooling")]
    public GameObject Dot;
    public bool managerInit=false;

    [Header("Prefabs for Pooling")]
    public GameObject controlPoint;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    
    void Start () {
        ObjectPoolingManager.Instance.CreatePool(Dot, 1000, 10000);
        ObjectPoolingManager.Instance.CreatePool(controlPoint, 1000, 10000);
        managerInit = true;
    }
	
	
	void Update () {
		
	}
}

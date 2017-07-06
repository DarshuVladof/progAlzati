using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance = null;
    public bool managerInit = false;

    [Header("Prefabs for Pooling")]
    public GameObject Dot;
    [Header("Prefabs for Pooling")]
    public GameObject controlPoint;
    [Header("Prefabs for Pooling")]
    public GameObject splinePoint;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ObjectPoolingManager.Instance.CreatePool(Dot, 1000, 10000);
        ObjectPoolingManager.Instance.CreatePool(controlPoint, 500, 1000);
        ObjectPoolingManager.Instance.CreatePool(splinePoint, 1000, 10000);

        managerInit = true;
    }
}
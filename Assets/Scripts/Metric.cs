using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Metric : MonoBehaviour
{
    public abstract string Name { get; }
    [HideInInspector]
    public List<float> MetricValues;
    protected float ValueToLock;

    protected void Awake()
    {
        MetricValues = new List<float>();
        DontDestroyOnLoad(gameObject);
    }
    public abstract void Initialize();
    public abstract void Finalize();

    public void LockIn()
    {
        MetricValues.Add(ValueToLock);
    }
}

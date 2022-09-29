using System;
using System.Collections;
using System.Collections.Generic;
using MotionCaptureBasic.OSConnector;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<KeyPointItem> vectorlist = RepeatedDefaultInstance<KeyPointItem>(3);

        foreach (var vector in vectorlist)
        {
            Debug.Log($"x={vector.x}, y={vector.y}, z={vector.z}, score={vector.score}, name={vector.name}");
        }
    }
    
    private List<T> RepeatedDefaultInstance<T>(int count)
    {
        List<T> ret = new List<T>(count);
        for (var i = 0; i < count; i++)
        {
            ret.Add((T)Activator.CreateInstance(typeof(T)));
        }
        return ret;
    }
    
}

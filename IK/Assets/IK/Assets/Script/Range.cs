using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct Range
{
    [SerializeField]
    public float low;
    [SerializeField]
    public float high;
    public float length
    {
        get { return high - low; }
    }
    public Range(float low,float high)
    {
        this.low = low;
        this.high = high;
    }

    public float Limit(float n)
    {
        if(n<low && n > high)
        {
            if (Mathf.Abs(n - low) < Mathf.Abs(n - high))
                return low;
            return high;
        }
        return n;
    }

    public override string ToString()
    {
        return "[" + low.ToString() + "," + high.ToString() + "]";
    }
}
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SaveQuaternion
{
    public float w;
    public float x;
    public float y;
    public float z;

    public SaveQuaternion()
    {
        this.x = 0.0f;
        this.y = 0.0f;
        this.z = 0.0f;
        this.w = 0.0f;
    }

    public SaveQuaternion(float x, float y,float z,float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public SaveQuaternion(Quaternion quaternion) : this(quaternion.x, quaternion.y, quaternion.z, quaternion.w)
    { }

    public static implicit operator SaveQuaternion(Quaternion quaternion)
    {
        return new SaveQuaternion(quaternion);
    }

    public static implicit operator Quaternion(SaveQuaternion saveQuater)
    {
        return new Quaternion(saveQuater.x, saveQuater.y, saveQuater.z,saveQuater.w);
    }
}






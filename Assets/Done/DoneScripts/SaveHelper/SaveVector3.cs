using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SaveVector3
{
    public float x;
    public float y;
    public float z;

    public SaveVector3()
    {
        this.x = 0.0f;
        this.y = 0.0f;
        this.z = 0.0f;
    }

    public SaveVector3(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public SaveVector3(float x, float y, float z):this(x,y)
    {
        this.z = z;
    }

    public SaveVector3(Vector3 vec3):this(vec3.x,vec3.y,vec3.z)
    {}

    public static implicit operator SaveVector3( Vector3 vec)
    {
        return new SaveVector3(vec);
    }

    public static implicit operator Vector3(SaveVector3 saveVec)
    {
        return new Vector3(saveVec.x, saveVec.y, saveVec.z);
    }
}





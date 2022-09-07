using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Axis-Aligned-Bounding-Box, which is a rectangle in 2d space aligned along the x and y axis


public struct AABBCC
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;


    public AABBCC(Vector3[] vertices)
    {
        Vector3 p1 = vertices[0];

        float minX = p1.x;
        float maxX = p1.x;
        float minY = p1.y;
        float maxY = p1.y;
        float minZ = p1.z;
        float maxZ = p1.z;

        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 p = vertices[i];

            if (p.x < minX)
            {
                minX = p.x;
            }
            else if (p.x > maxX)
            {
                maxX = p.x;
            }

            if (p.y < minY)
            {
                minY = p.y;
            }
            else if (p.y > maxY)
            {
                maxY = p.y;
            }

            if (p.z < minZ)
            {
                minZ = p.z;
            }
            else if (p.z > maxZ)
            {
                maxZ = p.z;
            }
        }

        // if (minY < .01f) minY = 0;
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        this.minZ = minZ;
        this.maxZ = maxZ;
    }

    public override string ToString()
    {
        return "(" + "[" + minX + ", " + maxX + "]" + ", " + "[" + minY + ", " + maxY + "]" + ", " + "[" + minZ + ", " + maxZ + "]" + ")";
    }
}
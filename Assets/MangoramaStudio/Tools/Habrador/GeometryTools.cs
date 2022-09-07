using UnityEngine;

public static class GeometryTools
{
    public static Vector3 InsideXZ(Transform point, AABBCC aabbcc, float epsilon)
    {
        // var epsilon = .01f;
        var pos = point.position;

        if (point.position.x < aabbcc.minX) pos = new Vector3(aabbcc.minX + epsilon, pos.y, pos.z);
        else if (point.position.x > aabbcc.maxX) pos = new Vector3(aabbcc.maxX - epsilon, pos.y, pos.z);
        else if (point.position.z < aabbcc.minZ) pos = new Vector3(pos.x, pos.y, aabbcc.minZ + epsilon);
        else if (point.position.z > aabbcc.maxZ) pos = new Vector3(pos.x, pos.y, aabbcc.maxZ - epsilon);

        return pos;
    }

    public static Vector3 InsideZ(Transform point, AABBCC aabbcc)
    {
        var epsilon = .01f;
        var pos = point.position;

        if (point.position.z < aabbcc.minZ) pos = new Vector3(pos.x, pos.y, aabbcc.minZ);
        else if (point.position.z > aabbcc.maxZ) pos = new Vector3(aabbcc.maxZ, pos.y, pos.z);


        return pos;
    }
}
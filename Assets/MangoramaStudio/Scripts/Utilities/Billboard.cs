using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : CustomBehaviour
{
    void Update()
    {
        Vector3 camPos = MainCamera.transform.position;

        Transform.LookAt(
            new Vector3(-camPos.x, transform.position.y, -camPos.z)
            );
    }
}

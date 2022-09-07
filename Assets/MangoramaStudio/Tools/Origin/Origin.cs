using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Origin : MonoBehaviour
{
    public float measure = 1;

    // Update is called once per frame
    void OnDrawGizmos()
    {
        // var measure = 100f;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * measure), Color.blue);

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up * measure), Color.green);

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right * measure), Color.red);
    }
}
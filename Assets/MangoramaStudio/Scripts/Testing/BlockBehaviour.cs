using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    public static event Action<GridBehaviour> OnBlockPlaced;
    public GridBehaviour RaycastedGridBehaviour => _raycastedGridBehaviour;

    [SerializeField] private LayerMask _layerMask;

    private GridBehaviour _raycastedGridBehaviour;
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.yellow);
        if (true)
        {
           
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, _layerMask))
            {
                if (_raycastedGridBehaviour == null || hit.transform != _raycastedGridBehaviour.transform)
                {
                    _raycastedGridBehaviour = hit.transform.gameObject.GetComponent<GridBehaviour>();
                }
            }
            else
            {
                _raycastedGridBehaviour = null;
            }
        }

    }

    internal void Place(GridBehaviour raycastedGridBehaviour)
    {
        transform.position = raycastedGridBehaviour.transform.position + new Vector3(0, 0, -0.1f);
        raycastedGridBehaviour.CurrentBlockBehaviour = this;
        OnBlockPlaced?.Invoke(raycastedGridBehaviour);
        
    }
}

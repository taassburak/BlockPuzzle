using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreater : MonoBehaviour
{
    [SerializeField] GridBehaviour _gridCellObject;
    [SerializeField] int _gridWidth;
    [SerializeField] int _gridHeight;

    [Button]
    public void CreateGrid()
    {
        for (int i = 0; i < _gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                Instantiate(_gridCellObject, new Vector3( (_gridWidth- i ) * 1.1f, (_gridHeight- j) * 1.1f, 0), Quaternion.identity, transform);
            }
        }
    }
}

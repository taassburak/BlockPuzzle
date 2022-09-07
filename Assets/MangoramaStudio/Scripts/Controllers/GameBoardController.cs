using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardController : CustomBehaviour
{
    public GridBehaviour[,] GridArray => _gridArray;

    [SerializeField] GridBehaviour _gridCellObject;
    [SerializeField] int _gridWidth;
    [SerializeField] int _gridHeight;
    private GridBehaviour[,] _gridArray;
    private List<int> _destroyableColumns;
    private List<int> _destroyableRows;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        CreateGrid(_gridHeight, _gridWidth);
        BlockBehaviour.OnBlockPlaced += BlockPlaced;
        BlockController.OnBlockPlaced += DestroyBlocksOnCompletedColumsAndRows;
    }

    //public void Initialize()
    //{
    //    CreateGrid(_gridHeight, _gridWidth);
    //    BlockBehaviour.OnBlockPlaced += BlockPlaced;
    //    BlockController.OnBlockPlaced += DestroyBlocksOnCompletedColumsAndRows;
    //}
    private void OnDestroy()
    {
        BlockBehaviour.OnBlockPlaced -= BlockPlaced;
        BlockController.OnBlockPlaced += DestroyBlocksOnCompletedColumsAndRows;
    }

    private void BlockPlaced(GridBehaviour gridBehaviour)
    {
        gridBehaviour.IsOccupied = true;
        GameManager.EventManager.EarnPoint(1);
        //DestroyBlocksOnCompletedColumsAndRows();
    }

    private void CreateGrid(int column, int row)
    {
        _gridArray = new GridBehaviour[column, row];
        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                var grid =  Instantiate(_gridCellObject, new Vector3(((row - i) * 1.1f)-3f, (column - j) * 1.1f, 0), Quaternion.identity, transform);
                grid.Column = i;
                grid.Row = j;

                _gridArray[i, j] = grid;
            
            }
        }
    }

    private void CheckColumnsAndRowsIsCompleted()
    {
        _destroyableColumns = new List<int>();
        _destroyableRows = new List<int>();
        

        for (int i = 0; i < _gridWidth; i++)
        {
            int a = 0;
            for (int j = 0; j < _gridHeight; j++)
            {
                if (_gridArray[i,j].IsOccupied)
                {
                    a++;
                }
                if (a == _gridHeight)
                {
                   _destroyableColumns.Add(i);
                }
            }
        }

        for (int i = 0; i < _gridHeight; i++)
        {
            int a = 0;
            for (int j = 0; j < _gridWidth; j++)
            {
                if (_gridArray[j,i].IsOccupied)
                {
                    a++;
                }
                if (a == _gridWidth)
                {
                    _destroyableRows.Add(i);
                }
            }
        }
    }

    [Button]
    private void DestroyBlocksOnCompletedColumsAndRows()
    {
        CheckColumnsAndRowsIsCompleted();
        for (int i = 0; i < _destroyableColumns.Count; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                if (_gridArray[_destroyableColumns[i], j].CurrentBlockBehaviour != null)
                {
                    //Destroy(_gridArray[_destroyableColumns[i], j].CurrentBlockBehaviour.gameObject);
                    //_gridArray[_destroyableColumns[i], j].CurrentBlockBehaviour = null;
                    _gridArray[_destroyableColumns[i], j].IsOccupied = false;
                }
            }
        }

        for (int i = 0; i < _destroyableRows.Count; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                if (_gridArray[j, _destroyableRows[i]].CurrentBlockBehaviour != null)
                {
                    //Destroy(_gridArray[j, _destroyableRows[i]].CurrentBlockBehaviour.gameObject);
                    //_gridArray[j, _destroyableRows[i]].CurrentBlockBehaviour = null;
                    _gridArray[j, _destroyableRows[i]].IsOccupied = false;
                }
            }
        }

        StartCoroutine(DestroyBlockObjectCo());
    }

    private IEnumerator DestroyBlockObjectCo()
    {
        for (int i = 0; i < _destroyableColumns.Count; i++)
        {
            for (int j = 0; j < _gridHeight; j++)
            {
                if (_gridArray[_destroyableColumns[i], j].CurrentBlockBehaviour != null)
                {
                    yield return new WaitForSeconds(0.05f);
                    Destroy(_gridArray[_destroyableColumns[i], j].CurrentBlockBehaviour.gameObject);
                    GameManager.EventManager.EarnPoint(_gridHeight);
                    _gridArray[_destroyableColumns[i], j].CurrentBlockBehaviour = null;
                    //_gridArray[_destroyableColumns[i], j].IsOccupied = false;
                }
            }
        }

        for (int i = 0; i < _destroyableRows.Count; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                if (_gridArray[j, _destroyableRows[i]].CurrentBlockBehaviour != null)
                {
                    yield return new WaitForSeconds(0.05f);
                    Destroy(_gridArray[j, _destroyableRows[i]].CurrentBlockBehaviour.gameObject);
                    GameManager.EventManager.EarnPoint(_gridWidth);
                    _gridArray[j, _destroyableRows[i]].CurrentBlockBehaviour = null;
                    //_gridArray[j, _destroyableRows[i]].IsOccupied = false;
                }
            }
        }
    }
}

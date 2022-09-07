using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHoldStates : IState
{
    private BlockController _blockController;
    private bool _canPlace;
    public BlockHoldStates(BlockController blockController)
    {
        _blockController = blockController;
    }

    public void OnEnter()
    {
       
    }

    public void OnExit()
    {
        _blockController.CanPlace = _canPlace;
        foreach (var gridBehaviour in _blockController.BlockManager.GameManager.GameBoardController.GridArray)
        {
            gridBehaviour.ChangeMyColor(Color.white);
        }
        Debug.Log($"_canPlace {_canPlace} ");
    }

    public void OnUpdate()
    {
        if (!_blockController.BlockBehaviours.Find(x => x.RaycastedGridBehaviour == null) 
            && !_blockController.BlockBehaviours.Find(x=> x.RaycastedGridBehaviour.IsOccupied == true))
        {
            _canPlace = true;
            foreach (var blockBehaviour in _blockController.BlockBehaviours)
            {
                blockBehaviour.RaycastedGridBehaviour.ChangeMyColor(new Color(0.9f, 0.77f, 0.48f, 1));
            }
        }
        else
        {
            _canPlace = false;
            foreach (var gridBehaviour in _blockController.BlockManager.GameManager.GameBoardController.GridArray)
            {
                gridBehaviour.ChangeMyColor(Color.white);
            }
        }
    }
}

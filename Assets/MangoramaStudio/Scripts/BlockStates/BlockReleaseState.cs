using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockReleaseState : IState
{
    private BlockController _blockController;
    public BlockReleaseState(BlockController blockController)
    {
        _blockController = blockController;
    }

    public void OnEnter()
    {
        if (_blockController.CanPlace)
        {
            foreach (var block in _blockController.BlockBehaviours)
            {
                block.Place(block.RaycastedGridBehaviour);
            }
        }
        else
        {
            _blockController.GoBackToWaitingArea();
        }
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
       
    }

}

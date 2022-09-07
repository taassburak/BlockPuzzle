using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWaitingAreaState : IState
{

    private BlockController _blockController;

    public BlockWaitingAreaState(BlockController blockController)
    {
        _blockController = blockController;
    }

    public void OnEnter()
    {
        _blockController.CanPlace = false;
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        _blockController.transform.position = Vector3.Lerp(_blockController.transform.position, _blockController.BlockWaitingTransform.position, Time.deltaTime * 3);
    }
}

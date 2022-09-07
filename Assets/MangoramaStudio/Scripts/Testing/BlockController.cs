using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MangoramaStudio.Scripts.Managers;

public class BlockController : MonoBehaviour
{
    public static event Action OnBlockPlaced;
    public List<BlockBehaviour> BlockBehaviours => _blockBehaviours;

    public bool CanPlace { get; set; }
    public Transform BlockWaitingTransform { get; set; }
    public BlockManager BlockManager => _blockManager;

    [SerializeField] List<BlockBehaviour> _blockBehaviours;

    private StateMachine _blockStateMachine;
    private BlockManager _blockManager;



    [Button]
    public void Initialize(BlockManager blockManager)
    {
        _blockManager = blockManager;
        InitializeStateMachine();
        var blockWaitingAreaState = _blockStateMachine.GetState(typeof(BlockWaitingAreaState));
        _blockStateMachine.SetState(blockWaitingAreaState);
    }

    internal void GoBackToWaitingArea()
    {
        var blockWaitingAreaState = _blockStateMachine.GetState(typeof(BlockWaitingAreaState));
        _blockStateMachine.SetState(blockWaitingAreaState);
    }

    public void HoldBlock()
    {
        var blockHoldState = _blockStateMachine.GetState(typeof(BlockHoldStates));
        _blockStateMachine.SetState(blockHoldState);
    }

    public void ReleaseBlock()
    {
        var blockReleaseState = _blockStateMachine.GetState(typeof(BlockReleaseState));
        _blockStateMachine.SetState(blockReleaseState);
        if (CanPlace)
        {
            OnBlockPlaced?.Invoke();
            GetComponent<BoxCollider>().enabled = false;
        }
        
    }

    private void Update()
    {
        _blockStateMachine.Update();
    }

    private void InitializeStateMachine()
    {
        List<IState> stateList = new List<IState>();

        stateList.Add(new BlockHoldStates(this));
        stateList.Add(new BlockWaitingAreaState(this));
        stateList.Add(new BlockReleaseState(this));

        _blockStateMachine = new StateMachine(stateList);
    }
}

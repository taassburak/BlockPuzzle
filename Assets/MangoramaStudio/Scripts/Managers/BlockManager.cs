using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers
{

    public class BlockManager : CustomBehaviour
    {
        [SerializeField] List<BlockController> _blocks;
        [SerializeField] List<Transform> _blockInitialTransforms;
        private int _currentMoveCountInOneBlockGroup;

        public override void Initialize(GameManager gameManager)
        {
            base.Initialize(gameManager);
            
            CreateBlocks();
            BlockController.OnBlockPlaced += CreateNewBlockOnBlockPlaced;
        }

        private void OnDestroy()
        {
            BlockController.OnBlockPlaced -= CreateNewBlockOnBlockPlaced;
        }



        private void CreateNewBlockOnBlockPlaced()
        {
            _currentMoveCountInOneBlockGroup++;
            if (_currentMoveCountInOneBlockGroup >= 3)
            {
                _currentMoveCountInOneBlockGroup = 0;
                CreateBlocks();
            }
        }

        private void CreateBlocks()
        {

            for (int i = 0; i < 3; i++)
            {
                var rnd = Random.Range(0, _blocks.Count);
                var block = Instantiate(_blocks[rnd]);
                block.BlockWaitingTransform = _blockInitialTransforms[i];
                block.Initialize(this);
            }

        }

    }
}

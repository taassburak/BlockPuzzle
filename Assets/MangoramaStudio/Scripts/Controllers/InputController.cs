using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputController : CustomBehaviour
{
    public static bool IsInputDeactivated { get; set; } // All gameplay input depends on this, close from UI

    public static event Action OnPressPerformed;
    public static event Action<Vector2> OnDrag; // This can be used as a input value, if x > 0, player swiped right, ex.
    public static event Action OnPressCancelled;

    private Vector2 _firstPosition;
    private Vector2 _lastPosition;
    private Vector2 _dragVector;

    private bool _isDragging;

    private BlockController _currentBlockController;
    public Camera mainCamera;
    Vector3 mousePos;
    private float _mZCoord;
    private Vector3 _mOffset;
    #region Initialize

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void OnDestroy()
    {
    }

    #endregion

    private void Update()
    {
        if (!IsInputDeactivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _firstPosition = Input.mousePosition;
                OnPressPerformed?.Invoke();
                SendRayToBlockParent();
                _isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _lastPosition = Input.mousePosition;
                OnPressCancelled?.Invoke();
                _isDragging = false;
                if (_currentBlockController)
                {
                    _currentBlockController.ReleaseBlock();
                    _currentBlockController = null;
                }
            }

            if (_isDragging)
            {
                _dragVector = (Vector2)Input.mousePosition - _firstPosition;

                OnDrag?.Invoke(_dragVector * 0.02f);

                _firstPosition = Input.mousePosition;

                //mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

                mousePos = Input.mousePosition;
                mousePos.z = 1;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                if (_currentBlockController != null)
                {
                    //_currentBlockController.transform.position = new Vector3(mousePos.x, mousePos.y, _currentBlockController.transform.position.z);
                    _currentBlockController.transform.position = new Vector3(GetMouseAsWorldPoint().x + _mOffset.x, GetMouseAsWorldPoint().y + _mOffset.y, -0.5f) ;
                }
            }


        }
    }

    private void SendRayToBlockParent()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Block"))
            {
                _currentBlockController = hit.transform.GetComponent<BlockController>();
            }

            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject.GetComponent<BlockController>() is BlockController blockController)
                {
                    if (_currentBlockController != null && blockController != _currentBlockController)
                    {
                        _currentBlockController.ReleaseBlock();
                    }

                    _currentBlockController = blockController;

                    _mZCoord = Camera.main.WorldToScreenPoint(_currentBlockController.gameObject.transform.position).z;

                    _mOffset = _currentBlockController.gameObject.transform.position - GetMouseAsWorldPoint();

                    _currentBlockController.HoldBlock();
                }
            }
        }
        
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = _mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}

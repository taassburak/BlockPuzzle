using System;
using System.Collections;
using System.Collections.Generic;
using Iso;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    // [SerializeField] public Image TEST;

    #region Fields

    private Text mText;

    public Text Text
    {
        get
        {
            if (mText == null)
            {
                mText = base.GetComponent<Text>();
            }

            return mText;
        }
    }

    private Text mChildText;

    public Text ChildText
    {
        get
        {
            if (mChildText == null)
            {
                mChildText = base.GetComponentInChildren<Text>();
            }

            return mChildText;
        }
    }

    private Image mImage;

    public Image Image
    {
        get
        {
            if (mImage == null)
            {
                mImage = base.GetComponent<Image>();
            }

            return mImage;
        }
    }

    private Image mChildImage;

    public Image ChildImage
    {
        get
        {
            if (mChildImage == null)
            {
                mChildImage = base.GetComponentInChildren<Image>();
            }

            return mChildImage;
        }
    }

    public bool DontPlayAnimation { get; set; }

    protected UIManager UIManager { get; set; }

    private event Action mOnClick;

    private event Action<PointerEventData> mOnClickWithData;

    private event Action<PointerEventData> mOnPointerDown;

    private event Action<PointerEventData> mOnPointerUp;

    private Vector3 mStartScale;

    protected bool isEnabled;

    #endregion

    #region Methods

    [HideInInspector] public Vector3 initialScale;
    [HideInInspector] public Vector3 initialPos;

    public virtual void Initialize(UIManager uiManager, Action onClick, bool active = true)
    {
        gameObject.SetActive(active);
        UIManager = uiManager;
        mOnClick = onClick;

        mStartScale = transform.localScale;

        if (transform.localScale.x < 0.1f)
        {
            mStartScale = Vector3.one;
        }

        SetEnabled(true);
        initialScale = transform.localScale;
        initialPos = transform.position;
    }

    public virtual void SetEnabled(bool _isEnabled)
    {
        isEnabled = _isEnabled;
    }

    public override bool IsInteractable()
    {
        return isEnabled;
    }

    #endregion

    #region Events

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!isEnabled)
        {
            return;
        }

        if (eventData != null)
        {
            base.OnPointerClick(eventData);

            if (mOnClickWithData != null)
            {
                mOnClickWithData(eventData);
            }
        }

        if (mOnClick != null)
        {
            mOnClick();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!isEnabled)
        {
            return;
        }

        if (!DontPlayAnimation && UIManager != null)
        {
            transform.localScale = new Vector3(mStartScale.x - .1f, mStartScale.y - .1f, mStartScale.z - .1f);
        }

        base.OnPointerDown(eventData);

        if (mOnPointerDown != null)
        {
            mOnPointerDown(eventData);
        }

        // mOnClick?.Invoke();
        transform.Y(-10);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!isEnabled)
        {
            return;
        }

        base.OnPointerUp(eventData);

        if (!DontPlayAnimation && UIManager != null)
        {
            transform.localScale = mStartScale;
        }

        if (mOnPointerUp != null)
        {
            mOnPointerUp(eventData);
        }

        transform.Y(10);
    }

    #endregion
}
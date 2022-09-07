using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomToggle : Toggle
{
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

    protected UIManager UIManager { get; set; }

    private event Action mOnSelect;

    private event Action<BaseEventData> mOnSelectWithData;

    #endregion

    #region Methods

    public virtual void Initialize(UIManager uiManager, Action onSelect, bool active = true)
    {
        gameObject.SetActive(active);
        UIManager = uiManager;
        mOnSelect = onSelect;
    }

    public virtual void Initialize(UIManager uimanager, Action<BaseEventData> onSelectWithData, bool active = true)
    {
        gameObject.SetActive(active);
        UIManager = uimanager;
        mOnSelectWithData = onSelectWithData;
    }

    public virtual void SetInteraction(bool interaction)
    {
        interactable = interaction;
    }

    #endregion

    #region Events

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            if (eventData != null)
            {
                base.OnPointerClick(eventData);

                if (mOnSelectWithData != null)
                {
                    mOnSelectWithData(eventData);
                }
            }

            if (mOnSelect != null)
            {
                mOnSelect();
            }
        }
    }

    #endregion
}

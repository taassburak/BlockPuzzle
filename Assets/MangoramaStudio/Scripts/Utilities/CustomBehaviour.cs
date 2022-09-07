using UnityEngine;
using UnityEngine.UI;


public class CustomBehaviour : MonoBehaviour
{
    #region Components

    public GameManager GameManager { get; set; }

    private Transform mTransform;

    public Transform Transform
    {
        get
        {
            if (mTransform == null)
            {
                mTransform = base.transform;
            }

            return mTransform;
        }
    }

    private AudioSource mAudioSource;

    public AudioSource AudioSource
    {
        get
        {
            if (mAudioSource == null)
            {
                mAudioSource = base.GetComponent<AudioSource>();
            }

            return mAudioSource;
        }
    }

    private Camera mMainCamera;

    public Camera MainCamera
    {
        get
        {
            if (mMainCamera == null)
            {
                mMainCamera = Camera.main;
            }

            return mMainCamera;
        }
    }

    private Rigidbody mRigidbody;

    public Rigidbody Rigidbody
    {
        get
        {
            if (mRigidbody == null)
            {
                mRigidbody = base.GetComponent<Rigidbody>();
            }

            return mRigidbody;
        }
    }

    private RectTransform mRectTransform;

    public RectTransform RectTransform
    {
        get
        {
            if (mRectTransform == null)
            {
                mRectTransform = base.GetComponent<RectTransform>();
            }

            return mRectTransform;
        }
    }

    private Animator mAnimator;

    public Animator Animator
    {
        get
        {
            if (mAnimator == null)
            {
                mAnimator = base.GetComponent<Animator>();
            }

            return mAnimator;
        }
    }

    private Animation mAnimation;

    public Animation Animation
    {
        get
        {
            if (mAnimation == null)
            {
                mAnimation = base.GetComponent<Animation>();
            }

            return mAnimation;
        }
    }

    private MeshRenderer mMeshRenderer;

    public MeshRenderer MeshRenderer
    {
        get
        {
            if (mMeshRenderer == null)
            {
                mMeshRenderer = base.GetComponent<MeshRenderer>();
            }

            return mMeshRenderer;
        }
    }

    private MeshRenderer mChildMeshRenderer;

    public MeshRenderer ChildMeshRenderer
    {
        get
        {
            if (mChildMeshRenderer == null)
            {
                mChildMeshRenderer = base.GetComponentInChildren<MeshRenderer>();
            }

            return mChildMeshRenderer;
        }
    }

    private CustomButton mCustomButton;

    public CustomButton CustomButton
    {
        get
        {
            if (mCustomButton == null)
            {
                mCustomButton = base.GetComponent<CustomButton>();
            }

            return mCustomButton;
        }
    }

    private CanvasGroup mCanvasGroup;

    public CanvasGroup CanvasGroup
    {
        get
        {
            if (mCanvasGroup == null)
            {
                mCanvasGroup = base.GetComponent<CanvasGroup>();
            }

            return mCanvasGroup;
        }
    }

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

    #endregion

    #region Methods

    public virtual void Initialize(GameManager gameManager)
    {
        GameManager = gameManager;
    }

    public virtual void Initialize(GameManager gameManager, bool isActive = true)
    {
        Initialize(gameManager);
        gameObject.SetActive(isActive);
    }

    #endregion
}
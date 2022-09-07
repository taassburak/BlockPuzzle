using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-5000)]
[ExecuteInEditMode]
public class GizmosFromEveryWhereManager : MonoBehaviour
{
    public static GizmosFromEveryWhereManager _instance;
    public static DrawingEvent gizmosDrawingEvents = new DrawingEvent();

#if UNITY_EDITOR
    void OnDrawGizmos() => gizmosDrawingEvents?.Invoke();
    void OnDisable() => RemoveAllListeners();
    void OnApplicationQuit() => RemoveAllListeners();
    void FixedUpdate() => gizmosDrawingEvents.gizmoF.RemoveAllListeners();
    void Update() => DrawingEvent.gizmoU.RemoveAllListeners();
#endif
    static void RemoveAllListeners() => gizmosDrawingEvents.RemoveAllListeners();

    static void CreateInstanceIfClassInstanceNotExist()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<GizmosFromEveryWhereManager>();
            if (_instance == null)
            {
                GameObject gO = new GameObject();
                // gO.name = $"[{typeof(GizmosFromEveryWhereManager).Name}]";
                gO.name = "__Gizmos__";
                _instance = gO.AddComponent<GizmosFromEveryWhereManager>();
            }
        }
    }

    public class DrawingEvent : UnityEvent
    {
        public UnityEvent gizmoF = new UnityEvent();
        public static UnityEvent gizmoU = new UnityEvent();

        public void Invoke()
        {
            gizmoF.Invoke();
            gizmoU.Invoke();
        }

        public void AddListener(UnityAction call)
        {
#if UNITY_EDITOR
            CreateInstanceIfClassInstanceNotExist();
            if (Time.inFixedTimeStep)
            {
                gizmoF.AddListener(call);
            }
            else
            {
                gizmoU.AddListener(call);
            }
#endif
        }

        public void RemoveAllListeners()
        {
            gizmoF.RemoveAllListeners();
            gizmoU.RemoveAllListeners();
        }
    }
}
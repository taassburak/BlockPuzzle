using UnityEngine;

public static class TransformTools
{
    public static RaycastHit RaycastHit(Transform origin, Vector3 direction, float distance, int layerMask)
    {
        RaycastHit hit;
        Physics.Raycast(origin.position, Vector3.down, out hit, 1, layerMask);
        return hit;
    }

    public static void PosAndRot(this Transform transform, Transform target)
    {
        transform.position = target.position;
        transform.eulerAngles = target.eulerAngles;
    }

    public static void X(this Transform tr, float value)
    {
        tr.localPosition = new Vector3(tr.localPosition.x + value, tr.localPosition.y, tr.localPosition.z);
    }

    public static void Y(this Transform tr, float value)
    {
        tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y + value, tr.localPosition.z);
    }

    public static void Z(this Transform tr, float value)
    {
        tr.localPosition = new Vector3(tr.localPosition.x, tr.localPosition.y, tr.localPosition.z + value);
    }

    public static void SetActive(this Component component, bool isActive)
    {
        component.gameObject.SetActive(isActive);
    }
}
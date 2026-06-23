using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Map Bounds")]
    [SerializeField] private Transform bottomLeft;
    [SerializeField] private Transform topRight;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float clampedX = Mathf.Clamp(
            target.position.x,
            bottomLeft.position.x + halfWidth,
            topRight.position.x - halfWidth);

        float clampedY = Mathf.Clamp(
            target.position.y,
            bottomLeft.position.y + halfHeight,
            topRight.position.y - halfHeight);

        transform.position = new Vector3(
            clampedX,
            clampedY,
            transform.position.z);
    }
}
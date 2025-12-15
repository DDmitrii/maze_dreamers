using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Игрок
    public float smoothSpeed = 0.5f;
    public Vector3 offset = new Vector3(0, 0, -10); // Камера на -10 по Z для 2D

    private void LateUpdate()
{
    if (target == null) return;
    transform.position = target.position + offset;
}
}
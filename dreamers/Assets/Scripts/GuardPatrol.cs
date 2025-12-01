using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 2f;
    public float reachDistance = 0.1f;

    public Transform player;
    public float detectRadius = 1.5f;

    private int currentIndex = 0;

    public float viewAngle = 90f; // общий угол обзора в градусах
    public Color viewColor = new Color(1f, 1f, 0f, 0.3f);
    public Color detectColor = Color.red;

    private void Update()
    {
        Patrol();
        DetectPlayer();
    }

    private void Patrol()
{
    if (waypoints == null || waypoints.Length == 0)
        return;

    Transform target = waypoints[currentIndex];
    Vector3 direction = (target.position - transform.position).normalized;

    // Движение
    transform.position += direction * moveSpeed * Time.deltaTime;

    // Поворот: считаем, что "вперёд" у спрайта — это вверх (Y)
    if (direction.sqrMagnitude > 0.0001f)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Если вперёд = up, добавляем -90 градусов; если right, можно без сдвига
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    if (Vector3.Distance(transform.position, target.position) < reachDistance)
    {
        currentIndex = (currentIndex + 1) % waypoints.Length;
    }
}
    private void DetectPlayer()
{
    if (player == null) return;

    Vector3 toPlayer = player.position - transform.position;
    float distanceToPlayer = toPlayer.magnitude;

    // 1. Радиус
    if (distanceToPlayer > detectRadius)
        return;

    // 2. Угол
    Vector3 forwardGuard = transform.up; // ВАЖНО: см. пункт 2 ниже
    float angle = Vector3.Angle(forwardGuard, toPlayer.normalized);

    float halfAngle = viewAngle * 0.5f;
    if (angle > halfAngle)
        return;

    // 3. Raycast
    // RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, distanceToPlayer);
    // if (hit.collider != null && hit.collider.transform != player)
    //     return;

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}


}

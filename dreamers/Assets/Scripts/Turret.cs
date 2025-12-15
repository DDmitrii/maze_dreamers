using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint; // Точка, откуда вылетает снаряд
    public float fireRate = 2f; // Выстрел каждые 2 секунды
    public float bulletSpeed = 8f;

    public Transform player;
    public float rotationSpeed = 100f;

    private float nextFireTime = 0f;

    private void Update()
    {
        // Поворот к игроку
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Стрельба
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Fire()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        // Создаём снаряд
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        // Настраиваем скорость (если нужно переопределить)
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = bulletSpeed;
        }

        Debug.Log("Турель выстрелила!");
    }
}

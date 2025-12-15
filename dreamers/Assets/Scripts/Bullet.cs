using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 5f; // Автоуничтожение через 5 секунд

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        // Движение вперёд относительно направления снаряда
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок попал под снаряд! Рестарт...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Уничтожаем снаряд при столкновении со стеной
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}

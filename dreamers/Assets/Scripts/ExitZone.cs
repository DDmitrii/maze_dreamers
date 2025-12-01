using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Пока просто перезапускаем сцену как заглушку для "следующего уровня"
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

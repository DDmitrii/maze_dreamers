using UnityEngine;

public class LeverSwitch : MonoBehaviour
{
    public Door door; // Ссылка на дверь
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что это игрок и рычаг ещё не активирован
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;

            // Открываем дверь
            if (door != null)
            {
                door.Open();
            }

            // Меняем цвет рычага на зелёный (показываем, что активирован)
            GetComponent<SpriteRenderer>().color = Color.green;

            Debug.Log("Рычаг активирован!");
        }
    }
}

using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private Collider2D doorCollider;
    
    void Start()
    {
        doorCollider = GetComponent<Collider2D>();
    }
    
    public void OpenDoor()
    {
        // 1. Сдвигаем дверь
        transform.position += new Vector3(3, 0, 0);
        
        // 2. Отключаем коллайдер - теперь можно пройти!
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
        
        // 3. Меняем цвет на открытый
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Полупрозрачный
        }
        
        Debug.Log("ДВЕРЬ ОТКРЫТА! Можно пройти.");
    }
}
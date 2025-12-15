using UnityEngine;

public class TimedLever : MonoBehaviour
{
    public DoorTimedPuzzle door;
    public float activeTime = 4f;
    public bool isLever1 = true; // true для первого рычага, false для второго
    
    private bool isActivated = false;
    private float timer = 0f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Activate();
        }
    }

    private void Activate()
    {
        // Перезапускаем таймер даже если уже активирован
        isActivated = true;
        timer = activeTime;
        spriteRenderer.color = Color.green;
        
        if (door != null)
        {
            if (isLever1)
                door.SetLever1(true);
            else
                door.SetLever2(true);
        }
        
        Debug.Log($"Рычаг {(isLever1 ? "1" : "2")} активирован на {activeTime} сек");
    }

    private void Update()
    {
        if (isActivated)
        {
            timer -= Time.deltaTime;

            // Мигание когда время заканчивается
            if (timer <= 1f)
            {
                spriteRenderer.color = Time.time % 0.2f < 0.1f ? Color.red : Color.yellow;
            }

            if (timer <= 0f)
            {
                Deactivate();
            }
        }
    }

    private void Deactivate()
    {
        isActivated = false;
        spriteRenderer.color = originalColor;
        
        if (door != null)
        {
            if (isLever1)
                door.SetLever1(false);
            else
                door.SetLever2(false);
        }
        
        Debug.Log($"Рычаг {(isLever1 ? "1" : "2")} погас!");
    }
}

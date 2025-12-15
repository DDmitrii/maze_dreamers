using UnityEngine;

public class TimedLever : MonoBehaviour
{
    public DoorTimedPuzzle door;
    public float activeTime = 4f; // Сколько секунд горит рычаг
    
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
        if (other.CompareTag("Player") && !isActivated)
        {
            Activate();
        }
    }

    private void Activate()
    {
        isActivated = true;
        timer = activeTime;
        spriteRenderer.color = Color.green;
        
        if (door != null)
        {
            door.ActivateLever1();
        }
        
        Debug.Log($"Таймер рычага запущен: {activeTime} сек");
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
            door.DeactivateLever1();
        }
        
        Debug.Log("Рычаг 1 погас! Можно активировать снова.");
    }
}

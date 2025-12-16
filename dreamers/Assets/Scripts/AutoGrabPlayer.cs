using UnityEngine;

public class AutoGrabPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float grabDistance = 1.5f;
    
    private Rigidbody2D rb;
    private GameObject heldWire;
    private SimpleWire heldWireScript;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Движение
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(x, y) * moveSpeed;
        
        // Взять провод
        if (heldWire == null && Input.GetKeyDown(KeyCode.E))
        {
            TryGrab();
        }
        
        // Бросить провод
        if (heldWire != null && Input.GetKeyDown(KeyCode.Q))
        {
            DropWire();
        }
        
        // Движение провода
        if (heldWire != null)
        {
            heldWire.transform.position = transform.position + new Vector3(0.5f, 0, 0);
        }
    }
    
    void TryGrab()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, grabDistance);
        
        foreach (Collider2D col in colliders)
        {
            SimpleWire wireScript = col.GetComponent<SimpleWire>();
            if (wireScript != null && !wireScript.IsConnected())
            {
                heldWire = col.gameObject;
                heldWireScript = wireScript;
                Debug.Log("Взял провод (E)");
                return;
            }
        }
    }
    
    void DropWire()
    {
        if (heldWire != null)
        {
            // Ищем ближайший разъем
            Collider2D[] colliders = Physics2D.OverlapCircleAll(heldWire.transform.position, 0.5f);
            
            foreach (Collider2D col in colliders)
            {
                SimpleSocket socket = col.GetComponent<SimpleSocket>();
                if (socket != null && socket.IsSocketAvailable())
                {
                    // Подключаем к разъему
                    heldWire.transform.position = socket.transform.position;
                    socket.ConnectWire(heldWireScript);
                    Debug.Log($"Положил провод в разъем {socket.socketNumber}");
                    
                    heldWire = null;
                    heldWireScript = null;
                    return;
                }
            }
            
            // Если не нашли разъем - просто бросаем
            Debug.Log("Бросил провод (не попал в разъем)");
            heldWire = null;
            heldWireScript = null;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, grabDistance);
    }
}
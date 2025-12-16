// SimplePlayerGrab.cs - добавь на Player2
using UnityEngine;

public class SimplePlayerGrab : MonoBehaviour
{
    public float grabRange = 1.5f;
    public KeyCode grabKey = KeyCode.E;
    
    private SimpleWire heldWire;
    private Transform holdPoint;
    
    void Start()
    {
        // Создаем точку для удержания провода
        GameObject point = new GameObject("HoldPoint");
        point.transform.parent = transform;
        point.transform.localPosition = new Vector3(0.5f, 0, 0);
        holdPoint = point.transform;
    }
    
    void Update()
    {
        // Движение (оставь свой Player2Controller для движения)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveInput = new Vector2(moveX, moveY).normalized;
        GetComponent<Rigidbody2D>().linearVelocity = moveInput * 5f;
        
        // Взятие/бросок провода
        if (Input.GetKeyDown(grabKey))
        {
            if (heldWire == null)
            {
                GrabNearestWire();
            }
            else
            {
                DropWire();
            }
        }
        
        // Держим провод
        if (heldWire != null)
        {
            heldWire.transform.position = holdPoint.position;
        }
    }
    
    void GrabNearestWire()
    {
        // Ищем ближайший провод
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, grabRange);
        
        foreach (Collider2D col in colliders)
        {
            SimpleWire wire = col.GetComponent<SimpleWire>();
            if (wire != null)
            {
                heldWire = wire;
                Debug.Log("Поднял провод!");
                return;
            }
        }
    }
    
    void DropWire()
    {
        if (heldWire != null)
        {
            // Проверяем, попал ли на разъем
            Collider2D[] colliders = Physics2D.OverlapCircleAll(heldWire.transform.position, 0.5f);
            foreach (Collider2D col in colliders)
            {
                SimpleSocket socket = col.GetComponent<SimpleSocket>();
                if (socket != null)
                {
                    heldWire.transform.position = socket.transform.position;
                }
            }
            
            heldWire = null;
            Debug.Log("Бросил провод!");
        }
    }
    
    // Рисуем радиус взятия
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, grabRange);
    }
}
// PlayerWiresLevel.cs - ТОЛЬКО для твоего уровня с проводами
using UnityEngine;

public class PlayerWiresLevel : MonoBehaviour
{
    [Header("Движение")]
    public float moveSpeed = 5f;
    
    [Header("Взаимодействие с проводами")]
    public Transform wireHoldPoint;  // Создай пустой объект как дочерний
    public float wirePickupRange = 1.5f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private SimpleWire heldWire;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Автоматически создаем точку для провода если забыли
        if (wireHoldPoint == null)
        {
            GameObject holdPointObj = new GameObject("WireHoldPoint");
            holdPointObj.transform.SetParent(transform);
            holdPointObj.transform.localPosition = new Vector3(0.5f, 0, 0);
            wireHoldPoint = holdPointObj.transform;
        }
    }
    
    void Update()
    {
        // Движение (как в оригинальном PlayerController)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;
        
        // Взаимодействие с проводами (только в твоем уровне!)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldWire == null)
                TryPickUpWire();
            else
                DropWire();
        }
        
        // Если держим провод - двигаем его с собой
        if (heldWire != null && wireHoldPoint != null)
        {
            heldWire.transform.position = wireHoldPoint.position;
        }
    }
    
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
    
    void TryPickUpWire()
    {
        SimpleWire[] allWires = FindObjectsOfType<SimpleWire>();
        
        foreach (SimpleWire wire in allWires)
        {
            float distance = Vector2.Distance(transform.position, wire.transform.position);
            if (distance < wirePickupRange)
            {
                heldWire = wire;
                Debug.Log($"Игрок поднял {wire.wireColor} провод");
                return;
            }
        }
    }
    
    void DropWire()
    {
        if (heldWire != null)
        {
            Debug.Log($"Игрок положил {heldWire.wireColor} провод");
            heldWire = null;
        }
    }
}
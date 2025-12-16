using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    [Header("Настройки движения")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    
    [Header("Взаимодействие с проводами")]
    public Transform wireHoldPoint;
    public float wirePickupRange = 1.5f;
    public KeyCode pickupKey = KeyCode.E;
    
    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private SimpleWire heldWire;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (wireHoldPoint == null)
            CreateDefaultHoldPoint();
    }
    
    void Update()
    {
        HandleMovementInput();
        HandleWireInteraction();
        UpdateHeldWirePosition();
    }
    
    void HandleMovementInput()
    {
        Vector2 targetVelocity = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized * moveSpeed;
        
        float changeRate = (targetVelocity.magnitude > 0.1f ? acceleration : deceleration) * Time.deltaTime;
        currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, changeRate);
    }
    
    void HandleWireInteraction()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            if (heldWire == null)
            {
                TryPickUpWire();
            }
            else
            {
                DropWire();
            }
        }
    }
    
    void TryPickUpWire()
    {
        SimpleWire[] allWires = FindObjectsByType<SimpleWire>(FindObjectsSortMode.None);
        SimpleWire closestWire = null;
        float closestDistance = Mathf.Infinity;
        
        foreach (SimpleWire wire in allWires)
        {
            float distance = Vector2.Distance(transform.position, wire.transform.position);
            if (distance < wirePickupRange && distance < closestDistance && wire.CanBePickedUp())
            {
                closestWire = wire;
                closestDistance = distance;
            }
        }
        
        if (closestWire != null)
        {
            heldWire = closestWire;
            closestWire.PickUpByPlayer(wireHoldPoint);
            Debug.Log($"[Player2] Поднял {closestWire.wireColor} провод");
            
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = Color.yellow;
        }
    }
    
    void DropWire()
    {
        if (heldWire != null)
        {
            heldWire.DropByPlayer();
            Debug.Log($"[Player2] Положил {heldWire.wireColor} провод");
            heldWire = null;
            
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = Color.white;
        }
    }
    
    void UpdateHeldWirePosition()
    {
        if (heldWire != null && wireHoldPoint != null)
        {
            heldWire.transform.position = wireHoldPoint.position;
        }
    }
    
    void FixedUpdate()
    {
        rb.linearVelocity = currentVelocity;
    }
    
    void CreateDefaultHoldPoint()
    {
        GameObject point = new GameObject("WireHoldPoint");
        point.transform.SetParent(transform);
        point.transform.localPosition = new Vector3(0.5f, 0, 0);
        wireHoldPoint = point.transform;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, wirePickupRange);
        
        if (wireHoldPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(wireHoldPoint.position, 0.1f);
        }
    }
}
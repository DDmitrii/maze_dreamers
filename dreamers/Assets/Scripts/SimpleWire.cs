using UnityEngine;

public class SimpleWire : MonoBehaviour
{
    public WireColors wireColor = WireColors.Blue;
    public float pickUpRadius = 0.5f;
    
    public SpriteRenderer wireRenderer;
    
    private Vector3 startPosition;
    private bool isDragging = false;
    private SimpleSocket connectedSocket;
    private bool isHeldByPlayer = false;
    private Transform playerHoldPoint;
    
    void Start()
    {
        if (wireRenderer == null)
            wireRenderer = GetComponent<SpriteRenderer>();
            
        startPosition = transform.position;
        SetWireColor();
    }
    
    void SetWireColor()
    {
        if (wireRenderer != null)
        {
            switch (wireColor)
            {
                case WireColors.Blue:
                    wireRenderer.color = Color.blue;
                    break;
                case WireColors.Red:
                    wireRenderer.color = Color.red;
                    break;
                case WireColors.Green:
                    wireRenderer.color = Color.green;
                    break;
            }
        }
    }
    
    void OnMouseDown()
    {
        if (isHeldByPlayer) return;
        
        Debug.Log($"Кликнули на провод {wireColor}");
        isDragging = true;
        
        if (connectedSocket != null)
        {
            connectedSocket.DisconnectWire();
            connectedSocket = null;
        }
    }
    
    void OnMouseDrag()
    {
        if (!isDragging || isHeldByPlayer) return;
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        
        transform.position = mousePos;
    }
    
    void OnMouseUp()
    {
        if (!isDragging || isHeldByPlayer) return;
        
        isDragging = false;
        Debug.Log($"Отпустили провод {wireColor}");
        
        TryConnectToSocket();
    }
    
    void TryConnectToSocket()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, pickUpRadius);
        
        foreach (Collider2D col in hitColliders)
        {
            SimpleSocket socket = col.GetComponent<SimpleSocket>();
            if (socket != null && socket.IsSocketAvailable())
            {
                ConnectToSocket(socket);
                return;
            }
        }
        
        ReturnToStart();
    }
    
    void ConnectToSocket(SimpleSocket socket)
    {
        connectedSocket = socket;
        socket.ConnectWire(this);
        transform.position = socket.transform.position;
        Debug.Log($"{wireColor} провод подключен к разъему {socket.socketNumber}");
    }
    
    void ReturnToStart()
    {
        transform.position = startPosition;
        Debug.Log($"{wireColor} провод возвращен на место");
    }
    
    public void PickUpByPlayer(Transform holdPoint)
    {
        isHeldByPlayer = true;
        isDragging = false;
        playerHoldPoint = holdPoint;
        
        if (connectedSocket != null)
        {
            connectedSocket.DisconnectWire();
            connectedSocket = null;
        }
        
        // Отключаем коллайдер когда игрок держит
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }
    
    public void DropByPlayer()
    {
        isHeldByPlayer = false;
        playerHoldPoint = null;
        
        // Включаем коллайдер обратно
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
        
        TryConnectToSocket();
    }
    
    public bool CanBePickedUp()
    {
        return !isHeldByPlayer && !isDragging;
    }
    
    void Update()
    {
        // Если игрок держит - двигаем провод за ним
        if (isHeldByPlayer && playerHoldPoint != null)
        {
            transform.position = playerHoldPoint.position;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRadius);
    }
    
    public bool IsConnected()
    {
        return connectedSocket != null;
    }
    
    public SimpleSocket GetConnectedSocket()
    {
        return connectedSocket;
    }
    
    public void ResetToStart()
    {
        ReturnToStart();
        if (connectedSocket != null)
        {
            connectedSocket.DisconnectWire();
            connectedSocket = null;
        }
        isHeldByPlayer = false;
        isDragging = false;
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }
}
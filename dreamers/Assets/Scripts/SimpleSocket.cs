using UnityEngine;

public class SimpleSocket : MonoBehaviour
{
    [Header("Настройки разъема")]
    public int socketNumber = 1;
    
    [Header("Компоненты")]
    public SpriteRenderer socketRenderer;
    
    [Header("Цвета")]
    public Color defaultColor = Color.gray;
    public Color occupiedColor = Color.white;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    
    private SimpleWire connectedWire;
    
    void Start()
    {
        if (socketRenderer == null)
            socketRenderer = GetComponent<SpriteRenderer>();
            
        socketRenderer.color = defaultColor;
    }
    
    // Подключить провод
    public void ConnectWire(SimpleWire wire)
    {
        connectedWire = wire;
        socketRenderer.color = occupiedColor;
        Debug.Log($"Разъем {socketNumber}: подключен {wire.wireColor} провод");
    }
    
    // ОТСОЕДИНИТЬ провод - ДОБАВЬ ЭТОТ МЕТОД!
    public void DisconnectWire()
    {
        if (connectedWire != null)
        {
            Debug.Log($"Разъем {socketNumber}: отключен {connectedWire.wireColor} провод");
            connectedWire = null;
        }
        socketRenderer.color = defaultColor;
    }
    
    // Проверить свободен ли разъем
    public bool IsSocketAvailable()
    {
        return connectedWire == null;
    }
    
    // Показать результат (правильно/неправильно)
    public void ShowResult(bool isCorrect)
    {
        socketRenderer.color = isCorrect ? correctColor : wrongColor;
    }
    
    // Сбросить разъем
    public void ResetSocket()
    {
        socketRenderer.color = defaultColor;
        connectedWire = null;
    }
    
    // Получить номер разъема
    public int GetSocketNumber()
    {
        return socketNumber;
    }
}
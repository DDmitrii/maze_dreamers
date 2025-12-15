using UnityEngine;

public class Door : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(false); // Дверь исчезает
        Debug.Log("Дверь открыта!");
    }

    public void Close()
    {
        gameObject.SetActive(true); // Дверь появляется
        Debug.Log("Дверь закрыта!");
    }
}
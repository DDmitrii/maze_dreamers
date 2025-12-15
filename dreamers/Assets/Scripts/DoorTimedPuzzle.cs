using UnityEngine;

public class DoorTimedPuzzle : MonoBehaviour
{
    private bool lever1Active = false;
    private bool lever2Active = false;
    private bool isOpen = false;

    public void ActivateLever1()
    {
        if (isOpen) return;
        lever1Active = true;
        Debug.Log("Рычаг 1 активирован! Быстрее ко второму!");
        CheckBothLevers();
    }

    public void DeactivateLever1()
    {
        if (isOpen) return;
        lever1Active = false;
        lever2Active = false; // Сброс всей головоломки
        Debug.Log("Рычаг 1 погас! Попробуй снова.");
    }

    public void ActivateLever2()
    {
        if (isOpen) return;
        lever2Active = true;
        Debug.Log("Рычаг 2 активирован!");
        CheckBothLevers();
    }

    private void CheckBothLevers()
    {
        if (lever1Active && lever2Active)
        {
            Open();
        }
    }

    private void Open()
    {
        isOpen = true;
        gameObject.SetActive(false);
        Debug.Log("Дверь открыта! Оба рычага активны одновременно!");
    }
}

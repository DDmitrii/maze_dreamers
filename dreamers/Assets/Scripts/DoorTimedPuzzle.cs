using UnityEngine;

public class DoorTimedPuzzle : MonoBehaviour
{
    private bool lever1Active = false;
    private bool lever2Active = false;
    private bool isOpen = false;

    public void SetLever1(bool active)
    {
        if (isOpen) return;
        lever1Active = active;
        
        if (active)
            Debug.Log("Рычаг 1 активен!");
        else
            Debug.Log("Рычаг 1 погас!");
            
        CheckBothLevers();
    }

    public void SetLever2(bool active)
    {
        if (isOpen) return;
        lever2Active = active;
        
        if (active)
            Debug.Log("Рычаг 2 активен!");
        else
            Debug.Log("Рычаг 2 погас!");
            
        CheckBothLevers();
    }

    private void CheckBothLevers()
    {
        if (lever1Active && lever2Active && !isOpen)
        {
            Open();
        }
    }

    private void Open()
    {
        isOpen = true;
        gameObject.SetActive(false);
        Debug.Log("ДВЕРЬ ОТКРЫТА! Оба рычага горели одновременно!");
    }
}

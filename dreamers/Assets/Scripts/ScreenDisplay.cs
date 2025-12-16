// ScreenDisplay.cs - минимальная версия
using UnityEngine;

public class ScreenDisplay : MonoBehaviour
{
    // ВАЖНО: метод называется ShowPuzzleStep
    public void ShowPuzzleStep(WireColors color, int socketNumber)
    {
        string colorName = GetColorName(color);
        Debug.Log($"══════════════════════════════════");
        Debug.Log($"  ЭКРАН: {colorName} провод → Разъем {socketNumber}");
        Debug.Log($"══════════════════════════════════");
    }
    
    public void ShowResult(bool isCorrect)
    {
        if (isCorrect)
        {
            Debug.Log($"██████████████████████████████");
            Debug.Log($"  ✓ ПРАВИЛЬНО! Следующий шаг...");
            Debug.Log($"██████████████████████████████");
        }
        else
        {
            Debug.Log($"▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            Debug.Log($"  ✗ ОШИБКА! Начинаем заново...");
            Debug.Log($"▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
        }
    }
    
    public void ShowComplete()
    {
        Debug.Log($"☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆");
        Debug.Log($"  🎉 ГОЛОВОЛОМКА РЕШЕНА! ДВЕРЬ ОТКРЫТА!");
        Debug.Log($"☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆☆");
    }
    
    string GetColorName(WireColors color)
    {
        switch (color)
        {
            case WireColors.Blue: return "СИНИЙ";
            case WireColors.Red: return "КРАСНЫЙ";
            case WireColors.Green: return "ЗЕЛЕНЫЙ";
            default: return "???";
        }
    }
}
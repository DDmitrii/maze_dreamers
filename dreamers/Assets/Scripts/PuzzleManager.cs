using UnityEngine;
using System.Collections.Generic;

// Этот класс нужен для хранения шага головоломки
public class PuzzleStep
{
    public WireColors requiredColor;
    public int requiredSocket;
}

public class PuzzleManager : MonoBehaviour
{
    [Header("Список проводов")]
    public List<SimpleWire> wires;
    
    [Header("Список разъемов")] 
    public List<SimpleSocket> sockets;
    
    [Header("Экран головоломки")]
    public ScreenDisplay puzzleScreen;
    
    [Header("Настройки головоломки")]
    public int totalSteps = 3; // Сколько шагов в головоломке
    
    [Header("События")]
    public UnityEngine.Events.UnityEvent onPuzzleSolved; // Сюда подключим дверь позже
    public UnityEngine.Events.UnityEvent onStepCompleted;
    
    // Приватные переменные
    private List<PuzzleStep> steps = new List<PuzzleStep>();
    private int currentStepIndex = 0;
    private bool isPuzzleActive = false;
    
    void Start()
    {
        // Проверяем что списки заполнены
        Debug.Log("Проводов в списке: " + wires.Count);
        Debug.Log("Разъемов в списке: " + sockets.Count);
        
        // Если списки пустые, выводим предупреждение
        if (wires.Count == 0 || sockets.Count == 0)
        {
            Debug.LogWarning("Заполните списки проводов и разъемов в Inspector!");
        }
    }
    
    // Функция чтобы начать головоломку (вызовется при нажатии кнопки START)
    public void StartNewPuzzle()
    {
        GenerateSteps();
        currentStepIndex = 0;
        isPuzzleActive = true;
        
        ShowCurrentStep();
        Debug.Log("=== НОВАЯ ГОЛОВОЛОМКА НАЧАТА ===");
        
        // Обновляем экран
        if (puzzleScreen != null)
        {
            var step = steps[currentStepIndex];
            puzzleScreen.ShowPuzzleStep(step.requiredColor, step.requiredSocket);
        }
    }
    
    void GenerateSteps()
    {
        steps.Clear();
        
        // Создаем случайных шагов (сколько указано в totalSteps)
        for (int i = 0; i < totalSteps; i++)
        {
            PuzzleStep step = new PuzzleStep();
            
            // Случайный цвет провода
            step.requiredColor = (WireColors)Random.Range(0, 3);
            
            // Случайный номер разъема (1, 2 или 3)
            step.requiredSocket = Random.Range(1, 4);
            
            steps.Add(step);
        }
        
        Debug.Log("Сгенерирована новая головоломка из " + steps.Count + " шагов:");
        for (int i = 0; i < steps.Count; i++)
        {
            Debug.Log($"Шаг {i+1}: {steps[i].requiredColor} провод → разъем {steps[i].requiredSocket}");
        }
    }
    
    void ShowCurrentStep()
    {
        if (currentStepIndex < steps.Count)
        {
            PuzzleStep step = steps[currentStepIndex];
            Debug.Log($"\nТЕКУЩИЙ ШАГ {currentStepIndex + 1}/{totalSteps}:");
            Debug.Log($"Подключите {step.requiredColor} провод к разъему {step.requiredSocket}");
        }
    }
    
    // Эта функция будет вызываться когда игрок нажимает кнопку "Проверить"
    public void CheckSolution()
    {
        if (!isPuzzleActive)
        {
            Debug.Log("Головоломка еще не начата! Нажмите START.");
            return;
        }
        
        Debug.Log("\n=== ПРОВЕРКА РЕШЕНИЯ ===");
        
        bool isStepCorrect = true;
        PuzzleStep currentStep = steps[currentStepIndex];
        
        Debug.Log($"Проверяем: должен быть {currentStep.requiredColor} провод в разъеме {currentStep.requiredSocket}");
        
        // Проверяем каждый провод
        foreach (SimpleWire wire in wires)
        {
            // Проверяем подключен ли провод
            if (wire.IsConnected())
            {
                SimpleSocket socket = wire.GetConnectedSocket();
                Debug.Log($"- {wire.wireColor} провод подключен к разъему {socket.socketNumber}");
                
                // Проверяем правильность подключения
                if (wire.wireColor == currentStep.requiredColor)
                {
                    // Правильный цвет провода
                    if (socket.socketNumber == currentStep.requiredSocket)
                    {
                        // И правильный разъем!
                        Debug.Log($"  ✓ Правильно!");
                    }
                    else
                    {
                        // Неправильный разъем
                        Debug.Log($"  ✗ {wire.wireColor} провод должен быть в разъеме {currentStep.requiredSocket}, а он в {socket.socketNumber}");
                        isStepCorrect = false;
                    }
                }
            }
            else
            {
                Debug.Log($"- {wire.wireColor} провод не подключен");
                // Если это тот провод который нужен для текущего шага
                if (wire.wireColor == currentStep.requiredColor)
                {
                    isStepCorrect = false;
                }
            }
        }
        
        // Проверяем результат
        if (isStepCorrect)
        {
            Debug.Log($"\n✓ Шаг {currentStepIndex + 1} выполнен ПРАВИЛЬНО!");
            
            // Показываем результат на экране
            if (puzzleScreen != null)
            {
                puzzleScreen.ShowResult(true);
                Invoke(nameof(ShowNextStepOnScreen), 1f);
            }
            
            // Вызываем событие успешного шага
            if (onStepCompleted != null)
                onStepCompleted.Invoke();
            
            currentStepIndex++;
            
            // Проверяем закончена ли головоломка
            if (currentStepIndex >= totalSteps)
            {
                PuzzleComplete();
            }
            else
            {
                Debug.Log("Переходим к следующему шагу...");
                ShowCurrentStep();
            }
        }
        else
        {
            Debug.Log("\n✗ ОШИБКА! Неправильное подключение.");
            
            // Показываем ошибку на экране
            if (puzzleScreen != null)
            {
                puzzleScreen.ShowResult(false);
                Invoke(nameof(ResetScreen), 1.5f);
            }
            
            Debug.Log("Начинаем головоломку заново...");
            
            // Сбрасываем провода
            foreach (SimpleWire wire in wires)
            {
                wire.ResetToStart();
            }
            
            // Начинаем заново
            StartNewPuzzle();
        }
    }
    
    void ShowNextStepOnScreen()
    {
        if (currentStepIndex < steps.Count && puzzleScreen != null)
        {
            var step = steps[currentStepIndex];
            puzzleScreen.ShowPuzzleStep(step.requiredColor, step.requiredSocket);
        }
    }
    
    void ResetScreen()
    {
        if (puzzleScreen != null)
        {
            var step = steps[currentStepIndex];
            puzzleScreen.ShowPuzzleStep(step.requiredColor, step.requiredSocket);
        }
    }
    
    void PuzzleComplete()
    {
        isPuzzleActive = false;
        Debug.Log("\n★★★★★★★★★★★★★★★★★★★★★★★★★★★★");
        Debug.Log("★★★★★  ГОЛОВОЛОМКА РЕШЕНА!  ★★★★★");
        Debug.Log("★★★★★   ДВЕРЬ ОТКРЫТА!   ★★★★★");
        Debug.Log("★★★★★★★★★★★★★★★★★★★★★★★★★★★★");
        
        // Показываем завершение на экране
        if (puzzleScreen != null)
        {
            puzzleScreen.ShowComplete();
        }
        
        // Вызываем событие - дверь откроется
        if (onPuzzleSolved != null)
            onPuzzleSolved.Invoke();
    }
    
    // Функция для получения текущего шага (для UI)
    public PuzzleStep GetCurrentStep()
    {
        if (currentStepIndex < steps.Count)
            return steps[currentStepIndex];
        
        return new PuzzleStep();
    }
    
    // Проверка завершена ли головоломка
    public bool IsPuzzleComplete()
    {
        return currentStepIndex >= totalSteps;
    }
    
    // Проверка активна ли головоломка
    public bool IsPuzzleActive()
    {
        return isPuzzleActive;
    }
    
    // Получить номер текущего шага (для UI)
    public int GetCurrentStepNumber()
    {
        return currentStepIndex + 1;
    }
    
    // Получить общее количество шагов
    public int GetTotalSteps()
    {
        return totalSteps;
    }
}
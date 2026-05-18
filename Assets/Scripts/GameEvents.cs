using System;

public static class GameEvents
{
    
    public static event Action<float, float> OnBlockLanded; 
    public static event Action OnPerfectDrop;
    public static event Action OnBlockFailed; 

   
    public static event Action<int> OnScoreChanged;
    public static event Action<int> OnPerfectStreakChanged;
    public static event Action<int> OnTowersPlacedChanged;
    public static event Action OnGameOver;

    
    public static void TriggerBlockLanded(float height, float errorX) => OnBlockLanded?.Invoke(height, errorX);
    public static void TriggerPerfectDrop() => OnPerfectDrop?.Invoke();
    public static void TriggerBlockFailed() => OnBlockFailed?.Invoke();

    public static void TriggerScoreChanged(int score) => OnScoreChanged?.Invoke(score);
    public static void TriggerPerfectStreakChanged(int streak) => OnPerfectStreakChanged?.Invoke(streak);
    public static void TriggerTowersPlacedChanged(int count) => OnTowersPlacedChanged?.Invoke(count);
    public static void TriggerGameOver() => OnGameOver?.Invoke();
}

using System;

public static class GameEvents
{
    
    public static event Action<float> OnBlockLanded;

    public static void TriggerBlockLanded(float height)
    {
        OnBlockLanded?.Invoke(height);
    }
}

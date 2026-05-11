using System;

public static class GameEvents
{
    public static event Action OnBlockLanded;

    public static void TriggerBlockLanded()
    {
        OnBlockLanded?.Invoke();
    }
}

using System;

public static class GameEvents
{
    public static event Action<float> OnBlockLanded;
    // NUEVO: Se dispara solo cuando el encastre es perfecto
    public static event Action OnPerfectDrop;

    public static void TriggerBlockLanded(float height) => OnBlockLanded?.Invoke(height);
    public static void TriggerPerfectDrop() => OnPerfectDrop?.Invoke();
}

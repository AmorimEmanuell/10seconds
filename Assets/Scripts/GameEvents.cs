using System;

public class GameEvents
{
    public static Action OnGameStart;

    public static void RaiseOnGameStart()
    {
        OnGameStart?.Invoke();
    }
}

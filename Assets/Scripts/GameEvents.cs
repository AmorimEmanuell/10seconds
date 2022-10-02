using System;

public class GameEvents
{
    public static Action OnGameStart;
    public static Action OnGameEnded;

    public static void RaiseOnGameStart()
    {
        OnGameStart?.Invoke();
    }

    public static void RaiseOnGameEnded()
    {
        OnGameEnded?.Invoke();
    }
}

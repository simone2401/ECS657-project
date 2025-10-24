
using System;
public class LevelEvents
{
    public static Action OnLevelWin;
    public static Action OnLevelFail;
    public static void Win() => OnLevelWin?.Invoke();
    public static void Fail() => OnLevelFail?.Invoke();
   
}

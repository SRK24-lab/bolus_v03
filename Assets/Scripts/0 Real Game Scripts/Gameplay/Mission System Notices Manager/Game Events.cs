using System;

public static class GameEvents
{
    public static Action<GameStage> OnStageTriggered;

    public static void TriggerStage(GameStage stage)
    {
        OnStageTriggered?.Invoke(stage);
    }

    public static System.Action<GameStage> OnStageHide;

    public static void HideStage(GameStage stage)
    {
        OnStageHide?.Invoke(stage);
    }
}
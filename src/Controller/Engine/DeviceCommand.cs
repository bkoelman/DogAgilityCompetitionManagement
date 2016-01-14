namespace DogAgilityCompetition.Controller.Engine
{
    /// <summary>
    /// Indicates which command is received from the wireless network.
    /// </summary>
    public enum DeviceCommand
    {
        PlaySoundA,
        ToggleElimination,
        DecreaseRefusals,
        IncreaseRefusals,
        DecreaseFaults,
        IncreaseFaults,
        ResetRun,
        Ready,
        MuteSound
    }
}
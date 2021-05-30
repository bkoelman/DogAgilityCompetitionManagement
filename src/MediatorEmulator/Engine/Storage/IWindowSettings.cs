namespace DogAgilityCompetition.MediatorEmulator.Engine.Storage
{
    /// <summary>
    /// Defines the contract for positioning data of a WinForms MDI window.
    /// </summary>
    public interface IWindowSettings
    {
        int? WindowLocationX { get; set; }

        int? WindowLocationY { get; set; }

        int WindowWidth { get; set; }

        int WindowHeight { get; set; }

        bool IsMinimized { get; set; }
    }
}

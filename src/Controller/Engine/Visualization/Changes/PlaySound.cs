using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Visualization.Changes
{
    /// <summary>
    /// A visualization change that starts playing a sound file, or mutes a currently playing sound.
    /// </summary>
    public sealed class PlaySound : NullableVisualizationChange<string>
    {
        public static PlaySound Mute => new(null);

        public PlaySound(string? path)
            : base(path)
        {
        }

        public override void ApplyTo(IVisualizationActor actor)
        {
            Guard.NotNull(actor, nameof(actor));
            actor.PlaySound(Value);
        }
    }
}

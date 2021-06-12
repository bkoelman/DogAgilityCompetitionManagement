namespace DogAgilityCompetition.Specs.Builders
{
    /// <summary>
    /// Defines the contract to use the builder design patter to compose test objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITestDataBuilder<out T>
    {
        // ReSharper disable once UnusedMemberInSuper.Global
        // Justification: This interface exists to enforce a design pattern; it is not intended for polymorphism.
        T Build();
    }
}

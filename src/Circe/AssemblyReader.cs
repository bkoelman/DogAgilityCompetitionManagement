using System.Reflection;

namespace DogAgilityCompetition.Circe;

public static class AssemblyReader
{
    public static string GetInformationalVersion()
    {
        var assembly = Assembly.GetEntryAssembly();

        CustomAttributeData? versionAttribute =
            assembly?.CustomAttributes.FirstOrDefault(data => data.AttributeType.Name == "AssemblyInformationalVersionAttribute");

        if (versionAttribute != null && versionAttribute.ConstructorArguments.Any())
        {
            CustomAttributeTypedArgument versionText = versionAttribute.ConstructorArguments.FirstOrDefault();
            return " v" + versionText.Value;
        }

        return string.Empty;
    }
}

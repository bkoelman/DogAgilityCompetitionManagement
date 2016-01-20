using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    public static class AssemblyReader
    {
        [NotNull]
        public static string GetInformationalVersion()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            CustomAttributeData versionAttribute =
                assembly.CustomAttributes.FirstOrDefault(
                    data => data.AttributeType.Name == "AssemblyInformationalVersionAttribute");

            if (versionAttribute != null && versionAttribute.ConstructorArguments.Any())
            {
                CustomAttributeTypedArgument versionText = versionAttribute.ConstructorArguments.FirstOrDefault();
                return " v" + versionText.Value;
            }

            return string.Empty;
        }
    }
}
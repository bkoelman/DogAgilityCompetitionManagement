﻿using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.MediatorEmulator.Engine.Storage.Serialization;

namespace DogAgilityCompetition.MediatorEmulator.Engine.Storage;

/// <summary>
/// Provides filesystem access to persisted files for emulated wireless networks.
/// </summary>
public sealed class NetworkConfigurationFile
{
    public NetworkConfigurationXml Configuration { get; }
    public string? FilePath { get; private set; }

    public NetworkConfigurationFile()
        : this(null, new NetworkConfigurationXml())
    {
    }

    private NetworkConfigurationFile(string? path, NetworkConfigurationXml configuration)
    {
        Guard.NotNull(configuration, nameof(configuration));

        FilePath = path;
        Configuration = configuration;
    }

    public static NetworkConfigurationFile Load(string path)
    {
        Guard.NotNullNorEmpty(path, nameof(path));

        using var reader = XmlReader.Create(path, new XmlReaderSettings
        {
            CloseInput = true
        });

        var serializer = new DataContractSerializer(typeof(NetworkConfigurationXml));

        var configuration = (NetworkConfigurationXml)serializer.ReadObject(reader)!;
        return new NetworkConfigurationFile(path, configuration);
    }

    public void SaveAs(string path)
    {
        Guard.NotNullNorEmpty(path, nameof(path));

        var settings = new XmlWriterSettings
        {
            CloseOutput = true,
            Indent = true,
            Encoding = new UTF8Encoding()
        };

        using (var writer = XmlWriter.Create(path, settings))
        {
            var serializer = new DataContractSerializer(typeof(NetworkConfigurationXml));
            serializer.WriteObject(writer, Configuration);
        }

        FilePath = path;
    }
}

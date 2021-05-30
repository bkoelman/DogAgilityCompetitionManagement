using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DogAgilityCompetition.Circe;

namespace DogAgilityCompetition.Controller.Engine.Storage.Serialization
{
    /// <summary>
    /// Provides XML serialization for <see cref="CompetitionClassModel" />.
    /// </summary>
    public sealed class ModelSerializer
    {
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        private readonly string path;

        public ModelSerializer(string path)
        {
            Guard.NotNullNorEmpty(path, nameof(path));

            this.path = path;
        }

        public CompetitionClassModel Load()
        {
            using var reader = XmlReader.Create(path, new XmlReaderSettings
            {
                CloseInput = true
            });

            var serializer = new DataContractSerializer(typeof(CompetitionClassModelXml));

            try
            {
                var xmlObject = (CompetitionClassModelXml)serializer.ReadObject(reader)!;
                return CompetitionClassModelXml.FromXmlObject(xmlObject);
            }
            catch (Exception ex)
            {
                Log.Error("Failed to load model from XML file.", ex);

                string message = $"Failed to load run configuration from file:\n\n{path}\n\n" +
                    $"Error message: {ex.Message}\n\nClick Ok to discard this file and use default settings.\n" +
                    "Click Cancel to close this application without making changes.";

                DialogResult response = MessageBox.Show(message, "Error - Dog Agility Competition Management System", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

                if (response == DialogResult.OK)
                {
                    return new CompetitionClassModel();
                }

                Environment.Exit(0);
                throw;
            }
        }

        public void Save(CompetitionClassModel model)
        {
            Guard.NotNull(model, nameof(model));

            CompetitionClassModelXml xmlObject = CompetitionClassModelXml.ToXmlObject(model);

            var settings = new XmlWriterSettings
            {
                CloseOutput = true,
                Indent = true,
                Encoding = new UTF8Encoding()
            };

            using var writer = XmlWriter.Create(path, settings);
            var serializer = new DataContractSerializer(typeof(CompetitionClassModelXml));
            serializer.WriteObject(writer, xmlObject);
        }
    }
}

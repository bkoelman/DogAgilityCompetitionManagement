using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.Controller.Engine.Storage.Serialization;

namespace DogAgilityCompetition.Controller.Engine.Storage
{
    /// <summary>
    /// Provides shared access to the in-memory competition data snapshot.
    /// </summary>
    public sealed class CacheManager
    {
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private static readonly Lazy<CacheManager> InnerDefaultInstance = new(CreateDefaultInstance, LazyThreadSafetyMode.ExecutionAndPublication);

        private readonly ModelSerializer serializer;
        private readonly object stateLock = new();
        private readonly FreshObjectReference<CompetitionClassModel> activeModel = new(new CompetitionClassModel());

        public static CacheManager DefaultInstance => InnerDefaultInstance.Value;

        public CompetitionClassModel ActiveModel => activeModel.Value;

        private CacheManager(string stateFilePath)
        {
            serializer = new ModelSerializer(stateFilePath);

            if (File.Exists(stateFilePath))
            {
                CompetitionClassModel model = serializer.Load();
                model = model.RecalculatePlacements();
                activeModel.Value = model;
            }
        }

        private static CacheManager CreateDefaultInstance()
        {
            string commonAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string defaultPath = Path.Combine(commonAppDataFolder, "DogAgilityCompetition", "Controller", "ClassResultStore.xml");
            return new CacheManager(defaultPath);
        }

        public CompetitionClassModel ReplaceModel(CompetitionClassModel replacementVersion, CompetitionClassModel originalVersion)
        {
            Guard.NotNull(replacementVersion, nameof(replacementVersion));
            Guard.NotNull(originalVersion, nameof(originalVersion));

            using var lockTracker = new LockTracker(Log, MethodBase.GetCurrentMethod()!);

            lock (stateLock)
            {
                lockTracker.Acquired();

                if (originalVersion != activeModel.Value)
                {
                    // Should never get here.
                    throw new DBConcurrencyException("Unexpected model update from multiple threads.");
                }

                try
                {
                    serializer.Save(replacementVersion);
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to save in-memory model to disk.", ex);
                }

                activeModel.Value = replacementVersion;
                return replacementVersion;
            }
        }
    }
}

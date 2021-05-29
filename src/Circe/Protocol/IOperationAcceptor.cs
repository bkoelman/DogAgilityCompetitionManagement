using DogAgilityCompetition.Circe.Protocol.Operations;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Defines the contract for implementing the Visitor design pattern on CIRCE operations.
    /// </summary>
    public interface IOperationAcceptor
    {
        // Operations from Controller to Mediator.
        void Accept([NotNull] LoginOperation operation);

        void Accept([NotNull] LogoutOperation operation);

        void Accept([NotNull] AlertOperation operation);

        void Accept([NotNull] NetworkSetupOperation operation);

        void Accept([NotNull] DeviceSetupOperation operation);

        void Accept([NotNull] SynchronizeClocksOperation operation);

        void Accept([NotNull] VisualizeOperation operation);

        // Operations from Mediator to Controller.
        void Accept([NotNull] KeepAliveOperation operation);

        void Accept([NotNull] NotifyStatusOperation operation);

        void Accept([NotNull] NotifyActionOperation operation);
    }
}

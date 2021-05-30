using DogAgilityCompetition.Circe.Protocol.Operations;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Defines the contract for implementing the Visitor design pattern on CIRCE operations.
    /// </summary>
    public interface IOperationAcceptor
    {
        // Operations from Controller to Mediator.
        void Accept(LoginOperation operation);

        void Accept(LogoutOperation operation);

        void Accept(AlertOperation operation);

        void Accept(NetworkSetupOperation operation);

        void Accept(DeviceSetupOperation operation);

        void Accept(SynchronizeClocksOperation operation);

        void Accept(VisualizeOperation operation);

        // Operations from Mediator to Controller.
        void Accept(KeepAliveOperation operation);

        void Accept(NotifyStatusOperation operation);

        void Accept(NotifyActionOperation operation);
    }
}

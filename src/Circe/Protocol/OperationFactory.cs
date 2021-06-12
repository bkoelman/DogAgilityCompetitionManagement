using System;
using DogAgilityCompetition.Circe.Protocol.Operations;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Provides a factory for creating known CIRCE <see cref="Operation" /> instances.
    /// </summary>
    public static class OperationFactory
    {
        /// <summary>
        /// Creates an <see cref="Operation" /> for the specified operation code.
        /// </summary>
        /// <param name="operationCode">
        /// The operation code of the operation to create.
        /// </param>
        /// <returns>
        /// The created operation.
        /// </returns>
        /// <exception cref="NotSupportedException" />
        public static Operation Create(int operationCode)
        {
            switch (operationCode)
            {
                case LoginOperation.TypeCode:
                    return new LoginOperation();
                case LogoutOperation.TypeCode:
                    return new LogoutOperation();
                case AlertOperation.TypeCode:
                    return new AlertOperation();
                case NetworkSetupOperation.TypeCode:
                    return new NetworkSetupOperation();
                case DeviceSetupOperation.TypeCode:
                    return new DeviceSetupOperation();
                case SynchronizeClocksOperation.TypeCode:
                    return new SynchronizeClocksOperation();
                case VisualizeOperation.TypeCode:
                    return new VisualizeOperation();

                case KeepAliveOperation.TypeCode:
                    return new KeepAliveOperation();
                case NotifyStatusOperation.TypeCode:
                    return new NotifyStatusOperation();
                case NotifyActionOperation.TypeCode:
                    return new NotifyActionOperation();
                case LogOperation.TypeCode:
                    return new LogOperation();

                default:
                    throw new NotSupportedException($"Unsupported operation code {operationCode}.");
            }
        }
    }
}

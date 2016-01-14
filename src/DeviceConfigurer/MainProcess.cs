﻿using System.Reflection;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.Circe.Session;
using DogAgilityCompetition.DeviceConfigurer.Phases;
using JetBrains.Annotations;

namespace DogAgilityCompetition.DeviceConfigurer
{
    /// <summary>
    /// Controls the phase transitions for the wireless network address assignment process.
    /// </summary>
    public sealed class MainProcess
    {
        [NotNull]
        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NotNull]
        private readonly StartupArguments startupArguments;

        [NotNull]
        private readonly IncomingOperationDispatcher dispatcher;

        private bool IsConfiguringMediator => startupArguments.Capabilities == null;

        public MainProcess([NotNull] StartupArguments startupArguments)
        {
            Guard.NotNull(startupArguments, nameof(startupArguments));

            this.startupArguments = startupArguments;
            dispatcher = new IncomingOperationDispatcher(this);
        }

        public void Run()
        {
            var stateMachine = new AssignmentStateMachine(new PhaseWaitingForConnection());
            CirceComConnection connection = null;

            Log.Info($"Connecting to mediator on {startupArguments.ComPortName}...");
            stateMachine.ExecuteIfInPhase<PhaseWaitingForConnection>(phase =>
            {
                connection = new CirceComConnection(startupArguments.ComPortName);
                connection.OperationReceived +=
                    (sender, eventArgs) => ConnectionOperationReceived(eventArgs, stateMachine);
                connection.Open();
                connection.Send(new LoginOperation());

                return new PhaseWaitingForLoginResponse();
            });

            Log.Info("Waiting for login response...");
            var readyForDeviceSetup = stateMachine.WaitForPhase<PhaseReadyForDeviceSetup>();
            Log.Info($"Mediator status in login response: {readyForDeviceSetup.MediatorStatus}.");

            if (!IsConfiguringMediator &&
                readyForDeviceSetup.MediatorStatus == KnownMediatorStatusCode.MediatorUnconfigured)
            {
                Log.Info("ERROR: Connected to unconfigured mediator. Please configure mediator first.");
                return;
            }

            Log.Info("Sending address assignment...");
            stateMachine.ExecuteIfInPhase<PhaseReadyForDeviceSetup>(phase1 =>
            {
                connection.Send(new DeviceSetupOperation(startupArguments.NewAddress)
                {
                    DestinationAddress = startupArguments.OldAddress,
                    Capabilities = startupArguments.Capabilities
                });

                return new PhaseWaitingForSetupResponse(startupArguments.NewAddress);
            });

            Log.Info("Waiting for response from new device...");
            var assignmentCompleted = stateMachine.WaitForPhase<PhaseAssignmentCompleted>();

            Log.Info(assignmentCompleted.MediatorStatus == KnownMediatorStatusCode.MediatorUnconfigured
                ? "ERROR: Failed to assign mediator address."
                : "Received response on new address.");

            Log.Info("Disconnecting...");
            connection.Send(new LogoutOperation());
        }

        private void ConnectionOperationReceived([NotNull] IncomingOperationEventArgs e,
            [NotNull] AssignmentStateMachine stateMachine)
        {
            dispatcher.SetStateMachine(stateMachine);
            e.Operation.Visit(dispatcher);
        }

        private sealed class IncomingOperationDispatcher : IOperationAcceptor
        {
            [NotNull]
            private readonly MainProcess owner;

            [CanBeNull]
            private AssignmentStateMachine assignmentStateMachine;

            public IncomingOperationDispatcher([NotNull] MainProcess owner)
            {
                Guard.NotNull(owner, nameof(owner));
                this.owner = owner;
            }

            public void SetStateMachine([NotNull] AssignmentStateMachine stateMachine)
            {
                Guard.NotNull(stateMachine, nameof(stateMachine));
                assignmentStateMachine = stateMachine;
            }

            public void Accept(LoginOperation operation)
            {
            }

            public void Accept(LogoutOperation operation)
            {
            }

            public void Accept(AlertOperation operation)
            {
            }

            public void Accept(NetworkSetupOperation operation)
            {
            }

            public void Accept(DeviceSetupOperation operation)
            {
            }

            public void Accept(SynchronizeClocksOperation operation)
            {
            }

            public void Accept(VisualizeOperation operation)
            {
            }

            public void Accept(KeepAliveOperation operation)
            {
                AssignmentStateMachine stateMachine = AssertStateMachineIsAssigned(assignmentStateMachine);

                bool transitioned =
                    stateMachine.ExecuteIfInPhase<PhaseWaitingForLoginResponse>(
                        phase => new PhaseReadyForDeviceSetup(operation.MediatorStatus));

                if (!transitioned)
                {
                    stateMachine.ExecuteIfInPhase<PhaseWaitingForSetupResponse>(
                        phase =>
                            owner.IsConfiguringMediator
                                ? (AssignmentPhase) new PhaseAssignmentCompleted(operation.MediatorStatus)
                                : null);
                }
            }

            public void Accept(NotifyStatusOperation operation)
            {
                AssignmentStateMachine stateMachine = AssertStateMachineIsAssigned(assignmentStateMachine);

                stateMachine.ExecuteIfInPhase<PhaseWaitingForSetupResponse>(
                    phase =>
                        phase.NewAddress == operation.OriginatingAddress ? new PhaseAssignmentCompleted(null) : null);
            }

            public void Accept(NotifyActionOperation operation)
            {
            }

            [AssertionMethod]
            [NotNull]
            private static AssignmentStateMachine AssertStateMachineIsAssigned(
                [CanBeNull] AssignmentStateMachine stateMachine)
            {
                Guard.NotNull(stateMachine, nameof(stateMachine));
                return stateMachine;
            }
        }
    }
}
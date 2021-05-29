using System;
using System.Threading;
using System.Threading.Tasks;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.Circe.Session;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Specs.Facilities
{
    /// <summary>
    /// Supports writing unit tests that utilize the USB loop-back cable.
    /// </summary>
    public class CirceUsbLoopbackTestRunner : IDisposable
    {
        private static readonly TimeSpan DefaultRunTimeout = TimeSpan.FromMilliseconds(1000);

        [NotNull]
        private readonly ManualResetEvent manualResetEvent;

        [CanBeNull]
        private TimeSpan? timeout;

        [CanBeNull]
        private Version protocolVersion;

        [NotNull]
        public CirceControllerSessionManager RemoteSessionManager { get; }

        [NotNull]
        public CirceComConnection Connection { get; }

        public TimeSpan RunTimeout
        {
            get => timeout ?? DefaultRunTimeout;
            set => timeout = value;
        }

        [NotNull]
        public Version ProtocolVersion
        {
            get => protocolVersion ?? KeepAliveOperation.CurrentProtocolVersion;
            set
            {
                Guard.NotNull(value, nameof(value));
                protocolVersion = value;
            }
        }

        public int MediatorStatusCode { get; set; }

        public event EventHandler<IncomingOperationEventArgs> OperationReceived;

        public CirceUsbLoopbackTestRunner()
        {
            try
            {
                manualResetEvent = new ManualResetEvent(false);

                Connection = ComPortSelector.GetConnection(AttachConnectionHandlers, DetachConnectionHandlers);

                RemoteSessionManager = new CirceControllerSessionManager();
            }
            catch (Exception)
            {
                Connection?.Dispose();
                manualResetEvent?.Dispose();
                RemoteSessionManager?.Dispose();

                throw;
            }
        }

        private void AttachConnectionHandlers([NotNull] CirceComConnection connection)
        {
            connection.OperationReceived += ConnectionOnOperationReceived;
        }

        private void DetachConnectionHandlers([NotNull] CirceComConnection connection)
        {
            connection.OperationReceived -= ConnectionOnOperationReceived;
        }

        private void ConnectionOnOperationReceived([CanBeNull] object sender, [NotNull] IncomingOperationEventArgs e)
        {
            if (e.Operation is LoginOperation)
            {
                Connection.Send(new KeepAliveOperation(ProtocolVersion, MediatorStatusCode));
            }

            OperationReceived?.Invoke(this, e);
        }

        public bool Start()
        {
            RemoteSessionManager.Start();

            return manualResetEvent.WaitOne(RunTimeout);
        }

        public bool StartWithKeepAliveLoopInBackground()
        {
            Task.Factory.StartNew(() =>
            {
                bool done;

                do
                {
                    done = manualResetEvent.WaitOne(500);

                    if (!done)
                    {
                        Connection.Send(new KeepAliveOperation(ProtocolVersion, 5));
                    }
                }
                while (!done);
            });

            return Start();
        }

        public void SignalSucceeded()
        {
            manualResetEvent.Set();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Connection.Dispose();
                manualResetEvent.Dispose();
                RemoteSessionManager.Dispose();
            }
        }
    }
}

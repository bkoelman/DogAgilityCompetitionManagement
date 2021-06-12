using System;
using System.Threading;
using System.Threading.Tasks;
using DogAgilityCompetition.Circe;
using DogAgilityCompetition.Circe.Controller;
using DogAgilityCompetition.Circe.Protocol.Operations;
using DogAgilityCompetition.Circe.Session;

namespace DogAgilityCompetition.Specs.Facilities
{
    /// <summary>
    /// Supports writing unit tests that utilize the USB loop-back cable.
    /// </summary>
    public class CirceUsbLoopbackTestRunner : IDisposable
    {
        private static readonly TimeSpan DefaultRunTimeout = TimeSpan.FromMilliseconds(1000);

        private readonly ManualResetEvent manualResetEvent;

        private TimeSpan? timeout;
        private Version? protocolVersion;

        public CirceControllerSessionManager RemoteSessionManager { get; }
        public CirceComConnection Connection { get; }

        public TimeSpan RunTimeout
        {
            get => timeout ?? DefaultRunTimeout;
            set => timeout = value;
        }

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

        public event EventHandler<IncomingOperationEventArgs>? OperationReceived;

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

        private void AttachConnectionHandlers(CirceComConnection connection)
        {
            connection.OperationReceived += ConnectionOnOperationReceived;
        }

        private void DetachConnectionHandlers(CirceComConnection connection)
        {
            connection.OperationReceived -= ConnectionOnOperationReceived;
        }

        private void ConnectionOnOperationReceived(object? sender, IncomingOperationEventArgs e)
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
            Task.Run(() =>
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

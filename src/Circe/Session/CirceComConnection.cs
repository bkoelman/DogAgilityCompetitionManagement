using System;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Threading;
using DogAgilityCompetition.Circe.Protocol;
using DogAgilityCompetition.Circe.Protocol.Exceptions;
using DogAgilityCompetition.Circe.Protocol.Operations;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Session
{
    /// <summary>
    /// Provides reading and writing of CIRCE packets to/from an RS-232 port.
    /// </summary>
    public sealed class CirceComConnection : IDisposable
    {
        // External resources about using the SerialPort class:
        // http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/e36193cd-a708-42b3-86b7-adff82b19e5e/
        // http://blogs.msdn.com/b/bclteam/archive/2006/10/10/top-5-serialport-tips-_5b00_kim-hamilton_5d00_.aspx
        // http://zachsaw.blogspot.nl/2010/07/net-serialport-woes.html

        private static readonly ISystemLogger Log = new Log4NetSystemLogger(MethodBase.GetCurrentMethod()!.DeclaringType!);

        private readonly SerialPort comPort;
        private readonly PacketAssembler assembler = new();
        private readonly PacketReader reader = new();

        public string PortName => comPort.PortName;

        public event EventHandler? PacketSending;
        public event EventHandler? PacketReceived;
        public event EventHandler<IncomingOperationEventArgs>? OperationReceived;

        public CirceComConnection(string comPortName)
        {
            Guard.NotNullNorEmpty(comPortName, nameof(comPortName));

            comPort = new SerialPort
            {
                BaudRate = 115200,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None,
                PortName = comPortName,

                // Specify timeouts before calling Open(), to prevent infinite hang on a call 
                // to Close() from different thread.
                ReadTimeout = 500,
                WriteTimeout = 500
            };

            comPort.DataReceived += ComPortOnDataReceived;
            comPort.ErrorReceived += ComPortOnErrorReceived;
            comPort.PinChanged += ComPortOnPinChanged;

            assembler.CompletePacketAdded += AssemblerOnCompletePacketAdded;
        }

        public void Open()
        {
            Log.Debug($"Opening port {PortName}.");
            comPort.Open();
        }

        public void Send(Operation operation)
        {
            Guard.NotNull(operation, nameof(operation));

            PacketSending?.Invoke(this, EventArgs.Empty);

            Log.Debug($"Entering Send with {operation}.");
            byte[] buffer = PacketWriter.Write(operation, true);

            Log.Debug($"=> RAW: {buffer.FormatHexBuffer()}");
            comPort.Write(buffer, 0, buffer.Length);
        }

        public void Dispose()
        {
            Log.Debug($"Closing port {PortName}.");

            bool hasClosed = false;

            try
            {
                comPort.Close();
                hasClosed = true;
            }
            catch (InvalidOperationException)
            {
                // This may happen when you unplugged the serial port cable before disposal.
                // The exception indicates that the port has already been closed.
            }
            catch (Exception ex)
            {
                Log.Error($"Unexpected error while closing {PortName}.", ex);
            }

            if (hasClosed)
            {
                // The receiver thread inside the .NET framework needs some time to perform 
                // async cleanup after close, before another port can be opened.
                Thread.Sleep(200);
            }
        }

        [Pure]
        public override string ToString()
        {
            return PortName;
        }

        private void ComPortOnDataReceived(object? sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                bool done;

                do
                {
                    done = true;

                    int bytesToRead = SafeGetBytesToRead();
                    int bytesRead = SafeComRead(bytesToRead, out byte[]? buffer);

                    if (bytesRead > 0 && buffer != null)
                    {
                        done = false;

                        var block = new ArraySegment<byte>(buffer, 0, bytesRead);
                        Log.Debug($"<= RAW: {block.FormatHexBuffer()}");

                        assembler.Add(block);
                    }
                }
                while (!done);
            }
            catch (Exception ex)
            {
                Log.Error($"Unexpected error while processing received data from {PortName}.", ex);
            }
        }

        private int SafeGetBytesToRead()
        {
            try
            {
                return comPort.BytesToRead;
            }
            catch (InvalidOperationException ex)
            {
                // This may happen when you unplug the serial port cable while reading from the port.
                // The exception indicates that the port has been closed, so reading fails.
                Log.Debug($"Failed to read from {PortName}.", ex);
            }
            catch (IOException ex)
            {
                Log.Debug($"Failed to read from {PortName}.", ex);
            }

            return -1;
        }

        private int SafeComRead(int bytesToRead, out byte[]? buffer)
        {
            buffer = null;

            if (bytesToRead <= 0)
            {
                return 0;
            }

            buffer = new byte[bytesToRead];

            try
            {
                return comPort.Read(buffer, 0, bytesToRead);
            }
            catch (InvalidOperationException ex)
            {
                // This may happen when you unplug the serial port cable while reading from the port.
                // The exception indicates that the port has been closed, so reading fails.
                Log.Debug($"Failed to read from {PortName}.", ex);
            }
            catch (IOException ex)
            {
                Log.Debug($"Failed to read from {PortName}.", ex);
            }

            return -1;
        }

        private void AssemblerOnCompletePacketAdded(object? sender, EventArgs<byte[]> e)
        {
            Operation? operation = null;

            try
            {
                operation = reader.Read(e.Argument);
            }
            catch (PacketFormatException ex)
            {
                Log.Warn("Discarding non-compliant incoming packet.", ex);
            }
            catch (OperationValidationException ex)
            {
                Log.Warn("Discarding non-compliant incoming packet.", ex);
            }

            if (operation != null)
            {
                PacketReceived?.Invoke(this, EventArgs.Empty);

                if (operation is LogOperation logOperation)
                {
                    Log.Debug($"Discarding assembled operation: {logOperation}{logOperation.FormatLogData()}");
                }
                else
                {
                    Log.Debug($"Raising event for assembled operation: {operation}");
                    OperationReceived?.Invoke(this, new IncomingOperationEventArgs(operation, this));
                }
            }
        }

        private void ComPortOnErrorReceived(object? sender, SerialErrorReceivedEventArgs e)
        {
            // When not all data is received, may need to increase SerialPort.ReadBufferSize / WriteBufferSize 
            // (default value: 4096 bytes) or consider switching to hardware flow control.
            Log.Error($"Port {PortName} reported error {e.EventType}.");
        }

        private void ComPortOnPinChanged(object? sender, SerialPinChangedEventArgs e)
        {
            Log.Debug($"Port {PortName} reported pin change {e.EventType}.");
        }
    }
}

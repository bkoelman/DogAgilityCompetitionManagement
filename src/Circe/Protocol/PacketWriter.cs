﻿using System.Text;
using DogAgilityCompetition.Circe.Protocol.Exceptions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol;

/// <summary>
/// Writes a CIRCE operation to a block of bytes.
/// </summary>
public static class PacketWriter
{
    /// <summary>
    /// Converts the specified operation to a packet in binary format.
    /// </summary>
    /// <param name="operation">
    /// The operation to convert.
    /// </param>
    /// <param name="includeChecksum">
    /// If set to <c>true</c>, a checksum will be included in the packet.
    /// </param>
    /// <returns>
    /// The binary contents of the created packet.
    /// </returns>
    /// <exception cref="OperationValidationException" />
    public static byte[] Write(Operation operation, bool includeChecksum)
    {
        Guard.NotNull(operation, nameof(operation));

        operation.Validate();

        byte[] headerBytes = GetPacketHeaderBytes(operation.Code);

        byte[] payloadBytes = GetPacketPayloadBytes(operation.Parameters);

        int? checksum = includeChecksum ? 0 : null;
        UpdateChecksum(headerBytes, ref checksum);
        UpdateChecksum(payloadBytes, ref checksum);

        byte[] trailerBytes = GetPacketTrailerBytes(checksum);

        byte[] packetBytes = CombineBuffers(new[]
        {
            headerBytes,
            payloadBytes,
            trailerBytes
        });

        return packetBytes;
    }

    private static byte[] GetPacketHeaderBytes(int operationCode)
    {
        string operationCodeString = $"{operationCode:00}";
        byte[] operationCodeBytes = Encoding.ASCII.GetBytes(operationCodeString);

        // ReSharper disable once RedundantExplicitArraySize
        // Justification: Extra compiler check when packet format changes.
        return new byte[PacketFormat.PacketHeaderLength]
        {
            PacketFormatDelimiters.StartOfText,
            operationCodeBytes[0],
            operationCodeBytes[1],
            PacketFormatDelimiters.Tab
        };
    }

    private static byte[] GetPacketPayloadBytes(IEnumerable<Parameter> parameters)
    {
        byte[] buffer = StreamToBuffer(stream =>
        {
            foreach (Parameter parameter in parameters.Where(parameter => parameter.HasValue))
            {
                WriteParameterBytesTo(parameter, stream);
            }
        });

        return buffer;
    }

    private static byte[] StreamToBuffer(Action<Stream> writeCallback)
    {
        using var stream = new MemoryStream();
        writeCallback(stream);

        byte[] buffer = new byte[stream.Length];
        stream.Seek(0, SeekOrigin.Begin);
        stream.Read(buffer, 0, buffer.Length);
        return buffer;
    }

    private static void WriteParameterBytesTo(Parameter parameter, Stream output)
    {
        string parameterIdString = $"{parameter.Id:000}";
        byte[] parameterIdBytes = Encoding.ASCII.GetBytes(parameterIdString);

        output.Write(parameterIdBytes, 0, parameterIdBytes.Length);
        output.WriteByte(PacketFormatDelimiters.Colon);
        byte[] parameterBytes = parameter.ExportValue();
        output.Write(parameterBytes, 0, parameterBytes.Length);
        output.WriteByte(PacketFormatDelimiters.Tab);
    }

    [Pure]
    private static byte[] GetPacketTrailerBytes(int? checksum)
    {
        if (checksum != null)
        {
            string checksumString = $"{checksum:X2}";
            byte[] checksumBytes = Encoding.ASCII.GetBytes(checksumString);

            // ReSharper disable once RedundantExplicitArraySize
            // Justification: Extra compiler check when packet format changes.
            return new byte[PacketFormat.PacketTrailerMinLength + PacketFormat.OptionalChecksumLength]
            {
                checksumBytes[0],
                checksumBytes[1],
                PacketFormatDelimiters.EndOfText
            };
        }

        // ReSharper disable once RedundantExplicitArraySize
        // Justification: Extra compiler check when packet format changes.
        return new byte[PacketFormat.PacketTrailerMinLength]
        {
            PacketFormatDelimiters.EndOfText
        };
    }

    private static void UpdateChecksum(IEnumerable<byte> buffer, ref int? checksum)
    {
        if (checksum != null)
        {
            foreach (byte bt in buffer)
            {
                checksum += bt;
                checksum &= 0xFF;
            }
        }
    }

    [Pure]
    private static byte[] CombineBuffers(IList<byte[]> buffers)
    {
        int size = buffers.Sum(sourceBuffer => sourceBuffer.Length);

        byte[] destinationBuffer = new byte[size];
        int destinationOffset = 0;

        foreach (byte[] sourceBuffer in buffers)
        {
            Buffer.BlockCopy(sourceBuffer, 0, destinationBuffer, destinationOffset, sourceBuffer.Length);
            destinationOffset += sourceBuffer.Length;
        }

        return destinationBuffer;
    }
}

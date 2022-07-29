namespace DogAgilityCompetition.Circe.Protocol;

/// <summary>
/// Scans buffer blocks and raises an event for each complete CIRCE packet.
/// </summary>
public sealed class PacketAssembler
{
    // When non-empty, contains the start bytes of the next packet.
    private readonly Queue<ArraySegment<byte>> startOfPacketQueue = new();

    public event EventHandler<EventArgs<byte[]>>? CompletePacketAdded;

    public void Add(ArraySegment<byte> bufferSegment)
    {
        byte[]? packet;

        while ((packet = ConsumeNextPacket(ref bufferSegment)) != null)
        {
            CompletePacketAdded?.Invoke(this, new EventArgs<byte[]>(packet));
        }
    }

    /// <summary>
    /// Consumes and returns the next complete packet from the buffers.
    /// </summary>
    /// <param name="buffer">
    /// The most recently added buffer block.
    /// </param>
    /// <returns>
    /// The bytes of a complete packet, or <c>null</c> when no more complete packets can be created from the buffers.
    /// </returns>
    private byte[]? ConsumeNextPacket(ref ArraySegment<byte> buffer)
    {
        // When preceding buffers are non-empty, we do not have to find a
        // start-of-packet first (because the first byte in the preceding buffers is always a 
        // start-of-packet).
        int packetStartIndex = startOfPacketQueue.Count > 0 ? buffer.Offset : -1;

        // Iterate over the specified buffer, searching for start-of-packet and
        // end-of-packet.
        for (int index = buffer.Offset; index < buffer.Offset + buffer.Count; index++)
        {
            if (buffer.Array![index] == PacketFormatDelimiters.StartOfText)
            {
                // Start-of-packet found before end-of-packet. Any queued buffers will 
                // not be part of the next packet, so consider them garbage.
                startOfPacketQueue.Clear();
                packetStartIndex = index;
            }
            else if (buffer.Array[index] == PacketFormatDelimiters.EndOfText && packetStartIndex != -1)
            {
                // Create new packet. It starts by eating contents of the queued buffers.
                var packetSegments = new List<ArraySegment<byte>>();

                while (startOfPacketQueue.Count > 0)
                {
                    packetSegments.Add(startOfPacketQueue.Dequeue());
                }

                // Then add buffer contents in range packetStartIndex - index (inclusive).
                var lastPacketSegment = new ArraySegment<byte>(buffer.Array, packetStartIndex, index - packetStartIndex + 1);
                packetSegments.Add(lastPacketSegment);

                // Remaining buffer data should be processed the next time this method
                // is called.
                buffer = new ArraySegment<byte>(buffer.Array, index + 1, buffer.Offset + buffer.Count - index - 1);

                // Copy the collected segments into a single byte array (which represents a complete packet).
                return CombineSegments(packetSegments);
            }
        }

        if (packetStartIndex != -1)
        {
            // Only found start-of-packet or the queued buffers were non-empty.
            // So the range packetStartIndex up to the end of the buffer are part of
            // a new packet (of which the end has not been received yet).
            var segment = new ArraySegment<byte>(buffer.Array!, packetStartIndex, buffer.Offset + buffer.Count - packetStartIndex);
            startOfPacketQueue.Enqueue(segment);
        }
        else
        {
            // Queued buffers were empty and not found a start-of-packet. So all buffer
            // data is garbage and can be discarded.
            buffer = new ArraySegment<byte>();
        }

        // The entire buffer has been processed, so we're done.
        return null;
    }

    /// <summary>
    /// Copies the data from the specified segments into a new buffer.
    /// </summary>
    /// <param name="segments">
    /// The segments to combine.
    /// </param>
    private static T[] CombineSegments<T>(IList<ArraySegment<T>> segments)
    {
        int bufferSize = segments.Sum(segment => segment.Count);

        var bufferItems = new T[bufferSize];
        int bufferOffset = 0;

        foreach (ArraySegment<T> segment in segments)
        {
            Buffer.BlockCopy(segment.Array!, segment.Offset, bufferItems, bufferOffset, segment.Count);
            bufferOffset += segment.Count;
        }

        return bufferItems;
    }
}

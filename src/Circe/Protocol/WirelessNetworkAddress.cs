using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DogAgilityCompetition.Circe.Protocol.Parameters;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol
{
    /// <summary>
    /// Represents the address of a hardware device in the CIRCE wireless network.
    /// </summary>
    [Serializable]
    public sealed class WirelessNetworkAddress : IComparable<WirelessNetworkAddress>, IEquatable<WirelessNetworkAddress>, IXmlSerializable
    {
        [NotNull]
        public static readonly WirelessNetworkAddress Default = new("000000");

        [NotNull]
        public string Value { get; private set; }

        public WirelessNetworkAddress([NotNull] string value)
        {
            AssertValidAddress(value);
            Value = value;
        }

        private WirelessNetworkAddress()
        {
            // Private constructor is required for XML serialization.
            Value = Default.Value;
        }

        [AssertionMethod]
        private static void AssertValidAddress([NotNull] string value)
        {
            Guard.NotNullNorEmpty(value, nameof(value));

            if (!NetworkAddressParameter.IsValidAddress(value))
            {
                throw new ArgumentException($"'{value}' is not a valid wireless network address.", nameof(value));
            }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            throw new NotSupportedException();
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Guard.NotNull(reader, nameof(reader));

            string content = reader.ReadInnerXml();

            AssertValidAddress(content);
            Value = content;
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            Guard.NotNull(writer, nameof(writer));

            writer.WriteRaw(Value);
        }

        [Pure]
        public override string ToString()
        {
            return Value;
        }

        public int CompareTo([CanBeNull] WirelessNetworkAddress other)
        {
            return ReferenceEquals(other, null) ? 1 : string.CompareOrdinal(Value, other.Value);
        }

        public bool Equals([CanBeNull] WirelessNetworkAddress other)
        {
            return !ReferenceEquals(other, null) && other.Value == Value;
        }

        public override bool Equals([CanBeNull] object obj)
        {
            return Equals(obj as WirelessNetworkAddress);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            // Reason: GetHashCode() is not expected to be called before deserialization has completed.
            return Value.GetHashCode();
        }

        public static bool operator ==([CanBeNull] WirelessNetworkAddress left, [CanBeNull] WirelessNetworkAddress right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null))
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=([CanBeNull] WirelessNetworkAddress left, [CanBeNull] WirelessNetworkAddress right)
        {
            return !(left == right);
        }

        public static bool operator <([CanBeNull] WirelessNetworkAddress left, [CanBeNull] WirelessNetworkAddress right)
        {
            return left == null ? right != null : left.CompareTo(right) == -1;
        }

        public static bool operator <=([CanBeNull] WirelessNetworkAddress left, [CanBeNull] WirelessNetworkAddress right)
        {
            return left == null || left.CompareTo(right) <= 0;
        }

        public static bool operator >([CanBeNull] WirelessNetworkAddress left, [CanBeNull] WirelessNetworkAddress right)
        {
            return left?.CompareTo(right) == 1;
        }

        public static bool operator >=([CanBeNull] WirelessNetworkAddress left, [CanBeNull] WirelessNetworkAddress right)
        {
            return left == null ? right == null : left.CompareTo(right) >= 0;
        }
    }
}

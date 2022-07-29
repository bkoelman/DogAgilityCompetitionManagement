using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Parameters;

/// <summary>
/// Represents a parameter whose value indicates variable-length binary data.
/// </summary>
public sealed class BinaryParameter : Parameter
{
    private static readonly Regex HexFormatRegex = new("^([0-9A-F][0-9A-F])+$", RegexOptions.Compiled);

    private readonly List<byte> innerValue = new();

    /// <summary>
    /// Gets or sets the value of this parameter.
    /// </summary>
    public IList<byte> Value => innerValue;

    /// <summary>
    /// Indicates whether the value of this parameter has been set.
    /// </summary>
    /// <value>
    /// <c>true</c> if this parameter has a value; otherwise, <c>false</c>.
    /// </value>
    public override bool HasValue => innerValue.Count > 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryParameter" /> class.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter.
    /// </param>
    /// <param name="id">
    /// The identifier of the parameter.
    /// </param>
    /// <param name="isRequired">
    /// If set to <c>true</c>, the parameter is required.
    /// </param>
    public BinaryParameter(string name, int id, bool isRequired)
        : base(name, id, null, isRequired)
    {
    }

    /// <summary>
    /// Exports the value of this parameter to binary format.
    /// </summary>
    /// <returns>
    /// The exported binary value of this parameter.
    /// </returns>
    public override byte[] ExportValue()
    {
        if (!HasValue)
        {
            throw new InvalidOperationException($"{GetType().Name} {Name} has no value.");
        }

        string hexString = BytesToHexEncodedText(innerValue);
        return Encoding.ASCII.GetBytes(hexString);
    }

    private static string BytesToHexEncodedText(ICollection<byte> source)
    {
        var textBuilder = new StringBuilder(source.Count * 2);

        foreach (byte bt in source)
        {
            textBuilder.Append($"{bt:X2}");
        }

        return textBuilder.ToString();
    }

    /// <summary>
    /// Imports the value of this parameter from binary format.
    /// </summary>
    /// <param name="value">
    /// The bytes that contain the value to import.
    /// </param>
    /// <exception cref="ArgumentException">
    /// <paramref name="value" /> do not represent a valid parameter value.
    /// </exception>
    public override void ImportValue(byte[] value)
    {
        base.ImportValue(value);

        char[] chars = Encoding.ASCII.GetChars(value);
        string hexText = new(chars);
        IEnumerable<byte> hexEncoded = HexEncodedTextToBytes(hexText);

        ReplaceValueWith(hexEncoded);
    }

    private IEnumerable<byte> HexEncodedTextToBytes(string hexText)
    {
        if (HexFormatRegex.IsMatch(hexText))
        {
            var bytes = new List<byte>(hexText.Length / 2);

            for (int index = 0; index < hexText.Length; index += 2)
            {
                string hexCharacter = hexText.Substring(index, 2);
                byte bt = byte.Parse(hexCharacter, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                bytes.Add(bt);
            }

            return bytes;
        }

        throw new ArgumentOutOfRangeException(nameof(hexText), hexText,
            $"Value of {GetType().Name} {Name} must consist of even number of characters in range 0-9 or A-F.");
    }

    public void ReplaceValueWith(IEnumerable<byte> value)
    {
        Guard.NotNull(value, nameof(value));

        innerValue.Clear();
        innerValue.AddRange(value);
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents the current <see cref="object" />.
    /// </summary>
    /// <returns>
    /// A <see cref="string" /> that represents the current <see cref="object" />.
    /// </returns>
    [Pure]
    public override string ToString()
    {
        return HasValue ? base.ToString() + ": Length=" + innerValue.Count : base.ToString();
    }
}

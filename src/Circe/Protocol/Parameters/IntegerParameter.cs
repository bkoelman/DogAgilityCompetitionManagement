using System.Globalization;
using System.Text;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Parameters;

/// <summary>
/// Represents a parameter whose value indicates a whole number.
/// </summary>
public sealed class IntegerParameter : Parameter
{
    private int? innerValue;

    /// <summary>
    /// Gets the lowest allowed value.
    /// </summary>
    public int MinValue { get; }

    /// <summary>
    /// Gets the highest allowed value.
    /// </summary>
    public int MaxValue { get; }

    /// <summary>
    /// Gets or sets the value of this parameter.
    /// </summary>
    public int? Value
    {
        get => innerValue;
        set
        {
            if (value != null && (value < MinValue || value > MaxValue))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, $"Value of {GetType().Name} {Name} must be in range [{MinValue}-{MaxValue}].");
            }

            innerValue = value;
        }
    }

    /// <summary>
    /// Indicates whether the value of this parameter has been set.
    /// </summary>
    /// <value>
    /// <c>true</c> if this parameter has a value; otherwise, <c>false</c>.
    /// </value>
    public override bool HasValue => innerValue != null;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntegerParameter" /> class.
    /// </summary>
    /// <param name="name">
    /// The name of the parameter.
    /// </param>
    /// <param name="id">
    /// The identifier of the parameter.
    /// </param>
    /// <param name="minValue">
    /// The lowest allowed value.
    /// </param>
    /// <param name="maxValue">
    /// The highest allowed value.
    /// </param>
    /// <param name="isRequired">
    /// If set to <c>true</c>, the parameter is required.
    /// </param>
    public IntegerParameter(string name, int id, int minValue, int maxValue, bool isRequired)
        : base(name, id, CalculateMaxDigitCount(minValue, maxValue), isRequired)
    {
        if (minValue > maxValue)
        {
            throw new ArgumentException("minValue cannot be higher than maxValue.");
        }

        MinValue = minValue;
        MaxValue = maxValue;
    }

    private static int CalculateMaxDigitCount(int minValue, int maxValue)
    {
        int lengthForMinValue = minValue.ToString(CultureInfo.InvariantCulture).Length;
        int lengthForMaxValue = maxValue.ToString(CultureInfo.InvariantCulture).Length;
        return Math.Max(lengthForMinValue, lengthForMaxValue);
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

        string valueString = Value.ToString() ?? string.Empty;
        return Encoding.ASCII.GetBytes(valueString);
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

        try
        {
            Value = int.Parse(new string(chars), CultureInfo.InvariantCulture);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException($"Failed to convert '{chars}' to integer.", nameof(value), ex);
        }
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
        return HasValue ? base.ToString() + ": " + innerValue : base.ToString();
    }
}

using System.Threading;

namespace DogAgilityCompetition.Circe.Session;

/// <summary>
/// Wraps a <see cref="System.Boolean" />, where reading and writing the wrapped value always atomically returns the latest value.
/// </summary>
/// <remarks>
/// It is strongly recommended to mark <see cref="FreshBoolean" /> members in your class as <c>readonly</c>, because accidentally replacing a
/// FreshBoolean object with another FreshBoolean object defeats the whole purpose of this class.
/// </remarks>
public sealed class FreshBoolean
{
    private const int TrueValue = 1;
    private const int FalseValue = 0;

    private int innerValue;

    public bool Value
    {
        get
        {
            int intValue = Interlocked.CompareExchange(ref innerValue, 999, 999);
            return FromInt32(intValue);
        }
        set
        {
            int intValue = ToInt32(value);
            Interlocked.Exchange(ref innerValue, intValue);
        }
    }

    public FreshBoolean(bool value)
    {
        Value = value;
    }

    private static bool FromInt32(int value)
    {
        return value == TrueValue;
    }

    private static int ToInt32(bool value)
    {
        return value ? TrueValue : FalseValue;
    }
}

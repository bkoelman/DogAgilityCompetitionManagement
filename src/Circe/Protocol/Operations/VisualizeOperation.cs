using DogAgilityCompetition.Circe.Protocol.Exceptions;
using DogAgilityCompetition.Circe.Protocol.Parameters;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol.Operations;

/// <summary>
/// This operation is used by a controller during a competition run, to change one or more visualization elements in wireless display devices in the
/// logical network.
/// </summary>
/// <remarks>
/// <para>
/// Note that for bandwidth efficiency, only changes must be sent by a controller. For all parameters omitted, the receiving display device must leave
/// the associated visualization elements unaffected.
/// </para>
/// <para>
/// The controller may choose to include the Destination Address parameter multiple times, each time with the unique network address of the specific
/// display device.
/// </para>
/// </remarks>
public sealed class VisualizeOperation : Operation
{
    internal const int TypeCode = 7;

    private static readonly NetworkAddressParameter PrototypeDestinationAddressParameter =
        ParameterFactory.Create(ParameterType.NetworkAddress.DestinationAddress, true);

    private readonly WirelessNetworkAddressCollection destinationAddresses;
    private readonly IntegerParameter currentCompetitorNumberParameter = ParameterFactory.Create(ParameterType.Integer.CurrentCompetitorNumber, false);
    private readonly IntegerParameter nextCompetitorNumberParameter = ParameterFactory.Create(ParameterType.Integer.NextCompetitorNumber, false);
    private readonly IntegerParameter startTimerParameter = ParameterFactory.Create(ParameterType.Integer.StartTimer, false);
    private readonly IntegerParameter primaryTimerValueParameter = ParameterFactory.Create(ParameterType.Integer.PrimaryTimerValue, false);
    private readonly IntegerParameter secondaryTimerValueParameter = ParameterFactory.Create(ParameterType.Integer.SecondaryTimerValue, false);
    private readonly IntegerParameter faultCountParameter = ParameterFactory.Create(ParameterType.Integer.FaultCount, false);
    private readonly IntegerParameter refusalCountParameter = ParameterFactory.Create(ParameterType.Integer.RefusalCount, false);
    private readonly BooleanParameter eliminatedParameter = ParameterFactory.Create(ParameterType.Boolean.Eliminated, false);
    private readonly IntegerParameter previousPlacementParameter = ParameterFactory.Create(ParameterType.Integer.PreviousPlacement, false);

    /// <summary>
    /// Required. Gets or sets the destination addresses of the devices in the wireless network.
    /// </summary>
    public ICollection<WirelessNetworkAddress> DestinationAddresses => destinationAddresses;

    /// <summary>
    /// Optional. Gets or sets the value to display for current competitor number, or 0 to hide it.
    /// </summary>
    public int? CurrentCompetitorNumber
    {
        get => currentCompetitorNumberParameter.Value;
        set => currentCompetitorNumberParameter.Value = value;
    }

    /// <summary>
    /// Optional. Gets or sets the value to display for next competitor number, or 0 to hide it.
    /// </summary>
    public int? NextCompetitorNumber
    {
        get => nextCompetitorNumberParameter.Value;
        set => nextCompetitorNumberParameter.Value = value;
    }

    /// <summary>
    /// Optional. Indicates to toggle activity of the running timer.
    /// </summary>
    public bool StartTimer
    {
        get => startTimerParameter.HasValue;
        set => startTimerParameter.Value = value ? 1 : null;
    }

    /// <summary>
    /// Optional. Gets or sets the primary time value (in milliseconds) to display for current competitor, or 999999 to hide it.
    /// </summary>
    public TimeSpan? PrimaryTimerValue
    {
        get
        {
            if (primaryTimerValueParameter.Value == null)
            {
                return null;
            }

            double milliseconds = (double)primaryTimerValueParameter.Value;

            // TimeSpan.FromMilliseconds() accepts a double as input, but it internally 
            // rounds the input value to whole milliseconds.
            return TimeSpan.FromMilliseconds(milliseconds);
        }
        set
        {
            if (value == null)
            {
                primaryTimerValueParameter.Value = null;
            }
            else
            {
                int milliseconds = (int)Math.Round(value.Value.TotalMilliseconds, MidpointRounding.AwayFromZero);
                primaryTimerValueParameter.Value = milliseconds;
            }
        }
    }

    /// <summary>
    /// Optional. Gets or sets the secondary time value (in milliseconds) to display for current competitor, or 999999 to hide it.
    /// </summary>
    public TimeSpan? SecondaryTimerValue
    {
        get
        {
            if (secondaryTimerValueParameter.Value == null)
            {
                return null;
            }

            double milliseconds = (double)secondaryTimerValueParameter.Value;

            // TimeSpan.FromMilliseconds() accepts a double as input, but it internally 
            // rounds the input value to whole milliseconds.
            return TimeSpan.FromMilliseconds(milliseconds);
        }
        set
        {
            if (value == null)
            {
                secondaryTimerValueParameter.Value = null;
            }
            else
            {
                int milliseconds = (int)Math.Round(value.Value.TotalMilliseconds, MidpointRounding.AwayFromZero);
                secondaryTimerValueParameter.Value = milliseconds;
            }
        }
    }

    /// <summary>
    /// Optional. Gets or sets the value to display for fault count, or 99 to hide it.
    /// </summary>
    public int? FaultCount
    {
        get => faultCountParameter.Value;
        set => faultCountParameter.Value = value;
    }

    /// <summary>
    /// Optional. Gets or sets the value to display for refusal count, or 99 to hide it.
    /// </summary>
    public int? RefusalCount
    {
        get => refusalCountParameter.Value;
        set => refusalCountParameter.Value = value;
    }

    /// <summary>
    /// Optional. Toggles display of the elimination indicator.
    /// </summary>
    public bool? Eliminated
    {
        get => eliminatedParameter.Value;
        set => eliminatedParameter.Value = value;
    }

    /// <summary>
    /// Optional. Gets or sets the value to display for placement of previous competitor, or 0 to hide it.
    /// </summary>
    public int? PreviousPlacement
    {
        get => previousPlacementParameter.Value;
        set => previousPlacementParameter.Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VisualizeOperation" /> class with required parameters.
    /// </summary>
    /// <param name="destinationAddresses">
    /// The destination addresses of the devices in the wireless network.
    /// </param>
    public VisualizeOperation(IEnumerable<WirelessNetworkAddress> destinationAddresses)
        : this()
    {
        Guard.NotNull(destinationAddresses, nameof(destinationAddresses));

        foreach (WirelessNetworkAddress destinationAddress in destinationAddresses)
        {
            this.destinationAddresses.Add(destinationAddress);
        }

        Guard.GreaterOrEqual(this.destinationAddresses.Count, nameof(destinationAddresses), 1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VisualizeOperation" /> class.
    /// </summary>
    internal VisualizeOperation()
        : base(TypeCode)
    {
        // Important: Try to ensure that elimination change is handled before handling time changes.
        Parameters.Add(eliminatedParameter);

        Parameters.Add(currentCompetitorNumberParameter);
        Parameters.Add(nextCompetitorNumberParameter);
        Parameters.Add(startTimerParameter);
        Parameters.Add(primaryTimerValueParameter);
        Parameters.Add(secondaryTimerValueParameter);
        Parameters.Add(faultCountParameter);
        Parameters.Add(refusalCountParameter);
        Parameters.Add(previousPlacementParameter);

        destinationAddresses = new WirelessNetworkAddressCollection(this, ParameterType.NetworkAddress.DestinationAddress);
    }

    public override void Validate()
    {
        base.Validate();

        AssertOneOrMoreDestinationParameters();
        AssertNoDuplicateDestinationParameters();

        AssertOneOrMoreOptionalParametersAssigned();
    }

    [AssertionMethod]
    private void AssertOneOrMoreDestinationParameters()
    {
        if (destinationAddresses.Count == 0)
        {
            NetworkAddressParameter prototypeParameter = ParameterFactory.Create(ParameterType.NetworkAddress.DestinationAddress, true);
            throw new OperationValidationException(this, $"{nameof(NetworkAddressParameter)} {prototypeParameter.Name} must occur one or more times.");
        }
    }

    [AssertionMethod]
    private void AssertNoDuplicateDestinationParameters()
    {
        var addressSet = new HashSet<WirelessNetworkAddress>();

        foreach (WirelessNetworkAddress destinationAddress in destinationAddresses)
        {
            if (addressSet.Contains(destinationAddress))
            {
                throw new OperationValidationException(this,
                    $"Duplicate {nameof(NetworkAddressParameter)} {PrototypeDestinationAddressParameter.Name} found with value '{destinationAddress.Value}'.");
            }

            addressSet.Add(destinationAddress);
        }
    }

    [AssertionMethod]
    private void AssertOneOrMoreOptionalParametersAssigned()
    {
        bool hasParametersAssigned = Parameters.Any(parameter => parameter.Id != PrototypeDestinationAddressParameter.Id && parameter.HasValue);

        if (!hasParametersAssigned)
        {
            throw new OperationValidationException(this, "At least one optional parameter must have a value.");
        }
    }

    /// <summary>
    /// Implements the Visitor design pattern.
    /// </summary>
    /// <param name="acceptor">
    /// The object accepting this operation.
    /// </param>
    public override void Visit(IOperationAcceptor acceptor)
    {
        Guard.NotNull(acceptor, nameof(acceptor));

        acceptor.Accept(this);
    }

    protected internal override bool AllowMultiple(int parameterId)
    {
        return parameterId == PrototypeDestinationAddressParameter.Id;
    }

    protected internal override Parameter? GetParameterOrNull(int parameterId)
    {
        return parameterId == PrototypeDestinationAddressParameter.Id ? destinationAddresses.CreateAttachParameter() : base.GetParameterOrNull(parameterId);
    }
}

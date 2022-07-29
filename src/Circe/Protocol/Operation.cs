using System.Text;
using DogAgilityCompetition.Circe.Protocol.Exceptions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe.Protocol;

/// <summary>
/// Represents the base class for all CIRCE operations that can be exchanged between an application controller and a mediator device.
/// </summary>
public abstract class Operation
{
    /// <summary>
    /// Gets the operation parameters.
    /// </summary>
    protected internal IList<Parameter> Parameters { get; }

    /// <summary>
    /// Gets the code that identifies this type of operation.
    /// </summary>
    public int Code { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Operation" /> class.
    /// </summary>
    /// <param name="code">
    /// The code that identifies this type of operation.
    /// </param>
    protected Operation(int code)
    {
        Guard.InRangeInclusive(code, nameof(code), 0, 99);

        Code = code;
        Parameters = new List<Parameter>();
    }

    /// <summary>
    /// Validates that this instance represents a valid CIRCE operation.
    /// </summary>
    /// <exception cref="OperationValidationException" />
    public virtual void Validate()
    {
        foreach (Parameter parameter in Parameters)
        {
            parameter.Validate(this);
        }
    }

    /// <summary>
    /// Implements the Visitor design pattern.
    /// </summary>
    /// <param name="acceptor">
    /// The object accepting this operation.
    /// </param>
    public abstract void Visit(IOperationAcceptor acceptor);

    protected internal virtual bool AllowMultiple(int parameterId)
    {
        return false;
    }

    protected internal virtual Parameter? GetParameterOrNull(int parameterId)
    {
        return Parameters.FirstOrDefault(p => p.Id == parameterId);
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    [Pure]
    public override string ToString()
    {
        var textBuilder = new StringBuilder();
        textBuilder.Append(GetType().Name);
        textBuilder.Append(" (");
        textBuilder.Append(Code);
        textBuilder.Append(')');

        if (Parameters.Count > 0)
        {
            textBuilder.Append(", Parameters=[");
            bool isFirstParameter = true;

            foreach (Parameter parameter in Parameters)
            {
                if (parameter.HasValue)
                {
                    if (isFirstParameter)
                    {
                        isFirstParameter = false;
                    }
                    else
                    {
                        textBuilder.Append(", ");
                    }

                    textBuilder.Append(parameter);
                }
            }

            textBuilder.Append(']');
        }

        return textBuilder.ToString();
    }
}

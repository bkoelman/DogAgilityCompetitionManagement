using System;
using System.Linq.Expressions;
using System.Text;

namespace DogAgilityCompetition.Circe
{
    /// <summary>
    /// Writes a list of name/value pairs in a comma-separated formatted string.
    /// </summary>
    public sealed class ObjectFormatter : IDisposable
    {
        private readonly object? outerInstance;
        private readonly string? outerText;
        private readonly StringBuilder builder;

        public ObjectFormatter(StringBuilder builder, string? outerText = null)
        {
            Guard.NotNull(builder, nameof(builder));

            this.builder = builder;
            this.outerText = outerText;
        }

        public ObjectFormatter(StringBuilder builder, object? outerInstance = null)
        {
            Guard.NotNull(builder, nameof(builder));

            this.builder = builder;
            this.outerInstance = outerInstance;
        }

        public void Append<T>(GetReferenceCallback<T> getValue, Expression<Func<object?>> getValueExpression)
            where T : class
        {
            Guard.NotNull(getValue, nameof(getValue));
            Guard.NotNull(getValueExpression, nameof(getValueExpression));

            // Note: Caller needs to pass same expression twice for better performance.

            T? source = getValue();
            string? name = getValueExpression.GetExpressionName();
            AppendToBuilder(source, name);
        }

        public void Append<T>(GetValueCallback<T> getValue, Expression<Func<object>> getValueExpression)
            where T : struct
        {
            Guard.NotNull(getValue, nameof(getValue));
            Guard.NotNull(getValueExpression, nameof(getValueExpression));

            // Note: Caller needs to pass same expression twice for better performance.

            T source = getValue();
            string? name = getValueExpression.GetExpressionName();
            AppendToBuilder(source, name);
        }

        public void Append<T>(GetOptionalValueCallback<T> getValue, Expression<Func<object?>> getValueExpression)
            where T : struct
        {
            Guard.NotNull(getValue, nameof(getValue));
            Guard.NotNull(getValueExpression, nameof(getValueExpression));

            // Note: Caller needs to pass same expression twice for better performance.

            T? source = getValue();
            string? name = getValueExpression.GetExpressionName();
            AppendToBuilder(source, name);
        }

        public void AppendText(string? text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                AppendToBuilder(text, null);
            }
        }

        private void AppendToBuilder(object? value, string? name)
        {
            if (value != null)
            {
                if (builder.Length > 0)
                {
                    builder.Append(", ");
                }

                if (!string.IsNullOrEmpty(name))
                {
                    builder.Append(name);
                    builder.Append("=");
                }

                builder.Append(value);
            }
        }

        public void Dispose()
        {
            if (outerInstance != null)
            {
                builder.Insert(0, outerInstance.GetType().Name + " (");
                builder.Append(")");
            }
            else if (outerText != null)
            {
                builder.Insert(0, outerText + " ");
            }
        }
    }
}

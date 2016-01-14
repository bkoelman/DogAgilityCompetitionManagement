using System;
using System.Linq.Expressions;
using System.Text;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary>
    /// Writes a list of name/value pairs in a comma-separated formatted string.
    /// </summary>
    public sealed class ObjectFormatter : IDisposable
    {
        [CanBeNull]
        private readonly object outerInstance;

        [CanBeNull]
        private readonly string outerText;

        [NotNull]
        private readonly StringBuilder builder;

        public ObjectFormatter([NotNull] StringBuilder builder, [CanBeNull] string outerText = null)
        {
            Guard.NotNull(builder, nameof(builder));

            this.builder = builder;
            this.outerText = outerText;
        }

        public ObjectFormatter([NotNull] StringBuilder builder, [CanBeNull] object outerInstance = null)
        {
            Guard.NotNull(builder, nameof(builder));

            this.builder = builder;
            this.outerInstance = outerInstance;
        }

        public void Append<T>([NotNull] GetReferenceCallback<T> getValue,
            [NotNull] Expression<Func<object>> getValueExpression) where T : class
        {
            Guard.NotNull(getValue, nameof(getValue));
            Guard.NotNull(getValueExpression, nameof(getValueExpression));

            // Note: Caller needs to pass same expression twice for better performance.

            T source = getValue();
            string name = getValueExpression.GetExpressionName();
            AppendToBuilder(source, name);
        }

        public void Append<T>([NotNull] GetValueCallback<T> getValue,
            [NotNull] Expression<Func<object>> getValueExpression) where T : struct
        {
            Guard.NotNull(getValue, nameof(getValue));
            Guard.NotNull(getValueExpression, nameof(getValueExpression));

            // Note: Caller needs to pass same expression twice for better performance.

            T source = getValue();
            string name = getValueExpression.GetExpressionName();
            AppendToBuilder(source, name);
        }

        public void Append<T>([NotNull] GetOptionalValueCallback<T> getValue,
            [NotNull] Expression<Func<object>> getValueExpression) where T : struct
        {
            Guard.NotNull(getValue, nameof(getValue));
            Guard.NotNull(getValueExpression, nameof(getValueExpression));

            // Note: Caller needs to pass same expression twice for better performance.

            T? source = getValue();
            string name = getValueExpression.GetExpressionName();
            AppendToBuilder(source, name);
        }

        public void AppendText([CanBeNull] string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                AppendToBuilder(text, null);
            }
        }

        private void AppendToBuilder([CanBeNull] object value, [CanBeNull] string name)
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
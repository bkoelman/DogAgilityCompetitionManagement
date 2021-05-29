using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary />
    public static class Assertions
    {
        [AssertionMethod]
        [NotNull]
        public static T InternalValueIsNotNull<T>([NotNull] GetReferenceCallback<T> memberAsFunc,
            [NotNull] Expression<GetReferenceCallback<T>> memberAsExpression)
            where T : class
        {
            Guard.NotNull(memberAsFunc, nameof(memberAsFunc));
            Guard.NotNull(memberAsExpression, nameof(memberAsExpression));

            T value = memberAsFunc();

            if (value == null)
            {
                // Note: Caller needs to pass same expression twice for better performance.
                throw new InvalidOperationException($"Unexpected internal error: {memberAsExpression.GetExpressionFullName()} is null.");
            }

            return value;
        }

        [AssertionMethod]
        public static T InternalValueIsNotNull<T>([NotNull] GetOptionalValueCallback<T> memberAsFunc,
            [NotNull] Expression<GetOptionalValueCallback<T>> memberAsExpression)
            where T : struct
        {
            Guard.NotNull(memberAsFunc, nameof(memberAsFunc));
            Guard.NotNull(memberAsExpression, nameof(memberAsExpression));

            T? value = memberAsFunc();

            if (value == null)
            {
                // Note: Caller needs to pass same expression twice for better performance.
                throw new InvalidOperationException($"Unexpected internal error: {memberAsExpression.GetExpressionFullName()} is null.");
            }

            return value.Value;
        }
    }
}

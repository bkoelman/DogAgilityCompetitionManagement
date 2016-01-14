using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace DogAgilityCompetition.Circe
{
    /// <summary />
    public delegate T? GetOptionalValueCallback<T>() where T : struct;

    /// <summary />
    public delegate T GetValueCallback<out T>() where T : struct;

    /// <summary />
    public delegate T GetReferenceCallback<out T>() where T : class;

    /// <summary />
    public static class ExpressionExtensions
    {
        [CanBeNull]
        public static string GetExpressionFullName<T>([NotNull] this Expression<GetReferenceCallback<T>> expression)
            where T : class
        {
            Guard.NotNull(expression, nameof(expression));
            return expression.Body.GetMemberExpression().GetExpressionFullName();
        }

        [CanBeNull]
        public static string GetExpressionFullName<T>([NotNull] this Expression<GetOptionalValueCallback<T>> expression)
            where T : struct
        {
            Guard.NotNull(expression, nameof(expression));
            return expression.Body.GetMemberExpression().GetExpressionFullName();
        }

        [CanBeNull]
        public static string GetExpressionName<T>([NotNull] this Expression<Func<T>> expression)
        {
            Guard.NotNull(expression, nameof(expression));
            return expression.Body.GetMemberExpression().GetExpressionName();
        }

        [CanBeNull]
        private static string GetExpressionFullName([CanBeNull] this MemberExpression memberExpression)
        {
            if (memberExpression == null)
            {
                return null;
            }

            string child = memberExpression.Member.Name;
            string parent = GetExpressionFullName(memberExpression.Expression.GetMemberExpression());

            if (parent == null)
            {
                return child;
            }

            return parent + "." + child;
        }

        [CanBeNull]
        private static string GetExpressionName([CanBeNull] this MemberExpression memberExpression)
        {
            return memberExpression?.Member.Name;
        }

        [CanBeNull]
        private static MemberExpression GetMemberExpression([CanBeNull] this Expression expression)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression;
            }

            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
            {
                memberExpression = (MemberExpression) unaryExpression.Operand;
                return memberExpression;
            }

            return null;
        }
    }
}
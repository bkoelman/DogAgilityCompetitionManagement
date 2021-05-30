using System;
using System.Linq.Expressions;

namespace DogAgilityCompetition.Circe
{
    /// <summary />
    public delegate T? GetOptionalValueCallback<T>()
        where T : struct;

    /// <summary />
    public delegate T GetValueCallback<out T>()
        where T : struct;

    /// <summary />
    public delegate T? GetReferenceCallback<out T>()
        where T : class;

    /// <summary />
    public static class ExpressionExtensions
    {
        public static string? GetExpressionFullName<T>(this Expression<GetReferenceCallback<T>> expression)
            where T : class
        {
            Guard.NotNull(expression, nameof(expression));
            return expression.Body.GetMemberExpression().GetExpressionFullName();
        }

        public static string? GetExpressionFullName<T>(this Expression<GetOptionalValueCallback<T>> expression)
            where T : struct
        {
            Guard.NotNull(expression, nameof(expression));
            return expression.Body.GetMemberExpression().GetExpressionFullName();
        }

        public static string? GetExpressionName<T>(this Expression<Func<T>> expression)
        {
            Guard.NotNull(expression, nameof(expression));
            return expression.Body.GetMemberExpression().GetExpressionName();
        }

        private static string? GetExpressionFullName(this MemberExpression? memberExpression)
        {
            if (memberExpression == null)
            {
                return null;
            }

            string child = memberExpression.Member.Name;
            string? parent = GetExpressionFullName(memberExpression.Expression.GetMemberExpression());

            if (parent == null)
            {
                return child;
            }

            return parent + "." + child;
        }

        private static string? GetExpressionName(this MemberExpression? memberExpression)
        {
            return memberExpression?.Member.Name;
        }

        private static MemberExpression? GetMemberExpression(this Expression? expression)
        {
            if (expression is MemberExpression memberExpression)
            {
                return memberExpression;
            }

            if (expression is UnaryExpression unaryExpression)
            {
                memberExpression = (MemberExpression)unaryExpression.Operand;
                return memberExpression;
            }

            return null;
        }
    }
}

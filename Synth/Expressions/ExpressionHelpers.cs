using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Synth
{
    /// <summary>
    /// Helpers for <see cref="Expression"/>
    /// </summary>
    public static class ExpressionHelpers
    {
        /// <summary>
        /// Compiles an expression and gets the functions return value
        /// </summary>
        /// <typeparam name="T">The type of the return value</typeparam>
        /// <param name="lamda">The expression to compile</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this Expression<Func<T>> lambda)
        {
            return lambda.Compile().Invoke();
        }

        /// <summary>
        /// Sets the underlying properties value to the given value from an expression that contains the property
        /// </summary>
        /// <typeparam name="T">The type of value to set</typeparam>
        /// <param name="lamda">The expression</param>
        /// <param name="value">The value to set the property to</param>
        public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
        {
            //Converts a lambda to a property about a member
            MemberExpression expression = (lambda as LambdaExpression).Body as MemberExpression;

            //Get the property information so we can set it
            PropertyInfo propertyInfo = (PropertyInfo)expression.Member;
            object target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

            //Set the property value
            propertyInfo.SetValue(target, value);
        }
    }
}

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MyApp.Common
{
    public class ReflectHelper
    {
        public MemberInfo GetMember<T>(Expression<Func<T>> expression)
        {
            return GetMemberInfo(expression);
        }

        public PropertyInfo GetProperty<T>(Expression<Func<T>> expression)
        {
            var property = GetMember(expression) as PropertyInfo;
            return property;
        }

        internal MemberInfo GetMemberInfo(LambdaExpression lambda)
        {

            if (lambda.Body.NodeType == ExpressionType.Call)
            {
                return ((MethodCallExpression)lambda.Body).Method;
            }

            var memberExpression = GetMemberExpression(lambda.Body);

            return memberExpression.Member;
        }
        
        internal MemberExpression GetMemberExpression(Expression expression)
        {
            MemberExpression memberExpression = null;
            if (expression.NodeType == ExpressionType.Convert)
            {
                memberExpression = ((UnaryExpression)expression).Operand as MemberExpression;
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression as MemberExpression;
            }
            return memberExpression;
        }

        public static ReflectHelper Instance = new ReflectHelper();
    }
}

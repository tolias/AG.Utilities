using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AG.Utilities
{
    public static class DebugHelpers
    {
        private const string NULL_VALUE_STRING = "{null}";

        /// <summary>
        /// Returns string represented value of object or "{null}" text if the object is null
        /// </summary>
        /// <param name="obj">The object from which string value should be get</param>
        /// <returns>string represented value of object or "{null}" text if the object is null</returns>
        public static string GetValueOrNullString(this object obj)
        {
            return obj?.ToString() ?? NULL_VALUE_STRING;
        }

        /// <summary>
        /// Returns debug string value of variable and its property path (for example: var.Field1.Prop2 = 3)
        /// </summary>
        /// <typeparam name="T">The type of variable from which <param name="variableSelector"></param> starts</typeparam>
        /// <typeparam name="TProperty">The type of ending property in <param name="variableSelector"></param></typeparam>
        /// <param name="obj"></param>
        /// <param name="variableSelector"></param>
        /// <returns></returns>
        public static string GetDebugValueString<T, TProperty>(this T obj, Expression<Func<TProperty>> variableSelector)
        {
            string debugValueString;
            try
            {
                Type staticClassType;
                var selectorExpressionsChain = variableSelector.Body.GetMemberExpressionsChain(out staticClassType);

                var variableExpression = (MemberExpression)selectorExpressionsChain[0];

                var objType = typeof(T);
                if (objType != variableExpression.Type)
                {
                    throw new InvalidOperationException($"GetDebugValueString() was called on one variable (value is \"{obj.GetValueOrNullString()}\". Type: {objType})"
                                                        + $", but {nameof(variableSelector)} doesn't start from this variable but from another variable (Type: {variableExpression.Type})"
                                                        + $". \r\nGetDebugValueString() must be called from the same variable which is used in  {nameof(variableSelector)}");
                }

                var selectorName = staticClassType == null ? null : staticClassType.FullName + '.';
                selectorName += variableExpression.Member.Name;

                string selectorValueString;
                string nullProperties = null;

                if (selectorExpressionsChain.Length <= 1)
                {
                    // no properties were passed. The variable name is only presented in variableSelector.
                    selectorValueString = obj.GetValueOrNullString();
                }
                else
                {
                    var propertiesExpressionsInfo = selectorExpressionsChain.Skip(1).Select(GetExpressionInfo);

                    string propertiesNamesChain;
                    var selectorValue = GetExpressionValue(obj, propertiesExpressionsInfo, out propertiesNamesChain, out nullProperties);

                    selectorName += propertiesNamesChain;
                    selectorValueString = selectorValue.GetValueOrNullString();
                }

                debugValueString = $"{selectorName} = {selectorValueString}";

                if (nullProperties != null)
                {
                    debugValueString += $" (NULL properties: {nullProperties})";
                }
            }
            catch (Exception ex)
            {
                debugValueString = $"{{[UNHANDLED EXCEPTION: {ex.Message}]}}";
            }

            return debugValueString;
        }

        public static Expression[] GetMemberExpressionsChain(this Expression expression, out Type staticClassType)
        {
            staticClassType = null;
            List<Expression> expressions = new List<Expression>();

            var currentExpression = expression;

            while (true)
            {
                expressions.Add(currentExpression);

                var memberExpression = currentExpression as MemberExpression;

                if (memberExpression == null)
                {
                    break;
                }

                currentExpression = memberExpression.Expression;

                if (currentExpression == null)
                {
                    staticClassType = memberExpression.Member.DeclaringType;
                    break;
                }

                if (currentExpression.NodeType == ExpressionType.Constant)
                {
                    break;
                }
            }

            expressions.Reverse();
            return expressions.ToArray();
        }

        private static object GetExpressionValue(object obj, IEnumerable<ExpressionInfo> expressionChains, out string selectorName,
            out string nullProperties)
        {
            nullProperties = null;
            selectorName = null;

            if (obj == null)
            {
                nullProperties = expressionChains.GetPropertiesString();
                return null;
            }

            var enumerator = expressionChains.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                return null;
            }

            bool nullPropertiesExist = false;
            object expressionValue = obj;
            do
            {
                var expressionInfo = enumerator.Current;

                selectorName += '.' + expressionInfo.DebugView;
                expressionValue = expressionInfo.ValueGetter(expressionValue);

                if (!enumerator.MoveNext())
                {
                    break;
                }

                if (expressionValue == null)
                {
                    nullPropertiesExist = true;
                    break;
                }
            } while (true);

            if (nullPropertiesExist)
            {
                do
                {
                    nullProperties += '.' + enumerator.Current.DebugView;
                } while (enumerator.MoveNext());
            }

            return expressionValue;
        }

        private static ExpressionInfo GetExpressionInfo(Expression expression)
        {
            var memberExpression = expression as MemberExpression;

            if (memberExpression != null)
            {
                var memberSelectorName = memberExpression.Member.Name;
                return new ExpressionInfo(memberSelectorName, obj =>
                {
                    var objType = obj.GetType();
                    var propertyInfo = objType.GetProperty(memberSelectorName);

                    if (propertyInfo != null)
                    {
                        return propertyInfo.GetValue(obj, null);
                    }

                    var fieldInfo = objType.GetField(memberSelectorName);
                    return fieldInfo.GetValue(obj);
                });
            }

            //todo: other expression types to check

            return null;
        }

        private static string GetPropertiesString(this IEnumerable<ExpressionInfo> expressionChains)
        {
            return '.' + expressionChains.JoinAsString(".");
        }

        private class ExpressionInfo
        {
            public readonly string DebugView;
            public readonly Func<object, object> ValueGetter;

            public ExpressionInfo(string debugView, Func<object, object> valueGetter)
            {
                DebugView = debugView;
                ValueGetter = valueGetter;
            }

            public override string ToString()
            {
                return DebugView;
            }
        }
    }
}

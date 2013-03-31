//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Linq.Expressions;

namespace BootBaronLib.Operational
{
    public static class StaticReflection
    {
        public static string GetMemberName<T>(
            Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return GetMemberName(expression.Body);
        }

        public static string GetMemberName<T>(
            Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            return GetMemberName(expression.Body);
        }

        private static string GetMemberName(
            Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(
                    "The expression cannot be null.");
            }

            var expression1 = expression as MemberExpression;
            if (expression1 != null)
            {
                // Reference type property or field
                MemberExpression memberExpression =
                    expression1;
                return memberExpression.Member.Name;
            }

            var callExpression = expression as MethodCallExpression;
            if (callExpression != null)
            {
                // Reference type method
                MethodCallExpression methodCallExpression =
                    callExpression;
                return methodCallExpression.Method.Name;
            }

            var unaryExpression1 = expression as UnaryExpression;
            if (unaryExpression1 != null)
            {
                // Property, field of method returning value type
                UnaryExpression unaryExpression = unaryExpression1;
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException("Invalid expression");
        }

        private static string GetMemberName(
            UnaryExpression unaryExpression)
        {
            var operand = unaryExpression.Operand as MethodCallExpression;
            if (operand != null)
            {
                MethodCallExpression methodExpression =
                    operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression) unaryExpression.Operand)
                .Member.Name;
        }
    }
}
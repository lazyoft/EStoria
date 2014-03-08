using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EStoria
{
	internal static class Guard
	{
		public static void NotNull<T>(Expression<Func<T>> expression) where T : class
		{
			if(expression.Compile()() == null)	
				throw new ArgumentNullException(((MemberExpression)expression.Body).Member.Name);
		}

		public static void NotNullOrWhiteSpace(Expression<Func<string>> expression)
		{
			if (string.IsNullOrWhiteSpace(expression.Compile()()))
				throw new ArgumentException("string cannot be null, empty or white space", ((MemberExpression)expression.Body).Member.Name);
		}

		public static void DoesntContainNull<T>(Expression<Func<IEnumerable<T>>> expression) where T : class
		{
			if(expression.Compile()().Any(e => e == null))
				throw new ArgumentNullException("Sequence cannot contain null elements", ((MemberExpression)expression.Body).Member.Name);
		}
	}
}

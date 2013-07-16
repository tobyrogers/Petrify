// Copyright 2013 Toby Rogers
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using NUnit.Framework;
using Petrify.Core.TestData;
using Castle.DynamicProxy;

namespace Petrify.Core.Proxies
{
	[TestFixture]
	public class ProxyTest
	{
		[Test]
		public void TestReferencedProxy ()
		{
			var person = new Person ();
			person.Address = new Address ();
			person.FirstName = "Old Name";

			//ProxyGenerator generator = new ProxyGenerator();


//
//			person.Address = generator.CreateClassProxyWithTarget(person.Address.GetType(), person.Address ,new LoggingInterceptor()) as typeof(Address);
//
//			person.FirstName = "New Name";




			//Then invoke this using reflection:

//				MethodInfo castMethod = this.GetType().GetMethod("Cast").MakeGenericMethod(t);
//			object castedObject = castMethod.Invoke(null, new object[] { obj });
//
//			person.Address.Street = "new street";

		}

		public static T Cast<T>(object o)
		{
			return (T)o;
		}

		public class LoggingInterceptor : IInterceptor
		{
			public void Intercept(IInvocation invocation)
			{
				Console.Write("Log: Method Called: "+ invocation.Method.Name);
				invocation.Proceed();
			}
		}
	}
}


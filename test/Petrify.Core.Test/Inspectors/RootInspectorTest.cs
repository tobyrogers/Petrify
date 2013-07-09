using System;
using NUnit.Framework;
using Petrify.Core.TestData;

namespace Petrify.Core.Inspectors
{
	[TestFixture]
	public class RootInspectorTest
	{
		[Test]
		public void TestCanGetRootOfBasicAggrigate()
		{
			// given
			var type = typeof(BasicAggrigate);

			// then
			var inspector = new RootInspector ();
			var rootType = inspector.GetRootType (type);

			// should give
			Assert.That (rootType, Is.EqualTo(typeof(ValidRoot)));
		}		

		[Test]
		public void TestCanGetRootOfComplexAggrigate()
		{
			// given
			var type = typeof(ComplexAggrigate);

			// then
			var inspector = new RootInspector ();
			var rootType = inspector.GetRootType (type);

			// should give
			Assert.That (rootType, Is.EqualTo(typeof(ValidRoot)));
		}

		[Test]
		[ExpectedException(typeof(ApplicationException))] 
		public void TestThatExceptionThrownForInvalidRoot()
		{
			// given
			var type = typeof(InvalidAggrigate);

			// then
			var inspector = new RootInspector ();
			inspector.GetRootType (type); // should throw
		}
	}
}


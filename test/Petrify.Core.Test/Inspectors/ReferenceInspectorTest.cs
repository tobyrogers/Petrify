using System;
using NUnit.Framework;
using Petrify.Core.TestData;
using System.Linq;

namespace Petrify.Core.Inspectors
{
	[TestFixture]
	public class ReferenceInspectorTest
	{
		[Test]
		public void TestCanGetReferencesForComplexAggrigate()
		{
			// given
			var aggrigate = new ComplexAggrigate ();

			// then
			var inspector = new ReferenceInspector (typeof(ValidRoot));
			var references = inspector.GetReferences (aggrigate);

			// should give
			Assert.That (references.Count(), Is.EqualTo (1));
		}
	}
}


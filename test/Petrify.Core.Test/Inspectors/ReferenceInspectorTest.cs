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
			IEntityInspector entityInspector = new AutoEntityInspector();

			// then
			var inspector = new ReferenceInspector (entityInspector);
			var references = inspector.GetReferences (aggrigate);

			// should give
			Assert.That (references.Count(), Is.EqualTo (1));
		}
	}
}


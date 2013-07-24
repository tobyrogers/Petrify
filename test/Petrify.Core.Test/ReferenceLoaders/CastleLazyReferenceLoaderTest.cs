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
using Rhino.Mocks;

namespace Petrify.Core.ReferenceLoaders
{
	[TestFixture]
	public class CastleLazyReferenceLoaderTest
	{
		[Test]
		public void TestCanGenerateLazyReference ()
		{
			// given
			var id = Guid.NewGuid ();
			var type = typeof(BasicAggrigate);

			IRepository repository = MockRepository.GenerateMock<IRepository> ();
			repository.Expect (x => x.Load (type, id)).Return (new BasicAggrigate () { Id=id });

			// then 
			var referenceLoader = new CastleLazyReferenceLoader (repository);
			var reference = referenceLoader.LoadReference (type, id);

			// should give
			Assert.IsInstanceOf<BasicAggrigate> (reference);

			// then
			var basicAggrigate = (BasicAggrigate)reference;
			var entityId = basicAggrigate.Id;
			Assert.That (entityId, Is.EqualTo (id));
			repository.VerifyAllExpectations ();
		}
	}
}


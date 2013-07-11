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
using Petrify.Core.Database;
using Petrify.MongoDB.Driver;

namespace Basic
{
	public class Entity
	{
		public virtual Guid Id { get; set; }
	}

	public class Person : Entity
	{
		public virtual string FirstName { get; set; }

		public virtual string LastName { get; set; }

		public virtual Address Address { get; set; }
	}

	public class Address : Entity
	{
		public virtual string Street { get; set; }

		public virtual string Town { get; set; }

		public virtual string Postcode { get; set; }
	}

	class MainClass
	{
		// initial idea about how I think this should work....
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			// create an address and person aggrigate
			var address = new Address () { Street = "10 Downing Street", Town = "London", Postcode = "SW1A 2AA" };
			var person = new Person () { FirstName = "David", LastName = "Cameron", Address = address };

			// connect to the database
			var database = new PetrifyDB (new MongoDbDriver ("myDatabase"));

			// save the person aggrigate
			var id = database.Save (person);

			// load the person aggrigate
			var person2 = database.Load<Person> (id);

			Console.WriteLine (person2.Address.Street);
		}
	}
}

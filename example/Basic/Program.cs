using System;
using Petrify.Core.Database;
using Petrify.MongoDB.Driver;

namespace Basic
{
	public class Entity
	{
		public Guid Id { get; set; }
	}

	public class Person : Entity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public Address Address { get; set; }
	}

	public class Address : Entity
	{
		public string Street { get; set; }
		public string Town { get; set; }
		public string Postcode { get; set; }
	}

	class MainClass
	{
		// initial idea about how I think this should work....

		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			// create an address and person aggrigate
			var address = new Address () { Street="10 Downing Street", Town ="London", Postcode = "SW1A 2AA" };
			var person = new Person () { FirstName = "David", LastName = "Cameron", Address = address };

			// connect to the database
			var database = new PetrifyDB (new MongoDbDriver(), "basic");

			// save the person aggrigate
			var id = database.Save (person);

			// load the person aggrigate
			var person2 = database.Load<Person> (id);

			Console.WriteLine (person2.Address.Street);
		}
	}
}

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
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Options;

namespace Petrify.MongoDB.Driver
{
	public class MongoDbDriver : IPetrifyDriver
	{
		MongoClient client;
		MongoServer server;
		MongoDatabase mongoDatabase;

		public MongoDbDriver (string database)
		{
			//todo: this has been depreicated
			DateTimeSerializationOptions.Defaults = DateTimeSerializationOptions.LocalInstance;

			client = new MongoClient (); // connect to localhost (this will do for now)
			server = client.GetServer ();
			mongoDatabase = server.GetDatabase (database);
		}

		#region IPetrifyDriver implementation

		public void Save (object value)
		{
			var collection = mongoDatabase.GetCollection(value.GetType (),value.GetType().Name.ToLower());
			var xx = collection.Save (value);
		}

		public void Update (object value)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}


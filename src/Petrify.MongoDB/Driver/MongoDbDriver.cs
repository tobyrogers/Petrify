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
using Petrify.Core.Repository;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Petrify.MongoDB.Driver
{
	public class MongoDbDriver : IPetrifyDriver
	{
		MongoClient client;
		MongoServer server;
		MongoDatabase mongoDatabase;
		string database;

		public MongoDbDriver (string database)
		{
			this.database = database;
			//todo: this has been depreicated
			//DateTimeSerializationOptions.Defaults = DateTimeSerializationOptions.LocalInstance;
		}

		#region IPetrifyDriver implementation
		public void Initialize (PetrifyRepository petrifyRepository)
		{
			var referenceSerializer = new ReferenceSerializer (petrifyRepository);
			var referenceSerialisationProvider = new ReferenceSerialisationProvider (petrifyRepository.EntityIdProvider, referenceSerializer);
			BsonSerializer.RegisterSerializationProvider (referenceSerialisationProvider);

			client = new MongoClient (); // connect to localhost (this will do for now)
			server = client.GetServer ();
			mongoDatabase = server.GetDatabase (database);
		}

		public void Save (Type defaultType, string collectionName, object entity)
		{
			var collection = mongoDatabase.GetCollection( defaultType, collectionName);
			collection.Save (entity);
		}

		public void Update (Type defaultType, string collectionName, object entity)
		{
			throw new NotImplementedException ();
		}

		public object Load (Type defaultType, string collectionName, object id)
		{
			var collection = mongoDatabase.GetCollection (defaultType, collectionName);
			var bsonId = BsonValue.Create (id);
			var obj = collection.FindOneByIdAs (defaultType, bsonId);
			return obj;
		}
		#endregion
	}
}


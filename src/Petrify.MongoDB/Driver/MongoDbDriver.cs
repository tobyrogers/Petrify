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
using Petrify.Core.Inspectors;
using Petrify.Core.ReferenceLoaders;

namespace Petrify.MongoDB.Driver
{
	public class MongoDbDriver : IPetrifyDriver
	{
		MongoDatabase _mongoDatabase;

		public MongoDbDriver (string connectionString) 
		{
			_mongoDatabase = GetDatabaseFromConnectionString (connectionString);
		}

		private static MongoDatabase GetDatabaseFromConnectionString (string connectionString)
		{
			var connectionStringBuilder = new MongoUrlBuilder (connectionString);
			var client = new MongoClient (connectionStringBuilder.ToMongoUrl());
			var server = client.GetServer ();
			var mongoDatabase = server.GetDatabase (connectionStringBuilder.DatabaseName);
			return mongoDatabase;
		}

		#region IPetrifyDriver implementation
		public void Initialize (IEntityInspector entityInspector, IReferenceLoader referenceLoader)
		{
			//todo: this has been depreicated
			//DateTimeSerializationOptions.Defaults = DateTimeSerializationOptions.LocalInstance;

			var referenceSerializer = new ReferenceSerializer (referenceLoader, entityInspector);
			var referenceSerialisationProvider = new ReferenceSerialisationProvider (entityInspector, referenceSerializer);
			BsonSerializer.RegisterSerializationProvider (referenceSerialisationProvider);

		}

		public void Save (Type defaultType, string collectionName, object entity)
		{
			var collection = _mongoDatabase.GetCollection (defaultType, collectionName);
			collection.Save (entity);
		}

		public void Update (Type defaultType, string collectionName, object entity)
		{
			throw new NotImplementedException ();
		}

		public object Load (Type defaultType, string collectionName, object id)
		{
			var collection = _mongoDatabase.GetCollection (defaultType, collectionName);
			var bsonId = BsonValue.Create (id);
			var obj = collection.FindOneByIdAs (defaultType, bsonId);
			return obj;
		}
		#endregion
	}
}


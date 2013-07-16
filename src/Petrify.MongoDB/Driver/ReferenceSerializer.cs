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
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.IO;
using System.Linq;
using MongoDB.Bson.Serialization.Conventions;
using Petrify.Core.Proxies;

namespace Petrify.MongoDB.Driver
{

	public class ReferenceSerializer : BsonBaseSerializer, IBsonIdProvider
	{
		private IPetrifyRepository _petrifyRepository;

		public ReferenceSerializer(IPetrifyRepository petrifyRepository)
		{
			_petrifyRepository = petrifyRepository;
		}

		public bool GetDocumentId (object document, out object id, out Type idNominalType, out IIdGenerator idGenerator)
		{
			var idPropertyInfo = document.GetType ().GetProperty ("Id", typeof(Guid));
			id = idPropertyInfo.GetValue (document, null);
			idNominalType = typeof(Guid);
			idGenerator = null;
			return true;
		}

		public void SetDocumentId (object document, object id)
		{
			throw new NotImplementedException ();
		}

		public override void Serialize (BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			if(bsonWriter.SerializationDepth > 0)
			{
				// this should be saved as a reference as it is not the top level entity
				var idPropertyInfo = value.GetType ().GetProperty ("Id", typeof(Guid));
				Guid id = (Guid)idPropertyInfo.GetValue (value, null);
				value = new EntityReference () { EntityType = nominalType.Name, EntityId = id };
			}
	
			BsonClassMap classMap = BsonClassMap.LookupClassMap (value.GetType ());
			var serializer = new BsonClassMapSerializer (classMap);
			serializer.Serialize (bsonWriter, nominalType, value, options);
		}

		public override object Deserialize (BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
		{
			BsonClassMap classMap = BsonClassMap.LookupClassMap (actualType);
			var serializer = new BsonClassMapSerializer (classMap);
			var obj = serializer.Deserialize (bsonReader, nominalType, actualType, options);

			if (actualType == typeof(EntityReference))
			{
				obj = _petrifyRepository.LoadEntity ((EntityReference)obj);
			}

			return obj;
		}
	}

}

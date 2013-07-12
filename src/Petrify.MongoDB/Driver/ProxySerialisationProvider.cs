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
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.IO;

namespace Petrify.MongoDB.Driver
{

	public class ProxySerialisationProvider : IBsonSerializationProvider
	{
		private Type _rootType;

		public ProxySerialisationProvider (Type rootType)
		{
			_rootType = rootType;
		}

		#region IBsonSerializationProvider implementation
		public IBsonSerializer GetSerializer (Type type)
		{
//			var xx = type.GetInterface ("IReferenceProxy");
			if (type.IsSubclassOf(_rootType))
			{
				return new ReferenceProxySerializer ();
			}


			return null;
		}
		#endregion
	}

	public class Ref
	{
		public object RefId { get; set; }

		public string RefType { get; set; }
	}

	public class ReferenceProxySerializer : BsonBaseSerializer, IBsonIdProvider
	{
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
			if(value.GetType().GetInterface ("IReferenceProxy")!=null)
			{
				// this should be saved as a reference
				var idPropertyInfo = value.GetType().GetProperty ("Id", typeof(Guid));
				Guid id = (Guid)idPropertyInfo.GetValue (value, null);
				value = new Ref () { RefType = nominalType.Name, RefId = id };
			}
	
			BsonClassMap classMap = BsonClassMap.LookupClassMap (value.GetType());
			var serializer = new BsonClassMapSerializer (classMap);
			serializer.Serialize (bsonWriter, nominalType, value, options);
		}
	
		public override object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
		{
			if (bsonReader.GetCurrentBsonType() == BsonType.Null)
			{
				bsonReader.ReadNull();
				return null;
			}
			else
			{
				var discriminatorConvention = BsonSerializer.LookupDiscriminatorConvention(nominalType);
				var actualType = discriminatorConvention.GetActualType(bsonReader, nominalType);
				if (actualType != nominalType)
				{
					var serializer = BsonSerializer.LookupSerializer(actualType);
					if (serializer != this)
					{
						return serializer.Deserialize(bsonReader, nominalType, actualType, options);
					}
				}

				return Deserialize(bsonReader, nominalType, actualType, options);
			}
		}

		public override object Deserialize (BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
		{


			BsonClassMap classMap = BsonClassMap.LookupClassMap (actualType);
			var serializer = new BsonClassMapSerializer (classMap);
			var objRef = serializer.Deserialize (bsonReader, nominalType, actualType, options);

			// create proxy
			return objRef;
		}
	}
}


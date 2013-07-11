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

		public ProxySerialisationProvider ()
		{
		}

		public ProxySerialisationProvider (Type rootType)
		{
			_rootType = rootType;
		}
		#region IBsonSerializationProvider implementation
		public IBsonSerializer GetSerializer (Type type)
		{
			var xx = type.GetInterface ("IReferenceProxy");
			if (xx != null)
			{
				return new ReferenceSerializer ();
			}
			return null;
		}
		#endregion
	}

	public class ReferenceSerializer : BsonBaseSerializer
	{
		public override void Serialize (BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
		{
			bsonWriter.WriteStartDocument ();
			//bsonWriter.WriteString ("test");
			bsonWriter.WriteEndDocument ();
		}
	}
}


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
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.IO;
using System.Linq;
using MongoDB.Bson.Serialization.Conventions;
using Petrify.Core.ReferenceLoaders;
using Petrify.Core.Inspectors;
using Petrify.Core.EntityReferences;

namespace Petrify.MongoDB.Driver
{
	public class ReferenceSerialisationProvider : IBsonSerializationProvider
	{
		private IEntityInspector _entityInspector;
		private IBsonSerializer _referenceSerializer;

		public ReferenceSerialisationProvider (IEntityInspector entityInspector, IBsonSerializer referenceSerializer)
		{
			_entityInspector = entityInspector;
			_referenceSerializer = referenceSerializer;
		}

		#region IBsonSerializationProvider implementation
		public IBsonSerializer GetSerializer (Type type)
		{
			if (_entityInspector.IsEntity(type))
			{
				BsonSerializer.RegisterDiscriminatorConvention (type, new ReferenceDescriminatorConvention ());
				return _referenceSerializer;
			} 
			else if (type == typeof(EntityReference))
			{
				return _referenceSerializer;
			}

			return null;
		}
		#endregion
	}


}


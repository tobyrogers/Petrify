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
using Petrify.Core.ReferenceLoaders;
using Petrify.Core.Exceptions;
using Petrify.Core.EntityReferences;

namespace Petrify.MongoDB.Driver
{
	public class ReferenceDescriminatorConvention : IDiscriminatorConvention
	{
		private readonly Type _referenceType;
		private readonly string _referenceDescriminator;
		IDiscriminatorConvention _underlyingConvention = StandardDiscriminatorConvention.Hierarchical;

		public ReferenceDescriminatorConvention ()
		{
			_referenceType = typeof(EntityReference);
			_referenceDescriminator = _referenceType.Name;
		}

		public Type GetActualType (BsonReader bsonReader, Type nominalType)
		{
			var bsonType = bsonReader.GetCurrentBsonType ();
			if (bsonType == BsonType.Document)
			{
				var bookmark = bsonReader.GetBookmark ();
				bsonReader.ReadStartDocument ();
				var actualType = nominalType;
				if (bsonReader.FindElement (_underlyingConvention.ElementName))
				{
					var discriminator = (BsonValue)BsonValueSerializer.Instance.Deserialize (bsonReader, typeof(BsonValue), null);
					bsonReader.ReturnToBookmark (bookmark);
					if (discriminator.Equals (_referenceDescriminator))
					{
						actualType = _referenceType;
					} else
					{
						actualType = _underlyingConvention.GetActualType (bsonReader, nominalType);
					}
				}

				return actualType;
			}

			throw new PetrifyException ("ReferenceDescrimator::GetActualType called on a non document bson element");
		}

		public BsonValue GetDiscriminator (Type nominalType, Type actualType)
		{
			if (actualType == _referenceType)
			{
				return _referenceDescriminator;
			}

			return _underlyingConvention.GetDiscriminator (nominalType, actualType);
		}

		public string ElementName
		{
			get
			{
				return _underlyingConvention.ElementName;
			}
		}
	}
}

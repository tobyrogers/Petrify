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
using Petrify.Core.Exceptions;
using System.Reflection;

namespace Petrify.Core.Inspectors
{
	public class AutoEntityInspector : IEntityInspector
	{
		private string _idPropertyName = "Id";
		private Type _entityType;
		private PropertyInfo _idPropertyInfo;

		public void AssertIsEntity (Type type)
		{
			if (!IsEntity (type))
			{
				throw new PetrifyException (string.Format ("Type {0} is not an entity type", type));
			}
		}

		public object GetEntityId (object entity)
		{
			AssertIsEntity (entity.GetType ());
			var id = _idPropertyInfo.GetValue (entity, null);
			return id;
		}	

		public void SetEntityId (object entity, object id)
		{
			AssertIsEntity (entity.GetType ());
			_idPropertyInfo.SetValue (entity, id, null);
		}

		public bool IsEntity (Type type)
		{
			if (_entityType == null)
			{
				// autodetect entity type;
				Type rootType = new DefaultRootFinder ().GetRootType (type);
				var propertyInfo = rootType.GetProperty (_idPropertyName);
				if (propertyInfo != null)
				{
					_entityType = rootType;
					_idPropertyInfo = propertyInfo;
				}

			}

			bool isEntity = type.IsSubclassOf (_entityType);

			return isEntity;
		}
	}
}


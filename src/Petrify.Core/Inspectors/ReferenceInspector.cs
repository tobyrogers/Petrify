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
using System.Reflection;
using System.Collections.Generic;

namespace Petrify.Core.Inspectors
{
	public interface IReferenceInspector
	{

	}

	public class ReferenceInspector
	{
		IEntityInspector _entityIdProvider;

		public ReferenceInspector (IEntityInspector entityIdProvider)
		{
			_entityIdProvider = entityIdProvider;
		}

		public  IEnumerable<ReferenceProperty> GetReferences (object obj)
		{
			return GetReferences (obj, 0);
		}

		private IEnumerable<ReferenceProperty> GetReferences (object obj, int level)
		{
			level++;
			var references = new List<ReferenceProperty> ();

			// get all public instance methods
			var propertyInfos = obj.GetType ().GetProperties (BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				// if it not a class then it can't be an entity or have children to inspect
				// todo: what about interfaces??
				if (propertyInfo.PropertyType.IsClass)
				{
					var value = propertyInfo.GetValue (obj, null);
					if (value != null)
					{
						// find all properties on this class that are entities in their own right
						// ie. this property represents a reference
						if (_entityIdProvider.IsEntity (value.GetType()))
						{
							references.Add (new ReferenceProperty (propertyInfo, obj, level));
						}

						// recurse down the tree
						var childReferences = GetReferences (value, level);
						references.AddRange (childReferences);
					}
				}
			}

			return references;
		}
	}
}


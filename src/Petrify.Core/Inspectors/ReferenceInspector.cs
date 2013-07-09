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
	public class ReferenceInspector
	{
		Type _rootType;

		public ReferenceInspector (Type rootType)
		{
			_rootType = rootType;
		}

		public  IEnumerable<Reference> GetReferences (object obj)
		{
			return GetReferences (obj, 0);
		}

		private IEnumerable<Reference> GetReferences (object obj, int level)
		{
			level++;
			var references = new List<Reference> ();

			// get all public instance methods
			var propertyInfos = obj.GetType ().GetProperties (BindingFlags.Public|BindingFlags.Instance);
			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				if (propertyInfo.PropertyType.IsClass)
				{
					// find all properties on this class that are aggrigates (ie derive from rootType)
					if (propertyInfo.PropertyType.IsSubclassOf (_rootType))
					{
						references.Add (new Reference(propertyInfo, obj, level));
					}

					// recurse down the tree
					var value = propertyInfo.GetValue (obj, null);
					if (value != null)
					{
						var childReferences = GetReferences (value, level);
						references.AddRange (childReferences);
					}
				}
			}

			return references;
		}
	}

	public class Reference
	{
		PropertyInfo _propertyInfo;
		object _obj;

		public Reference (PropertyInfo propertyInfo, object obj, int level)
		{
			_obj = obj;
			_propertyInfo = propertyInfo;
			Depth = level;
		}

		public int Depth { get; private set;} 

		public object GetValue ()
		{
			return _propertyInfo.GetValue (_obj, null);
		}

		public void SetValue (object value)
		{
			_propertyInfo.SetValue (_obj, value, null);
		}
	}
}


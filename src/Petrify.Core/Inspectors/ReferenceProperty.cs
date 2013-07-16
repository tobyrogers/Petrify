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
	/// <summary>
	/// Represents a property of an entity that is a reference to another entity
	/// </summary>
	public class ReferenceProperty
	{
		PropertyInfo _propertyInfo;
		object _obj;

		public ReferenceProperty (PropertyInfo propertyInfo, object obj, int level)
		{
			_obj = obj;
			_propertyInfo = propertyInfo;
			Depth = level;
		}

		/// <summary>
		/// The depth if the reference in the entity hierachy
		/// </summary>
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

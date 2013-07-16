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

namespace Petrify.Core.Inspectors
{
	public class DefaultRootFinder
	{
		public Type GetRootType (Type type)
		{
			while (type.BaseType != typeof(object) || type.IsInterface)
			{
				type = type.BaseType;
			}

			CheckAggrigateRootType (type);

			return type;
		}

		public bool IsValidAggrigateRoot (Type type)
		{
			// type must have an Id property of type Guid;
			// todo: need to be more flexible here!
			var propertyInfo = type.GetProperty ("Id", typeof(Guid));
			return (propertyInfo != null);
		}

		public void CheckAggrigateRootType (Type type)
		{
			if (!IsValidAggrigateRoot (type))
			{
				throw new ApplicationException (string.Format ("Type {0} is an invalid class to use as an aggrigate root. The class must have an Id property of type Guid.", type.Name));
			}
		}
	}
}


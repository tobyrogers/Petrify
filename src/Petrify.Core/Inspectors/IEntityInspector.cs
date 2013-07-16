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
	public interface IEntityInspector
	{
		object GetEntityId (object entity);

		void SetEntityId (object entity, object id);

		bool IsEntity (Type type);

		void AssertIsEntity (Type type);
	}
	
}

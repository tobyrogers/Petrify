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
using Petrify.Core.Inspectors;
using Petrify.Core.ReferenceLoaders;

namespace Petrify.Core.Drivers
{
	public interface IPetrifyDriver
	{
		/// <summary>
		/// Saves a new object to the database.
		/// </summary>
		void Save (Type defaultType, string collectionName, object entity);

		/// <summary>
		/// Updates an existing object in the database
		/// </summary>
		void Update (Type defaultType, string collectionName, object entity);

		/// <summary>
		/// Load the specified type and id.
		/// </summary>
		object Load (Type defaultType, string collectionName, object id);	

		// shoud this just be a confriguration object?
		void Initialize (IEntityInspector entityInspector, IReferenceLoader referenceLoader);
	}
}

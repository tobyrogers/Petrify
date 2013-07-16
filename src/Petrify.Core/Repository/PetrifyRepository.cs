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
using System.Linq;
using System.Collections.Generic;
using Petrify.Core.TableMappers;

namespace Petrify.Core.Repository
{
	public interface IPetrifyRepository
	{
		object Load (Type type, object id);
	}

	public class PetrifyRepository : IPetrifyRepository
	{
		IPetrifyDriver _driver;

		public IEntityInspector EntityInspector { get; set; }

		public ITableMapper TableMapper { get; set; }

		public PetrifyRepository (IPetrifyDriver driver)
		{
			_driver = driver;
			EntityInspector = new AutoEntityInspector ();
			TableMapper = new  AutoTableMapper ();

			_driver.Initialize (this); // driver needs to know what the root type is 
		}

		private object SaveOrUpdate (object entity)
		{
			var tableMapping = TableMapper.GetTableMapping (entity.GetType ());

			// todo: don't like this!
			// get the Id, if null then assign one
			Guid id = (Guid)EntityInspector.GetEntityId (entity);
			if (id == Guid.Empty)
			{
				// todo: create a id generator
				id = Guid.NewGuid ();
				// make sure the id has been set back on the entity
				EntityInspector.SetEntityId (entity, id);
				// if the Id was null then it must be saved
				_driver.Save (tableMapping.BaseType, tableMapping.TableName, entity);
			} else
			{
				// todo: if the document already has an id then we need to determine if
				// it has been modified. Need some dirty flag...a job for Castle Dynamic Proxy?
				_driver.Update (tableMapping.BaseType, tableMapping.TableName, entity);
			}

			return id;
		}

		public object Save (object entity)
		{
			EntityInspector.AssertIsEntity (entity.GetType ());

			// now scan the document to find all referenced entities
			// (i.e. find all properties that derive from root class)
			var referenceProperties = new ReferenceInspector (EntityInspector).GetReferences (entity);

			// sort by depth to ensure that that deepest depth get saved first
			// if this save failed then deeper depth items may become orphans 
			// (is this a problem?...probably not, could unwind if required)
			referenceProperties = referenceProperties.OrderByDescending (x => x.Depth);

			// now save, update any referenced entities
			foreach (var referenceProperty in referenceProperties)
			{
				// get the value of this reference
				var value = referenceProperty.GetValue ();

				// save or update this reference
				SaveOrUpdate (value);
			}

			// now save the top level document (all it's references will have been saved by now)
			var id = SaveOrUpdate (entity);
			return id;
		}

		public T Load<T> (object id)
		{ 
			return (T)Load (typeof(T),id);
		}

		public object Load (Type type, object id)
		{
			var tableMapping = TableMapper.GetTableMapping (type);
			return _driver.Load (tableMapping.BaseType, tableMapping.TableName, id);
		}
	}
}


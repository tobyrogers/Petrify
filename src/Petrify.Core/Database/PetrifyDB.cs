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
using Castle.DynamicProxy;
using System.Linq;

namespace Petrify.Core.Database
{
	public class PetrifyDB
	{
		IPetrifyDriver _driver;
		Type rootType;

		public PetrifyDB (IPetrifyDriver driver)
		{
			_driver = driver;
		}

		public object Save (object aggrigate)
		{
			// if it has not been specified, get the rootType from the object
			if (rootType == null)
			{
				rootType = new RootInspector ().GetRootType (aggrigate.GetType ());
			}

			// now scan the aggrigate to find all referenced aggrigates
			// (i.e. find all properties that derive from root class)
			// and replace them with a proxy

			var references = new ReferenceInspector (rootType).GetReferences (aggrigate);
			// todo: replace with proxy...

			// sort by depth to ensure that that deepest depth get saved first
			references = references.OrderByDescending (x => x.Depth).ToList ();

			// now save the aggrigate and any referenced aggrigates that are new...
			// if this save failed then deeper depth items may become orphans (is this a problem?)
			foreach (var reference in references)
			{
				// get the object of this reference
				var value = reference.GetValue ();

				// get the Id, if null then assign one
				var idPropertyInfo = rootType.GetProperty ("Id", typeof(Guid));
				Guid id = (Guid)idPropertyInfo.GetValue (value, null);
				if (id. == null)
				{
					// todo: create a id generator
					var newId = Guid.NewGuid ();

					// make sure the id has been set back on the aggrigate
					idPropertyInfo.SetValue (value, newId, null);

					// if the Id was null then it must be saved
					_driver.Save(value);
				}
				else
				{
					// todo: if the aggrigate already has an id then we need to determine if
					// it has been modified
					_driver.Update (value);
				}
			}

			return aggrigate;
		}

		public T Load<T> (object id)
		{
			return default(T);
		}
	}
}


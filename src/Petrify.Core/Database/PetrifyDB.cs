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
using System.Collections.Generic;

namespace Petrify.Core.Database
{
	public class PetrifyDB
	{
		IPetrifyDriver _driver;
		public Type RootType { get; private set;}

		public PetrifyDB (IPetrifyDriver driver)
		{
			_driver = driver;
		}

		private object SaveOrUpdate (object value)
		{
			// get the Id, if null then assign one
			var idPropertyInfo = RootType.GetProperty ("Id", typeof(Guid));
			Guid id = (Guid)idPropertyInfo.GetValue (value, null);
			if (id == Guid.Empty)
			{
				// todo: create a id generator
				id = Guid.NewGuid ();
				// make sure the id has been set back on the aggrigate
				idPropertyInfo.SetValue (value, id, null);
				// if the Id was null then it must be saved
				_driver.Save (value);
			}
			else
			{
				// todo: if the aggrigate already has an id then we need to determine if
				// it has been modified. Need some dirty flag...a job for Castle Dynamic Proxy?
				_driver.Update (value);
			}

			return id;
		}

		public object Save (object aggrigate)
		{
			// if it has not been specified, get the rootType from the object
			if (RootType == null)
			{
				RootType = new RootInspector ().GetRootType (aggrigate.GetType ());
				_driver.Initialize (this);
			}

			// now scan the aggrigate to find all referenced aggrigates
			// (i.e. find all properties that derive from root class)
			var references = new ReferenceInspector (RootType).GetReferences (aggrigate);

			// sort by depth to ensure that that deepest depth get saved first
			// if this save failed then deeper depth items may become orphans (is this a problem?...probably not)
			references = references.OrderByDescending (x => x.Depth);

			// now save, update any referenced aggrigates
			foreach (var reference in references)
			{
				// get the object of this reference
				var value = reference.GetValue ();

				// replace on parent object with proxy...
				ProxyGenerator generator = new ProxyGenerator();
				ProxyGenerationOptions options = new ProxyGenerationOptions ();
				options.AddMixinInstance (new ReferenceProxy ());
				var proxyValue = generator.CreateClassProxyWithTarget (value.GetType (), value, options);
				reference.SetValue (proxyValue);

				// save or update this reference
				SaveOrUpdate (value);
			}

			// now save the top level aggrigate (all it's references will have been saved by now)
			var id = SaveOrUpdate (aggrigate);

			return id;
		}

		public T Load<T> (object id)
		{
			return (T)_driver.Load(typeof(T),id);
		}

	}

	public interface IReferenceProxy
	{
//		Guid Id { get; }
//		Type Type { get; }
	}

	public class ReferenceProxy : IReferenceProxy
	{
//
//		public ReferenceProxy(object reference)
//		{
//			Type = reference.GetType ();
//			var propertyInfo =  Type.GetProperty ("Id", typeof (Guid));
//			Id = (Guid)propertyInfo.GetValue (reference, null);
//		}
//
//		#region IReferenceProxy implementation
//
//		public Guid Id { get; private set; }
//		public Type Type { get; private set; }
//
//		#endregion
	}

}


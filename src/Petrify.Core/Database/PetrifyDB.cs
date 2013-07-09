using System;
using Petrify.Core.Inspectors;
using Castle.DynamicProxy;

namespace Petrify.Core.Database
{
	public class PetrifyDB
	{
		IPetrifyDriver _driver;
		string _database;
		Type rootType;

		public PetrifyDB (IPetrifyDriver driver, string database)
		{
			_database = database;
			_driver = driver;
		}

		public object Save (object aggrigate)
		{
			// if it has not been specified, get the rootType from the object
			if (rootType == null) {
				rootType = new RootInspector().GetRootType (aggrigate.GetType());
			}

			// now scan the aggrigate to find all referenced aggrigates
			// (i.e. find all properties that derive from root class)
			// and replace them with a proxy

			var references = new ReferenceInspector (rootType).GetReferences (aggrigate);
			// todo: replace with proxy...

			// now save the aggrigate and any referenced aggrigates that are new...

			// do some non - transactional wizidary here to save the agrigate
			// make sure the id has been saved on the aggrigate (assuming it was a new one)


			return aggrigate;
		}

		public T Load<T>(object id)
		{
			return default(T);
		}
	}
}


using System;

namespace Petrify.Core.Inspectors
{
	public class RootInspector
	{
		public RootInspector ()
		{
		}

		public Type GetRootType(Type type)
		{
			while(type.BaseType != typeof(object) || type.IsInterface)
			{
				type = type.BaseType;
			}

			CheckAggrigateRootType (type);

			return type;
		}

		public bool IsValidAggrigateRoot(Type type)
		{
			// type must be derived from object

			// type must have an Id property od type Guid;
			var propertyInfo =  type.GetProperty ("Id", typeof (Guid));
			if (propertyInfo == null)
			{
				return false;
			}

			return true;

		}

		public void CheckAggrigateRootType(Type type)
		{
			if (!IsValidAggrigateRoot (type)) {
				throw new ApplicationException (string.Format("Type {0} is an invalid class to use as an aggrigate root. The class must have an Id property of type Guid.", type.Name));
			}

		}

	}
}


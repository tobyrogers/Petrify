using System;

namespace Petrify.Core.TestData
{
	class ComplexAggrigate : BasicAggrigate, IDisposable
	{
		public BasicAggrigate ReferencedAggrigate{ get; set; }
		#region IDisposable implementation
		public void Dispose ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}


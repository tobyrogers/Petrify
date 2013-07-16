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
using Castle.DynamicProxy;
using Petrify.Core.Inspectors;

namespace Petrify.Core.Proxies
{
	public class CastleDynamicProxyReferenceMarker : IReferenceMarker
	{
		/// <summary>
		/// Marks an object as being a entity reference
		/// </summary>
		public void MarkAsReference (ReferenceProperty referenceProperty)
		{
			// todo: check that all the methods are virtual, if not throw exception
			ProxyGenerator generator = new ProxyGenerator ();
			ProxyGenerationOptions options = new ProxyGenerationOptions ();
			options.AddMixinInstance (new ReferenceProxy ());

			var value = referenceProperty.GetValue();
			var proxyValue = generator.CreateClassProxyWithTarget (value.GetType (), value, options);
			referenceProperty.SetValue(proxyValue);
		}

		/// <summary>
		/// Determines whether the object is an entity reference.
		/// </summary>
		public bool IsMarkedAsReference (object obj)
		{
			return  obj.GetType ().GetInterface (typeof(IReferenceProxy).Name) != null;
		}

		internal interface IReferenceProxy
		{
		}

		internal class ReferenceProxy : IReferenceProxy
		{
		}
	}

}


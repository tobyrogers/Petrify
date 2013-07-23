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
using Petrify.Core.Repository;

namespace Petrify.Core.ReferenceLoaders
{
	public class CastleLazyReferenceLoader : IReferenceLoader
	{
		private IRepository _repository;

		public CastleLazyReferenceLoader (IRepository repository)
		{
			_repository = repository;
		}

		public object LoadReference (Type type, object id)
		{
			// todo: check that all the methods are virtual, if not throw exception
			ProxyGenerator generator = new ProxyGenerator ();
			var proxy = generator.CreateClassProxy (type, new LazyLoadInterceptor (_repository, type, id));

			return proxy;
		}

		internal class LazyLoadInterceptor : IInterceptor
		{
			IRepository _repository;
			Type _type;
			object _id;
			object _target;
			readonly object _lock = new object();

			public LazyLoadInterceptor (IRepository repository, Type type, object id)
			{
				_id = id;
				_type = type;
				_repository = repository;
			}

			public void Intercept (IInvocation invocation)
			{
				if (_target == null)
				{
					lock (_lock)
					{
						if (_target == null)
						{
							// idealy what I would want to do here is to replace the target of
							// invocation. However with castle this only appears possible with 
							// interface proxies
							_target = _repository.Load (_type, _id);
						}
					}
				}
				invocation.ReturnValue = invocation.Method.Invoke (_target, invocation.Arguments);
			}
		}
	}
}


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
using Petrify.Core.Drivers;

namespace Petrify.Core
{
	public class RepositoryConfiguration
	{
		IPetrifyDriver _petrifyDriver;

		public RepositoryConfiguration ()
		{
		}

		void SetDatabaseConfiguration (IPetrifyDriver petrifyDriver)
		{
			if (petrifyDriver == null) throw new ArgumentNullException ("petrifyDriver");
			_petrifyDriver = petrifyDriver;
		}

		public RepositoryDatabaseConfiguration ConnectTo
		{
			get
			{
				return new RepositoryDatabaseConfiguration (this, s => SetDatabaseConfiguration (s));
			}
		}

		public IRepository BuildRepository()
		{
			return new Repository (_petrifyDriver);
		}
	}

	public class RepositoryDatabaseConfiguration
	{
		readonly RepositoryConfiguration _repositoryConfiguration;
		readonly Action<IPetrifyDriver> _setDatabase;

		internal RepositoryDatabaseConfiguration (RepositoryConfiguration repositoryConfiguration, Action<IPetrifyDriver> setDatabase)
		{
			if (repositoryConfiguration == null) throw new ArgumentNullException("repositoryConfiguration");
			if (setDatabase == null) throw new ArgumentNullException("setDatabase");
			_repositoryConfiguration = repositoryConfiguration;
			_setDatabase = setDatabase;		
		}

		public RepositoryConfiguration Database (IPetrifyDriver petrifyDriver)
		{
			_setDatabase (petrifyDriver);
			return _repositoryConfiguration;
		}
	}

}


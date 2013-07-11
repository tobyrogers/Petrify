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

namespace Petrify.Core.TestData
{
	class BasicAggrigate : ValidRoot
	{

	}

	public class Entity
	{
		public Guid Id { get; set; }
	}

	public class Person : Entity
	{
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual Address Address { get; set; }
	}

	public class Address : Entity
	{
		public virtual string Street { get; set; }
		public virtual string Town { get; set; }
		public virtual string Postcode { get; set; }
	}
}


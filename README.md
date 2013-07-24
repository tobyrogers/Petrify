Welcome to Petrify!
===================

Petrify is a .NET entity relationship manager for NoSQL databases.

Based on the Aggregate pattern described in [Domain Driven Design] [DDD], Petrify identifies the root entity of an aggrigate, ensures that aggregates of different base types are persisted in separate collections, and enforces that nothing outside an aggregate boundary can hold a reference to anything inside, except to the root entity. Combined with the inherent properties of NoSQL databases, this enforces all the rules governing the implementation of aggregates as described in [Domain Driven Design] [DDD], Ch.6 p128.

Petrify also supports the lazy loading of associated aggregated (references).

[DDD]: http://dddcommunity.org/book/evans_2003/

Getting Started
---------------

The getting started guide and further documentation can be found on the [Wiki](https://github.com/tobyrogers/Petrify/wiki)

Feedback
--------

Please submit any feedback to our [Google Group](https://groups.google.com/forum/#!forum/petrifyforum)

Latest Version
--------------

The quickest way to get the latest release of Petrify is to add it to your project using NuGet (http://nuget.org/List/Packages/Petrify).

Select the package to support your NoSql Database:

* MongoDB (http://nuget.org/List/Packages/Petrify.MongoDB)
* RavenDB (coming soon!)

License
-------
The library is licensed under the terms of the [Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0.html).

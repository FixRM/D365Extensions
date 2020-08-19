# What's new
### [1.2.1](https://github.com/FixRM/D365Extensions/compare/v1.2.0...v1.2.1) (2020-08-19)


### Bug Fixes

* methods GetPreTarget<T> and GetPostTarget<T> cause exception ([1d86f3d](https://github.com/FixRM/D365Extensions/commit/1d86f3d))



## [1.2.0](https://github.com/FixRM/D365Extensions/compare/v1.1.0...v1.2.0) (2020-07-19)


### Bug Fixes

* add parameter types ([b54094f](https://github.com/FixRM/D365Extensions/commit/b54094f))
* check if default constructor works for all ,Query extensions ([c386c3a](https://github.com/FixRM/D365Extensions/commit/c386c3a))
* expressions can be null ([c79bde3](https://github.com/FixRM/D365Extensions/commit/c79bde3))
* get rid of EntityName in GetAliasedEntity method ([811d75d](https://github.com/FixRM/D365Extensions/commit/811d75d))
* get rid of unnecessary EntityName parameter ([8027dcd](https://github.com/FixRM/D365Extensions/commit/8027dcd))
* get rid of unnecessary parameter ([2e3c151](https://github.com/FixRM/D365Extensions/commit/2e3c151))
* oder type is not assigned as expected ([b9042b4](https://github.com/FixRM/D365Extensions/commit/b9042b4))


### Features

* add ColumnSet<T> extension ([dc87311](https://github.com/FixRM/D365Extensions/commit/dc87311))
* add ColumnSetExtensions class ([d6ffcb1](https://github.com/FixRM/D365Extensions/commit/d6ffcb1))
* add ConditionExpression<T> ([89fc043](https://github.com/FixRM/D365Extensions/commit/89fc043))
* add FilterExpression extensions ([190018e](https://github.com/FixRM/D365Extensions/commit/190018e))
* add IOrganizationService.Update extensions ([fd52053](https://github.com/FixRM/D365Extensions/commit/fd52053))
* add LinkEntity<TFrom, TTo> class ([5a92741](https://github.com/FixRM/D365Extensions/commit/5a92741))
* add LinkEntityExtensions class ([0590f90](https://github.com/FixRM/D365Extensions/commit/0590f90))
* add OrderExpression<T> extension ([599805e](https://github.com/FixRM/D365Extensions/commit/599805e))
* add QueryByAttributeExtensions ([7899dfa](https://github.com/FixRM/D365Extensions/commit/7899dfa))
* add QueryExpression extensions ([967c2d6](https://github.com/FixRM/D365Extensions/commit/967c2d6))



## [1.1.0](https://github.com/FixRM/D365Extensions/compare/v1.0.45...v1.1.0) (2019-08-30)


### Bug Fixes

* fix major version number ([7ce93a0](https://github.com/FixRM/D365Extensions/commit/7ce93a0))


### Features

* semver support started ([2c365c9](https://github.com/FixRM/D365Extensions/commit/2c365c9))



## Version 1.0.45
Fix .nuspec issue

## Version 1.0.43
Add support for alternative keys for Associate and Disassociate

## Version 1.0.42
Add shortcut for Upsert message.

Fix a bug in FetchQuery.NextPage

## Version 1.0.40
No changes. .csproj files where migrated to last format, + corresponding changes in CI pipeline

## Version 1.0.38
Add support for working with large datasets for all query types including FetchExpression! This feature is implemented as simple but resource effective RetrieveMultiple override. At the moment it's some kind of experimental, so please, give me feedback on that!

Add generic override of Execute method. Probably the most stupid feature here :)

## Version 1.0.37
Add override of ToEntityReference to support alternative keys.

## Version 1.0.35
Now assembly have a strong name and should work with VS Live Unit Testing.

## Version 1.0.29
Add support for alternative keys for Retrieve and Delete!
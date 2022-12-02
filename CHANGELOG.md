# What's new
### [1.5.3](https://github.com/FixRM/D365Extensions/compare/v1.5.2...v1.5.3) (2022-12-02)


### Bug Fixes

* fix null reference for OOB ExecuteMultipleResponse objects ([b0f1114](https://github.com/FixRM/D365Extensions/commit/b0f1114b2a4219909c0fab018d4032c06cf4e36e))

### [1.5.2](https://github.com/FixRM/D365Extensions/compare/v1.5.1...v1.5.2) (2022-12-02)


### Features

* **experimental:** return request collection with results ([31d8d7f](https://github.com/FixRM/D365Extensions/commit/31d8d7f5ca8730e5829d8fd59c1e3cdbe04912c8))

### [1.5.1](https://github.com/FixRM/D365Extensions/compare/v1.5.0...v1.5.1) (2022-12-01)


### Bug Fixes

* remove useless callback ([3be6d53](https://github.com/FixRM/D365Extensions/commit/3be6d530f5a165bd1d05436556d9dcbcecad66f8))

## [1.5.0](https://github.com/FixRM/D365Extensions/compare/v1.4.1...v1.5.0) (2022-12-01)


### Features

* add Execute(IEnumerable<OrganizationRequest> requests) method ([284197b](https://github.com/FixRM/D365Extensions/commit/284197b9eec23f1689091fb3393f94c426eea5cd))
* add helper method for breaking collection of OrganizationRequest elements to OrganizationRequestCollections of given size ([0cb7d58](https://github.com/FixRM/D365Extensions/commit/0cb7d583c1909db81ed95f22cb5c71916960a360))

### [1.4.1](https://github.com/FixRM/D365Extensions/compare/v1.4.0...v1.4.1) (2022-04-15)


### Bug Fixes

* GetOrganization now return correct value ([797b501](https://github.com/FixRM/D365Extensions/commit/797b501340e96f77a22f43e68c9b6187b4d7bc70))

## [1.4.0](https://github.com/FixRM/D365Extensions/compare/v1.3.0...v1.4.0) (2022-02-07)


### Features

* add DataCollection extension methods ([34ed760](https://github.com/FixRM/D365Extensions/commit/34ed7609ad99ae03af413bec6980a40f43feb2f1))
* attributeLogicalName is now optional for GetAliasedValue method ([a39c983](https://github.com/FixRM/D365Extensions/commit/a39c9833eb86fbab400b822d57e48e3f2be7ec3c))

## [1.3.0](https://github.com/FixRM/D365Extensions/compare/v1.2.1...v1.3.0) (2021-10-15)


### Features

* add fallback method to get member name ([e23ddfe](https://github.com/FixRM/D365Extensions/commit/e23ddfe2fd3217f1a3464e10d0e75e6d2ccf17aa))
* add simple bencmark to measure reflection speed ([a4b68d7](https://github.com/FixRM/D365Extensions/commit/a4b68d7a1b1c70d5d4b80b96252a4a022c7ee260))
* basic implementation ([dd6773e](https://github.com/FixRM/D365Extensions/commit/dd6773e5b981948935bef5064260baa74be449d6))
* get Entity LogicalName via reflection ([8e4984f](https://github.com/FixRM/D365Extensions/commit/8e4984f269abda2dc81846a40b7ca70f3cb2271b))
* now using reflection by default ([d55dbe9](https://github.com/FixRM/D365Extensions/commit/d55dbe9ccf34747a356baca817716c1dd9ee37a2))


### Bug Fixes

* build fixed ([31cc121](https://github.com/FixRM/D365Extensions/commit/31cc1212d1b6ceba73eba996e2f07c5126965c9a))

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
# What's new
## [2.7.0](https://github.com/FixRM/D365Extensions/compare/v2.6.0...v2.7.0) (2025-06-16)


### Features

* add RetrieveSingle and RetrieveSingleOrDefault extension methods for IOrganizationService ([003ccd7](https://github.com/FixRM/D365Extensions/commit/003ccd7f95af62dac76aa3cffb8333dbdc08bd7a))
* add SetTopCount extension method for QueryBase ([0e9bca8](https://github.com/FixRM/D365Extensions/commit/0e9bca856efc59c3bad60c292d50a6e0820eebd6))

## [2.6.0](https://github.com/FixRM/D365Extensions/compare/v2.5.0...v2.6.0) (2025-06-14)


### Features

* ColumnSet<T> now supports expressions with anonimous object + general improvements in expression handling ([99fe57b](https://github.com/FixRM/D365Extensions/commit/99fe57bdd0c929dc93cd3c187809834b03ab5286))

## [2.5.0](https://github.com/FixRM/D365Extensions/compare/v2.4.0...v2.5.0) (2025-05-02)


### Features

* add generic value of AliasedValue ([4d35791](https://github.com/FixRM/D365Extensions/commit/4d35791b71078c25765746ada21bf80261a096e0))
* GetAliasedEntity now return Entity with Id ([45d1554](https://github.com/FixRM/D365Extensions/commit/45d1554c84904e15334dfd12a48a55ec09bbd1ad))

## [2.4.0](https://github.com/FixRM/D365Extensions/compare/v2.3.0...v2.4.0) (2025-04-27)


### Features

* add ApplyChanges extension method for Entity ([70a4446](https://github.com/FixRM/D365Extensions/commit/70a44465c61e77f3392e17c866d3b0f028b1bfa7))
* add Entity.Clone extention method ([4bb6314](https://github.com/FixRM/D365Extensions/commit/4bb6314249f709d13d0047947cb293cc28a7113a))
* add GetRelatedEntitiesAsTuples method as the replacement for GetRelatedEntitiesByTarget ([9b154fe](https://github.com/FixRM/D365Extensions/commit/9b154fe3858119c1c439d1cd3e4c12c275761042))
* add strongly typed version of the EntityReference class ([d339382](https://github.com/FixRM/D365Extensions/commit/d339382597868e864fd24e006c021075a2ee9e2a))
* add strongly typed version of the KeyAttributeCollection class ([0af1fec](https://github.com/FixRM/D365Extensions/commit/0af1fece7d800baebce24d52f43a50c4ef0a61bf))
* add strongly typed version of the OptionSetValue class ))) ([13797f3](https://github.com/FixRM/D365Extensions/commit/13797f3ce6cc48f4310a3e8d2c76b6300a698a58))
* add ToColumnSet method ([3209c99](https://github.com/FixRM/D365Extensions/commit/3209c9935c58054bc57895517b36f1e4ada19415))
* add UpdateChanged extension method for IOrganizationService ([17249ec](https://github.com/FixRM/D365Extensions/commit/17249ec0f13b7936f2122e27851da10f4f802df9))


### Bug Fixes

* GetPreTarget and GetPostTarget now returns a new Entity instead of modifying original Target ([4bde5c3](https://github.com/FixRM/D365Extensions/commit/4bde5c3ed1d97b9b96cc7a3702c30fb4fb1e836d))
* MergeAttributes now takes FormattedValues into account ([3fee6df](https://github.com/FixRM/D365Extensions/commit/3fee6df721a5e07d46ee78fe679c407530f7d49d))

## [2.3.0](https://github.com/FixRM/D365Extensions/compare/v2.2.0...v2.3.0) (2024-12-28)


### Features

* add GetFormatedValue override with alias support ([2c64444](https://github.com/FixRM/D365Extensions/commit/2c64444b4c2763a88143b5f84cc4cc0affbd16c7))
* add new Retrieve<T> overrides ([bb9ab85](https://github.com/FixRM/D365Extensions/commit/bb9ab851bcbf9fc815025b2390b96ca5d609ef29))
* add optional parameter to MergeAttributes to address very specific issue ([457dbe4](https://github.com/FixRM/D365Extensions/commit/457dbe4cf7a44e02e0c336f3bc7f62921a40fb8d))
* add some EntityCollection extensions ([f5b764a](https://github.com/FixRM/D365Extensions/commit/f5b764a0bbce9711f08cf5e5a4754e8d3a2c8645))


### Bug Fixes

* GetAliasedEntity may return unwanted attributes ([4872b51](https://github.com/FixRM/D365Extensions/commit/4872b519d43630c53e13bcbe85611239d302cadc))
* GetAliasedEntity now return FormatedValues ([a428b92](https://github.com/FixRM/D365Extensions/commit/a428b921d5abd2eeea14876c9e80888b9a8be7c0))

## [2.2.0](https://github.com/FixRM/D365Extensions/compare/v2.1.0...v2.2.0) (2024-12-24)


### Features

* add EntityImageCollectionExtensions ([1a2c1f6](https://github.com/FixRM/D365Extensions/commit/1a2c1f60aea7031192799940f6b52cf26ef00c48))
* add ErrorCodes enumeration generated from official Microsoft documentation ([9f46f48](https://github.com/FixRM/D365Extensions/commit/9f46f4827c636c09299ff96b6e1d9212b5c113e0))
* add extensions for ExecuteMultipleResponse and ExecuteMultipleResponseItem ([c8ddf12](https://github.com/FixRM/D365Extensions/commit/c8ddf1278e2282b63e76b832fa3ab9747216fe0b))
* add IEnumerable<ExecuteMultipleOperationResponse> Execute(IEnumerable<OrganizationRequest> requests, int batchSize, ExecuteMultipleSettings settings, Action<OrganizationRequestCollection, ExecuteMultipleResponse> callback) extension method ([c3f7d78](https://github.com/FixRM/D365Extensions/commit/c3f7d789fd1173193da0a33978d8775f5f8b40fd))
* add public static ErrorCodes? GetErrorCode(this FaultException<OrganizationServiceFault> fault) extension method ([b862ed0](https://github.com/FixRM/D365Extensions/commit/b862ed04b3f34b9547475cf27112c6e7767f7d43))
* add public void Execute(QueryBase query, ([393e33d](https://github.com/FixRM/D365Extensions/commit/393e33d217e8966dcdcb7256988ecda735a97982))
* add RemoveUnchanged extension method to Entity ([0ffe971](https://github.com/FixRM/D365Extensions/commit/0ffe971b6f8f2e03e9e661b245f2a59fdffc82c6))
* add two simple RetrieveMultiple overrides ([044dbbd](https://github.com/FixRM/D365Extensions/commit/044dbbdf7f820f2886dbf7316fbd8b08db9b5022))


### Bug Fixes

* add default value for ExecuteMultipleSettings ([63ccfcb](https://github.com/FixRM/D365Extensions/commit/63ccfcb5aca4a7efe302fd205fd9aa1220083dda))
* add null check for implicit cast operators ([63986cc](https://github.com/FixRM/D365Extensions/commit/63986cc6fe6c0b3ac3189864971ed4ae9f7fd826))
* EntityName attribute of ConditionExpression<T> was assigned with LogicalName of T by default ([60c731d](https://github.com/FixRM/D365Extensions/commit/60c731d2c3f8d5cf13a758c1d3a41d6d8ab27413))
* IOrganizationService.Execute(IEnumerable<OrganizationRequest> requests) method now respect ContinueOnError setting ([edbe9f8](https://github.com/FixRM/D365Extensions/commit/edbe9f88deba9977b49c10b55ce6d1554f5654f5))

## [2.1.0](https://github.com/FixRM/D365Extensions/compare/v2.0.0...v2.1.0) (2023-05-30)


### Features

* add ITracingService extensions ([c1d3dcc](https://github.com/FixRM/D365Extensions/commit/c1d3dcc4f7d6682bbc44682395f302c091b40ef1))
* add PluginExecutionTraceContext ([7991677](https://github.com/FixRM/D365Extensions/commit/7991677b2ffde2583f4f744155cd93b81d48770b))
* add ToTraceString extension method to Entity ([cf781ab](https://github.com/FixRM/D365Extensions/commit/cf781ab3056d29deb1a7a517266e8eb6a479d043))
* add ToTraceString extension to EntityReference, Money and OptionSetValue ([2bcc2fd](https://github.com/FixRM/D365Extensions/commit/2bcc2fd2edb0680df6793ef5a35bd887aa32586a))

## [2.0.0](https://github.com/FixRM/D365Extensions/compare/v1.5.3...v2.0.0) (2023-05-01)


### ⚠ BREAKING CHANGES

* RetrieveMultiple extension methods now will throw ArgumentException if query page number is bigger than zero
* LinkEntity<TFrom, TTo> got rid of LinkFromEntityName and LinkToEntityName properties as they were useless from the beginning
* LinkEntity<TFrom, TTo> got rid of both AddLink methods as they were useless from the beginning
* ColumnSet<T>, LinkEntity<TFrom, TTo> and OrderExpression<T> are now sealed
* Columns property is now List<Expression<Func<T, object>>>

### Features

* add default names for pre and post images ([92fd2e9](https://github.com/FixRM/D365Extensions/commit/92fd2e95dab90497c8fd01e38948ffb58a77c495))
* add GetPageNumber helper method ([26390bb](https://github.com/FixRM/D365Extensions/commit/26390bb6e01bae929272881f028f51fda09e968b))
* RetrieveMultiple now checks page number before iterating ([610b090](https://github.com/FixRM/D365Extensions/commit/610b090864272222f65d0a7e8b0b42fbf6d92104))


### refact

* align CollumnSet<T> with similar generic types ([531d73c](https://github.com/FixRM/D365Extensions/commit/531d73ce7fbd5bf8ceb7b3c097ebe28d12a38b5b))
* mark classes as sealed ([fa03115](https://github.com/FixRM/D365Extensions/commit/fa031151696edf14a4c82d9229d3b9a20a55630d))
* remove AddLink methods ([2dd0056](https://github.com/FixRM/D365Extensions/commit/2dd0056f72f4e87f8e0867204f350cc93713b42f))
* remove LinkFromEntityName and LinkToEntityName properties ([d0720f2](https://github.com/FixRM/D365Extensions/commit/d0720f283f2d727a856d06df27f35d0f10e22b82))

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
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
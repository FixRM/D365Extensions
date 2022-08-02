[![NuGet version (D365Extensions)](https://img.shields.io/nuget/v/D365Extensions.svg?style=flat-square)](https://www.nuget.org/packages/D365Extensions/) [![Build Status](https://dev.azure.com/fixrm/FixRM/_apis/build/status/D365Extensions%20YAML?branchName=master)](https://dev.azure.com/fixrm/FixRM/_build/latest?definitionId=20&branchName=master) [![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-yellow.svg)](https://conventionalcommits.org)

# D365Extensions
A collection of Extension methods for Microsoft Dynamics CRM/D365 SDK base classes

# Setup
All extension methods are declared in the same namespace as related SDK types. No additional `using` statements required.

# Usage
This library is assumed to be used for plugin development. As D365 for CE currently doesn't support assembly dependencies, you have to merge it with your primary plugin assembly. We recommend using this tool:

[ILRepack.Lib.MSBuild.Task](https://github.com/ravibpatel/ILRepack.Lib.MSBuild.Task)

ILRepack use the same technique as ILMerge but it is build on newer versions of Mono instruments so it is more fast and efficient. Please refer to link above for documentation.

To configure this task your should add `ILRepack.targets` file to you project. File contents should be looking  like the following:
```XML
<?xml version="1.0" encoding="utf-8" ?>
<!-- ILRepack -->
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Target Name="AfterBuild" Condition="'$(BuildingForLiveUnitTesting)' != 'true'">
    <ItemGroup>
      <InputAssemblies Include="$(OutputPath)\$(AssemblyName).dll" />
      <InputAssemblies Include="$(OutputPath)\D365Extensions.dll" />
    </ItemGroup>
    <ILRepack Parallel="true"
              DebugInfo="true"
              InputAssemblies="@(InputAssemblies)"
              LibraryPath="$(OutputPath)"
              KeyFile="$(AssemblyOriginatorKeyFile)"
              OutputFile="$(OutputPath)\$(AssemblyName).Merged.dll" />
  </Target>
</Project>
```
You should use `KeyFile` parameter as your plugin assembly should be signed. We also recommend use `LibraryPath` parameter as shown to avoid merge problems with dependent SDK assemblies. You shouldn't overrite your assembly with merged one as has some side effects. For instance, it may complicate developing of unit tests if test project and plugin library has common dependencies. In this case you can get runtime errors saying that some types are ambiguous.

>**!!! Never merge SDK assemblies in your code. It will cause runtime errors !!!**

# What's new

[Semantic Versioning](https://semver.org) is used since v1.1.0. For version history please refer to [CHANGELOG.md](CHANGELOG.md)

# Extensions

## [Entity Extensions](../../wiki/Entity-Extensions)
Set of extension methods for Microsoft.Xrm.Sdk.Entity base class. Simplifies dealing with Aliased and Formated values as well as working with Attributes collection.

## [IOrganizationService Extensions](../../wiki/IOrganizationService-Extensions)
Set of extension methods for IOrganizationService base class. Basically these are simple overrides of existing methods which take EntityReference or Entity instead of separate `Id` and `LogicalName` parameters.

## [IPluginExecutionContext Extensions](../../wiki/IPluginExecutionContext-Extensions)
Set of extension methods for Microsoft.Xrm.Sdk.IPluginExecutionContext base class. Most of this helpers are shortcuts for existing properties but provides additional checks or type casts. Unlike Entity class extensions most of the following extensions are not exception safe! It is done so because you most likely want to get an error if plugin is registered for a wrong message or you have a typo in parameter name.

## [CodeActivityContext Extensions](../../wiki/CodeActivityContext-Extensions)
Set of extension methods for System.Activities.CodeActivityContext base class. Shortcut methods for getting D365 related services from workflow execution context.

## [IServiceProvider Extensions](../../wiki/IServiceProvider-Extensions)
Set of extension methods for Microsoft.Xrm.Sdk.IServiceProvider base class. Just shortcut methods to save you few lines of code during plugin development.

## [EntityReference Extensions](../../wiki/EntityReference-Extensions)
Set of extension methods for Microsoft.Xrm.Sdk.EntityReference base class. At the moment just two simple but sometimes useful type conversion methods.

## [Query Extensions](../../wiki/Query-Extensions)
Set of extension methods to support some expression-style/LINQ techniques while using QueryExpression/QueryByAttribute classes.

# Contributing
Please fill free to create issue if you find a bug or have an idea. PR's are welcomed as well! :) Help wanted in the following areas:
+ Unit tests. Most of extensions are just wrappers/overrides of SDK classes but as list of extension grows method call chain grows as well. It seems like it's a time to unit check all methods
+ Code documentation. As it turns out XML code documentation and wiki documentation are very different. Help with updating code doc is appreciated 

[![NuGet version (D365Extensions)](https://img.shields.io/nuget/v/D365Extensions.svg?style=flat-square)](https://www.nuget.org/packages/D365Extensions/) [![Build status](https://fixrm.visualstudio.com/fixrm/_apis/build/status/D365Extensions%20Build)](https://fixrm.visualstudio.com/fixrm/_build/latest?definitionId=5)
# D365Extensions
A collection of Extension methods for Microsoft Dynamics CRM/D365 SDK base classes

# Setup
All extension methods are declared in the same namespace as related SDK types. No additional `using` statements required.

# Usage
This assembly is assumed to be used for plugin development. As D365 for CE currently doesn't support assembly dependencies you have to merge it in your primary plugin assembly. We recommend using this tool:

[ILRepack.Lib.MSBuild.Task](https://github.com/ravibpatel/ILRepack.Lib.MSBuild.Task)

ILRepack use the same technique as ILMerge but it is build on newer versions of Mono instruments so it is more fast and efficient. Please refer to link above for documentation.

After edit you .cproj file should be looking like the following:
```XML
  <Target Name="AfterBuild">
    <ItemGroup>
      <InputAssemblies Include="$(OutputPath)\$(AssemblyName).dll" />
      <InputAssemblies Include="$(OutputPath)\D365Extensions.dll" />
    </ItemGroup>
    <ILRepack Parallel="true" 
              InputAssemblies="@(InputAssemblies)"
              LibraryPath="$(OutputPath)" 
              KeyFile="$(AssemblyOriginatorKeyFile)" 
              OutputFile="$(OutputPath)\$(AssemblyName).dll" />
  </Target>
```
You should use `KeyFile` parameter as your plugin assembly should be signed. We also recommend use `LibraryPath` parameter as shown to avoid merge problems with dependent SDK assemblies.

>**!!! Never merge SDK assemblies in your code. It will cause runtime errors !!!**

# What's new

## Version 1.0.35
Now assembly have a strong name and should work with VS Live Unit Testing.

## Version 1.0.29
Add support for alternative keys for Retrieve and Delete!

# Extensions

## [Entity Extensions](../../wiki/Entity-Extensions)
Set of extension methods for Microsoft.Xrm.Sdk.Entity base class. Simplifies dealing with Aliased and Formated values as well as working with Attributes collection.

## [IOrganizationService Extensions](../../wiki/IOrganizationService-Extensions)
Set of extension methods for IOrganizationService base class. Basically these are simple overrides of existing methods which take EntityReference or Entity instead of separate `Id` and `LogicalName` parameters.

## [IPluginExecutionContext Extensions](../../wiki/IPluginExecutionContext-Extensions)
Set of extension methods for Microsoft.Xrm.Sdk.IPluginExecutionContext base class. Most of this helpers are shortcuts for existing properties but provides additional checks or type casts. Unlike Entity class extensions most of the following extensions are not exception safe! It is done so because you most likely want to get an error if plugin is registered for a wrong message or you have a typo in parameter name.

## [CodeActivityContext Extensions](../../wiki/CodeActivityContext-Extensions)
Set of extension methods for System.Activities.CodeActivityContext base class. Short cut methods for getting D365 related services from workflow execution context.

## [IServiceProvider Extensions](../../wiki/IServiceProvider-Extensions)
Set of extension methods for Microsoft.Xrm.Sdk.IServiceProvider base class. Just shortcut methods to save you few lines of code during plugin development.

## [EntityReference Extensions](../../wiki/EntityReference-Extensions)
Set of extension methods for Microsoft.Xrm.Sdk.EntityReference base class. At the moment just two simple but sometimes useful type conversion methods.

# Contributing
Please fill free to create issue if you find a bug or have an idea. PR's are welcomed as well! :) Help wanted in the following areas:
+ Unit tests. Most of extensions are just wrappers/overrides of SDK classes but as list of extension grows method call chain grows as well. It seems like it's a time to unit check all methods
+ Code documentation. As it turns out XML code documentation and wiki documentation are very different. Help with updating code doc is appreciated 

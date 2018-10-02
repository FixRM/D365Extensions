[![NuGet version (D365Extensions)](https://img.shields.io/nuget/v/D365Extensions.svg?style=flat-square)](https://www.nuget.org/packages/D365Extensions/) [![Build status](https://fixrm.visualstudio.com/fixrm/_apis/build/status/D365Extensions%20Build)](https://fixrm.visualstudio.com/fixrm/_build/latest?definitionId=5)
# D365Extensions
A collection of Extension methods for Microsoft Dynamics CRM/D365 SDK base classes

# Usage
This assembly is assumed to be used for plugin development. As D365 for CE currently doesn't support assembly dependencies you have to merge it in your primary plugin assembly. We recommend using this tool:

[ILRepack.Lib.MSBuild.Task](https://github.com/ravibpatel/ILRepack.Lib.MSBuild.Task)

ILRepack use the same technique as ILMerge but it is build on newer versions of Mono instruments so it is more fast and efficient. Please refer to link above for documentation.

After edit you .cproj file should be looking like the folowing:
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

# Extensions

## Entity Extensions
Set of extension methods for Microsoft.Xrm.Sdk.Entity base class.

### GetFormatedValue
Simplifies getting values from Entity.FormattedValues collection
```C#
public String GetFormatedValue(String attributeLogicalName);
```

### GetAliasedValue
Simplifies getting values from linked entities attributes wraped in AliasedValue class. This kind of attributes can be queried by FetchExpression or QueryExpression using Linked Entities 
```C#
public T GetAliasedValue<T>(String attributeLogicalName, String alias);
```

### GetAliasedEntity
Simplifies getting multiple linked entitiy attrubutes by allocating them to separate Entity
```C#
public Entity GetAliasedEntity(String entityLogicalName, String alias = null);
```

### GetAliasedEntity\<T\>
Generic version of GetAliasedEntity
```C#
public T GetAliasedEntity<T>(String entityLogicalName, String alias = null) where T : Entity;
```

### MergeAttributes
Add attributes form source Entity if they don't exist in target Entity. Very convenient way to compose attribute values from plugin Target and PreImage to operate single Entity instance.
```C#
public void MergeAttributes(Entity source);
```

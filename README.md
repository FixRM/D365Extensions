[![NuGet version (D365Extensions)](https://img.shields.io/nuget/v/D365Extensions.svg?style=flat-square)](https://www.nuget.org/packages/D365Extensions/) [![Build status](https://fixrm.visualstudio.com/fixrm/_apis/build/status/D365Extensions%20Build)](https://fixrm.visualstudio.com/fixrm/_build/latest?definitionId=5)
# D365Extensions
A collection of Extension methods for Microsoft Dynamics CRM/D365 SDK base classes

# Setup
All extension methods are declared in the same namespace as related SDK types. No additional `using` statements required.

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
## IOrganizationService Extensions
Set of extension methods for IOrganizationService base class. Basically these are simple overrides of existing methods which take EntityReference or Entity instead of separate `Id` and `LogicalName` parameters.

### Associate & Disassociate
Associate & Disassociate methods override. Take EntityReference (insted of separate Id + LogicalName) input parameter
```C#
public void Associate(EntityReference primaryEntity, Relationship relationship, EntityReferenceCollection relatedEntities);
public void Associate(EntityReference primaryEntity, Relationship relationship, IList<EntityReference> relatedEntities);

public void Disassociate(EntityReference primaryEntity, Relationship relationship, EntityReferenceCollection relatedEntities);
public void Disassociate(EntityReference primaryEntity, Relationship relationship, IList<EntityReference> relatedEntities)
```

### Delete
Delete method override. Take EntityReference (insted of separate Id + LogicalName) input parameter
```C#
public void Delete(EntityReference reference);
public void Delete(Entity entity);
```

### Retrieve
Retrieve method override.  Take EntityReference (insted of separate Id + LogicalName) input parameter
```C#
public Entity Retrieve(EntityReference reference, ColumnSet columnSet);
public Entity Retrieve(EntityReference reference, params String[] columns);
```

### Retrieve\<T\>
Generic version of Retrieve
```C#
public T Retrieve<T>(EntityReference reference, ColumnSet columnSet) where T : Entity;
public T Retrieve<T>(EntityReference reference, params String[] columns) where T : Entity;
```

## IPluginExecutionContext Extensions
Set of extension methods for Microsoft.Xrm.Sdk.IPluginExecutionContext base class. Most of this helpers are shortcuts for existing properties but provides additional checks or type casts. Unlike Entity class extensions most of the following extensions are not exception safe! It is done so because you most likely want to get an error if plugin is registered for a wrong message or you have a typo in parameter name.
### GetOrganization
Return OrganizationId and OrganizationName fields as single EntityReference
```C#
public EntityReference GetOrganization();
```

### GetPrimaryEntity
Return PrimaryEntityId and PrimaryEntityName fields as single EntityReference
```C#
public EntityReference GetPrimaryEntity();
```

### GetUser
Return UserId field as EntityReference
```C#
public EntityReference GetUser();
```

### GetInitiatingUser
Return InitiatingUserId field as EntityReference
```C#
public EntityReference GetInitiatingUser();
```

### GetBusinessUnit
Return BusinessUnitId field as EntityReference
```C#
public EntityReference GetBusinessUnit();
```

### GetInputParameter\<T\>
Gets input paramer
```C#
public T GetInputParameter<T>(String name) where T : class;
```

### GetOutputParameter\<T\>
Gets output paramer
```C#
public T GetOutputParameter<T>(String name) where T : class;
```

### GetPreImage
Gets pre image
```C#
public Entity GetPreImage(String name);
```

### GetPreImage\<T\>
Gets pre image as the specified type
```C#
public T GetPreImage<T>(String name) where T : Entity;
```

### GetPostImage
Gets post image
```C#
public Entity GetPostImage(String name);
```

### GetPostImage\<T\>
Gets post image as the specified type
```C#
public T GetPostImage<T>(String name) where T : Entity;
```

### GetTarget
Shortcut for getting "Target" input parameter of type Entity
```C#
public Entity GetTarget();
```

### GetTarget\<T\>
Shortcut for getting "Target" input parameter  as the specified type
```C#
public T GetTarget<T>() where T : Entity;
```

### GetPreTarget\<T\>
Get "Target" entity parameter merged with specified pre image 
```C#
public T GetPreTarget<T>(String name) where T : Entity;
```

### GetPostTarget\<T\>
Get "Target" entity parameter merged with specified post image 
```C#
public T GetPostTarget<T>(String name) where T : Entity;
```

### GetSharedVariable\<T\>
Gets shared Variable
```C#
public T GetSharedVariable<T>(String name) where T : class;
```

### GetRelatedEntitiesByTarget
Simplifies handling of Associate and Disassociate messages. This messages can't be filtered by entity type, furthermore two options possible: when "A" entity is associated with array of "B", or "B" is associated with array of "A". This method generates universal dictionary of arguments which is suitable in all cases
```C#
public Dictionary<EntityReference, EntityReferenceCollection> GetRelatedEntitiesByTarget(String keyEntity, String valueEntity);
```
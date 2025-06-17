using Microsoft.Xrm.Sdk.Metadata;

namespace FakeXrmEasy.Extensions
{
    public static class XrmFakedContextExtension
    {
        public static void InitializeKeyMetadata(this XrmFakedContext context, string entityLogicalName, string[] keyAttributeNames)
        {
            var alternateKeyMetadata = new EntityKeyMetadata
            {
                KeyAttributes = keyAttributeNames
            };

            var entityMetadata = new EntityMetadata
            {
                LogicalName = entityLogicalName
            };

            entityMetadata.SetFieldValue("_keys", new EntityKeyMetadata[]
            {
                alternateKeyMetadata
            });

            context.InitializeMetadata(entityMetadata);
        }
    }
}

using Microsoft.Xrm.Sdk;
using System;
using System.Text;

namespace D365Extensions
{
    internal static class StringBuilderExtensions
    {
        internal static StringBuilder AppendEntity(this StringBuilder builder, Entity entity)
        {
            builder.AppendLine($@"Entity {{ LogicalName = ""{entity.LogicalName}"", Id = ""{entity.Id:B}"" }}");
            builder.AppendDataCollection(entity.Attributes, "Attributes");

            return builder;
        }

        internal static StringBuilder AppendEntityCollection(this StringBuilder builder, EntityCollection collection)
        {
            builder.AppendLine($@"EntityCollection {{ EntityName = ""{collection.EntityName}"" }}");
            builder.Append($"Entities: {{ Count = {collection.Entities.Count} }} ");

            foreach (var entity in collection.Entities)
            {
                builder.AppendLine();
                builder.AppendEntity(entity);
            }

            return builder;
        }

        internal static StringBuilder AppendDataCollection<T>(this StringBuilder builder, DataCollection<string, T> collection, string header)
        {
            builder.Append($"{header}: {collection.GetType().Name} {{ Count = {collection.Count} }}");

            foreach (var keyValue in collection)
            {
                builder.AppendLine();
                builder.Append($@"""{keyValue.Key}"": ").AppendObject(keyValue.Value);
            }

            return builder;
        }

        internal static StringBuilder AppendObject(this StringBuilder builder, object value)
        {
            switch (value)
            {
                case EntityReference reference:
                    return builder.Append(reference.ToTraceString());

                case Money money:
                    return builder.Append(money.ToTraceString());

                case OptionSetValue optionSet:
                    return builder.Append(optionSet.ToTraceString());

                case Entity entity:
                    return builder.AppendEntity(entity);

                case EntityCollection entityCollection:
                    return builder.AppendEntityCollection(entityCollection);

                case string str:
                    return builder.Append($@"""{str}""");

                case Guid guid:
                    return builder.Append($@"""{guid:B}""");

                default:
                    return builder.Append(value);
            }
        }
    }
}

using D365Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Xrm.Sdk
{
    public interface IDeconstructor<T>
    {
        IEnumerable<KeyValuePair<string, object>> GetAttributeValues(T dto);
    }

    internal class EntityDeconstructor<T> : IDeconstructor<T> where T : Entity
    {
        public IEnumerable<KeyValuePair<string, object>> GetAttributeValues(T entity)
        {
            return entity.Attributes;
        }
    }

    //TODO: tets
    internal class EntityReflectionDeconstructor<T> : IDeconstructor<T>
    {
        public IEnumerable<KeyValuePair<string, object>> GetAttributeValues(T dto)
        {
            var sourceType = typeof(T);

            //AttributeLogicalNameAttribute support only properties
            var sourceProps = sourceType.GetProperties();

            foreach (var p in sourceProps)
            {
                var name = LogicalName.GetName(p);
                var value = p.GetValue(dto);

                yield return new KeyValuePair<string, object>(name, value);
            }
        }
    }

    public class MyDto
    {
        public string Name { get; set; }

        public int? Age { get; set; }
    }

    public class MyDtoDeconstructor : IDeconstructor<MyDto>
    {
        public IEnumerable<KeyValuePair<string, object>> GetAttributeValues(MyDto dto)
        {
            yield return new KeyValuePair<string, object>("name", dto.Name);

            yield return new KeyValuePair<string, object>("age", dto.Age);
        }
    }

    internal static class KeyValuePairExt
    {
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> keyValuePair, out T1 key, out T2 value)
        {
            key = keyValuePair.Key;
            value = keyValuePair.Value;
        }
    }
}
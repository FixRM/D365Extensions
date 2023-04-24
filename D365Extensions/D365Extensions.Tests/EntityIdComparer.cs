#pragma warning disable CS0618 // Type or member is obsolete

using Microsoft.Xrm.Sdk;
using System.Collections;
using System.Collections.Generic;

namespace D365Extensions.Tests
{
    class EntityIdComparer : IComparer<Entity>, IComparer
    {
        public int Compare(Entity x, Entity y)
        {
            return x.Id.CompareTo(y.Id);
        }

        public int Compare(object x, object y)
        {
            return this.Compare(x as Entity, y as Entity);
        }
    }
}
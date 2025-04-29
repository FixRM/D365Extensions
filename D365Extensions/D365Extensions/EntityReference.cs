using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk
{
    /// <summary>
    /// Strongly typed version of the EntityReference class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EntityReference<T> where T : Entity
    {
        /// <summary>
        /// EntityReference has complex overrides of Equals and GetHashCode
        /// So unlike other strongly typed versions in this library, it's easier to have this one to be
        /// a wrapper of OOB type instead of independent implementation
        /// </summary>
        private readonly EntityReference reference;

        public EntityReference()
        {
            this.reference = new EntityReference(D365Extensions.LogicalName.GetName<T>());
        }

        public EntityReference(Guid id)
        {
            this.reference = new EntityReference()
            {
                Id = id,
                LogicalName = D365Extensions.LogicalName.GetName<T>()
            };
        }

        public EntityReference(KeyAttributeCollection keyAttributes)
        {
            this.reference = new EntityReference()
            {
                LogicalName = D365Extensions.LogicalName.GetName<T>(),
                KeyAttributes = keyAttributes
            };
        }

        public EntityReference(Expression<Func<T, object>> keyName, object keyValue)
        {
            this.reference = new EntityReference()
            {
                LogicalName = D365Extensions.LogicalName.GetName<T>(),
                KeyAttributes =
                {
                    { D365Extensions.LogicalName.GetName(keyName), keyValue }
                }
            };
        }

        /// <summary>
        /// Gets or sets the ID of the record
        /// </summary>
        public Guid Id
        {
            get => reference.Id;
            set => reference.Id = value;
        }

        /// <summary>
        /// Gets or sets the logical name of the entity.
        /// </summary>
        public string LogicalName
        {
            get => reference.LogicalName;
        }

        /// <summary>
        /// Gets or sets the value of the primary attribute of the entity.
        /// </summary>
        public string Name
        {
            get => reference.Name;
            set => reference.Name = value;
        }

        /// <summary>
        /// Gets or sets the key attributes.
        /// </summary>
        public KeyAttributeCollection KeyAttributes
        {
            get => reference.KeyAttributes;
            set => reference.KeyAttributes = value;
        }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public string RowVersion
        {
            get => reference.RowVersion;
            set => reference.RowVersion = value;
        }

        /// <summary>
        /// WARNING: EntityReference and EntityReference<T> are different types! You can't compare them directly!
        /// You should always explicitly cast EntityReference<T> to EntityReference before comparison:
        /// <code>
        /// reference1.Equals((EntityReference) reference2)
        /// </code>
        /// </summary>
        public override bool Equals(object obj)
        {
            return reference.Equals(obj);
        }

        public override int GetHashCode()
        {
            return reference.GetHashCode();
        }

        public static implicit operator EntityReference(EntityReference<T> t)
        {
            if (t == null) return null;

            return t.reference;
        }
    }
}

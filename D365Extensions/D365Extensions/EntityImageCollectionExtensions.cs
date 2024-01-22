using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xrm.Sdk
{
    public static class EntityImageCollectionExtensions
    {
        /// <summary>
        /// Safely gets value from EntityImageCollection 
        /// </summary>
        /// <param name="name">image name</param>
        /// <returns></returns>
        public static Entity GetImage(this EntityImageCollection images, string name)
        {
            if (images.TryGetValue(name, out var image))
            {
                return image;
            }

            return null;
        }

        /// <summary>
        /// Safely gets value from EntityImageCollection 
        /// </summary>
        /// <typeparam name="T">Early Bound Entity type</typeparam>
        /// <param name="name">image name</param>
        /// <returns></returns>
        public static T GetImage<T>(this EntityImageCollection images, string name) where T : Entity
        {            
            return images.GetImage(name)?.ToEntity<T>();
        }
    }
}
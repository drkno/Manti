using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace CoreLoader.Plugins.Loader
{
    internal static class ClassExtender
    {
        /// <summary>
        /// Convert object to a dynamic expando. Should be used sparingly as it literally copies the object definition.
        /// Credit to Jorge Fioranelli.
        /// </summary>
        /// <see cref="http://blog.jorgef.net/2011/06/converting-any-object-to-dynamic.html"/>
        /// <param name="value">object to convert.</param>
        /// <returns>A dynamic expando.</returns>
        public static dynamic MakeExtensable(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
            {
                expando.Add(property.Name, property.GetValue(value));
            }
            return expando;
        }
    }
}

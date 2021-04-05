// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Retouch_Photo2.Layers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a Layer from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created <see cref="Layerage"/>. </returns>
        private static ILayer CreateLayer(string type)
        {
            if (string.IsNullOrEmpty(type) == false)
            {
                Assembly assembly = typeof(ILayer).GetTypeInfo().Assembly;
                IEnumerable<TypeInfo> typeInfos = from t in assembly.DefinedTypes where t.IsClass select t;

                TypeInfo typeInfo = typeInfos.FirstOrDefault(t => t.Name == $"{type}Layer");
                if (typeInfo != null)
                {
                    object obj = Activator.CreateInstance(typeInfo.AsType());
                    if (obj is ILayer layer)
                    {
                        return new GroupLayer();
                    }
                }
            }

            return new GroupLayer();
        }

    }
}
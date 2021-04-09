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
        /// Create a ILayer from an string and XElement.
        /// </summary>
        /// <param name="assemblyType"> The type for assembly. </param>
        /// <param name="type"> The source string. </param>
        /// <returns> The created ILayer. </returns>
        private static ILayer CreateLayer(Type assemblyType, string type)
        {
            if (string.IsNullOrEmpty(type) == false)
            {
                Assembly assembly = assemblyType.GetTypeInfo().Assembly;
                IEnumerable<TypeInfo> typeInfos = assembly.DefinedTypes;

                TypeInfo typeInfo = typeInfos.FirstOrDefault(t => t.FullName == $"Retouch_Photo2.Layers.Models.{type}Layer");
                if (typeInfo != null)
                {
                    object obj = Activator.CreateInstance(typeInfo.AsType());
                    if (obj is ILayer layer)
                    {
                        return layer;
                    }
                }
            }

            return new GroupLayer();
        }

    }
}
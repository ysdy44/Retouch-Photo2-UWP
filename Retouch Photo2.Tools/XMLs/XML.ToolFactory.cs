// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a ITool from an string and XElement.
        /// </summary>
        /// <param name="assemblyType"> The type for assembly. </param>
        /// <param name="type"> The source string. </param>
        /// <returns> The created ITool. </returns>
        public static ITool CreateTool(Type assemblyType, string type)
        {
            if (string.IsNullOrEmpty(type) == false)
            {
                Assembly assembly = assemblyType.GetTypeInfo().Assembly;
                IEnumerable<TypeInfo> typeInfos = assembly.DefinedTypes;

                TypeInfo typeInfo = typeInfos.FirstOrDefault(t =>t.FullName == $"Retouch_Photo2.Tools.Models.{type}Tool");
                if (typeInfo != null)
                {
                    object obj = Activator.CreateInstance(typeInfo.AsType());
                    if (obj is ITool layer)
                    {
                        return layer;
                    }
                }
            }

            return new NoneTool();
        }

    }
}
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

        /// <summary> Gets or sets the all tools. </summary>   
        private static readonly IDictionary<ToolType, ITool> Tools = new Dictionary<ToolType, ITool>();

        /// <summary>
        /// Create a ITool from an string and XElement.
        /// </summary>
        /// <param name="assemblyType"> The type for assembly. </param>
        /// <param name="type"> The source type. </param>
        /// <returns> The created ITool. </returns>
        public static ITool CreateTool(Type assemblyType, ToolType type)
        {
            if (XML.Tools.ContainsKey(type)) return XML.Tools[type];

            if (type != ToolType.None)
            {
                Assembly assembly = assemblyType.GetTypeInfo().Assembly;
                IEnumerable<TypeInfo> typeInfos = assembly.DefinedTypes;

                TypeInfo typeInfo = typeInfos.FirstOrDefault(t => t.FullName == $"Retouch_Photo2.Tools.Models.{type}Tool");
                if ((typeInfo is null) == false)
                {
                    object obj = Activator.CreateInstance(typeInfo.AsType());
                    if (obj is ITool tool)
                    {
                        XML.Tools.Add(type, tool);
                        return tool;
                    }
                }
            }

            return new NoneTool();
        }

    }
}
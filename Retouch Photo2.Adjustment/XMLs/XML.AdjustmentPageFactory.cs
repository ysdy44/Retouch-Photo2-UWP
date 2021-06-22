// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Retouch_Photo2.Adjustments.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary> Gets or sets the all adjustment pages. </summary>   
        private static readonly IDictionary<AdjustmentType, IAdjustmentPage> AdjustmentPages = new Dictionary<AdjustmentType, IAdjustmentPage>();

        /// <summary>
        /// Create a IAdjustmentPage from the string.
        /// </summary>    
        /// <param name="assemblyType"> The type for assembly. </param>
        /// <param name="type"> The source type. </param>
        /// <returns> The created IAdjustmentPage. </returns>
        public static IAdjustmentPage CreateAdjustmentPage(Type assemblyType, AdjustmentType type)
        {
            if (XML.AdjustmentPages.ContainsKey(type)) return XML.AdjustmentPages[type];

            if (type != AdjustmentType.Gray)
            {
                Assembly assembly = assemblyType.GetTypeInfo().Assembly;
                IEnumerable<TypeInfo> typeInfos = assembly.DefinedTypes;

                TypeInfo typeInfo = typeInfos.FirstOrDefault(t => t.FullName == $"Retouch_Photo2.Adjustments.Pages.{type}Page");
                if ((typeInfo is null) == false)
                {
                    object obj = Activator.CreateInstance(typeInfo.AsType());
                    if (obj is IAdjustmentPage adjustmentPage)
                    {
                        XML.AdjustmentPages.Add(type, adjustmentPage);
                        return adjustmentPage;
                    }
                }
            }

            return new GrayPage();
        }

    }
}
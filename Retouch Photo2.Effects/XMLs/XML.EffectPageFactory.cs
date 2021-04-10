// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary> Gets or sets the all effect pages. </summary>   
        private static readonly IDictionary<EffectType, IEffectPage> EffectPages = new Dictionary<EffectType, IEffectPage>();

        /// <summary>
        /// Create a IEffectPage from an string and XElement.
        /// </summary>
        /// <param name="assemblyType"> The type for assembly. </param>
        /// <param name="type"> The source type. </param>
        /// <returns> The created IEffectPage. </returns>
        public static IEffectPage CreateEffectPage(Type assemblyType, EffectType type)
        {
            if (XML.EffectPages.ContainsKey(type)) return XML.EffectPages[type];

            if (type != EffectType.None)
            {
                Assembly assembly = assemblyType.GetTypeInfo().Assembly;
                IEnumerable<TypeInfo> typeInfos = assembly.DefinedTypes;

                TypeInfo typeInfo = typeInfos.FirstOrDefault(t => t.FullName == $"Retouch_Photo2.Effects.Pages.{type}EffectPage");
                if (typeInfo != null)
                {
                    object obj = Activator.CreateInstance(typeInfo.AsType());
                    if (obj is IEffectPage effectPage)
                    {
                        XML.EffectPages.Add(type, effectPage);
                        return effectPage;
                    }
                }
            }

            return new NoneEffectPage();
        }

    }
}
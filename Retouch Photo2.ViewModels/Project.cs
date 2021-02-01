// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★
// Only:              ★★★★★
// Complete:      ★
using Retouch_Photo2.Layers;
using System.Collections.Generic;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// Represents a project class with width and layerages.
    /// </summary>
    public class Project
    {
        /// <summary> Gets or sets the name. </summary>
        public string Name { set; get; }

        /// <summary> Gets or sets the width. </summary>
        public int Width { set; get; }

        /// <summary> Gets or sets the height. </summary>
        public int Height { set; get; }

        /// <summary> Gets or sets the layerages. </summary>
        public IEnumerable<Layerage> Layerages;
    }
}
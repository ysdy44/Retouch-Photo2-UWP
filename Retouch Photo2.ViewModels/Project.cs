using Retouch_Photo2.Layers;
using System.Collections.Generic;

namespace Retouch_Photo2.ViewModels
{
    /// <summary>
    /// Retouch_Photo2 's Project
    /// </summary>
    public class Project
    {
        /// <summary> <see cref = "Project" />'s name. </summary>
        public string Name { set; get; }

        /// <summary> <see cref = "Project" />'s width. </summary>
        public int Width { set; get; }
        /// <summary> <see cref = "Project" />'s height. </summary>
        public int Height { set; get; }        
        /// <summary> <see cref = "Project" />'s layers. </summary>
        public IEnumerable<ILayer> Layers;
    }
}
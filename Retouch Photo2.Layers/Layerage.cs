using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// ID of <see cref="Layerage"/>.
    /// </summary>
    public class Layerage
    {
        public ILayer Self => Layer.FindFirstLayer(this);

        public string Id { get; set; }
        public Layerage Parents { get; set; }
        public IList<Layerage> Children { get; set; } = new List<Layerage>();

        public Layerage Clone()
        {
            return new Layerage
            {
                Id = this.Id,
                //Bug! infinite loop! 
                //Parents = this.Parents?.Clone(),
                Children = (from child in this.Children select child.Clone()).ToList()
            };
        }

    }
}
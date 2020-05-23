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
            Layerage layerage = new Layerage
            {
                Id = this.Id
            };

            if (this.Parents != null) Parents = this.Parents.Clone();

            foreach (Layerage child in this.Children)
            {
                if (child != null) layerage.Children.Add(child.Clone());
            }

            return layerage;
        }

    }
}
using System.Collections.Generic;
using System.Linq;
using FanKit.Transformers;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// ID of <see cref="Layerage"/>.
    /// </summary>
    public partial class Layerage : FanKit.Transformers.IGetActualTransformer
    {
        public ILayer Self => LayerBase.FindFirstLayer(this);
        public ILayer ClipboardSelf => Clipboard.FindFirstLayer(this);

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

        public Transformer GetActualTransformer()
        {
            ILayer layer = this.Self;
            return layer.GetActualTransformer(this);
        }

        /// <summary>
        /// Open the <see cref="ILayer.IsRefactoringTransformer"/> with <see cref="Layerage.Parents"/>.
        /// </summary>
        public void RefactoringParentsTransformer()
        {
            //RefactoringTransformer
            if (this.Parents != null)
            {
                ILayer layer = this.Parents.Self;

                if (layer.Type == LayerType.Group)
                {
                    ILayer groupLayer = layer;
                    groupLayer.IsRefactoringTransformer = true;
                }
            }
        }

    }
}
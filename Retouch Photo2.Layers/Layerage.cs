// Core:              ★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★
// Only:              ★★★★
// Complete:      ★★★
using System.Collections.Generic;
using System.Linq;
using FanKit.Transformers;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// ID of <see cref="ILayer"/>.
    /// </summary>
    public partial class Layerage : IGetActualTransformer
    {

        /// <summary> Find layer from <see cref="LayerBase.Instances"/> </summary>
        public ILayer Self => LayerBase.FindFirstLayer(this);
        /// <summary> Find layer from <see cref="Clipboard.Instances"/> </summary>
        public ILayer ClipboardSelf => Clipboard.FindFirstLayer(this);


        /// <summary> Gets or sets the Id. </summary>
        public string Id { get; set; }
        /// <summary> Gets or sets the Parents. </summary>
        public Layerage Parents { get; set; }
        /// <summary> Gets or sets the Children. </summary>
        public IList<Layerage> Children { get; set; } = new List<Layerage>();


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned Layerage. </returns>
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

        /// <summary>
        ///  Get the actual transformer.
        /// </summary>
        public Transformer GetActualTransformer()
        {
            ILayer layer = this.Self;
            return layer.GetActualTransformer(this);
        }

        //Refactoring
        /// <summary>
        /// Open the <see cref="ILayer.IsRefactoringTransformer"/> with <see cref="Layerage.Parents"/>.
        /// </summary>
        public void RefactoringParentsTransformer()
        {
            if (this.Parents != null)
            {
                ILayer layer = this.Parents.Self;

                if (layer.Type == LayerType.Group)
                {
                    //Refactoring
                    ILayer groupLayer = layer;
                    groupLayer.IsRefactoringTransformer = true;
                    this.Parents.RefactoringParentsTransformer();
                }
            }
        }
        /// <summary>
        /// Open the <see cref="ILayer.IsRefactoringRender"/> with <see cref="Layerage.Parents"/>.
        /// </summary>
        public void RefactoringParentsRender()
        {
            if (this.Parents != null)
            {
                ILayer layer = this.Parents.Self;

                //Refactoring
                layer.IsRefactoringRender = true;
                this.Parents.RefactoringParentsRender();
            }
        }
        /// <summary>
        /// Open the <see cref="ILayer.IsRefactoringIconRender"/> with <see cref="Layerage.Parents"/>.
        /// </summary>
        public void RefactoringParentsIconRender()
        {
            if (this.Parents != null)
            {
                ILayer layer = this.Parents.Self;

                //Refactoring
                layer.IsRefactoringIconRender = true;
                this.Parents.RefactoringParentsIconRender();
            }
        }

    }
}
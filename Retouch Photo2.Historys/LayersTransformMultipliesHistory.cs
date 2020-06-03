using FanKit.Transformers;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history used to change layer transform.
    /// </summary>
    public class LayersTransformMultipliesHistory : HistoryBase, IHistory
    {
        /// <summary>
        /// Undo actions
        /// </summary>
        readonly IDictionary<ILayer, Transformer> UndoActions = new Dictionary<ILayer, Transformer>();

        //@Construct
        /// <summary>
        /// Initializes a LayersPropertyHistory.
        /// </summary>
        /// <param name="title"> The title. </param>  
        public LayersTransformMultipliesHistory(string title)
        {
            base.Title = title;
        }

        /// <summary>
        /// Push a transform history in Undo actions.
        /// </summary>
        /// <param name="layer"> The layer. </param>
        public void PushTransform(ILayer layer)
        {
            var previous = layer.Transform.Transformer;

            this.UndoActions.Add(layer, previous);
        }

        /// <summary>
        /// Push a starting transform history in Undo actions.
        /// </summary>
        public void PushStartingTransform(ILayer layer)
        {
            var previous = layer.Transform.StartingTransformer;

            this.UndoActions.Add(layer, previous);
        }

        public override void Undo()
        {
            foreach (var undo in this.UndoActions)
            {
                ILayer layer = undo.Key;
                Transformer transformer = layer.Transform.Transformer;
                Matrix3x2 matrix = Transformer.FindHomography(transformer, undo.Value);
                      
                //Refactoring
                layer.IsRefactoringTransformer = true;
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                //undo.RefactoringParentsTransformer();
                //undo.RefactoringParentsRender();
                //undo.RefactoringParentsIconRender();
                layer.CacheTransform();
                layer.TransformMultiplies(matrix);
            }
        }

    }
}
// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
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

        private IDictionary<ILayer, Transformer> UndoActions = new Dictionary<ILayer, Transformer>();

        //@Construct
        /// <summary>
        /// Initializes a LayersPropertyHistory.
        /// </summary>
        /// <param name="type"> The type. </param>  
        public LayersTransformMultipliesHistory(HistoryType type)
        {
            base.Type = type;
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

        /// <summary> Undo method. </summary>
        public override void Undo()
        {
            foreach (var undo in this.UndoActions)
            {
                ILayer layer = undo.Key;
                Transformer transformer = layer.Transform.Transformer;
                Matrix3x2 matrix = Transformer.FindHomography(transformer, undo.Value);
                      
                // Refactoring
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

        public void Dispose()
        {
            this.UndoActions.Clear();
            this.UndoActions = null;
        }
    }
}
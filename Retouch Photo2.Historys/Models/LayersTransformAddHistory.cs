// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history used to change layer transform.
    /// </summary>
    public class LayersTransformAddHistory : HistoryBase, IHistory
    {

        private IDictionary<ILayer, Vector2> UndoActions = new Dictionary<ILayer, Vector2>();

        //@Construct
        /// <summary>
        /// Initializes a LayersPropertyHistory.
        /// </summary>
        /// <param name="type"> The type. </param>  
        public LayersTransformAddHistory(HistoryType type)
        {
            base.Type = type;
        }

        /// <summary>
        /// Push a transform history in Undo actions.
        /// </summary>
        /// <param name="layer"> The layer. </param>
        /// <param name="vector"> The add value use to summed. </param>
        public void PushTransform(ILayer layer, Vector2 vector)
        {
            var previous = vector;

            this.UndoActions.Add(layer, previous);
        }

        /// <summary> Undo method. </summary>
        public override void Undo()
        {
            foreach (var undo in this.UndoActions)
            {
                ILayer layer = undo.Key;
                Vector2 previous = undo.Value;

                //Refactoring
                layer.IsRefactoringTransformer = true;
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                //undo.RefactoringParentsTransformer();
                //undo.RefactoringParentsRender();
                //undo.RefactoringParentsIconRender();
                layer.CacheTransform();
                layer.TransformAdd(-previous);
            }
        }

        public void Dispose()
        {
            this.UndoActions.Clear();
            this.UndoActions = null;
        }
    }
}
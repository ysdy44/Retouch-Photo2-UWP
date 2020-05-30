using Retouch_Photo2.Layers;
using System.Collections.Generic;

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Represents a history used to change layer transform.
    /// </summary>
    public class LayersTransformHistory : HistoryBase, IHistory
    {
        /// <summary>
        /// Undo actions
        /// </summary>
        readonly IDictionary<ILayer, TransformPosition> UndoActions = new Dictionary<ILayer, TransformPosition>();

        //@Construct
        /// <summary>
        /// Initializes a LayersPropertyHistory.
        /// </summary>
        /// <param name="title"> The title. </param>  
        public LayersTransformHistory(string title)
        {
            base.Title = title;
        }

        /// <summary>
        /// Push a transform history in Undo actions.
        /// </summary>
        /// <param name="layer"> The layer. </param>
        public void PushTransform(ILayer layer)
        {
            var previous = new TransformPosition
            {
                Style_Fill_Center = layer.Style.Fill.Center,
                Style_Fill_XPoint = layer.Style.Fill.XPoint,
                Style_Fill_YPoint = layer.Style.Fill.YPoint,

                Style_Stroke_Center = layer.Style.Stroke.Center,
                Style_Stroke_XPoint = layer.Style.Stroke.XPoint,
                Style_Stroke_YPoint = layer.Style.Stroke.YPoint,

                Transform_Transformer = layer.Transform.Transformer,
                Transform_CropTransformer = layer.Transform.CropTransformer,
            };

            this.UndoActions.Add(layer, previous);
        }

        /// <summary>
        /// Push a starting transform history in Undo actions.
        /// </summary>
        public void PushStartingTransform(ILayer layer)
        {
            var previous = new TransformPosition
            {
                Style_Fill_Center = layer.Style.Fill.StartingCenter,
                Style_Fill_XPoint = layer.Style.Fill.StartingXPoint,
                Style_Fill_YPoint = layer.Style.Fill.StartingYPoint,

                Style_Stroke_Center = layer.Style.Stroke.StartingCenter,
                Style_Stroke_XPoint = layer.Style.Stroke.StartingXPoint,
                Style_Stroke_YPoint = layer.Style.Stroke.StartingYPoint,

                Transform_Transformer = layer.Transform.StartingTransformer,
                Transform_CropTransformer = layer.Transform.StartingCropTransformer,
            };

            this.UndoActions.Add(layer, previous);
        }

        public override void Undo()
        {
            foreach (var undo in this.UndoActions)
            {
                ILayer layer = undo.Key;
                TransformPosition position = undo.Value;

                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;

                layer.Style.Fill.Center = position.Style_Fill_Center;
                layer.Style.Fill.XPoint = position.Style_Fill_XPoint;
                layer.Style.Fill.YPoint = position.Style_Fill_YPoint;

                layer.Style.Stroke.Center = position.Style_Stroke_Center;
                layer.Style.Stroke.XPoint = position.Style_Stroke_XPoint;
                layer.Style.Stroke.YPoint = position.Style_Stroke_YPoint;

                layer.Transform.Transformer = position.Transform_Transformer;
                layer.Transform.CropTransformer = position.Transform_CropTransformer;
            }
        }

    }
}
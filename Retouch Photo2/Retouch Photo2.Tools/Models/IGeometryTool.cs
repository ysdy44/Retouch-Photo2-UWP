using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="IGeometryTool"/>'s ICreateTool.
    /// </summary>
    public abstract class IGeometryTool : ICreateTool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Abstract
        /// <summary>
        /// Create a specific geometry layer.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <returns> The created layer. </returns>
        public abstract IGeometryLayer CreateGeometryLayer(Transformer transformer);

        //@Override
        public override ILayer CreateLayer(Transformer transformer)
        {
            IGeometryLayer geometryLayer = this.CreateGeometryLayer(transformer);

            geometryLayer.SelectMode = SelectMode.Selected;
            geometryLayer.TransformManager = new TransformManager(transformer);

            geometryLayer.FillBrush = new Brush
            {
                Type = BrushType.Color,
                Color = this.SelectionViewModel.FillColor,
            };

            return geometryLayer;
        }
    }
}
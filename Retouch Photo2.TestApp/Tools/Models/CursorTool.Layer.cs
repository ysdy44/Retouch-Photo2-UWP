using Retouch_Photo2.Layers;
using Retouch_Photo2.Transformers;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.TestApp.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s CursorTool .
    /// </summary>
    public partial class CursorTool
    {
        /// <summary>
        /// Select a layer from a point,
        /// make it to <see langword="sealed" cref = "SelectionLayer" />
        /// and make the <see cref = "CursorTool.TransformerMode" /> to move,
        /// find the layer that makes it unique, and 
        /// </summary>
        /// <param name="point"> point </param>
        /// <returns> Return **false** if you do not select to any layer. </returns>
        private bool SelectLayer(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, matrix);

            Layer selectedLayer = this.ViewModel.Layers.FirstOrDefault((layer) =>
            {
                if (layer.Visibility== Visibility.Visible)
                {
                    bool layerInQuadrangle = Transformer.InQuadrangle(canvasPoint, layer.TransformerMatrix.Destination);
                    if (layerInQuadrangle)
                    {
                        selectedLayer = layer;
                        return true;
                    }
                }
                return false;
            });
            if (selectedLayer == null) return false;


            //Selection
            this.Selection.SetValue((layer) =>
            {
                layer.IsChecked = false;
            });
            this.TransformerMode = TransformerMode.Translation;//TransformerMode


            //Selection
            selectedLayer.IsChecked = true;
            this.Selection.SetModeSingle(selectedLayer);//Transformer
            this.ViewModel.Invalidate();//Invalidate
            return true;
        }

        /// <summary>
        /// Add the layer to the layers. 
        /// </summary>
        /// <param name="point"> point </param>
        /// <param name="isAdd"> <see cref = "CompositeMode.Add" /> or <see cref = "CompositeMode.Subtract" /> </param>
        /// <returns> Return **false** if you do not select to any layer. </returns>
        private bool AddLayer(Vector2 point, bool isAdd = true)
        {
            Vector2 canvasPoint = Vector2.Transform(point, this.ViewModel.CanvasTransformer.GetInverseMatrix());

            Layer selectedLayer = this.ViewModel.Layers.FirstOrDefault((layer) =>
            {
                if (layer.Visibility == Visibility.Visible)
                {
                    Transformer layerTransformer = layer.TransformerMatrix.Destination;
                    bool layerInQuadrangle = Transformer.InQuadrangle(canvasPoint, layerTransformer);
                    if (layerInQuadrangle) return true;
                }

                return false;
            });

            if (selectedLayer == null) return false;
            selectedLayer.IsChecked = isAdd;

            this.TransformerMode = TransformerMode.None;//TransformerMode
            this.Selection.SetMode(this.ViewModel.Layers);//Transformer
            this.ViewModel.Invalidate();//Invalidate

            return true;
        }
    }
        
}
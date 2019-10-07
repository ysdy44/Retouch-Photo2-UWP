using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITransformerTool"/>'s TransformerTool.
    /// </summary>
    public partial class TransformerTool : ITransformerTool
    {

        private bool StartingFromTransformerMode(Vector2 point, TransformerMode transformerMode)
        {
            switch (this.CompositeMode)
            {
                case CompositeMode.New:
                case CompositeMode.Intersect:
                    {
                        if (transformerMode == TransformerMode.None)
                        {
                            bool isSelected = this.SelectSingleLayer(point);
                            return isSelected;
                        }
                    }
                    break;
                case CompositeMode.Add:
                    {
                        if (transformerMode == TransformerMode.None || transformerMode == TransformerMode.Translation)
                        {
                            bool isAdd = this.AddOrSubtractLayer(point, CompositeMode.Add);
                            return isAdd;
                        }
                    }
                    break;
                case CompositeMode.Subtract:
                    {
                        if (transformerMode == TransformerMode.None || transformerMode == TransformerMode.Translation)
                        {
                            bool isSubtract = this.AddOrSubtractLayer(point, CompositeMode.Subtract);
                            return isSubtract;
                        }
                    }
                    break;
            }
            return true;
        }


        public bool SelectSingleLayer(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, matrix);

            //Select a layer of the same depth
            bool isChildSingle = (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.Single
                && this.SelectionViewModel.Layer.Parents != null);
            IList<ILayer> parentsChildren = isChildSingle ?
                this.SelectionViewModel.Layer.Parents.Children :
                this.ViewModel.Layers.RootLayers;

            ILayer selectedLayer = parentsChildren.FirstOrDefault((layer) =>
            {
                if (layer.Visibility == Visibility.Visible)
                {
                    Transformer transformer = layer.GetActualDestinationWithRefactoringTransformer;
                    bool isFillContainsPoint = transformer.FillContainsPoint(canvasPoint);
                    if (isFillContainsPoint)
                    {
                        selectedLayer = layer;
                        return true;
                    }
                }
                return false;
            });
            if (selectedLayer == null) return false;


            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.SelectMode = SelectMode.UnSelected;
            });
            this._transformerMode = TransformerMode.Translation;//TransformerMode


            //Selection
            selectedLayer.SelectMode = SelectMode.Selected;

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
            this.ViewModel.Invalidate();//Invalidate
            return true;
        }

        /// <summary>
        /// Add or subtract the layer to the all checked layers. 
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="mode"> <see cref = "CompositeMode.Add" /> or <see cref = "CompositeMode.Subtract" /> </param>
        /// <returns> Return **false** if you do not select to any layer. </returns>
        private bool AddOrSubtractLayer(Vector2 point, CompositeMode mode)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            ILayer selectedLayer = this.ViewModel.Layers.RootLayers.FirstOrDefault((layer) =>
            {
                if (layer.Visibility == Visibility.Visible)
                {
                    //Transformer
                    Transformer transformer = layer.GetActualDestinationWithRefactoringTransformer;
                    bool isFillContainsPoint = transformer.FillContainsPoint(canvasPoint);
                    if (isFillContainsPoint) return true;
                }

                return false;
            });

            if (selectedLayer == null) return false;
            switch (mode)
            {
                case CompositeMode.Add:
                    selectedLayer.SelectMode = SelectMode.Selected;
                    break;
                case CompositeMode.Subtract:
                    selectedLayer.SelectMode = SelectMode.UnSelected;
                    break;
            }

            this._transformerMode = TransformerMode.None;//TransformerMode

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Transformer
            this.ViewModel.Invalidate();//Invalidate

            return true;
        }

    }
}
using FanKit.Transformers;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some selection propertys of the application.
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary>
        ///  Sets the mode and notify all properties.
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        public void SetMode(LayerageCollection layerageCollection)
        {
            //Layerages
            IEnumerable<Layerage> selectedLayeragesRecursive = LayerageCollection.GetAllSelectedRecursive(layerageCollection);
            int count = selectedLayeragesRecursive.Count();

            if (count == 0)
            {
                this.SetModeNone();//None
            }
            else if (count == 1)
            {
                Layerage outermost = LayerageCollection.FindOutermostLayerage(selectedLayeragesRecursive);
                this.SetModeSingle(outermost);//Single
            }
            else if (count >= 2)
            {
                this.SetModeMultiple(selectedLayeragesRecursive);//Multiple
            }
        }


        //////////////////////////


        /// <summary>
        ///  Sets the mode to None.
        /// </summary>
        public void SetModeNone()
        {
            this._setModeNone();//None
        }
        private void _setModeNone()
        {
            this.SelectionMode = ListViewSelectionMode.None;
            
            this.Transformer = new Transformer();
            this.DisabledRadian = false;

            this.SelectionLayerage = null;
            this.SelectionLayerages = null;

            //////////////////////////

            this.LayerType = LayerType.None;
            this.LayerName = string.Empty;
            this.SetOpacity(1.0f);
            this.BlendMode = null;
            this.SetVisibility(Visibility.Collapsed);
            this.SetTagType(TagType.None);

            //////////////////////////

            this.Effect = null;
            this.Filter = null;
            this.SetStyle(null);

            //////////////////////////

            this.SetGroupLayer();
            this.SetImageLayer(null);
            this.SetCurveLayer();
            this.SetFontLayer(null);
            
            //////////////////////////

            this.SetIGeometryLayer(null);            
        }


        //////////////////////////


        /// <summary>
        ///  Sets the mode to Single.
        /// </summary>
        /// <param name="layerage"> The single layer. </param>
        public void SetModeSingle(Layerage layerage)
        {
            this._setModeSingle(layerage);//None
        }
        private void _setModeSingle(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            this.SelectionMode = ListViewSelectionMode.Single;

            this.Transformer = layerage.GetActualTransformer();
            this.DisabledRadian = false;

            this.SelectionLayerage = layerage;
            this.SelectionLayerages = null;

            //////////////////////////

            this.LayerType = layer.Type;
            this.LayerName = layer.Name;
            this.SetOpacity(layer.Opacity);
            this.BlendMode = layer.BlendMode;
            this.SetVisibility(layer.Visibility);
            this.SetTagType(layer.TagType);

            //////////////////////////

            this.Effect = layer.Effect;
            this.Filter = layer.Filter;
            this.SetStyle(layer.Style);

            //////////////////////////

            this.SetGroupLayer(layer);
            this.SetImageLayer(layer);
            this.SetCurveLayer(layerage, layer);
            this.SetFontLayer(layer);

            //////////////////////////

            this.SetIGeometryLayer(layer);
        }


        //////////////////////////


        /// <summary>
        ///  Sets the mode to Multiple.
        /// </summary>
        /// <param name="layerages"> The multiple layerages. </param>
        public void SetModeMultiple(IEnumerable<Layerage> layerages)
        {
            this._setModeMultiple(layerages);//Multiple
        }
        private void _setModeMultiple(IEnumerable<Layerage> layerages)
        {
            Layerage outermost = LayerageCollection.FindOutermostLayerage(layerages);
            ILayer outermostLayer = outermost.Self;

            this.SelectionMode = ListViewSelectionMode.Multiple;//Transformer

            this.SelectionLayerage = null;
            this.SelectionLayerages = layerages;

            //TransformerBorder
            TransformerBorder border = new TransformerBorder(layerages);
            this.Transformer = border.ToTransformer();
            this.DisabledRadian = false;

            //////////////////////////

            this.LayerType = outermostLayer == null ? LayerType.None : outermostLayer.Type;
            this.SetOpacity(outermostLayer == null ? 1.0f : outermostLayer.Opacity);
            this.BlendMode = outermostLayer?.BlendMode;
            this.SetVisibility(outermostLayer == null ? Visibility.Visible : outermostLayer.Visibility);
            this.SetTagType(outermostLayer == null ? TagType.None : outermostLayer.TagType);

            //////////////////////////

            this.Effect = outermostLayer?.Effect;
            this.Filter = null;
            this.SetStyle(outermostLayer?.Style);

            //////////////////////////

            this.SetGroupLayer(layerages);
            this.SetImageLayer(outermostLayer);
            this.SetCurveLayer();
            this.SetFontLayer(null);

            //////////////////////////

            this.SetIGeometryLayer(outermostLayer);
        }

        
        //////////////////////////


        /// <summary>
        ///  Sets the mode to Extended.
        /// </summary>
        public void SetModeExtended()
        {
            this.SetModeNone();
            this.SelectionMode = ListViewSelectionMode.Extended;
        }

    }
}
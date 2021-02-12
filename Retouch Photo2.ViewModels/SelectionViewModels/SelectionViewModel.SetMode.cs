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
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary>
        ///  Sets the mode and notify all properties.
        /// </summary>
        public void SetMode()
        {
            //Layerages
            IEnumerable<Layerage> selectedLayeragesRecursive = LayerManager.GetAllSelectedRecursive();
            int count = selectedLayeragesRecursive.Count();

            if (count == 0)
            {
                this.SetModeNone();//None
            }
            else if (count == 1)
            {
                Layerage outermost = LayerManager.FindOutermostLayerage(selectedLayeragesRecursive);
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
            this.SelectionMode = ListViewSelectionMode.None;
            
            this.Transformer = new Transformer();

            this.SelectionLayerage = null;
            this.SelectionLayerages = null;

            //////////////////////////

            this.LayerType = LayerType.None;
            this.LayerName = string.Empty;
            this.Opacity = 1.0f;
            this.BlendMode = null;
            this.visibility = Visibility.Collapsed;
            this.TagType = TagType.None;

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

            this.SetPatternLayer(null);
            this.SetGeometryLayer(null);            
        }


        //////////////////////////


        /// <summary>
        ///  Sets the mode to Single.
        /// </summary>
        /// <param name="layerage"> The single layer. </param>
        public void SetModeSingle(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            this.SelectionMode = ListViewSelectionMode.Single;

            this.Transformer = layerage.GetActualTransformer();

            this.SelectionLayerage = layerage;
            this.SelectionLayerages = null;

            //////////////////////////

            this.LayerType = layer.Type;
            this.LayerName = layer.Name;
            this.Opacity = layer.Opacity;
            this.BlendMode = layer.BlendMode;
            this.visibility = layer.Visibility;
            this.TagType = layer.TagType;

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

            this.SetPatternLayer(layer);
            this.SetGeometryLayer(layer);
        }


        //////////////////////////


        /// <summary>
        ///  Sets the mode to Multiple.
        /// </summary>
        /// <param name="layerages"> The multiple layerages. </param>
        public void SetModeMultiple(IEnumerable<Layerage> layerages)
        {
            Layerage outermost = LayerManager.FindOutermostLayerage(layerages);
            ILayer outermostLayer = outermost.Self;

            this.SelectionMode = ListViewSelectionMode.Multiple;//Transformer

            this.SelectionLayerage = null;
            this.SelectionLayerages = layerages;

            //TransformerBorder
            TransformerBorder border = new TransformerBorder(layerages);
            this.Transformer = border.ToTransformer();

            //////////////////////////

            this.LayerType = outermostLayer == null ? LayerType.None : outermostLayer.Type;
            this.Opacity = outermostLayer == null ? 1.0f : outermostLayer.Opacity;
            this.BlendMode = outermostLayer?.BlendMode;
            this.visibility = outermostLayer == null ? Visibility.Visible : outermostLayer.Visibility;
            this.TagType = outermostLayer == null ? TagType.None : outermostLayer.TagType;

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

            this.SetPatternLayer(outermostLayer);
            this.SetGeometryLayer(outermostLayer);
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
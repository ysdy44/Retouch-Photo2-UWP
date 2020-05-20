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
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary>
        ///  Sets the mode and notify all properties.
        /// </summary>
        /// <param name="layerCollection"> The layer-collection. </param>
        public void SetMode(LayerCollection layerCollection)
        {
            IEnumerable<ILayer> checkedLayers = LayerCollection.GetAllSelectedLayers(layerCollection);
            int count = checkedLayers.Count();

            if (count == 0)
                this._setModeNone();//None
            else if (count == 1)
                this._setModeSingle(checkedLayers.Single());//Single
            else if (count >= 2)
            {
                this._setModeMultiple(checkedLayers);//Multiple
            }
        }
        

        //////////////////////////


        /// <summary>
        ///  Sets the mode to None.
        /// </summary>
        public void SetModeNone()
        {
            if (this.Layer != null)
            {
                this.Layer.IsSelected = false;
            }

            if (this.Layers != null)
            {
                foreach (ILayer child in this.Layers)
                {
                    child.IsSelected = false;
                }
            }
            this._setModeNone();//None
        }
        private void _setModeNone()
        {
            this.SelectionMode = ListViewSelectionMode.None;
            this.SelectionUnNone = false;
            this.SelectionSingle = false;
            
            //this.Transformer = new Transformer();
            this.DisabledRadian = false;

            this.Layer = null;
            this.Layers = null;

            //////////////////////////

            this.Type = LayerType.None;
            this.LayerName = string.Empty;
            this.SetOpacity(1.0f);
            this.BlendMode = null;
            this.SetVisibility(Visibility.Collapsed);
            this.SetTagType(TagType.None);

            //////////////////////////

            this.IsCrop = false;
            this.Effect = null;
            this.Filter = null;
            this.SetStyle(null);

            //////////////////////////

            this.SetGroupLayer();
            this.SetImageLayer(null);
            this.SetCurveLayer(null);
            this.SetFontLayer(null);
            
            //////////////////////////

            this.SetIGeometryLayer(null);            
        }


        //////////////////////////


        /// <summary>
        ///  Sets the mode to Single.
        /// </summary>
        /// <param name="layer"> The single layer. </param>
        public void SetModeSingle(ILayer layer)
        {
            if (this.Layers != null)
            {
                foreach (ILayer child in this.Layers)
                {
                    child.IsSelected = false;
                }
            }
            this._setModeSingle(layer);//Single
        }
        private void _setModeSingle(ILayer layer)
        {
            this.SelectionMode = ListViewSelectionMode.Single;
            this.SelectionUnNone = true;
            this.SelectionSingle = true;

            this.Transformer = layer.GetActualDestinationWithRefactoringTransformer;
            this.DisabledRadian = false;

            this.Layer = layer;
            this.Layers = null;

            //////////////////////////

            this.Type = layer.Type;
            this.LayerName = layer.Name;
            this.SetOpacity(layer.Opacity);
            this.BlendMode = layer.BlendMode;
            this.SetVisibility(layer.Visibility);
            this.SetTagType(layer.TagType);

            //////////////////////////

            this.IsCrop = layer.Transform.IsCrop;
            this.Effect = layer.Effect;
            this.Filter = layer.Filter;
            this.SetStyle(layer.Style);

            //////////////////////////

            this.SetGroupLayer(layer);
            this.SetImageLayer(layer);
            this.SetCurveLayer(layer);
            this.SetFontLayer(layer);

            //////////////////////////

            this.SetIGeometryLayer(layer);
        }


        //////////////////////////


        /// <summary>
        ///  Sets the mode to Multiple.
        /// </summary>
        /// <param name="layer"> The multiple layer. </param>
        /// <param name="outermost"> The outermost layer. </param>
        public void SetModeMultiple(IList<ILayer> layers)
        {
            if (this.Layer != null)
            {
                this.Layer.IsSelected = false;
            }
            this._setModeMultiple(layers);//Multiple
        }
        private void _setModeMultiple(IEnumerable<ILayer> layers)
        {
            ILayer outermost = LayerCollection.FindOutermost_SelectedLayer(layers);
            this.SelectionMode = ListViewSelectionMode.Multiple;//Transformer     
            this.SelectionUnNone = true;
            this.SelectionSingle = false;

            this.Layer = null;
            this.Layers = layers;

            //TransformerBorder
            IEnumerable<Transformer> transformers = from l in layers select l.GetActualDestinationWithRefactoringTransformer;
            TransformerBorder border = new TransformerBorder(transformers);
            this.Transformer = border.ToTransformer();
            this.DisabledRadian = false;

            //////////////////////////

            this.Type = outermost == null ? LayerType.None : outermost.Type;
            this.SetOpacity(outermost == null ? 1.0f : outermost.Opacity);
            this.BlendMode = outermost?.BlendMode;
            this.SetVisibility(outermost == null ? Visibility.Visible : outermost.Visibility);
            this.SetTagType(outermost == null ? TagType.None : outermost.TagType);

            //////////////////////////

            this.IsCrop = layers.Any(layer => layer.Transform.IsCrop);
            this.Effect = outermost?.Effect;
            this.Filter = null;
            this.SetStyle(outermost?.Style);

            //////////////////////////

            this.SetGroupLayer(layers);
            this.SetImageLayer(outermost);
            this.SetCurveLayer(null);
            this.SetFontLayer(null);

            //////////////////////////

            this.SetIGeometryLayer(outermost);
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
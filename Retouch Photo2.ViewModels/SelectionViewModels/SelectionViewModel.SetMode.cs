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
            IList<ILayer> checkedLayers = layerCollection.GetAllSelectedLayers();
            int count = checkedLayers.Count();

            if (count == 0)
                this._setModeNone();//None
            else if (count == 1)
                this._setModeSingle(checkedLayers.Single());//Single
            else if (count >= 2)
                this._setModeMultiple(checkedLayers);//Multiple
        }
        

        //////////////////////////


        /// <summary>
        ///  Sets the mode to None.
        /// </summary>
        public void SetModeNone()
        {
            if (this.Layer!=null)
            {
                this.Layer.SelectMode = Retouch_Photo2.Layers.SelectMode.UnSelected;
            }

            if (this.Layers != null)
            {
                foreach (ILayer child in this.Layers)
                {
                    child.SelectMode = Retouch_Photo2.Layers.SelectMode.UnSelected;
                }
            }
            this._setModeNone();//None
        }
        private void _setModeNone()
        {
            this.SelectionMode = ListViewSelectionMode.None;

            this.Transformer = new Transformer();
            this.DisabledRadian = false;

            this.Layer = null;
            this.Layers = null;

            //////////////////////////

            this.Type = LayerType.None;
            this.SetOpacity(1.0f);
            this.BlendMode = null;
            this.SetVisibility(Visibility.Collapsed);
            this.SetTagType(TagType.None);

            //////////////////////////

            this.IsCrop = false;
            this.EffectManager = null;
            this.AdjustmentManager = null;
            this.SetStyleManager(null);

            //////////////////////////

            this.SetGroupLayer(null);
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
                    child.SelectMode = Retouch_Photo2.Layers.SelectMode.UnSelected;
                }
            }
            this._setModeSingle(layer);//Single
        }
        private void _setModeSingle(ILayer layer)
        {
            this.SelectionMode = ListViewSelectionMode.Single;

            this.Transformer = layer.GetActualDestinationWithRefactoringTransformer;
            this.DisabledRadian = false;

            this.Layer = layer;
            this.Layers = null;

            //////////////////////////

            this.Type = layer.Type;
            this.SetOpacity(layer.Opacity);
            this.BlendMode = layer.BlendMode;
            this.SetVisibility(layer.Visibility);
            this.SetTagType(layer.TagType);

            //////////////////////////

            this.IsCrop = layer.TransformManager.IsCrop;
            this.EffectManager = layer.EffectManager;
            this.AdjustmentManager = layer.AdjustmentManager;
            this.SetStyleManager(layer.StyleManager);

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
        public void SetModeMultiple(IList<ILayer> layers)
        {
            if (this.Layer != null)
            {
                this.Layer.SelectMode = Retouch_Photo2.Layers.SelectMode.UnSelected;
            }
            this._setModeMultiple(layers);//Multiple
        }
        private void _setModeMultiple(IList<ILayer> layers)
        {
            this.SelectionMode = ListViewSelectionMode.Multiple;//Transformer     

            this.Layer = null;
            this.Layers = layers;

            this.Transformer = LayerCollection.RefactoringTransformer(layers);
            this.DisabledRadian = false;

            //////////////////////////

            ILayer firstLayer = layers.First();

            this.Type = firstLayer.Type;
            this.SetOpacity(firstLayer.Opacity);
            this.BlendMode = firstLayer.BlendMode;
            this.SetVisibility(firstLayer.Visibility);
            this.SetTagType(firstLayer.TagType);

            //////////////////////////

            this.IsCrop = layers.Any(layer => layer.TransformManager.IsCrop);
            this.EffectManager = firstLayer.EffectManager;
            this.AdjustmentManager = null;
            this.SetStyleManager(firstLayer.StyleManager);

            //////////////////////////

            this.SetGroupLayer(null);
            this.SetImageLayer(firstLayer);
            this.SetCurveLayer(null);
            this.SetFontLayer(null);

            //////////////////////////

            this.SetIGeometryLayer(firstLayer);
        }

    }
}
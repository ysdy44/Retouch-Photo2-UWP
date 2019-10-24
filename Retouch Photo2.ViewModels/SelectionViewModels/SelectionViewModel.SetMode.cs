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
        ///  Sets and notify all properties.
        /// </summary>
        /// <param name="layerCollection"> The layer-collection. </param>
        public void SetMode(LayerCollection layerCollection)
        {
            IList<ILayer> checkedLayers = layerCollection.GetAllSelectedLayers();
            int count = checkedLayers.Count();

            if (count == 0) this._setModeNone();//None
            else if (count == 1) this._setModeSingle(checkedLayers.Single());//Single
            else if (count >= 2) this._setModeMultiple(checkedLayers);//Multiple
        }


        public void SetModeNone()
        {
            if (this.Layer!=null)
            {
                this.Layer.SelectMode = SelectMode.UnSelected;
            }

            if (this.Layers != null)
            {
                foreach (ILayer child in this.Layers)
                {
                    child.SelectMode = SelectMode.UnSelected;
                }
            }
            this._setModeNone();//None
        }
        private void _setModeNone()
        {
            this.Transformer = new Transformer();
            this.DsabledRadian = false;//DisabledRadian

            this.Layer = null;
            this.Layers = null;

            this.SelectionMode = ListViewSelectionMode.None;

            //////////////////////////

            this.Type = string.Empty;
            this.SetOpacity(1.0f);
            this.BlendType = BlendType.None;
            this.Visibility = Visibility.Collapsed;
            this.TagType = TagType.None;

            //////////////////////////

            this.IsCrop = false;
            this.EffectManager = null;
            this.AdjustmentManager = null;
            this.SetStyleManager(null);

            //////////////////////////

            this.SetGroupLayer(null);
            this.SetAcrylicLayer(null);
            this.SetImageLayer(null);
            this.SetCurveLayer(null);

            //////////////////////////

            this.SetIGeometryLayer(null);            
        }


        public void SetModeSingle(ILayer layer)
        {
            if (this.Layers != null)
            {
                foreach (ILayer child in this.Layers)
                {
                    child.SelectMode = SelectMode.UnSelected;
                }
            }
            this._setModeSingle(layer);//Single
        }
        private void _setModeSingle(ILayer layer)
        {
            this.Transformer = layer.GetActualDestinationWithRefactoringTransformer;
            this.DsabledRadian = layer.TransformManager.DisabledRadian;//DisabledRadian
            
            this.Layer = layer;
            this.Layers = null;

            this.SelectionMode = ListViewSelectionMode.Single;

            //////////////////////////

            this.Type = layer.Type;
            this.SetOpacity(layer.Opacity);
            this.BlendType = layer.BlendType;
            this.Visibility = layer.Visibility;
            this.TagType = layer.TagType;

            //////////////////////////

            this.IsCrop = (layer.TransformManager.IsCrop && (layer.TransformManager.DisabledRadian == false));
            this.EffectManager = layer.EffectManager;
            this.AdjustmentManager = layer.AdjustmentManager;
            this.SetStyleManager(layer.StyleManager);

            //////////////////////////

            this.SetGroupLayer(layer);
            this.SetAcrylicLayer(layer);
            this.SetImageLayer(layer);
            this.SetCurveLayer(layer);

            //////////////////////////

            this.SetIGeometryLayer(layer);
        }


        public void SetModeMultiple(IList<ILayer> layers)
        {
            if (this.Layer != null)
            {
                this.Layer.SelectMode = SelectMode.UnSelected;
            }
            this._setModeMultiple(layers);//Multiple
        }
        private void _setModeMultiple(IList<ILayer> layers)
        {
            Transformer transformer = LayerCollection.RefactoringTransformer(layers);
            this._setModeMultipleWithTransformer(layers, transformer, false);
        }

        private void _setModeMultipleWithTransformer(IEnumerable<ILayer> layers, Transformer transformer, bool disabledRadian)
        {
            this.Transformer = transformer;
            this.DsabledRadian = disabledRadian;

            this.Layer = null;
            this.Layers = layers;

            this.SelectionMode = ListViewSelectionMode.Multiple;//Transformer      

            //////////////////////////

            this.Type = string.Empty;
            //this.SetOpacity(0);
            //this.SetBlendType( BlendType.Normal);
            //this.Visibility = Visibility.Collapsed;
            //this.TagType = TagType.None;

            //////////////////////////

            this.IsCrop = layers.Any(layer => (layer.TransformManager.IsCrop && (layer.TransformManager.DisabledRadian == false)));
            //this.EffectManager = layer.EffectManager;
            this.AdjustmentManager = null;
            //this.SetStyleManager(null);

            //////////////////////////

            this.SetGroupLayer(null);
            this.SetAcrylicLayer(null);
            //this.SetImageLayer(layer);
            //this.SetGeometryLayer(null);
            this.SetCurveLayer(null);

            //////////////////////////

            //this.SetIGeometryLayer(null);
        }

    }
}
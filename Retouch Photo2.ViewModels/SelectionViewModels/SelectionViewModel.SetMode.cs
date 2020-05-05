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
        /// Get the selected layer.
        /// <see cref="ListViewSelectionMode.None"/>: null;
        /// <see cref="ListViewSelectionMode.Single"/>: layer;
        /// <see cref="ListViewSelectionMode.Multiple"/>: first layer;
        /// </summary>
        /// <returns> The selected layer. </returns>
        public ILayer GetFirstLayer()
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None: return null;
                case ListViewSelectionMode.Single: return this.Layer;
                case ListViewSelectionMode.Multiple: return this.Layers.FirstOrDefault();
                default: return null;
            }
        }

        
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

        //////////////////////////

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
            this.Transformer = new Transformer();
            this.DisabledRadian = false;//DisabledRadian

            this.Layer = null;
            this.Layers = null;

            this.SelectionMode = ListViewSelectionMode.None;

            //////////////////////////

            this.Type = LayerType.None;
            this.SetOpacity(1.0f);
            this.SetBlendMode(null);
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
            this.SetTextFrameLayer(null);
            
            //////////////////////////

            this.SetIGeometryLayer(null);            
        }

        //////////////////////////

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
            this.Transformer = layer.GetActualDestinationWithRefactoringTransformer;
            
            this.Layer = layer;
            this.Layers = null;

            this.SelectionMode = ListViewSelectionMode.Single;

            //////////////////////////

            this.Type = layer.Type;
            this.SetOpacity(layer.Opacity);
            this.SetBlendMode(layer.BlendMode);
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
            this.SetTextFrameLayer(layer);

            //////////////////////////

            this.SetIGeometryLayer(layer);
        }

        //////////////////////////

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
            Transformer transformer = LayerCollection.RefactoringTransformer(layers);
            this._setModeMultipleWithTransformer(layers, transformer, false);
        }

        private void _setModeMultipleWithTransformer(IEnumerable<ILayer> layers, Transformer transformer, bool disabledRadian)
        {
            this.Transformer = transformer;
            this.DisabledRadian = disabledRadian;

            this.Layer = null;
            this.Layers = layers;

            this.SelectionMode = ListViewSelectionMode.Multiple;//Transformer      

            //////////////////////////

            ILayer firstLayer = layers.First();

            this.Type = firstLayer.Type;
            this.SetOpacity(firstLayer.Opacity);
            this.SetBlendMode(firstLayer.BlendMode);
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
            this.SetTextFrameLayer(null);

            //////////////////////////

            this.SetIGeometryLayer(firstLayer);
        }

    }
}
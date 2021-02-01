using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Filters;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Photos;
using Retouch_Photo2.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some selection propertys of the application.
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> Gets or sets the tool type. </summary>
        public ToolType ToolType
        {
            get => this.toolType;
            set
            {
                this.toolType = value;
                this.OnPropertyChanged(nameof(ToolType));//Notify 
            }
        }
        private ToolType toolType = ToolType.Cursor;


        /// <summary> Gets or sets the layer type. </summary>
        public LayerType LayerType
        {
            get => this.layerType;
            set
            {
                this.layerType = value;
                this.OnPropertyChanged(nameof(LayerType));//Notify 
            }
        }
        private LayerType layerType = LayerType.None;


        /// <summary> Gets or sets the layer name. </summary>
        public string LayerName
        {
            get => this.layerName;
            set
            {
                this.layerName = value;
                this.OnPropertyChanged(nameof(LayerName));//Notify 
            }
        }
        private string layerName = string.Empty;


        /// <summary> Gets or sets the layer opacity. </summary>
        public float Opacity
        {
            get => this.opacity;
            set
            {
                if (this.Opacity == value) return;
                this.opacity = value;
                this.OnPropertyChanged(nameof(Opacity));//Notify 
            }
        }
        private float opacity = 1.0f;


        /// <summary> Gets or sets the layer blend-mode. </summary>
        public BlendEffectMode? BlendMode
        {
            get => this.blendMode;
            set
            {
                this.blendMode = value;
                this.OnPropertyChanged(nameof(BlendMode));//Notify 
            }
        }
        private BlendEffectMode? blendMode = null;


        /// <summary> Gets or sets the layer visibility. </summary>
        public Visibility Visibility { get; set; } = Visibility.Visible;
        /// <summary> Sets the <see cref = "ViewModel.Visibility" />. </summary>
        public void SetVisibility(Visibility value)
        {
            if (this.Visibility == value) return;
            this.Visibility = value;
            this.OnPropertyChanged(nameof(Visibility));//Notify 
        }


        /// <summary> Gets or sets the layer tag-type. </summary>
        public TagType TagType { get; set; } = TagType.None;
        /// <summary> Sets the <see cref = "ViewModel.TagType" />. </summary>
        public void SetTagType(TagType value)
        {
            if (this.TagType == value) return;
            this.TagType = value;
            this.OnPropertyChanged(nameof(TagType));//Notify 
        }


        //////////////////////////


        /// <summary> Gets or sets the layer effect. </summary>
        public Effect Effect
        {
            get => this.effect;
            set
            {
                this.effect = value;
                this.OnPropertyChanged(nameof(Effect));//Notify 
            }
        }
        private Effect effect;


        /// <summary> Gets or sets the layer filter. </summary>
        public Filter Filter
        {
            get => this.filter;
            set
            {
                this.filter = value;
                this.OnPropertyChanged(nameof(Filter));//Notify 
            }
        }
        private Filter filter;


        //////////////////////////


        /// <summary> Gets or sets whether the current selected-layer is a group-layer.. </summary>     
        public bool IsGroupLayer
        {
            get => this.isGroupLayer;
            set
            {
                this.isGroupLayer = value;
                this.OnPropertyChanged(nameof(IsGroupLayer));//Notify 
            }
        }
        private bool isGroupLayer;
        /// <summary> Sets the GroupLayer. </summary>     
        private void SetGroupLayer() => this.IsGroupLayer = false;
        /// <summary> Sets the GroupLayer. </summary>     
        private void SetGroupLayer(ILayer layer) => this.IsGroupLayer = layer.Type == LayerType.Group;
        /// <summary> Sets the GroupLayer. </summary>     
        private void SetGroupLayer(IEnumerable<Layerage> layerages) => this.IsGroupLayer = layerages.Any(layer => layer.Self.Type == LayerType.Group);


        /// <summary> Gets or sets whether the current selected-layer is a image-layer.. </summary>     
        public bool IsImageLayer
        {
            get => this.isImageLayer;
            set
            {
                this.isImageLayer = value;
                this.OnPropertyChanged(nameof(IsImageLayer));//Notify 
            }
        }
        private bool isImageLayer;

        /// <summary> Gets or sets the current photocopier. </summary>
        public Photocopier Photocopier
        {
            get => this.photocopier;
            set
            {
                this.photocopier = value;
                this.OnPropertyChanged(nameof(Photocopier));//Notify 
            }
        }
        private Photocopier photocopier;
        /// <summary> Sets the ImageLayer. </summary>     
        private void SetImageLayer(ILayer layer)
        {
            if (layer == null) this.IsImageLayer = false;
            else
            {
                if (layer.Type == LayerType.Image)
                {
                    this.IsImageLayer = true;
                    this.Photocopier = layer.Style.Fill.Photocopier;
                }
                else this.IsImageLayer = false;
            }
        }


        /// <summary> Gets or sets the current curve layerage. </summary>
        public Layerage CurveLayerage { get; set; }
        /// <summary> Gets or sets the current curve layer. </summary>
        public CurveLayer CurveLayer { get; set; }
        /// <summary> Sets the CurveLayer. </summary>     
        private void SetCurveLayer()
        {
            this.CurveLayerage = null;
            this.CurveLayer = null;
        }
        private void SetCurveLayer(Layerage layerage, ILayer layer)
        {
            if (layer == null)
            {
                this.SetCurveLayer();
                return;
            }

            if (layer.Type == LayerType.Curve)
            {
                this.CurveLayerage = layerage;
                this.CurveLayer = (CurveLayer)layer;
            }
            else
            {
                this.SetCurveLayer();
            }
        }


    }
}
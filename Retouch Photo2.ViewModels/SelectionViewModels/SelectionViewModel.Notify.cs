using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary> <see cref = "ViewModel" />'s layer type. </summary>
        public LayerType LayerType
        {
            get => this.layerType;
            set
            {
                this.layerType = value;
                this.OnPropertyChanged(nameof(this.LayerType));//Notify 
            }
        }
        private LayerType layerType = LayerType.None;


        /// <summary> <see cref = "ViewModel" />'s name. </summary>
        public string LayerName
        {
            get => this.layerName;
            set
            {
                this.layerName = value;
                this.OnPropertyChanged(nameof(this.LayerName));//Notify 
            }
        }
        private string layerName = string.Empty;


        /// <summary> <see cref = "ViewModel" />'s opacity. </summary>
        public float Opacity { get; set; } = 1.0f;
        /// <summary> Sets the <see cref = "ViewModel.Opacity" />. </summary>
        public void SetOpacity(float value)
        {
            if (this.Opacity == value) return;
            this.Opacity = value;
            this.OnPropertyChanged(nameof(this.Opacity));//Notify 
        }


        /// <summary> <see cref = "ViewModel" />'s blend-mode. </summary>
        public BlendEffectMode? BlendMode
        {
            get => this.blendMode;
            set
            {
                this.blendMode = value;
                this.OnPropertyChanged(nameof(this.BlendMode));//Notify 
            }
        }
        private BlendEffectMode? blendMode = null;


        /// <summary> <see cref = "ViewModel" />'s visibility. </summary>
        public Visibility Visibility { get; set; } = Visibility.Visible;
        /// <summary> Sets the <see cref = "ViewModel.Visibility" />. </summary>
        public void SetVisibility(Visibility value)
        {
            if (this.Visibility == value) return;
            this.Visibility = value;
            this.OnPropertyChanged(nameof(this.Visibility));//Notify 
        }


        /// <summary> <see cref = "ViewModel" />'s tag type. </summary>
        public TagType TagType { get; set; } = TagType.None;
        /// <summary> Sets the <see cref = "ViewModel.TagType" />. </summary>
        public void SetTagType(TagType value)
        {
            if (this.TagType == value) return;
            this.TagType = value;
            this.OnPropertyChanged(nameof(this.TagType));//Notify 
        }


        //////////////////////////


        /// <summary> <see cref = "ViewModel" />'s effect. </summary>
        public Effect Effect
        {
            get => this.effect;
            set
            {
                this.effect = value;
                this.OnPropertyChanged(nameof(this.Effect));//Notify 
            }
        }
        private Effect effect;


        /// <summary> <see cref = "ViewModel" />'s filter. </summary>
        public Filter Filter
        {
            get => this.filter;
            set
            {
                this.filter = value;
                this.OnPropertyChanged(nameof(this.Filter));//Notify 
            }
        }
        private Filter filter;


        //////////////////////////


        /// <summary> GroupLayer's Exist. </summary>     
        public bool IsGroupLayer
        {
            get => this.isGroupLayer;
            set
            {
                this.isGroupLayer = value;
                this.OnPropertyChanged(nameof(this.IsGroupLayer));//Notify 
            }
        }
        private bool isGroupLayer;
        /// <summary> Sets GroupLayer. </summary>     
        private void SetGroupLayer() => this.IsGroupLayer = false;
        /// <summary> Sets GroupLayer. </summary>     
        private void SetGroupLayer(ILayer layer) => this.IsGroupLayer = layer.Type == LayerType.Group;
        /// <summary> Sets GroupLayer. </summary>     
        private void SetGroupLayer(IEnumerable<Layerage> layerages) => this.IsGroupLayer = layerages.Any(layer => layer.Self.Type == LayerType.Group);


        /// <summary> ImageLayer's Exist. </summary>      
        public bool IsImageLayer
        {
            get => this.isImageLayer;
            set
            {
                this.isImageLayer = value;
                this.OnPropertyChanged(nameof(this.IsImageLayer));//Notify 
            }
        }
        private bool isImageLayer;
        /// <summary> <see cref = "ViewModel" />'s Photocopier. </summary>
        public Photocopier Photocopier
        {
            get => this.photocopier;
            set
            {
                this.photocopier = value;
                this.OnPropertyChanged(nameof(this.Photocopier));//Notify 
            }
        }
        private Photocopier photocopier;
        /// <summary> Sets ImageLayer. </summary>     
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


        /// <summary> <see cref = "ViewModel" />'s CurveLayer. </summary>
        public CurveLayer CurveLayer { get; set; }
        /// <summary> Sets CurveLayer. </summary>     
        private void SetCurveLayer(ILayer layer)
        {
            if (layer == null)
            {
                this.CurveLayer = null;
                return;
            }

            switch (layer.Type)
            {
                case LayerType.Curve:
                    this.CurveLayer = (CurveLayer)layer;
                    break;

                case LayerType.CurveMulti:
                    break;

                default:
                    this.CurveLayer = null;
                    break;
            }
        }


    }
}
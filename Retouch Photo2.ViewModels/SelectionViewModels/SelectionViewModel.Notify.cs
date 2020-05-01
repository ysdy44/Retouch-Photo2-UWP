using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Brushs.Models;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary> <see cref = "SelectionViewModel" />'s layer type. </summary>
        public LayerType Type { get; set; } = LayerType.None;


        /// <summary> <see cref = "SelectionViewModel" />'s opacity. </summary>
        public float Opacity { get; set; } = 1.0f;
        /// <summary> Sets the <see cref = "SelectionViewModel.Opacity" />. </summary>
        public void SetOpacity(float value)
        {
            if (this.Opacity == value) return;
            this.Opacity = value;
            this.OnPropertyChanged(nameof(this.Opacity));//Notify 
        }


        /// <summary> <see cref = "SelectionViewModel" />'s blend mode. </summary>
        public BlendEffectMode? BlendMode = null;
        /// <summary> Sets the <see cref = "SelectionViewModel.BlendMode" />. </summary>
        public void SetBlendMode(BlendEffectMode? value)
        {
            if (this.BlendMode == value) return;
            this.BlendMode = value;
            this.OnPropertyChanged(nameof(this.BlendMode));//Notify 
        }


        /// <summary> <see cref = "SelectionViewModel" />'s visibility. </summary>
        public Visibility Visibility { get; set; } = Visibility.Visible;
        /// <summary> Sets the <see cref = "SelectionViewModel.Visibility" />. </summary>
        public void SetVisibility(Visibility value)
        {
            if (this.Visibility == value) return;
            this.Visibility = value;
            this.OnPropertyChanged(nameof(this.Visibility));//Notify 
        }


        /// <summary> <see cref = "SelectionViewModel" />'s tag type. </summary>
        public TagType TagType { get; set; } = TagType.None;
        /// <summary> Sets the <see cref = "SelectionViewModel.TagType" />. </summary>
        public void SetTagType(TagType value)
        {
            if (this.TagType == value) return;
            this.TagType = value;
            this.OnPropertyChanged(nameof(this.TagType));//Notify 
        }


        //////////////////////////


        /// <summary> <see cref = "SelectionViewModel" />'s IsCrop. </summary>
        public bool IsCrop
        {
            get => this.isCrop;
            set
            {
                this.isCrop = value;
                this.OnPropertyChanged(nameof(this.IsCrop));//Notify 
            }
        }
        private bool isCrop;


        /// <summary> <see cref = "SelectionViewModel" />'s EffectManager. </summary>
        public EffectManager EffectManager
        {
            get => this.effectManager;
            set
            {
                this.effectManager = value;
                this.OnPropertyChanged(nameof(this.EffectManager));//Notify 
            }
        }
        private EffectManager effectManager;


        /// <summary> <see cref = "SelectionViewModel" />'s AdjustmentManager. </summary>
        public AdjustmentManager AdjustmentManager
        {
            get => this.adjustmentManager;
            set
            {
                this.adjustmentManager = value;
                this.OnPropertyChanged(nameof(this.AdjustmentManager));//Notify 
            }
        }
        private AdjustmentManager adjustmentManager;


        //////////////////////////


        /// <summary> GroupLayer's Exist. </summary>     
        public bool IsGroupLayer { get; set; }
        /// <summary> Sets GroupLayer. </summary>     
        private void SetGroupLayer(ILayer layer)
        {
            this.IsGroupLayer = layer is GroupLayer;
            this.OnPropertyChanged(nameof(this.IsGroupLayer));//Notify 
        }


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
        /// <summary> <see cref = "SelectionViewModel" />'s Photocopier. </summary>
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
            if (layer is ImageLayer imageLayer)
            {
                this.IsImageLayer = true;

                if (imageLayer.StyleManager.FillBrush is ImageBrush imageBrush)
                {
                    this.Photocopier = imageBrush.Photocopier;
                }
            }
            else
            {
                this.IsImageLayer = false;
            }
        }


        /// <summary> Sets PenTool nodes mode. </summary>     
        public bool IsPenToolNodesMode
        {
            get => this.isPenToolNodesMode;
            set
            {
                this.isPenToolNodesMode = value;
                this.OnPropertyChanged(nameof(this.IsPenToolNodesMode));//Notify 
            }
        }
        private bool isPenToolNodesMode;
        /// <summary> <see cref = "SelectionViewModel" />'s CurveLayer. </summary>
        public GeometryCurveLayer CurveLayer { get; set; }
        /// <summary> Sets CurveLayer. </summary>     
        private void SetCurveLayer(ILayer layer)
        {
            if (layer is GeometryCurveLayer curveLayer)
            {
                this.IsPenToolNodesMode = (curveLayer.Nodes.Count != 2);
                this.CurveLayer = curveLayer;
            }
            else
            {
                this.IsPenToolNodesMode = false;
                this.CurveLayer = null;
            }
        }


        /// <summary> Sets text FontFamily. </summary>     
        public string TextFontFamily
        {
            get => this.textFontFamily;
            set
            {
                this.textFontFamily = value;
                this.OnPropertyChanged(nameof(this.TextFontFamily));//Notify 
            }
        }
        private string textFontFamily = "Arial";

        /// <summary> Sets text FontSize. </summary>     
        public int TextFontSize
        {
            get => this.textFontSize;
            set
            {
                this.textFontSize = value;
                this.OnPropertyChanged(nameof(this.TextFontSize));//Notify 
            }
        }
        private int textFontSize = 22;

        /// <summary> <see cref = "SelectionViewModel" />'s FrameLayer. </summary>
        public TextFrameLayer TextFrameLayer { get; set; }
        /// <summary> Sets FrameLayer. </summary>     
        private void SetTextFrameLayer(ILayer layer)
        {
            if (layer is TextFrameLayer frameLayer)
            {
                this.TextFrameLayer = frameLayer;
            }
            else
            {
                this.TextFrameLayer = null;
            }
        }


    }
}
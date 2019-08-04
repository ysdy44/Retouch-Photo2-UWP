using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels.Selections
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary> <see cref = "SelectionViewModel" />'s opacity. </summary>
        public float Opacity;
        /// <summary> Sets the <see cref = "SelectionViewModel.Opacity" />. </summary>
        public void SetOpacity(float value)
        {
            if (this.Opacity == value) return;
            this.Opacity = value;
            this.OnPropertyChanged(nameof(this.Opacity));//Notify 
        }


        /// <summary> <see cref = "SelectionViewModel" />'s blend type. </summary>
        public BlendType BlendType;
        /// <summary> Sets the <see cref = "SelectionViewModel.BlendType" />. </summary>
        public void SetBlendType(BlendType value)
        {
            if (this.BlendType == value) return;
            this.BlendType = value;
            this.OnPropertyChanged(nameof(this.BlendType));//Notify 
        }


        /// <summary> <see cref = "SelectionViewModel" />'s visibility. </summary>
        public Visibility Visibility
        {
            get => this.visibility;
            set
            {
                this.visibility = value;
                this.OnPropertyChanged(nameof(this.Visibility));//Notify 
            }
        }
        private Visibility visibility;


        /// <summary> GroupLayer's Exist. </summary>     
        public bool IsGroupLayer;
        /// <summary> Sets GroupLayer. </summary>     
        private void SetGroupLayer(ILayer layer)
        {
            if (layer==null)
            {
                this.IsGroupLayer = false;
                this.OnPropertyChanged(nameof(this.IsGroupLayer));//Notify 
            }

            if (layer is GroupLayer acrylicLayer)
            {
                this.IsGroupLayer = true;
                this.OnPropertyChanged(nameof(this.IsGroupLayer));//Notify 
            }
        }


        /// <summary> <see cref = "SelectionViewModel" />'s Children. </summary>
        public ObservableCollection<ILayer> Children
        {
            get => this.children;
            set
            {
                this.children = value;
                this.OnPropertyChanged(nameof(this.Children));//Notify 
            }
        }
        private ObservableCollection<ILayer> children;

               
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


        /// <summary> AcrylicLayer's TintOpacity. </summary>     
        public float AcrylicTintOpacity = 0.5f;
        /// <summary> AcrylicLayer's BlurAmount. </summary>     
        public float AcrylicBlurAmount = 12.0f;
        /// <summary> Sets AcrylicLayer. </summary>     
        private void SetAcrylicLayer(ILayer layer)
        {
            if (layer is AcrylicLayer acrylicLayer)
            {
                this.AcrylicTintOpacity = acrylicLayer.TintOpacity;
                this.OnPropertyChanged(nameof(this.AcrylicTintOpacity));//Notify 

                this.AcrylicBlurAmount = acrylicLayer.BlurAmount;
                this.OnPropertyChanged(nameof(this.AcrylicBlurAmount));//Notify 

                return;
            }
        }

        
        /// <summary> <see cref = "SelectionViewModel" />'s ImageRe. </summary>
        public ImageRe ImageRe
        {
            get => this.imageRe;
            set
            {
                this.imageRe = value;
                this.OnPropertyChanged(nameof(this.ImageRe));//Notify 
            }
        }
        private ImageRe imageRe;
        /// <summary> Sets ImageLayer. </summary>     
        private void SetImageLayer(ILayer layer)
        {
            if (layer is ImageLayer imageLayer)
            {
                this.ImageRe = imageLayer.ImageRe;
            }
        }


    }
}
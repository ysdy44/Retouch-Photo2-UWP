using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace Retouch_Photo2.TestApp.ViewModels
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


        /// <summary> <see cref = "SelectionViewModel" />'s group layer. </summary>
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


        /// <summary> <see cref = "SelectionViewModel" />'s Children. </summary>
        public ObservableCollection<Layer> Children
        {
            get => this.children;
            set
            {
                this.children = value;
                this.OnPropertyChanged(nameof(this.Children));//Notify 
            }
        }
        private ObservableCollection<Layer> children;

               
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
    }
}
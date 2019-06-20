using Retouch_Photo2.Elements;
using System.ComponentModel;

namespace ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {



        /// <summary> State of DebugMenuLayout. </summary>
        public MenuLayoutState DebugMenuLayoutState
        {
            get => this.debugMenuLayoutState;
            set
            {
                if (this.debugMenuLayoutState == value) return;
                this.debugMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.DebugMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState debugMenuLayoutState;

        /// <summary> State of AdjustmentMenuLayout. </summary>
        public MenuLayoutState AdjustmentMenuLayoutState
        {
            get => this.adjustmentMenuLayoutState;
            set
            {
                if (this.adjustmentMenuLayoutState == value) return;
                this.adjustmentMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.AdjustmentMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState adjustmentMenuLayoutState;

        /// <summary> State of EffectMenuLayout. </summary>
        public MenuLayoutState EffectMenuLayoutState
        {
            get => this.effectMenuLayoutState;
            set
            {
                if (this.effectMenuLayoutState == value) return;
                this.effectMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.EffectMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState effectMenuLayoutState;

        /// <summary> State of TransformerMenuLayout. </summary>
        public MenuLayoutState TransformerMenuLayoutState
        {
            get => this.transformerMenuLayoutState;
            set
            {
                if (this.transformerMenuLayoutState == value) return;
                this.transformerMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.TransformerMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState transformerMenuLayoutState;

        /// <summary> State of LayerMenuLayout. </summary>
        public MenuLayoutState LayerMenuLayoutState
        {
            get => this.layerMenuLayoutState;
            set
            {
                if (this.layerMenuLayoutState == value) return;
                this.layerMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.LayerMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState layerMenuLayoutState;

        /// <summary> State of ColorMenuLayout. </summary>
        public MenuLayoutState ColorMenuLayoutState
        {
            get => this.colorMenuLayoutState;
            set
            {
                if (this.colorMenuLayoutState == value) return;
                this.colorMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.ColorMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState colorMenuLayoutState;






        /// <summary> IsOpen of the <see cref = "TipViewModel.LayerMenuLayoutState" />. </summary>
        public bool IsLayerMenuLayoutOpen
        {
            get => this.isLayerMenuLayoutOpen;
            set
            {
                this.isLayerMenuLayoutOpen = value;
                this.OnPropertyChanged(nameof(this.IsLayerMenuLayoutOpen));//Notify 
            }
        }
        private bool isLayerMenuLayoutOpen;
    }
}
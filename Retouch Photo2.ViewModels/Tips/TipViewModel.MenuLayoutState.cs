using Retouch_Photo2.Elements;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels.Tips
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {



        private void SetMenuLayoutStateIsOpen(bool isOpen)
        {

            //Operate
            if (this.OperateMenuLayoutState == Elements.MenuLayoutState.RootExpanded)
                this.IsOperateMenuLayoutOpen = isOpen;

            //Transformer
            if (this.TransformerMenuLayoutState == Elements.MenuLayoutState.RootExpanded)
                this.IsTransformerMenuLayoutOpen = isOpen;

            //Layer
            if (this.LayerMenuLayoutState == Elements.MenuLayoutState.RootExpanded)
                this.IsLayerMenuLayoutOpen = isOpen;
        }


        ///////////////////////////////////////////////////////


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

        /// <summary> State of SelectionMenuLayoutState. </summary>
        public MenuLayoutState SelectionMenuLayoutState
        {
            get => this.selectionMenuLayoutState;
            set
            {
                if (this.selectionMenuLayoutState == value) return;
                this.selectionMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.SelectionMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState selectionMenuLayoutState;

        /// <summary> State of OperateMenuLayoutState. </summary>
        public MenuLayoutState OperateMenuLayoutState
        {
            get => this.operateMenuLayoutState;
            set
            {
                if (this.operateMenuLayoutState == value) return;
                this.operateMenuLayoutState = value;
                this.OnPropertyChanged(nameof(this.OperateMenuLayoutState));//Notify 
            }
        }
        private MenuLayoutState operateMenuLayoutState;
        
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


        /////////////////////////////////////////////////////////////////////////////////////////


        /// <summary> IsOpen of the <see cref = "TipViewModel.OperateMenuLayoutState" />. </summary>
        public bool IsOperateMenuLayoutOpen
        {
            get => this.isOperateMenuLayoutOpen;
            set
            {
                this.isOperateMenuLayoutOpen = value;
                this.OnPropertyChanged(nameof(this.IsOperateMenuLayoutOpen));//Notify 
            }
        }
        private bool isOperateMenuLayoutOpen;


        /// <summary> IsOpen of the <see cref = "TipViewModel.TransformerMenuLayoutState" />. </summary>
        public bool IsTransformerMenuLayoutOpen
        {
            get => this.isTransformerMenuLayoutOpen;
            set
            {
                this.isTransformerMenuLayoutOpen = value;
                this.OnPropertyChanged(nameof(this.IsTransformerMenuLayoutOpen));//Notify 
            }
        }
        private bool isTransformerMenuLayoutOpen;


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
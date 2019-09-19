using Retouch_Photo2.Menus;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "TipViewModel" />.
    /// </summary>
    public partial class TipViewModel : INotifyPropertyChanged
    {

        /// <summary> Tool. </summary>
        public IMenu ToolMenu;
        /// <summary> Layer. </summary>
        public IMenu LayerMenu;

        /// <summary> Debug. </summary>
        public IMenu DebugMenu;

        /// <summary> Selection. </summary>
        public IMenu SelectionMenu;
        /// <summary> Operate. </summary>
        public IMenu OperateMenu;

        /// <summary> Adjustment. </summary>
        public IMenu AdjustmentMenu;
        /// <summary> Effect. </summary>
        public IMenu EffectMenu;
        /// <summary> Transformer. </summary>
        public IMenu TransformerMenu;

        /// <summary> Color. </summary>
        public IMenu ColorMenu;

    }
}
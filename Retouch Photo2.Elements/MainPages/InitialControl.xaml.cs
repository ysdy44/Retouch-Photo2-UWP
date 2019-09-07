using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> when there are no projects, display <see cref = "InitialControl" />.
    /// </summary>
    public sealed partial class InitialControl : UserControl
    {
        //@Content
        /// <summary> <see cref = "InitialControl" />'s AddButton. </summary>
        public Windows.UI.Xaml.Controls.Button AddButton => this._AddButton;
        /// <summary> <see cref = "InitialControl" />'s PhotoButton. </summary>
        public Windows.UI.Xaml.Controls.Button PhotoButton => this.__PhotoButton;
        /// <summary> <see cref = "InitialControl" />'s DestopButton. </summary>
        public Windows.UI.Xaml.Controls.Button DestopButton => this._DestopButton;
        
        //@Construct
        public InitialControl()
        {
            this.InitializeComponent();
        }
    }
}
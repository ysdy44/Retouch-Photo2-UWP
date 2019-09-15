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
        public Button AddButton => this._AddButton;
        /// <summary> <see cref = "InitialControl" />'s PhotoButton. </summary>
        public Button PhotoButton => this.__PhotoButton;
        /// <summary> <see cref = "InitialControl" />'s DestopButton. </summary>
        public Button DestopButton => this._DestopButton;
        
        //@Construct
        public InitialControl()
        {
            this.InitializeComponent();
        }
    }
}
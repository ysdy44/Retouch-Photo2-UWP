using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Retouch_Photo2 Tools 's more Button.
    /// </summary>
    public sealed partial class MoreToolButton : UserControl
    {
        //@Content
        /// <summary> Button. </summary>
        public Button Button { set => this._Button = value; get => this._Button; }
        /// <summary> StackPanel. </summary>
        public StackPanel StackPanel { set => this._StackPanel = value; get => this._StackPanel; }

        //@Construct
        public MoreToolButton()
        {
            this.InitializeComponent();
        }
    }
}

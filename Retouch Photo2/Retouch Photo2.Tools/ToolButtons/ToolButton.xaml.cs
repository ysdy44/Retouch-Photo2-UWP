using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Retouch_Photo2 Tools 's Button.
    /// </summary>
    public sealed partial class ToolButton : UserControl
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;

        //@Content
        /// <summary> Button's IsSelected. </summary>
        public bool IsSelected { get => !this.Button.IsEnabled; set => this.Button.IsEnabled = !value; }
        /// <summary> ContentPresenter's Content. </summary>
        public object CenterContent { set => this.Button.Content = value; get => this.Button.Content; }
        /// <summary> ToolTip. </summary>
        public ToolTip ToolTip => this._ToolTip;

        //@Construct
        public ToolButton()
        {
            this.InitializeComponent();
        }
        public ToolButton(object centerContent) : this()
        {
            this.Button.Content = centerContent;
        }

    }
}
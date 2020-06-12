using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Button of <see cref="ITool"/>.
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
        /// <summary>
        /// Initializes a ToolButton. 
        /// </summary>
        public ToolButton()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Initializes a ToolButton. 
        /// </summary>
        /// <param name="content"> The content. </param>
        public ToolButton(object content) : this()
        {
            this.Button.Content = content; 
        }
    }
}
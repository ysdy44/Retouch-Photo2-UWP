using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Selections
{
    /// <summary>
    /// Button of <see cref="Selection">.
    /// </summary>
    public sealed partial class Button : UserControl
    {

        //@Content
        /// <summary> Root button. </summary>
        public Windows.UI.Xaml.Controls.Button RootButton => this._RootButton;
        /// <summary> Enabled icon. </summary>
        public UIElement EnabledIcon { get => this.EnabledViewbox.Child; set => this.EnabledViewbox.Child = value; }
        /// <summary> Disabled icon. </summary>
        public UIElement DisabledIcon { get => this.DisabledViewbox.Child; set => this.DisabledViewbox.Child = value; }
        /// <summary> TextBlock' text. </summary>
        public string Label { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }


        //@Converter
        private Visibility BoolToVisibleConverter(bool isEnabled) => isEnabled ? Visibility.Visible : Visibility.Collapsed;
        private Visibility BoolToCollapsedConverter(bool isEnabled) => isEnabled ? Visibility.Collapsed : Visibility.Visible;


        //@Construct
        public Button()
        {
            this.InitializeComponent();
        }
    }
}
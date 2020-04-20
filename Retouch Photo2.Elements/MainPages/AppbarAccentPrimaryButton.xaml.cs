using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    public sealed partial class AppbarAccentPrimaryButton : UserControl
    {
        //@Content
        /// <summary> RootButton. </summary>
        public Button RootButton => this._RootButton;
        /// <summary> TextBlock's Text </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }

        //@Construct
        public AppbarAccentPrimaryButton()
        {
            this.InitializeComponent();
        }
    }
}
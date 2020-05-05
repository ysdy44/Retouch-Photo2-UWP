using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    public sealed partial class AppbarAccentButton : UserControl
    {
        //@Content
        /// <summary> RootButton. </summary>
        public Button RootButton => this._RootButton;
        /// <summary> TextBlock's Text </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        /// <summary> ContentPresenter's Content </summary>
        public object CenterContent { get => this.ContentPresenter.Content; set => this.ContentPresenter.Content = value; }


        //@Construct
        public AppbarAccentButton()
        {
            this.InitializeComponent();
        }
    }
}

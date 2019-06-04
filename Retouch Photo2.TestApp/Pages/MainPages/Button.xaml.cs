using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Pages.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "Button" />.
    /// </summary>
    public sealed partial class Button : UserControl
    {
        /// <summary> <see cref = "Windows.UI.Xaml.Controls.FontIcon" /> s Glyph. </summary>
        public string Glyph { get => this.FontIcon.Glyph; set => this.FontIcon.Glyph = value; }
        /// <summary> <see cref = "Windows.UI.Xaml.Controls.TextBlock" /> s Text. </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        /// <summary> <see cref = "Button" />'s RootButton. </summary>
        public Windows.UI.Xaml.Controls.Button RootButton { get => this._RootButton; set => this._RootButton = value; }

        public Button()
        {
            this.InitializeComponent();
        }
    }
}

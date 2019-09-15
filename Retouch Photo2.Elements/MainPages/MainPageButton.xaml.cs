using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "MainPageButton" />.
    /// </summary>
    public sealed partial class MainPageButton : UserControl
    {        
        /// <summary> <see cref = "MainPageButton" />'s RootButton. </summary>
        public Button RootButton => this._RootButton;
        /// <summary> <see cref = "Windows.UI.Xaml.Controls.FontIcon" /> s Glyph. </summary>
        public string Glyph { get => this.FontIcon.Glyph; set => this.FontIcon.Glyph = value; }
        /// <summary> <see cref = "Windows.UI.Xaml.Controls.TextBlock" /> s Text. </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }

        //@Construct
        public MainPageButton()
        {
            this.InitializeComponent();
        }
    }
}
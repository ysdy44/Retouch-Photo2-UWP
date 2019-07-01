using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Label control.
    /// </summary>
    public sealed partial class LabelControl : UserControl
    {
        //@Content
        public object ContentContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }
        private bool isChecked;
        public bool IsChecked
        {
            get => this.isChecked;
            set
            {
                this.SegmenteColor(this.ContentPresenter,value);
                this.isChecked = value;
            }
        }

        //@Construct
        public LabelControl()
        {
            this.InitializeComponent();
        }

        Thickness AccentStroke = new Thickness(1, 1, 1, 0);
        Thickness UnAccentStroke = new Thickness(0, 0, 0, 1);
        private void SegmenteColor(ContentPresenter control, bool IsChecked)
        {
            control.BorderThickness = IsChecked ? this.AccentStroke : this.UnAccentStroke;
            control.Background = IsChecked ? this.AccentColor : this.UnAccentColor;
            control.Foreground = IsChecked ? this.CheckColor : this.UnCheckColor;
        }
    }
}
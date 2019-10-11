using Retouch_Photo2.Tools.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryEllipseTool"/>.
    /// </summary>
    public sealed partial class GeometryEllipsePage : Page, IToolPage
    {
        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        //@Construct
        public GeometryEllipsePage()
        {
            this.InitializeComponent();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.StrokeWidthButton.ModeNone();
        }
    }
}
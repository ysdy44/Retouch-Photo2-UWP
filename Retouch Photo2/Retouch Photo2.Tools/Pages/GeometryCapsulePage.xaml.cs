using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryCapsuleTool"/>.
    /// </summary>
    public sealed partial class GeometryCapsulePage : Page, IToolPage
    {
        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        //@Construct
        public GeometryCapsulePage()
        {
            this.InitializeComponent();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}
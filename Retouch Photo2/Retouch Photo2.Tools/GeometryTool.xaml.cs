using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// <see cref="ITool"/>'s GeometryTool.
    /// </summary>
    public partial class GeometryTool : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Construct
        public GeometryTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.StrokeWidthButton.ModeNone();
        }
        
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.FillTextBlock.Text = resource.GetString("/ToolElements/Fill");
            this.StrokeTextBlock.Text = resource.GetString("/ToolElements/Stroke");
            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }

    }
}
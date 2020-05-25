using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
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
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Construct
        public GeometryTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.FillColorEllipse.Tapped += (s, e) =>
            {
                if (this.Parent is FrameworkElement placementTarget)
                {
                    DrawPage.FillColorShowAt(placementTarget);
                }
                else
                {
                    DrawPage.FillColorShowAt(this);
                }
            };


            this.StrokeColorEllipse.Tapped += (s, e) =>
            {
                if (this.Parent is FrameworkElement placementTarget)
                {
                    DrawPage.StrokeColorShowAt(placementTarget);
                }
                else
                {
                    DrawPage.StrokeColorShowAt(this);
                }
            };


            this.StrokeShowControl.Tapped += (s, e) =>
            {
                this.TipViewModel.ShowMenuLayoutAt(MenuType.Stroke, this.StrokeShowControl);
            };
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }

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
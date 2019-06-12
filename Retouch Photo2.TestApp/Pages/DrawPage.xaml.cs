using Retouch_Photo2.TestApp.Models;
using Retouch_Photo2.TestApp.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Elements;

namespace Retouch_Photo2.TestApp.Pages
{  
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        //@Construct
        public DrawPage()
        {
            this.InitializeComponent();


            //Transformer
            MenuLayout.LayoutBinging(this.TransformerMenuLayout, this.TransformerToggleButton);
            //Layer
            MenuLayout.LayoutBinging(this.LayerMenuLayout, this.LayerToggleButton);
            Layer.FlyoutShowAction = (layer, placementTarget) => MenuLayout.ShowFlyoutAt(this.LayerMenuLayout, placementTarget);
            //Color
            MenuLayout.TappedBinging(this.ColorMenuLayout, this.ColorButton); 
            this.ColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.ViewModel.FillColor = value;
                this.ViewModel.SelectionSetValue((layer) =>
                {
                    layer.SetFillColor(value);
                });
                
                this.ViewModel.Invalidate();//Invalidate
            };


            //Theme
            this.ThemeControl.ApplicationTheme = App.Current.RequestedTheme;
            this.BackButton.Tapped += (sender, e) => this.Frame.GoBack();
            this.SaveButton.Tapped += (sender, e) => this.Frame.GoBack();


            //FillColor
            this.ViewModel.FillColorFlyout = this.FillColorFlyout;
            this.ViewModel.FillColorPicker = this.FillColorPicker;
            this.FillColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.ViewModel.FillColor = value;
                this.ViewModel.SelectionSetValue((layer) =>
                {
                    layer.SetFillColor(value);
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            //StrokeColor
            this.ViewModel.StrokeColorFlyout = this.StrokeColorFlyout;
            this.ViewModel.StrokeColorPicker = this.StrokeColorPicker;
            this.StrokeColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.ViewModel.StrokeColor = value;
                this.ViewModel.SelectionSetValue((layer) =>
                {
                    layer.SetStrokeColor(value);
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            return;
            if (e.Parameter is Project project)
            {
                if (project == null)
                {
                    base.Frame.GoBack();
                    return;
                }

             //   this.Loaded += (sender, e2) =>
                //{

            this.LoadingControl.Visibility = Visibility.Visible;//Loading
            this.ViewModel.LoadFromProject(project);//Project
            this.LoadingControl.Visibility = Visibility.Collapsed;//Loading   

            this.ViewModel.Invalidate();
               // };
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }
        
    }
}

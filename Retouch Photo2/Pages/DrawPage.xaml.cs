using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Extensions;

namespace Retouch_Photo2.Pages
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => Retouch_Photo2.App.ViewModel;
        SelectionViewModel SelectionViewModel => Retouch_Photo2.App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => Retouch_Photo2.App.KeyboardViewModel;
        TipViewModel TipViewModel => Retouch_Photo2.App.TipViewModel;


        //@Construct
        public DrawPage()
        {
            this.InitializeComponent();

            //Theme
            this.BackButton.Tapped += (s, e) => this.Frame.GoBack();
            this.SaveButton.Tapped += (s, e) => this.Frame.GoBack();
                       
            //Color
            this.ColorButton.Tapped += (s, e) => this.TipViewModel.ColorMenuLayoutState = MenuLayoutButton.GetState(this.TipViewModel.ColorMenuLayoutState);
            this.ColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.FillColor = value;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.SetFillColor(value);
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

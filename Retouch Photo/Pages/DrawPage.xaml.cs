using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Pages
{
    public sealed partial class DrawPage : Page
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public DrawPage()
        {
            this.InitializeComponent();

            //Selection
            MenuLayout.LayoutBinging(this.SelectionLayout, this.SelectionToggleButton);
            //Operate
            MenuLayout.LayoutBinging(this.OperateLayout, this.OperateToggleButton);
            //Adjustment
            MenuLayout.LayoutBinging(this.AdjustmentLayout, this.AdjustmentToggleButton);
            //Effect
            MenuLayout.LayoutBinging(this.EffectLayout, this.EffectToggleButton);
            //Transformer
            MenuLayout.LayoutBinging(this.TransformerLayout, this.TransformerToggleButton);
            //Navigator
            MenuLayout.LayoutBinging(this.NavigatorLayout, this.NavigatorToggleButton);

            //Color
            MenuLayout.TappedBinging(this.ColorLayout, this.ColorButton);


            //Layer
            this.LayersControl.FlyoutShow += (control) =>
            MenuLayout.ShowFlyoutAt(this.LayerLayout, control);


            this.BackButton.Tapped += (sender, e) => this.Frame.GoBack();
            this.SaveButton.Tapped += (sender, e) => this.Frame.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)//当前页面成为活动页面
        {
            if (e.Parameter is Project project)
            {
                if (project == null)
                {
                    base.Frame.GoBack();
                    return;
                }

                this.Loaded += (sender, e2) =>
                {

                    this.LoadingControl.Visibility = Visibility.Visible;//Loading
                    this.ViewModel.LoadFromProject(project);//Project
                    this.LoadingControl.Visibility = Visibility.Collapsed;//Loading   

                    this.ViewModel.Invalidate();
                };
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)//当前页面不再成为活动页面
        {
        }

    }
}

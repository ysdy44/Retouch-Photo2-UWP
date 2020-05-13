using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel ;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private FrameworkElement IconConverter(ITool tool) => tool.Icon;
        private Visibility BoolToVisibilityConverter(bool boolean) => boolean ? Visibility.Visible : Visibility.Collapsed;

        //@Construct
        public DrawPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructTransition();
            this.ConstructMenus();            
            this.Loaded += (s, e) => this._lockLoaded();

            //Photos
            this.DrawLayout.RightAddButton.Tapped += (s, e) =>
            {
                e.Handled = true;
                this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.AddImageLayer);//Navigate   
            };
            this.DrawLayout.IsFullScreenChanged += (isFullScreen) =>
            {
                Vector2 offset = this.SettingViewModel.FullScreenOffset;

                if (isFullScreen)
                    this.ViewModel.CanvasTransformer.Position += offset;
                else
                    this.ViewModel.CanvasTransformer.Position -= offset;

                this.ViewModel.CanvasTransformer.ReloadMatrix();
            };
            Retouch_Photo2.Tools.Models.ImageTool.Select += () => this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.SelectImage);//Navigate   
            Retouch_Photo2.Tools.Models.ImageTool.Replace += () => this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.ReplaceImage);//Navigate   
            Retouch_Photo2.Tools.Models.BrushTool.FillImage += () => this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.FillBrushToImage);//Navigate   
            Retouch_Photo2.Tools.Models.BrushTool.StrokeImage += () => this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.StrokeBrushToImage);//Navigate   


            //FlyoutTool
            this.ConstructColorFlyout(); 
            Retouch_Photo2.Tools.Elements.MoreTransformButton.Flyout = this.MoreTransformFlyout;
            Retouch_Photo2.Tools.Elements.MoreCreateButton.Flyout = this.MoreCreateFlyout;


            //Document
            this.DocumentButton.Tapped += async (s, e) =>
            {
                await this.Save();
                this.SettingViewModel.IsFullScreen = true;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}

                await Task.Delay(400);
                this.Frame.GoBack();
            };
            this.DocumentButton.Holding += (s, e) => this.DocumentFlyout.ShowAt(this.DocumentButton);
            this.DocumentButton.RightTapped += (s, e) => this.DocumentFlyout.ShowAt(this.DocumentButton);
            this.DocumentUnSaveButton.Tapped += async (s, e) =>
            {
                this.DocumentFlyout.Hide();
                this.SettingViewModel.IsFullScreen = true;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}

                await Task.Delay(400);
                this.Frame.GoBack();
            };
 
            
            #region ExpandAppbar


            this.ConstructExportDialog();
            this.ExportButton.Tapped += (s, e) => this.ExportDialog.Show();

            //this.UndoButton.Tapped += (s, e) => { };
            //this.RedoButton.Tapped += (s, e) => { };

            this.ConstructSetupDialog();
            this.SetupButton.Tapped += (s, e) => this.SetupDialog.Show();
            

            this.UnFullScreenButton.Tapped += (s, e) => this.SettingViewModel.IsFullScreen = !this.SettingViewModel.IsFullScreen;
            this.FullScreenButton.Tapped += (s, e) => this.SettingViewModel.IsFullScreen = !this.SettingViewModel.IsFullScreen;


            #endregion
            
        }

        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is TransitionData data)
            {
                this._lockOnNavigatedTo(data);
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

    }
}
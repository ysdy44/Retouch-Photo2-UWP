using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Elements;
using Retouch_Photo2.ViewModels;
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
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Converter
        private FrameworkElement IconConverter(ITool tool) => tool.Icon;
                
        //@Construct
        public DrawPage()
        {
            this.InitializeComponent();
            this.ConstructTransition();
            this.ConstructToolAndMenu();

            this.Loaded += (s, e) => this._lockLoaded();

            //ViewModel
            this.ConstructViewModel();
            this.ConstructKeyboardViewModel();


            //ImageRes
            this.DrawLayout.RightAddButton.Tapped += (s, e) =>
            {
                e.Handled = true;
                this.Frame.Navigate(typeof(ImageResPage), ImageResPageMode.AddImageLayer);//Navigate   
            };
            Retouch_Photo2.Tools.Pages.ImagePage.Select += () => this.Frame.Navigate(typeof(ImageResPage), ImageResPageMode.ImageToolSelect);//Navigate   
            Retouch_Photo2.Tools.Pages.ImagePage.Replace += () => this.Frame.Navigate(typeof(ImageResPage), ImageResPageMode.ImageToolReplace);//Navigate   
            Retouch_Photo2.Tools.Pages.BrushPage.Image += () => this.Frame.Navigate(typeof(ImageResPage), ImageResPageMode.BrushToolImage);//Navigate   

            //MoreButton
            Retouch_Photo2.Tools.Elements.MoreTransformButton.Flyout = this.MoreTransformFlyout;
            Retouch_Photo2.Tools.Elements.MoreCreateButton.Flyout = this.MoreCreateFlyout;


            #region ExpandAppbar


            this.DocumentButton.Tapped += async (s, e) =>
            {
                await this.Save();
                await Task.Delay(400);
                this.Frame.GoBack();
            };


            this.ConstructExportDialog();
            this.ExportButton.Tapped += (s, e) => this.ExportDialog.Show();

            //this.UndoButton.Tapped += (s, e) => { };
            //this.RedoButton.Tapped += (s, e) => { };
            
            this.ConstructSetupDialog();
            this.SetupButton.Tapped += (s, e) => this.SetupDialog.Show();

            this.ThemeButton.Tapped += (s, e) =>
            {
                // Trigger switching theme.
                ElementTheme theme = this.ThemeControl.Theme;
                theme = (theme == ElementTheme.Dark) ? ElementTheme.Light : ElementTheme.Dark;

                this.ThemeControl.Theme = theme;
                ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);

                this.SettingViewModel.ElementTheme = theme;
                this.SettingViewModel.WriteToLocalFolder();//Write
            };


            this.UnFullScreenButton.Tapped += (s, e) => this.KeyboardViewModel.IsFullScreen = !this.KeyboardViewModel.IsFullScreen;
            this.FullScreenButton.Tapped += (s, e) => this.KeyboardViewModel.IsFullScreen = !this.KeyboardViewModel.IsFullScreen;


            #endregion

        }

        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Theme
            ElementTheme theme = this.SettingViewModel.ElementTheme;
            this.ThemeControl.Theme = theme;

            //SettingViewModel
            this.DrawLayout.VisualStateDeviceType = this.SettingViewModel.LayoutDeviceType;
            this.DrawLayout.VisualStatePhoneMaxWidth = this.SettingViewModel.LayoutPhoneMaxWidth;
            this.DrawLayout.VisualStatePadMaxWidth = this.SettingViewModel.LayoutPadMaxWidth;

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
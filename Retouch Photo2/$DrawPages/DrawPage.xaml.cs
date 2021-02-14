// Core:              ★★★★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Retouch_Photo2.Elements.DrawPages;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Represents a page used to draw vector graphics.
    /// </summary>
    public sealed partial class DrawPage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Static
        /// <summary> Navigate to <see cref="PhotosPage"/> </summary>
        public static Action<PhotosPageMode> FrameNavigatePhotosPage;
        /// <summary> Show <see cref="SetupDialog"/> </summary>
        public static Action ShowSetup;
        /// <summary> Show <see cref="ExportDialog"/> </summary>
        public static Action ShowExport;
        /// <summary> Show <see cref="RenameDialog"/> </summary>
        public static Action ShowRename;
        /// <summary> Show <see cref="DrawLayout.IsFullScreen"/> </summary>
        public static Action FullScreen;


        //@Construct
        /// <summary>
        /// Initializes a DrawPage. 
        /// </summary>
        public DrawPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.RegisteTransition();

            this.ConstructCanvasControl();
            this.ConstructAppBar();

            this.ConstructLayersControl();
            this.ConstructLayerManager();


            this.Loaded += (s, e) => this._lockLoaded();
            if (DrawPage.FrameNavigatePhotosPage == null) DrawPage.FrameNavigatePhotosPage += (mode) => this.Frame.Navigate(typeof(PhotosPage), mode);//Navigate   


            //DrawLayout
            this.DrawLayout.LeftIcon = ToolManager.IconBorder;
            this.DrawLayout.RightIcon = new Retouch_Photo2.Layers.Icon();
            this.DrawLayout.FootPage = ToolManager.PageBorder;
            this.DrawLayout.TouchbarPicker = TouchbarButton.PickerBorder;
            this.DrawLayout.TouchbarSlider = TouchbarButton.SliderBorder;
            this.DrawLayout.GalleryButton.Click += (s, e) => this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.AddImage);//Navigate   
      

            //FlyoutTool
            Retouch_Photo2.Tools.Elements.MoreTransformButton.Flyout = this.MoreTransformFlyout;
            Retouch_Photo2.Tools.Elements.MoreCreateButton.Flyout = this.MoreCreateFlyout;


            //Dialog
            if (DrawPage.ShowExport == null) DrawPage.ShowExport += () => this.ShowExportDialog();
            this.ConstructExportDialog();
            if (DrawPage.ShowSetup == null) DrawPage.ShowSetup += () => this.ShowSetupDialog();
            this.ConstructSetupDialog();
            if (DrawPage.ShowRename == null) DrawPage.ShowRename += () => this.ShowRenameDialog();
            this.ConstructRenameDialog();
            if (DrawPage.FullScreen == null) DrawPage.FullScreen += () => this.FullScreenLayout();
        }

        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Extension
            this.AVTBBE.Invalidate();

            //Key
            this.SettingViewModel.RegisteKey();

            if (this.DrawLayout.IsFullScreen == false) return;

            if (e.Parameter is Rect sourceRect)
            {
                this._lockOnNavigatedTo(sourceRect);
            }
            else
            {
                this._lockOnNavigatedTo(null);
            }
        }
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //Key
            this.SettingViewModel.UnRegisteKey();
        }

    }
}
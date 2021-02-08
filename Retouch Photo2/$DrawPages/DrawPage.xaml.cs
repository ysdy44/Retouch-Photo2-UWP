// Core:              ★★★★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Retouch_Photo2.Tools;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
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
        /// <summary> Show <see cref="RenameDialog"/> </summary>
        public static Action ShowRename;


        //@Converter
        private Visibility BoolToVisibilityConverter(bool boolean) => boolean ? Visibility.Visible : Visibility.Collapsed;


        //@Construct
        /// <summary>
        /// Initializes a DrawPage. 
        /// </summary>
        public DrawPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructTransition();

            this.ConstructMainCanvasControl();
            this.ConstructHeadBarControl();

            this.ConstructLayersControl();
            this.ConstructLayerageCollection();


            this.Loaded += (s, e) => this._lockLoaded();
            Retouch_Photo2.DrawPage.FrameNavigatePhotosPage += (mode) => this.Frame.Navigate(typeof(PhotosPage), mode);//Navigate   


            //DrawLayout
            this.DrawLayout.LeftIcon = ToolBase.IconBorder;
            this.DrawLayout.RightIcon = new Retouch_Photo2.Layers.Icon();
            this.DrawLayout.FootPage = ToolBase.PageBorder;
            this.DrawLayout.TouchbarPicker = TouchbarButton.PickerBorder;
            this.DrawLayout.TouchbarSlider = TouchbarButton.SliderBorder;
            this.DrawLayout.RightPhotosButton.Click += (s, e) => this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.AddImage);//Navigate   
            this.DrawLayout.IsFullScreenChanged += (isFullScreen) =>
            {
                Vector2 offset = this.SettingViewModel.FullScreenOffset;

                if (isFullScreen)
                    this.ViewModel.CanvasTransformer.Position += offset;
                else
                    this.ViewModel.CanvasTransformer.Position -= offset;

                this.ViewModel.CanvasTransformer.ReloadMatrix();
            };


            //FlyoutTool
            Retouch_Photo2.Tools.Elements.MoreTransformButton.Flyout = this.MoreTransformFlyout;
            Retouch_Photo2.Tools.Elements.MoreCreateButton.Flyout = this.MoreCreateFlyout;
            

            //Rename
            Retouch_Photo2.DrawPage.ShowRename += () => this.ShowRenameDialog();
            this.ConstructRenameDialog();
        }

        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Extension
            this.AVTBBE.Invalidate();

            //Key
            this.SettingViewModel.KeyIsEnabled = true;

            if (this.SettingViewModel.IsFullScreen == false) return;

            if (e.Parameter is TransitionData data)
            {
                this._lockOnNavigatedTo(data);
            }
        }
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //Key
            this.SettingViewModel.KeyIsEnabled = false;
        }

    }
}
// Core:              ★★★★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System;
using Windows.Foundation;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;
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
        /// <summary> Show <see cref="SetupDialog"/> </summary>
        public static Action ShowSetup;
        /// <summary> Show <see cref="ExportDialog"/> </summary>
        public static Action ShowExport;
        /// <summary> Show <see cref="RenameDialog"/> </summary>
        public static Action ShowRename;
        /// <summary> Show <see cref="DrawLayout.IsFullScreen"/> </summary>
        public static Action FullScreen;
        /// <summary> Show <see cref="GalleryDialog"/> </summary>
        public static Action<GalleryMode> ShowGallery;

        /// <summary> Show <see cref="FillFlyout"/> </summary>
        public static Action<FrameworkElement, FrameworkElement> ShowFill;
        /// <summary> Show <see cref="StrokeFlyout"/> </summary>
        public static Action<FrameworkElement, FrameworkElement> ShowStroke;

        /// <summary> Show <see cref="MoreTransformFlyout"/> </summary>
        public static Action<FrameworkElement, FrameworkElement> ShowMoreTransform;
        /// <summary> Show <see cref="MoreCreateFlyout"/> </summary>
        public static Action<FrameworkElement, FrameworkElement> ShowMoreCreate;


        //@Construct
        /// <summary>
        /// Initializes a DrawPage. 
        /// </summary>
        public DrawPage()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this._lockLoaded();
            this.ConstructFlowDirection();
            this.ConstructStrings();
            this.RegisteTransition();
            this.ConstructAppBar();

            this.ConstructInvalidateAction();
            this.ConstructCanvasControl();
            this.ConstructCanvasOperator();

            this.ConstructLayersControl();
            this.ConstructLayerManager();

            //Dialog
            this.ConstructExportDialog();
            this.ConstructSetupDialog();
            this.ConstructRenameDialog();

            //Gallery
            this.ConstructGallery();
            this.ConstructDragAndDrop();

            //DrawLayout
            this.DrawLayout.RightIcon = new Retouch_Photo2.Layers.Icon();
            this.DrawLayout.GalleryButton.Click += (s, e) => this.ShowGalleryDialog(GalleryMode.AddImage);
            this.DrawLayout.PCGalleryButton.Click += (s, e) => this.ShowGalleryDialog(GalleryMode.AddImage);

            //Flyout
            this.ConstructFillFlyout();
            this.ConstructStrokeFlyout();
        }

        private void RegisterDrawPage()
        {
            //Dialog
            DrawPage.ShowExport = this.ShowExportDialog;
            DrawPage.ShowSetup = this.ShowSetupDialog;
            DrawPage.ShowRename = this.ShowRenameDialog;
            DrawPage.FullScreen = this.FullScreenChanged;
            //Gallery
            DrawPage.ShowGallery = this.ShowGalleryDialog;

            //DrawLayout
            this.DrawLayout.TouchbarPicker = TouchbarButton.PickerBorder;
            this.DrawLayout.TouchbarSlider = TouchbarButton.SliderBorder;

            //Flyout
            DrawPage.ShowFill = this.ShowFillFlyout;
            DrawPage.ShowStroke = this.ShowStrokeFlyout;
            //More
            DrawPage.ShowMoreTransform = this.ShowMoreTransformFlyout;
            DrawPage.ShowMoreCreate = this.ShowMoreCreateFlyout;
        }
        private void UnregisterDrawPage()
        {
            //Dialog
            DrawPage.ShowExport = null;
            DrawPage.ShowSetup = null;
            DrawPage.ShowRename = null;
            DrawPage.FullScreen = null;
            //Gallery
            DrawPage.ShowGallery = null;

            //DrawLayout
            this.DrawLayout.TouchbarPicker = null;
            this.DrawLayout.TouchbarSlider = null;

            //Flyout
            DrawPage.ShowFill = null;
            DrawPage.ShowStroke = null;
            //More
            DrawPage.ShowMoreTransform = null;
            DrawPage.ShowMoreCreate = null;
        }

    }


    public sealed partial class DrawPage : Page
    {

        //@BackRequested
        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.RegisterDrawPage();
            this.RegisterGallery();
            
            //Extension
            this.ApplicationView.Color = this.ApplicationView.Color;

            //Key
            this.SettingViewModel.RegisteKey();

            if (this.DrawLayout.IsFullScreen == false) return;

            if (e.Parameter is IProjectViewItem item)
            {
                //Name
                this.ApplicationView.Title = item.Name;

                //Project
                this.ViewModel.LoadFromProject(item.Project);

                //ImageVisualRect
                if (item.ImageVisualRect != Rect.Empty)
                {
                    this._lockOnNavigatedTo(item.ImageVisualRect);
                    this.SelectionViewModel.SetMode();
                }
                else
                {
                    this._lockOnNavigatedTo(null);
                }
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
        }
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.UnregisterDrawPage();
            this.UnregisterGallery();

            //Extension
            this.ApplicationView.Title = string.Empty;

            //Key
            this.SettingViewModel.UnRegisteKey();

            SystemNavigationManager.GetForCurrentView().BackRequested -= BackRequested;
        }
        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (BackRequestedExtension.DialogIsShow) return;
            if (BackRequestedExtension.LayoutIsShow) return;

            e.Handled = true;
            this.DocumentUnSave();
        }

    }

}
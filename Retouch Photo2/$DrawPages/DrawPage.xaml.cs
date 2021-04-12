// Core:              ★★★★★
// Referenced:   ★
// Difficult:         ★★★★
// Only:              ★★★★★
// Complete:      ★★★★★
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Adjustments.Pages;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Photos;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Globalization;
using Windows.UI.Core;
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
        IList<IProjectViewItem> Items => App.Projects;
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Static
        /// <summary> Show <see cref="OperateFlyout"/> </summary>
        public static Action<FrameworkElement> ShowOperateFlyout;
        /// <summary> Show <see cref="TextFlyout"/> </summary>
        public static Action<FrameworkElement> ShowTextFlyout;
        /// <summary> Show <see cref="StrokeFlyout"/> </summary>
        public static Action<FrameworkElement> ShowStrokeFlyout;
        /// <summary> Show <see cref="LayerFlyout"/> </summary>
        public static Action<FrameworkElement> ShowLayerFlyout;


        /// <summary> Show <see cref="SetupDialog"/> </summary>
        public static Action ShowSetup;
        /// <summary> Show <see cref="ExportDialog"/> </summary>
        public static Action ShowExport;
        /// <summary> Show <see cref="RenameDialog"/> </summary>
        public static Action ShowRename;
        /// <summary> Show <see cref="DrawLayout.IsFullScreen"/> </summary>
        public static Action FullScreen;
        /// <summary> Show <see cref="GalleryDialog"/> </summary>
        public static Action ShowGallery;


        /// <summary> Show <see cref="GalleryDialog"/> </summary>
        public static Func<Task<Photo>> ShowGalleryFunc;

        /// <summary> Show <see cref="FillColorFlyout"/> </summary>
        public static Action<FrameworkElement, FrameworkElement> ShowFillColorFlyout;
        /// <summary> Show <see cref="StrokeColorFlyout"/> </summary>
        public static Action<FrameworkElement, FrameworkElement> ShowStrokeColorFlyout;

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

            //CanvasControl
            this.ConstructCanvasControl();
            this.ConstructCanvasOperator();

            //ToolsControl
            this.ToolsControl.AssemblyType = typeof(Retouch_Photo2.Tools.Models.CursorTool);

            //LayerManager
            this.ConstructLayerManager();

            //Menus
            this.EffectMenu.PaneOpened += (s, e) => this.DrawLayout.IsFullScreen = false;
            this.EffectMenu.PaneClosed += (s, e) => this.DrawLayout.IsFullScreen = true;

            //Dialog
            this.ConstructExportDialog();
            this.ConstructSetupDialog();
            this.ConstructRenameDialog();
            this.ConstructGalleryDialog();

            //DrawLayout
            this.DrawLayout.RightIcon = new Retouch_Photo2.Layers.Icon();

            //Gallery
            this.DrawLayout.GalleryButtonClick += (s, e) => this.ShowGalleryDialog();
            this.ConstructDragAndDrop();

            //Flyout
            this.ConstructFillColorFlyout();
            this.ConstructStrokeColorFlyout();

            //Writable
            this.DrawLayout.WritableCancelButtonClick += (s, e) => this.DrawLayout.Hide();
        }

        private void RegisterDrawPage()
        {
            //CanvasControl
            this.ViewModel.InvalidateAction += this.CanvasControlInvalidate;

            //LayerManager
            this.LayersScrollViewer.Content = LayerManager.RootStackPanel;
            LayerManager.ItemClick += this.LayerItemClick;
            LayerManager.RightTapped += this.LayerRightTapped;
            LayerManager.VisibilityChanged += this.LayerVisibilityChanged;
            LayerManager.IsExpandChanged += this.LayerIsExpandChanged;
            LayerManager.IsSelectedChanged += this.LayerIsSelectedChanged;
            LayerManager.DragItemsStarted += this.LayerDragItemsStarted;
            LayerManager.DragItemsDelta += this.LayerDragItemsDelta;
            LayerManager.DragItemsCompleted += this.LayerDragItemsCompleted;

            //Menu
            DrawPage.ShowOperateFlyout += this.OperateExpander.FlyoutShowAt;
            DrawPage.ShowTextFlyout += this.TextExpander.FlyoutShowAt;
            DrawPage.ShowStrokeFlyout += this.StrokeExpander.FlyoutShowAt;
            DrawPage.ShowLayerFlyout += this.LayerExpander.FlyoutShowAt;

            //Dialog
            DrawPage.ShowExport += this.ShowExportDialog;
            DrawPage.ShowSetup += this.ShowSetupDialog;
            DrawPage.ShowRename += this.ShowRenameDialog;
            DrawPage.FullScreen += this.FullScreenChanged;
            DrawPage.ShowGallery += this.ShowGalleryDialog;

            //DrawLayout
            TouchbarButton.PickerBorder = this.DrawLayout.TouchbarPicker;
            TouchbarButton.SliderBorder = this.DrawLayout.TouchbarSlider;

            //Gallery
            DrawPage.ShowGalleryFunc += this.ShowGalleryDialogFunc;
            Photo.FlyoutShow += this.PhotoFlyoutShow;
            Photo.ItemClick += this.PhotoItemClick;

            //Flyout
            DrawPage.ShowFillColorFlyout += this.ShowFillColorFlyout2;
            DrawPage.ShowStrokeColorFlyout += this.ShowStrokeColorFlyout2;
            //More
            DrawPage.ShowMoreTransform += this.ShowMoreTransformFlyout;
            DrawPage.ShowMoreCreate += this.ShowMoreCreateFlyout;

            //Writable
            AdjustmentCommand.Edit += this.AdjustmentMenuEdit;
            AdjustmentCommand.Remove += this.AdjustmentMenu.Remove;
        }
        private void UnregisterDrawPage()
        {
            //CanvasControl
            this.ViewModel.InvalidateAction -= this.CanvasControlInvalidate;

            //LayerManager
            this.LayersScrollViewer.Content = null;
            LayerManager.ItemClick -= this.LayerItemClick;
            LayerManager.RightTapped -= this.LayerRightTapped;
            LayerManager.VisibilityChanged -= this.LayerVisibilityChanged;
            LayerManager.IsExpandChanged -= this.LayerIsExpandChanged;
            LayerManager.IsSelectedChanged -= this.LayerIsSelectedChanged;
            LayerManager.DragItemsStarted -= this.LayerDragItemsStarted;
            LayerManager.DragItemsDelta -= this.LayerDragItemsDelta;
            LayerManager.DragItemsCompleted -= this.LayerDragItemsCompleted;

            //Menu
            DrawPage.ShowOperateFlyout -= this.OperateExpander.FlyoutShowAt;
            DrawPage.ShowTextFlyout -= this.TextExpander.FlyoutShowAt;
            DrawPage.ShowStrokeFlyout -= this.StrokeExpander.FlyoutShowAt;
            DrawPage.ShowLayerFlyout -= this.LayerExpander.FlyoutShowAt;

            //Dialog
            DrawPage.ShowExport -= this.ShowExportDialog;
            DrawPage.ShowSetup -= this.ShowSetupDialog;
            DrawPage.ShowRename -= this.ShowRenameDialog;
            DrawPage.FullScreen -= this.FullScreenChanged;
            DrawPage.ShowGallery -= this.ShowGalleryDialog;

            //DrawLayout
            TouchbarButton.PickerBorder = null;
            TouchbarButton.SliderBorder = null;

            //Gallery
            DrawPage.ShowGalleryFunc = null;
            Photo.FlyoutShow -= this.PhotoFlyoutShow;
            Photo.ItemClick -= this.PhotoItemClick;

            //Flyout
            DrawPage.ShowFillColorFlyout -= this.ShowFillColorFlyout2;
            DrawPage.ShowStrokeColorFlyout -= this.ShowStrokeColorFlyout2;
            //More
            DrawPage.ShowMoreTransform -= this.ShowMoreTransformFlyout;
            DrawPage.ShowMoreCreate -= this.ShowMoreCreateFlyout;

            //Writable
            AdjustmentCommand.Edit -= this.AdjustmentMenuEdit;
            AdjustmentCommand.Remove -= this.AdjustmentMenu.Remove;
        }


        /// <summary>
        /// Edit the adjustment.
        /// </summary>
        /// <param name="adjustment"> The adjustment. </param>
        public void AdjustmentMenuEdit(IAdjustment adjustment)
        {
            if (adjustment == null)
            {
                this.DrawLayout.Hide();
                return;
            }
            if (adjustment.PageVisibility == Visibility.Collapsed)
            {
                this.DrawLayout.Hide();
                return;
            }

            {
                this.AdjustmentFlyout.Hide();
                this.LayerFlyout.Hide();
            }

            IAdjustmentPage adjustmentPage = Retouch_Photo2.Adjustments.XML.CreateAdjustmentPage(typeof(BrightnessPage), adjustment.Type);

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                int index = layer.Filter.Adjustments.IndexOf(adjustment);
                adjustmentPage.Index = index;
                adjustmentPage.Follow();
            }

            this.DrawLayout.ShowWritable(adjustmentPage.Icon, adjustmentPage.Title, adjustmentPage.Self);//Delegat
        }
    }


    public sealed partial class DrawPage : Page
    {

        //@BackRequested
        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (string.IsNullOrEmpty(ApplicationLanguages.PrimaryLanguageOverride) == false)
            {
                if (ApplicationLanguages.PrimaryLanguageOverride != this.Language)
                {
                    this.ConstructFlowDirection();
                    this.ConstructStrings();
                }
            }

            this.ConstructMenuTypes(this.SettingViewModel.Setting.MenuTypes);

            this.RegisterDrawPage();

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

            //Extension
            this.ApplicationView.Title = string.Empty;

            //Key
            this.SettingViewModel.UnregisteKey();

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
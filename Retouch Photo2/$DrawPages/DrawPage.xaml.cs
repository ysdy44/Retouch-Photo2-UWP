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
        /// <summary> Show <see cref="DrawLayout.SetupDialog"/> </summary>
        public static Action ShowSetup;
        /// <summary> Show <see cref="DrawLayout.ExportDialog"/> </summary>
        public static Action ShowExport;
        /// <summary> Show <see cref="DrawLayout.RenameDialog"/> </summary>
        public static Action ShowRename;
        /// <summary> Show <see cref="DrawLayout.IsFullScreen"/> </summary>
        public static Action FullScreen;
        /// <summary> Show <see cref="DrawLayout.GalleryDialog"/> </summary>
        public static Action ShowGallery;


        /// <summary> Show <see cref="DrawLayout.GalleryDialog"/> </summary>
        public static Func<Task<Photo>> ShowGalleryFunc;
        /// <summary> Show <see cref="DrawLayout.RenameDialog"/> </summary>
        public static Func<string, Task<string>> ShowRenameFunc;

        /// <summary> Show <see cref="DrawLayout.FillColorFlyout"/> </summary>
        public static Action<FrameworkElement, FrameworkElement> ShowFillColorFlyout;
        /// <summary> Show <see cref="DrawLayout.StrokeColorFlyout"/> </summary>
        public static Action<FrameworkElement, FrameworkElement> ShowStrokeColorFlyout;

        /// <summary> Show <see cref="DrawLayout.MoreFlyout"/> </summary>
        public static Action<FrameworkElement> ShowMoreFlyout;


        //@Construct
        /// <summary>
        /// Initializes a DrawPage. 
        /// </summary>
        public DrawPage()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.TransitionStaring();
            this.ConstructFlowDirection();
            this.ConstructStrings();
            this.RegisteTransition();
            this.ConstructAppBar();

            // CanvasControl
            this.ConstructCanvasControl();
            this.ConstructCanvasOperator();

            // ToolTypeComboBox
            this.ToolTypeComboBox.AssemblyType = typeof(Retouch_Photo2.Tools.Models.CursorTool);
            this.ToolTypeComboBox.Closed += (s, e) => this.SettingViewModel.RegisteKey(); // Setting
            this.ToolTypeComboBox.Opened += (s, e) => this.SettingViewModel.UnregisteKey(); // Setting

            // LayerManager
            this.ConstructLayerManager();

            // Menus
            this.ConstructMenus();
            this.EffectMenu.PaneOpened += (s, e) => this.DrawLayout.IsFullScreen = false;
            this.EffectMenu.PaneClosed += (s, e) => this.DrawLayout.IsFullScreen = true;

            // Dialog
            this.ConstructExportDialog();
            this.ConstructSetupDialog();
            this.ConstructRenameDialog();
            this.ConstructGalleryDialog();

            // DrawLayout
            this.DrawLayout.RightIcon = new Retouch_Photo2.Layers.Icon();

            // Gallery
            this.DrawLayout.GalleryButtonTapped += (s, e) => this.ShowGalleryDialog();
            this.ConstructDragAndDrop();

            // Flyout
            this.ConstructFillColorFlyout();
            this.ConstructStrokeColorFlyout();

            // More
            this.ConstructMore();

            // Writable
            this.DrawLayout.WritableOKButtonClick += (s, e) => this.DrawLayout.Hide();
        }

        private void UnregisterDrawPage()
        {
            // CanvasControl
            this.ViewModel.InvalidateAction -= this.CanvasControlInvalidate;

            // LayerManager
            this.LayersScrollViewer.Content = null;
            LayerManager.ItemClick -= this.LayerItemClick;
            LayerManager.RightTapped -= this.LayerRightTapped;
            LayerManager.VisibilityChanged -= this.LayerVisibilityChanged;
            LayerManager.IsExpandChanged -= this.LayerIsExpandChanged;
            LayerManager.IsSelectedChanged -= this.LayerIsSelectedChanged;
            LayerManager.DragItemsStarted -= this.LayerDragItemsStarted;
            LayerManager.DragItemsDelta -= this.LayerDragItemsDelta;
            LayerManager.DragItemsCompleted -= this.LayerDragItemsCompleted;

            // DrawLayout
            TouchbarExtension.PickerBorder = null;
            TouchbarExtension.SliderBorder = null;

            // Dialog
            DrawPage.ShowExport -= this.ShowExportDialog;
            DrawPage.ShowSetup -= this.ShowSetupDialog;
            DrawPage.ShowRename -= this.ShowRenameDialog;
            DrawPage.FullScreen -= this.FullScreenChanged;
            DrawPage.ShowGallery -= this.ShowGalleryDialog;

            // Gallery
            this.GalleryGridView.ItemsSource = null;
            DrawPage.ShowGalleryFunc -= this.ShowGalleryDialogTask;
            Photo.ItemClick -= this.GalleryDialogTrySetResult;
            Photo.FlyoutShow -= this.BillboardCanvas.Show;

            // Rename
            DrawPage.ShowRenameFunc -= this.ShowRenameDialogTask;

            // Color
            DrawPage.ShowFillColorFlyout -= this.ShowFillColorFlyout2;
            DrawPage.ShowStrokeColorFlyout -= this.ShowStrokeColorFlyout2;

            // More
            DrawPage.ShowMoreFlyout -= this.MoreFlyout.ShowAt;

            // Writable
            AdjustmentCommand.Edit -= this.AdjustmentMenuEdit;
            AdjustmentCommand.Remove -= this.AdjustmentMenu.Remove;
        }
        private void RegisterDrawPage()
        {
            // CanvasControl
            this.ViewModel.InvalidateAction += this.CanvasControlInvalidate;

            // LayerManager
            this.LayersScrollViewer.Content = LayerManager.RootStackPanel;
            LayerManager.ItemClick += this.LayerItemClick;
            LayerManager.RightTapped += this.LayerRightTapped;
            LayerManager.VisibilityChanged += this.LayerVisibilityChanged;
            LayerManager.IsExpandChanged += this.LayerIsExpandChanged;
            LayerManager.IsSelectedChanged += this.LayerIsSelectedChanged;
            LayerManager.DragItemsStarted += this.LayerDragItemsStarted;
            LayerManager.DragItemsDelta += this.LayerDragItemsDelta;
            LayerManager.DragItemsCompleted += this.LayerDragItemsCompleted;

            // DrawLayout
            TouchbarExtension.PickerBorder = this.DrawLayout.TouchbarPicker;
            TouchbarExtension.SliderBorder = this.DrawLayout.TouchbarSlider;

            // Dialog
            DrawPage.ShowExport += this.ShowExportDialog;
            DrawPage.ShowSetup += this.ShowSetupDialog;
            DrawPage.ShowRename += this.ShowRenameDialog;
            DrawPage.FullScreen += this.FullScreenChanged;
            DrawPage.ShowGallery += this.ShowGalleryDialog;

            // Gallery
            this.GalleryGridView.ItemsSource = Photo.InstancesCollection;
            DrawPage.ShowGalleryFunc += this.ShowGalleryDialogTask;
            Photo.ItemClick += this.GalleryDialogTrySetResult;
            Photo.FlyoutShow += this.BillboardCanvas.Show;

            // Rename
            DrawPage.ShowRenameFunc += this.ShowRenameDialogTask;

            // Color
            DrawPage.ShowFillColorFlyout += this.ShowFillColorFlyout2;
            DrawPage.ShowStrokeColorFlyout += this.ShowStrokeColorFlyout2;

            // More
            DrawPage.ShowMoreFlyout += this.MoreFlyout.ShowAt;

            // Writable
            AdjustmentCommand.Edit += this.AdjustmentMenuEdit;
            AdjustmentCommand.Remove += this.AdjustmentMenu.Remove;
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

            this.DrawLayout.ShowWritable(adjustmentPage.Icon, adjustmentPage.Title, adjustmentPage.Self); // Delegat
        }
    }


    public sealed partial class DrawPage : Page
    {

        //@BackRequested
        /// <summary> The current page no longer becomes an active page. </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.UnregisterDrawPage();

            // Extension
            this.ApplicationView.Title = string.Empty;

            // Key
            this.SettingViewModel.UnregisteKey();

            SystemNavigationManager.GetForCurrentView().BackRequested -= this.BackRequested;
        }
        /// <summary> The current page becomes the active page. </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ConstructLanguages();

            this.ConstructMenuTypes(this.SettingViewModel.Setting.MenuTypes);

            this.RegisterDrawPage();

            // Extension
            this.ApplicationView.IsAccent = false;

            // Key
            this.SettingViewModel.RegisteKey();

            if (this.DrawLayout.IsFullScreen == false) return;

            if (e.Parameter is IProjectViewItem item)
            {
                // Name
                this.ApplicationView.Title = item.Name;

                // Project
                this.ViewModel.LoadFromProject(item.Project);

                // ImageVisualRect
                if (item.ImageVisualRect != Rect.Empty)
                {
                    this._lockSourceRect = item.ImageVisualRect;
                    this.SelectionViewModel.SetMode();
                }
                else
                {
                    this._lockSourceRect = null;
                }
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += this.BackRequested;
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
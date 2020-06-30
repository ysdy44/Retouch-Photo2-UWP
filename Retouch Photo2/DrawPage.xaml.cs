using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools;
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
        SettingViewModel SettingViewModel => App.SettingViewModel ;
        TipViewModel TipViewModel => App.TipViewModel;

        //@Static
        /// <summary> Navigate to <see cref="PhotosPage"/> </summary>
        public static Action<PhotosPageMode> FrameNavigatePhotosPage;
        /// <summary> Show <see cref="RenameDialog"/> </summary>
        public static Action ShowRename;

        //@Converter
        private FrameworkElement IconConverter(ITool tool) => tool.Icon;
        private FrameworkElement PageConverter(ITool tool) => tool.Page;
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

            this.Loaded += (s, e) => this._lockLoaded();
            Retouch_Photo2.DrawPage.FrameNavigatePhotosPage += (mode) => this.Frame.Navigate(typeof(PhotosPage), mode);//Navigate   


            //DrawLayout
            this.DrawLayout.RightPhotosButton.Click += (s, e) => this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.AddImager);//Navigate   
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
            this.ConstructColorFlyout(); 
            Retouch_Photo2.Tools.Elements.MoreTransformButton.Flyout = this.MoreTransformFlyout;
            Retouch_Photo2.Tools.Elements.MoreCreateButton.Flyout = this.MoreCreateFlyout;
            

            //Rename
            Retouch_Photo2.DrawPage.ShowRename += () => this.ShowRenameDialog();
            this.ConstructRenameDialog();


            #region Document


            this.HeadBarControl.DocumentButton.Click += async (s, e) =>
            {
                this.LoadingControl.State = LoadingState.Saving;
                this.LoadingControl.IsActive = true;

                int countHistorys = this.ViewModel.Historys.Count;
                int countLayerages = this.ViewModel.LayerageCollection.RootLayerages.Count;

                if (countHistorys == 0 && countLayerages > 1)
                {
                    this.ViewModel.IsUpdateThumbnailByName = false;

                    await this.Exit();
                    this.SettingViewModel.IsFullScreen = true;
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                    this.LoadingControl.State = LoadingState.None;
                    this.LoadingControl.IsActive = false;
                    this.Frame.GoBack();
                }
                else
                {
                    await this.Save();
                    this.ViewModel.IsUpdateThumbnailByName = true;

                    await this.Exit();
                    this.SettingViewModel.IsFullScreen = true;
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}

                    this.LoadingControl.State = LoadingState.None;
                    this.LoadingControl.IsActive = false;
                    this.Frame.GoBack();
                }
            };
            this.HeadBarControl.DocumentUnSaveButton.Click += async (s, e) =>
            {
                this.LoadingControl.State = LoadingState.Saving;
                this.LoadingControl.IsActive = true;

                this.HeadBarControl.DocumentFlyout.Hide();
                this.ViewModel.IsUpdateThumbnailByName = false;

                await this.Exit();
                this.SettingViewModel.IsFullScreen = true;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.LoadingControl.State = LoadingState.None;
                this.LoadingControl.IsActive = false;
                this.Frame.GoBack();
            };


            #endregion


            #region ExpandAppbar


            this.ConstructExportDialog();
            this.HeadBarControl.ExportButton.Tapped += (s, e) => this.ShowExportDialog();

            this.HeadBarControl.UndoButton.Tapped += (s, e) => this.MethodViewModel.MethodEditUndo();
            //this.RedoButton.Click += (s, e) => { };

            this.ConstructSetupDialog();
            this.HeadBarControl.SetupButton.Tapped += (s, e) => this.ShowSetupDialog();
            
            this.HeadBarControl.RulerButton.Tapped += (s, e) => this.ViewModel.Invalidate();//Invalidate

            this.UnFullScreenButton.Click += (s, e) => this.SettingViewModel.IsFullScreen = false;
            this.HeadBarControl.FullScreenButton.Tapped += (s, e) => this.SettingViewModel.IsFullScreen = true;


            #endregion

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
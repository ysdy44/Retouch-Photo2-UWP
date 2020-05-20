using Retouch_Photo2.Layers;
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
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel ;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Static
        /// <summary> Navigate to <see cref="PhotosPage"/> </summary>
        public static Action<PhotosPageMode> FrameNavigatePhotosPage;
        /// <summary> Show <see cref="RenameDialog"/> </summary>
        public static Action ShowRename;

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
            Retouch_Photo2.DrawPage.FrameNavigatePhotosPage += (mode) => this.Frame.Navigate(typeof(PhotosPage), mode);//Navigate   


            //DrawLayout
            this.DrawLayout.RightAddButton.Click += (s, e) => this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.AddImager);//Navigate   
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
                await this.Save();
                await this.Exit();
                this.ViewModel.IsUpdateThumbnailByName = true;

                this.SettingViewModel.IsFullScreen = true;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}
                this.Frame.GoBack();
            };
            this.HeadBarControl.DocumentUnSaveButton.Click += async (s, e) =>
            {
                this.HeadBarControl.DocumentFlyout.Hide();
                await this.Exit();
                this.ViewModel.IsUpdateThumbnailByName = false;

                this.SettingViewModel.IsFullScreen = true;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}
                this.Frame.GoBack();
            };


            #endregion


            #region ExpandAppbar


            this.ConstructExportDialog();
            this.HeadBarControl.ExportButton.Tapped += (s, e) => this.ShowExportDialog();

            this.HeadBarControl.UndoButton.Tapped += (s, e) =>
            {
                bool isUndo = this.ViewModel.Undo();//History

                if (isUndo)
                {
                    this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection          

                    LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                    this.ViewModel.Invalidate();//Invalidate
                }
            };
            //this.RedoButton.Click += (s, e) => { };

            this.ConstructSetupDialog();
            this.HeadBarControl.SetupButton.Tapped += (s, e) => this.ShowSetupDialog();


            this.UnFullScreenButton.Click += (s, e) => this.SettingViewModel.IsFullScreen = !this.SettingViewModel.IsFullScreen;
            this.HeadBarControl.FullScreenButton.Tapped += (s, e) => this.SettingViewModel.IsFullScreen = !this.SettingViewModel.IsFullScreen;


            #endregion

        }

        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (this.SettingViewModel.IsFullScreen == false) return;

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
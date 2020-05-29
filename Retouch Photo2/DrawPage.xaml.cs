using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System;
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
        public DrawPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructTransition();

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


            async Task goBack()
            {
                await this.Exit();

                this.SettingViewModel.IsFullScreen = true;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}
                this.Frame.GoBack();
            }
            this.HeadBarControl.DocumentButton.Click += async (s, e) =>
            {
                int countHistorys = this.ViewModel.Historys.Count;
                int countLayerages = this.ViewModel.LayerageCollection.RootLayerages.Count;

                if (countHistorys == 0 && countLayerages > 0)
                {
                    this.ViewModel.IsUpdateThumbnailByName = false;
                    await goBack();
                }
                else
                {
                    await this.Save();
                    this.ViewModel.IsUpdateThumbnailByName = true;
                    await goBack();
                }
            };
            this.HeadBarControl.DocumentUnSaveButton.Click += async (s, e) =>
            {
                this.HeadBarControl.DocumentFlyout.Hide();
                this.ViewModel.IsUpdateThumbnailByName = false;
                await goBack();
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

        //The current page becomes the active page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Key
            this.SettingViewModel.KeyIsEnabled = true;

            if (this.SettingViewModel.IsFullScreen == false) return;

            if (e.Parameter is TransitionData data)
            {
                this._lockOnNavigatedTo(data);
            }
        }
        //The current page no longer becomes an active page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //Key
            this.SettingViewModel.KeyIsEnabled = false;
        }

    }
}
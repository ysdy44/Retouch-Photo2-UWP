using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Numerics;
using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Streams;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using System.Xml.Linq;
using System.IO;

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
            Retouch_Photo2.Tools.Models.BrushTool.FillImage += () => this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.FillToImage);//Navigate   
            Retouch_Photo2.Tools.Models.BrushTool.StrokeImage += () => this.Frame.Navigate(typeof(PhotosPage), PhotosPageMode.StrokeToImage);//Navigate   


            //FlyoutTool
            this.ConstructColorFlyout(); 
            Retouch_Photo2.Tools.Elements.MoreTransformButton.Flyout = this.MoreTransformFlyout;
            Retouch_Photo2.Tools.Elements.MoreCreateButton.Flyout = this.MoreCreateFlyout;
            

            #region Document


            this.HeadBarControl.DocumentButton.Tapped += async (s, e) =>
            {
                await this.Save();
                await this.Exit();

                this.SettingViewModel.IsFullScreen = true;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}
                this.Frame.GoBack();
            };
            this.HeadBarControl.DocumentUnSaveButton.Tapped += async (s, e) =>
            {
                this.HeadBarControl.DocumentFlyout.Hide();
                await this.Exit();

                this.SettingViewModel.IsFullScreen = true;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}
                this.Frame.GoBack();
            };


            #endregion


            #region ExpandAppbar


            this.ConstructExportDialog();
            this.HeadBarControl.ExportButton.Tapped += (s, e) => this.ExportDialog.Show();

            this.HeadBarControl.UndoButton.Tapped += (s, e) =>
            {
                bool isUndo = this.ViewModel.Undo();//History

                if (isUndo)
                {
                    this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                    this.ViewModel.Invalidate();//Invalidate
                }
            };
            //this.RedoButton.Tapped += (s, e) => { };

            this.ConstructSetupDialog();
            this.HeadBarControl.SetupButton.Tapped += (s, e) => this.SetupDialog.Show();


            this.UnFullScreenButton.Tapped += (s, e) => this.SettingViewModel.IsFullScreen = !this.SettingViewModel.IsFullScreen;
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
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        /// <summary>
        /// Gets or sets the state of AppBar.
        /// </summary>
        public bool? IsAppBarLeft
        {
            get => this.isAppBarLeft;
            set
            {
                if (this.isAppBarLeft == value) return;

                if (value == true)
                {
                    this.HeadLeftLength.Width = new GridLength(1, GridUnitType.Star);
                    this.HeadRightLength.Width = GridLength.Auto;
                    this.ShadowRectangle.Visibility = Visibility.Collapsed;
                }
                else if (value == false)
                {
                    this.HeadLeftLength.Width = new GridLength(40, GridUnitType.Pixel);
                    this.HeadRightLength.Width = new GridLength(1, GridUnitType.Star);
                    this.ShadowRectangle.Visibility = Visibility.Visible;
                }

                this.isAppBarLeft = value;
            }
        }
        private bool? isAppBarLeft = null;


        // AppBar. 
        private void ConstructAppBar()
        {
            //AppBarGrid
            this.AppBarGridSizeChanged(this.AppBarGrid.ActualWidth);
            this.AppBarGrid.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                this.AppBarGridSizeChanged(e.NewSize.Width);
            };


            // Document
            this.DocumentButton.Holding += (s, e) => this.DocumentFlyout.ShowAt(this.DocumentButton);
            this.DocumentButton.RightTapped += (s, e) => this.DocumentFlyout.ShowAt(this.DocumentButton);
      
            
            this.DocumentUnSaveButton.Click += (s, e) => this.DocumentUnSave();
            this.DocumentButton.Click += async (s, e) =>
            {
                int countHistorys = this.ViewModel.Historys.Count;
                int countLayerages = LayerManager.RootLayerage.Children.Count;

                if (countHistorys == 0 && countLayerages > 1)
                {
                    this.DocumentUnSave();
                }
                else
                {
                    this.Document();
                }
            };


            // ExpandAppbar
            this.ExportButton.Tapped += (s, e) => this.ShowExportDialog();
            this.UndoButton.Tapped += (s, e) => this.MethodViewModel.MethodEditUndo();
            //this.RedoButton.Click += (s, e) => { };
            this.SetupButton.Tapped += (s, e) => this.ShowSetupDialog();
            this.RulerButton.Tapped += (s, e) => this.ViewModel.Invalidate();//Invalidate
            this.UnFullScreenButton.Click += (s, e) => this.DrawLayout.IsFullScreen = false;
            this.FullScreenButton.Tapped += (s, e) => this.DrawLayout.IsFullScreen = true;
        }

        private void AppBarGridSizeChanged(double width)
        {
            double arrangeWidth = width - this.DocumentButton.ActualWidth - 40;
            double measureWidth = this.MenuButtonsControl.ActualWidth;

            this.IsAppBarLeft = arrangeWidth > measureWidth;
        }


        /// <summary>
        /// Save, exit and back.
        /// </summary>
        private async void Document()
        {
            this.LoadingControl.State = LoadingState.Saving;
            this.LoadingControl.IsActive = true;

            await this.Save();
            this.ViewModel.IsUpdateThumbnailByName = true;

            await this.Exit();
            this.DrawLayout.IsFullScreen = true;
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate}

            this.LoadingControl.State = LoadingState.None;
            this.LoadingControl.IsActive = false;
            this.Frame.GoBack();
        }

        /// <summary>
        /// Un save, exit and back.
        /// </summary>
        private async void DocumentUnSave()
        {
            this.LoadingControl.State = LoadingState.Saving;
            this.LoadingControl.IsActive = true;

            this.ViewModel.IsUpdateThumbnailByName = false;

            await this.Exit();
            this.DrawLayout.IsFullScreen = true;
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

            this.LoadingControl.State = LoadingState.None;
            this.LoadingControl.IsActive = false;
            this.Frame.GoBack();
        }

    }
}
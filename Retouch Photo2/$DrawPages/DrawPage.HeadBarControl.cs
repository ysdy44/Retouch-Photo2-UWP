using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Represents a page used to draw vector graphics.
    /// </summary>
    public sealed partial class DrawPage : Page
    {

        /// <summary>
        /// Gets or sets the state of head.
        /// </summary>
        public bool IsHeadLeft
        {
            get => this.isHeadLeft;
            set
            {
                if (this.isHeadLeft == value) return;

                if (value)
                {
                    this.HeadLeftLength.Width = new GridLength(1, GridUnitType.Star);
                    this.HeadRightLength.Width = new GridLength(0, GridUnitType.Auto);
                    this.ShadowRectangle.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.HeadLeftLength.Width = new GridLength(40, GridUnitType.Pixel);
                    this.HeadRightLength.Width = new GridLength(1, GridUnitType.Star);
                    this.ShadowRectangle.Visibility = Visibility.Visible;
                }

                this.isHeadLeft = value;
            }
        }
        private bool isHeadLeft;


        // HeadBarControl. 
        private void ConstructHeadBarControl()
        {
            //HeadGrid
            this.HeadBarGrid.Loaded += (s, e) => this.HeadGridSizeChange(this.ActualWidth);
            this.HeadBarGrid.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                {
                    this.HeadGridSizeChange(e.NewSize.Width);
                }
            };


            // Document
            this.DocumentButton.Holding += (s, e) => this.DocumentFlyout.ShowAt(this.DocumentButton);
            this.DocumentButton.RightTapped += (s, e) => this.DocumentFlyout.ShowAt(this.DocumentButton);


            this.DocumentButton.Click += async (s, e) =>
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
            this.DocumentUnSaveButton.Click += async (s, e) =>
            {
                this.LoadingControl.State = LoadingState.Saving;
                this.LoadingControl.IsActive = true;

                this.DocumentFlyout.Hide();
                this.ViewModel.IsUpdateThumbnailByName = false;

                await this.Exit();
                this.SettingViewModel.IsFullScreen = true;
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.LoadingControl.State = LoadingState.None;
                this.LoadingControl.IsActive = false;
                this.Frame.GoBack();
            };


            // ExpandAppbar
            this.ConstructExportDialog();
            this.ExportButton.Tapped += (s, e) => this.ShowExportDialog();

            this.UndoButton.Tapped += (s, e) => this.MethodViewModel.MethodEditUndo();
            //this.RedoButton.Click += (s, e) => { };

            this.ConstructSetupDialog();
            this.SetupButton.Tapped += (s, e) => this.ShowSetupDialog();

            this.RulerButton.Tapped += (s, e) => this.ViewModel.Invalidate();//Invalidate

            this.UnFullScreenButton.Click += (s, e) => this.SettingViewModel.IsFullScreen = false;
            this.FullScreenButton.Tapped += (s, e) => this.SettingViewModel.IsFullScreen = true;
        }

        private void HeadGridSizeChange(double width)
        {
            double arrangeWidth = width - 70 - 40;
            double measureWidth = this.MenuButtonsControl.ActualWidth;

            this.IsHeadLeft = arrangeWidth > measureWidth; ;
        }

    }
}
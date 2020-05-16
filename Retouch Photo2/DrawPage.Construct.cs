using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "DrawPage" />. 
    /// </summary>
    public sealed partial class DrawPage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();
            
            this.SetupDialog.Title = resource.GetString("/$DrawPage/SetupDialog_Title");
            this.SetupDialog.CloseButton.Content = resource.GetString("/$DrawPage/SetupDialog_Close");
            this.SetupDialog.PrimaryButton.Content = resource.GetString("/$DrawPage/SetupDialog_Primary");
            this.SetupSizePicker.WidthText = resource.GetString("/$DrawPage/SetupSizePicker_Width");
            this.SetupSizePicker.HeightText = resource.GetString("/$DrawPage/SetupSizePicker_Height");
            
            this.ExportDialog.Title = resource.GetString("/$DrawPage/ExportDialog_Title");
            this.ExportDialog.CloseButton.Content = resource.GetString("/$DrawPage/ExportDialog_Close");
            this.ExportDialog.PrimaryButton.Content = resource.GetString("/$DrawPage/ExportDialog_Primary");
        }


        //Export
        private void ConstructExportDialog()
        {
            this.ExportDialog.CloseButton.Click += (sender, args) => this.ExportDialog.Hide();

            this.ExportDialog.PrimaryButton.Click += async (_, __) =>
            {
                this.ExportDialog.Hide();

                this.LoadingControl.State = LoadingState.Saving;
                this.LoadingControl.IsActive = true;

                bool isSuccesful = await this.Export();

                this.LoadingControl.State = isSuccesful ? LoadingState.SaveSuccess : LoadingState.SaveFailed;
                await Task.Delay(1000);

                this.LoadingControl.IsActive = false;
                this.LoadingControl.State = LoadingState.None;
            };
        }


        //Setup
        private void ConstructSetupDialog()
        {
            this.SetupDialog.CloseButton.Click += (sender, args) => this.SetupDialog.Hide();

            this.SetupDialog.PrimaryButton.Click += (_, __) =>
            {
                this.SetupDialog.Hide();

                BitmapSize size = this.SetupSizePicker.Size;

                this.ViewModel.CanvasTransformer.Width = (int)size.Width;
                this.ViewModel.CanvasTransformer.Height = (int)size.Height;

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        

        #region ColorPicker


        //@Static
        /// <summary>
        /// Displays the fill-color flyout relative to the specified element.
        /// </summary>
        /// <param name="FrameworkElement"> The element to be used as the target for the location of the flyout. </param>
        public static Action<FrameworkElement> FillColorShowAt;
        /// <summary>
        /// Displays the stroke-color flyout relative to the specified element.
        /// </summary>
        /// <param name="FrameworkElement"> The element to be used as the target for the location of the flyout. </param>
        public static Action<FrameworkElement> StrokeColorShowAt;


        //FillColor
        private void ConstructColorFlyout()
        {
            DrawPage.FillColorShowAt += (FrameworkElement placementTarget) =>
            {
                switch (this.SelectionViewModel.FillBrush.Type)
                {
                    case BrushType.Color:
                        this.FillColorPicker.Color = this.SelectionViewModel.FillBrush.Color;
                        break;
                }
                this.FillColorFlyout.ShowAt(placementTarget);
            };
            DrawPage.StrokeColorShowAt += (FrameworkElement placementTarget) =>
            {
                switch (this.SelectionViewModel.StrokeBrush.Type)
                {
                    case BrushType.Color:
                        this.StrokeColorPicker.Color = this.SelectionViewModel.StrokeBrush.Color;
                        break;
                }
                this.StrokeColorFlyout.ShowAt(placementTarget);
            };

            this.FillColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetColor(value, FillOrStroke.Fill);

                this.ViewModel.Invalidate();//Invalidate
            };
            this.StrokeColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetColor(value, FillOrStroke.Stroke);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        #endregion


    }
}
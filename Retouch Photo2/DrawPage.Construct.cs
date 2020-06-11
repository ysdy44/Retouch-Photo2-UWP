using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
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
            this.SetupLayersTextBlock.Text = resource.GetString("/$DrawPage/SetupDialog_Layers");
            this.SetupResizeButton.Content = resource.GetString("/$DrawPage/SetupDialog_Resize");
            this.SetupAnchorButton.Content = resource.GetString("/$DrawPage/SetupDialog_Anchor");
            
            this.ExportDialog.Title = resource.GetString("/$DrawPage/ExportDialog_Title");
            this.ExportDialog.CloseButton.Content = resource.GetString("/$DrawPage/ExportDialog_Close");
            this.ExportDialog.PrimaryButton.Content = resource.GetString("/$DrawPage/ExportDialog_Primary");
            this.ExportSizePicker.WidthText = resource.GetString("/$DrawPage/ExportSizePicker_Width");
            this.ExportSizePicker.HeightText = resource.GetString("/$DrawPage/ExportSizePicker_Height");
            this.ExportQualityTextBlock.Text = resource.GetString("/$DrawPage/ExportDialog_Quality");
            this.ExportFileFormatTextBlock.Text = resource.GetString("/$DrawPage/ExportDialog_FileFormat"); 

            this.RenameDialog.Title = resource.GetString("/$DrawPage/RenameDialog_Title");
            this.RenameTextBox.PlaceholderText = resource.GetString("/$DrawPage/RenameDialog_PlaceholderText");
            this.RenameDialog.CloseButton.Content = resource.GetString("/$DrawPage/RenameDialog_Close");
            this.RenameDialog.PrimaryButton.Content = resource.GetString("/$DrawPage/RenameDialog_Primary");
        }


        //Export
        private void ConstructExportDialog()
        {
            this.DPIComboBox.DPI = DPI.DPI144;
            this.FileFormatComboBox.FileFormat = CanvasBitmapFileFormat.Jpeg;

            this.ExportQualityPicker.Value = 1;
            this.ExportQualityPicker.Minimum = 0;
            this.ExportQualityPicker.Maximum = 1;

            this.ExportDialog.CloseButton.Click += (sender, args) => this.ExportDialog.Hide();

            this.ExportDialog.PrimaryButton.Click += async (_, __) =>
            {
                this.ExportDialog.Hide();

                this.LoadingControl.State = LoadingState.Saving;
                this.LoadingControl.IsActive = true;

                bool isSuccesful = await this.Export();

                this.LoadingControl.State = isSuccesful ? LoadingState.SaveSuccess : LoadingState.SaveFailed;
                await Task.Delay(1000);

                this.LoadingControl.State = LoadingState.None;
                this.LoadingControl.IsActive = false;
            };
        }
        private void ShowExportDialog()
        {
            BitmapSize size = new BitmapSize
            {
                Width = (uint)this.ViewModel.CanvasTransformer.Width,
                Height = (uint)this.ViewModel.CanvasTransformer.Height,
            };
            this.ExportSizePicker.Size = size;
            this.ExportDialog.Show();
        }


        //Setup
        private void ConstructSetupDialog()
        {
            this.SetupResizeButton.IsEnabled = false;
            this.SetupAnchorButton.IsEnabled = true;
            this.SetupIndicatorControl.Mode = IndicatorMode.None;

            this.SetupResizeButton.Click += (sender, args) =>
            {
                this.SetupResizeButton.IsEnabled = false;
                this.SetupAnchorButton.IsEnabled = true;
                this.SetupIndicatorControl.Mode = IndicatorMode.None;
            };
            this.SetupAnchorButton.Click += (sender, args) =>
            {
                this.SetupResizeButton.IsEnabled = true;
                this.SetupAnchorButton.IsEnabled = false;
                this.SetupIndicatorControl.Mode = IndicatorMode.LeftTop;
            };


            this.SetupDialog.CloseButton.Click += (sender, args) => this.SetupDialog.Hide();

            this.SetupDialog.PrimaryButton.Click += (_, __) =>
            {
                this.SetupDialog.Hide();

                BitmapSize size = this.SetupSizePicker.Size;
                IndicatorMode mode = this.SetupIndicatorControl.Mode;

                if (mode== IndicatorMode.None)
                    this.MethodViewModel.MethodSetup(size);
                else
                    this.MethodViewModel.MethodSetup(size, mode);
            };
        }
        private void ShowSetupDialog()
        {
            BitmapSize size = new BitmapSize
            {
                Width = (uint)this.ViewModel.CanvasTransformer.Width,
                Height = (uint)this.ViewModel.CanvasTransformer.Height,
            };
            this.SetupSizePicker.Size = size;

            this.SetupDialog.Show();
        }


        //Rename
        private void ConstructRenameDialog()
        {
            //Key
            this.RenameTextBox.GettingFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = false;
            this.RenameTextBox.LosingFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = true;

            this.RenameDialog.CloseButton.Click += (sender, args) => this.RenameDialog.Hide();
            this.RenameDialog.PrimaryButton.Click += (_, __) =>
            {
                this.RenameDialog.Hide();
                string name = this.RenameTextBox.Text;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set name");
                
                //Selection
                this.SelectionViewModel.LayerName = name;
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.Name != name)
                    {
                        //History
                        var previous = layer.Name;
                        history.UndoAction += () =>
                        {
                            layer.Name = previous;
                        };

                        layer.Name = name;
                    }
                });

                //History
                this.ViewModel.HistoryPush(history);
            };
        }
        private void ShowRenameDialog()
        {
            this.RenameTextBox.Text = this.SelectionViewModel.LayerName;

            this.RenameDialog.Show();
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



        private void ConstructColorFlyout()
        {
            this.ConstructFillColorFlyout();
            this.ConstructStrokeColorFlyout();
        }


        //FillColor
        private void ConstructFillColorFlyout()
        {
            DrawPage.FillColorShowAt += (FrameworkElement placementTarget) =>
            {
                switch (this.SelectionViewModel.Fill.Type)
                {
                    case BrushType.Color:
                        this.FillColorPicker.Color = this.SelectionViewModel.Fill.Color;
                        break;
                }
                this.FillColorFlyout.ShowAt(placementTarget);
            };

            this.FillColorPicker.ColorChanged += (s, value) => this.MethodViewModel.MethodFillColorChanged(value);

            this.FillColorPicker.ColorChangeStarted += (s, value) => this.MethodViewModel.MethodFillColorChangeStarted(value);
            this.FillColorPicker.ColorChangeDelta += (s, value) => this.MethodViewModel.MethodFillColorChangeDelta(value);
            this.FillColorPicker.ColorChangeCompleted += (s, value) => this.MethodViewModel.MethodFillColorChangeCompleted(value);
        }


        //StrokeColor
        private void ConstructStrokeColorFlyout()
        {
            DrawPage.StrokeColorShowAt += (FrameworkElement placementTarget) =>
            {
                switch (this.SelectionViewModel.Stroke.Type)
                {
                    case BrushType.Color:
                        this.StrokeColorPicker.Color = this.SelectionViewModel.Stroke.Color;
                        break;
                }
                this.StrokeColorFlyout.ShowAt(placementTarget);
            };

            this.StrokeColorPicker.ColorChanged += (s, value) => this.MethodViewModel.MethodStrokeColorChanged(value);

            this.StrokeColorPicker.ColorChangeStarted += (s, value) => this.MethodViewModel.MethodStrokeColorChangeStarted(value);
            this.StrokeColorPicker.ColorChangeDelta += (s, value) => this.MethodViewModel.MethodStrokeColorChangeDelta(value);
            this.StrokeColorPicker.ColorChangeCompleted += (s, value) => this.MethodViewModel.MethodStrokeColorChangeCompleted(value);
        }


        #endregion


    }
}
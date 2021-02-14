using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System.Numerics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.DocumentButton.Content = resource.GetString("$DrawPage_Document");
            this.DocumentUnSaveButton.Content = resource.GetString("$DrawPage_DocumentUnSave");
            {
                this.ExportToolTip.Content =
                this.ExportButton.Text = resource.GetString("$DrawPage_Export");
                this.UndoToolTip.Content =
                this.UndoButton.Text = resource.GetString("$DrawPage_Undo");
                //this.RedoToolTip.Content = resourceLoader.GetString("$DrawPage_Redo");
                //this.RedoButton.Text =
                this.SetupToolTip.Content =
                this.SetupButton.Text = resource.GetString("$DrawPage_Setup");
                this.SnapToolTip.Content =
                this.SnapButton.Text = resource.GetString("$DrawPage_Snap");
                this.RulerToolTip.Content =
                this.RulerButton.Text = resource.GetString("$DrawPage_Ruler");
                this.FullScreenToolTip.Content =
                this.FullScreenButton.Text = resource.GetString("$DrawPage_FullScreen");
                this.TipButton.Text = resource.GetString("$DrawPage_Tip");
            }


            this.DrawLayout.GalleryToolTip.Content = resource.GetString("$DrawPage_GalleryTip");
            this.DrawLayout.WidthToolTip.Content = resource.GetString("$DrawPage_WidthTip");

            this.SetupDialog.Title = resource.GetString("$DrawPage_SetupDialog_Title");
            {
                this.SetupDialog.CloseButton.Content = resource.GetString("$DrawPage_SetupDialog_Close");
                this.SetupDialog.PrimaryButton.Content = resource.GetString("$DrawPage_SetupDialog_Primary");

                this.SetupSizePicker.WidthText = resource.GetString("$DrawPage_SetupDialog_SizePicker_Width");
                this.SetupSizePicker.HeightText = resource.GetString("$DrawPage_SetupDialog_SizePicker_Height");

                this.SetupLayersTextBlock.Text = resource.GetString("$DrawPage_SetupDialog_Layers");
                this.SetupResizeButton.Content = resource.GetString("$DrawPage_SetupDialog_Resize");
                this.SetupAnchorButton.Content = resource.GetString("$DrawPage_SetupDialog_Anchor");
            }
            
            this.ExportDialog.Title = resource.GetString("$DrawPage_ExportDialog_Title");
            {
                this.ExportDialog.CloseButton.Content = resource.GetString("$DrawPage_ExportDialog_Close");
                this.ExportDialog.PrimaryButton.Content = resource.GetString("$DrawPage_ExportDialog_Primary");

                this.ExportSizePicker.WidthText = resource.GetString("$DrawPage_ExportDialog_SizePicker_Width");
                this.ExportSizePicker.HeightText = resource.GetString("$DrawPage_ExportDialog_SizePicker_Height");

                this.ExportQualityTextBlock.Text = resource.GetString("$DrawPage_ExportDialog_Quality");
                this.ExportFileFormatTextBlock.Text = resource.GetString("$DrawPage_ExportDialog_FileFormat");
            }

            this.RenameDialog.Title = resource.GetString("$DrawPage_RenameDialog_Title");
            {
                this.RenameDialog.CloseButton.Content = resource.GetString("$DrawPage_RenameDialog_Close");
                this.RenameDialog.PrimaryButton.Content = resource.GetString("$DrawPage_RenameDialog_Primary");

                this.RenameTextBox.PlaceholderText = resource.GetString("$DrawPage_RenameDialog_PlaceholderText");
            }
        }


        //Export
        private float ExportQuality
        {
            get => this.exportQuality;
            set
            {
                this.ExportQualityPicker.Value = (int)(value * 100.0f);
                this.ExportQualitySlider.Value = value;
                this.exportQuality = value;
            }
        }
        private float exportQuality = 1.0f;

        private void ConstructExportDialog()
        {
            this.DPIComboBox.DPI = DPI.DPI144;
            this.FileFormatComboBox.FileFormat = CanvasBitmapFileFormat.Jpeg;


            this.ExportQualityPicker.Unit = "%";
            this.ExportQualityPicker.Minimum = 0;
            this.ExportQualityPicker.Maximum = 100;
            this.ExportQualityPicker.ValueChanged += (s, value) => this.ExportQualitySlider.Value = value / 100.0f;


            this.ExportQualitySlider.Minimum = 0.0d;
            this.ExportQualitySlider.Maximum = 1.0d;
            this.ExportQualitySlider.ValueChangeStarted += (s, value) => this.ExportQualityPicker.Value = (int)(value * 100.0f);
            this.ExportQualitySlider.ValueChangeDelta += (s, value) => this.ExportQualityPicker.Value = (int)(value * 100.0f);
            this.ExportQualitySlider.ValueChangeCompleted += (s, value) => this.ExportQualityPicker.Value = (int)(value * 100.0f);


            this.ExportDialog.CloseButton.Click += (sender, args) => this.ExportDialog.Hide();


            this.ExportDialog.PrimaryButton.Click += async (_, __) =>
            {
                this.ExportDialog.Hide();

                this.LoadingControl.State = LoadingState.Saving;
                this.LoadingControl.IsActive = true;

                bool isSuccesful = await this.Export();

                this.LoadingControl.State = isSuccesful ? LoadingState.SaveSuccess : LoadingState.SaveFailed;
                await Task.Delay(400);

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
            this.ExportQuality = this.ExportQuality;
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
            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            if (this.RenameTextBox is TextBox textBox)
            {
                //textBox.IsEnabled = false;
                //this.ColorFlyout.Opened += (s, e) => textBox.IsEnabled = true;
                //this.ColorFlyout.Closed += (s, e) => textBox.IsEnabled = false;
                textBox.GotFocus += (s, e) => this.SettingViewModel.UnRegisteKey();
                textBox.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            }

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


        //FullScreen
        private void FullScreenLayout()
        {
            Vector2 offset = this.SettingViewModel.FullScreenOffset;
            bool isFullScreen = this.DrawLayout.IsFullScreen;

            if (isFullScreen)
            {
                this.DrawLayout.IsFullScreen = false;
                this.UnFullScreenButton.Visibility = Visibility.Collapsed;

                this.ViewModel.CanvasTransformer.Position -= offset;
                this.ViewModel.CanvasTransformer.ReloadMatrix();
            }
            else
            {
                this.DrawLayout.IsFullScreen = true;
                this.UnFullScreenButton.Visibility = Visibility.Visible;

                this.ViewModel.CanvasTransformer.Position += offset;
                this.ViewModel.CanvasTransformer.ReloadMatrix();
            }
        }

    }
}
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Photos;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //FlowDirection
        private void ConstructFlowDirection()
        {
            bool isRightToLeft = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;

            base.FlowDirection = isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.DocumentTextBlock.Text = resource.GetString("$DrawPage_Document");
            this.DocumentUnSaveButton.Content = resource.GetString("$DrawPage_DocumentUnSave");
            {
                this.ExportToolTip.Content = this.OverflowExportButton.Content = resource.GetString("$DrawPage_Export");
                this.UndoToolTip.Content = this.OverflowUndoButton.Content = resource.GetString("$DrawPage_Undo");
                //this.RedoToolTip.Content = this.OverflowRedoButton.Content = resource.GetString("$DrawPage_Redo");
                this.SetupToolTip.Content = this.OverflowSetupButton.Content = resource.GetString("$DrawPage_Setup");
                this.SnapToolTip.Content = this.OverflowSnapButton.Content = resource.GetString("$DrawPage_Snap");
                this.RulerToolTip.Content = this.OverflowRulerButton.Content = resource.GetString("$DrawPage_Ruler");
                this.FullScreenToolTip.Content = this.OverflowFullScreenButton.Content = resource.GetString("$DrawPage_FullScreen");
                this.OverflowTipButton.Content = resource.GetString("$DrawPage_Tip");
            }

            this.SetupDialog.Title = resource.GetString("$DrawPage_SetupDialog_Title");
            {
                this.SetupDialog.SecondaryButtonText = resource.GetString("$DrawPage_SetupDialog_Close");
                this.SetupDialog.PrimaryButtonText = resource.GetString("$DrawPage_SetupDialog_Primary");

                this.SetupSizePicker.WidthText = resource.GetString("$DrawPage_SetupDialog_SizePicker_Width");
                this.SetupSizePicker.HeightText = resource.GetString("$DrawPage_SetupDialog_SizePicker_Height");

                this.SetupAnchorCheckBox.Content = resource.GetString("$DrawPage_SetupDialog_Anchor");
            }

            this.ExportDialog.Title = resource.GetString("$DrawPage_ExportDialog_Title");
            {
                this.ExportDialog.SecondaryButtonText = resource.GetString("$DrawPage_ExportDialog_Close");
                this.ExportDialog.PrimaryButtonText = resource.GetString("$DrawPage_ExportDialog_Primary");

                this.ExportSizePicker.WidthText = resource.GetString("$DrawPage_ExportDialog_SizePicker_Width");
                this.ExportSizePicker.HeightText = resource.GetString("$DrawPage_ExportDialog_SizePicker_Height");

                this.ExportQualityTextBlock.Text = resource.GetString("$DrawPage_ExportDialog_Quality");
                this.ExportFileFormatTextBlock.Text = resource.GetString("$DrawPage_ExportDialog_FileFormat");
            }

            this.RenameDialog.Title = resource.GetString("$DrawPage_RenameDialog_Title");
            {
                this.RenameDialog.SecondaryButtonText = resource.GetString("$DrawPage_RenameDialog_Close");
                this.RenameDialog.PrimaryButtonText = resource.GetString("$DrawPage_RenameDialog_Primary");

                this.RenameTextBox.PlaceholderText = resource.GetString("$DrawPage_RenameDialog_PlaceholderText");
            }

            this.GalleryDialog.Title = resource.GetString("$DrawPage_Gallery");
            {
                this.GalleryDialog.PrimaryButtonText = resource.GetString("$DrawPage_GalleryDialog_Primary");
            }

            this.DrawLayout.GalleryToolTip.Content = resource.GetString("$DrawPage_Gallery");
            this.DrawLayout.WidthToolTip.Content = resource.GetString("$DrawPage_WidthTip");
            {
                this.SquareTextBlock.Text = resource.GetString("Tools_MoreCreate_Square");
                this.CenterTextBlock.Text = resource.GetString("Tools_MoreCreate_Center");

                this.RatioTextBlock.Text = resource.GetString("Tools_MoreTransform_Ratio ");
                this.CenterTextBlock2.Text = resource.GetString("Tools_MoreTransform_Center");
            }

            this.EditExpander.Title = resource.GetString("Menus_Edit");
            this.OperateExpander.Title = resource.GetString("Menus_Operate");
            this.AdjustmentExpander.Title = resource.GetString("Menus_Adjustment");
            this.EffectExpander.Title = resource.GetString("Menus_Effect");
            this.TextExpander.Title = resource.GetString("Menus_Text");
            this.StrokeExpander.Title = resource.GetString("Menus_Stroke");
            this.StyleExpander.Title = resource.GetString("Menus_Style");
            this.HistoryExpander.Title = resource.GetString("Menus_History");
            this.TransformerExpander.Title = resource.GetString("Menus_Transformer");
            this.LayerExpander.Title = resource.GetString("Menus_Layer");
            this.ColorExpander.Title = resource.GetString("Menus_Color");
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


            this.ExportDialog.SecondaryButtonClick += (sender, args) => this.ExportDialog.Hide();
            this.ExportDialog.PrimaryButtonClick += async (_, __) =>
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
            this.SetupIndicatorControl.Mode = IndicatorMode.LeftTop;

            this.SetupAnchorCheckBox.Checked += (sender, args) => this.SetupIndicatorControl.Visibility = Visibility.Visible;
            this.SetupAnchorCheckBox.Unchecked += (sender, args) => this.SetupIndicatorControl.Visibility = Visibility.Collapsed;

            this.SetupDialog.SecondaryButtonClick += (sender, args) => this.SetupDialog.Hide();
            this.SetupDialog.PrimaryButtonClick += (_, __) =>
            {
                this.SetupDialog.Hide();

                BitmapSize size = this.SetupSizePicker.Size;

                if (this.SetupAnchorCheckBox.IsChecked == true)
                    this.MethodViewModel.MethodSetup(size);
                else if (this.SetupAnchorCheckBox.IsChecked == false)
                {
                    IndicatorMode mode = this.SetupIndicatorControl.Mode;
                    this.MethodViewModel.MethodSetup(size, mode);
                }
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

            this.RenameDialog.SecondaryButtonClick += (sender, args) => this.RenameDialog.Hide();
            this.RenameDialog.PrimaryButtonClick += (_, __) =>
            {
                this.RenameDialog.Hide();
                string name = this.RenameTextBox.Text;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetName);

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
        private void FullScreenChanged()
        {
            this.DrawLayout.IsFullScreen = !this.DrawLayout.IsFullScreen;
        }


        //Gallery
        private void ConstructGalleryDialog()
        {
            this.GalleryGridView.ItemsSource = Photo.Instances;

            this.GalleryDialog.CloseButtonClick += (s, e) => this.GalleryDialog.Hide();
            this.GalleryDialog.PrimaryButtonClick += async (s, e) =>
            {
                //Files
                IReadOnlyList<StorageFile> files = await FileUtil.PickMultipleImageFilesAsync(PickerLocationId.Desktop);
                await this.CopyMultipleImageFilesAsync(files);
            };
        }
        private async void ShowGalleryDialog()
        {
            Photo photo = await Retouch_Photo2.DrawPage.ShowGalleryFunc?.Invoke();
            if (photo == null) return;

            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer);
            this.ViewModel.HistoryPush(history);

            //Transformer
            Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);

            //Layer
            Photocopier photocopier = photo.ToPhotocopier();
            ImageLayer imageLayer = new ImageLayer
            {
                Photocopier = photocopier,
                IsSelected = true,
                Transform = new Transform(transformerSource)
            };
            Layerage imageLayerage = imageLayer.ToLayerage();
            string id = imageLayerage.Id;
            LayerBase.Instances.Add(id, imageLayer);

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                layer.IsSelected = false;
            });

            //Mezzanine
            LayerManager.Mezzanine(imageLayerage);

            this.SelectionViewModel.SetMode();//Selection
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.ViewModel.Invalidate();//Invalidate     
        }


        //Fill
        private void ShowFillColorFlyout2(FrameworkElement page, FrameworkElement button)
        {
            switch (this.SelectionViewModel.Fill.Type)
            {
                case BrushType.Color:
                    this.FilColorPicker.Color = this.SelectionViewModel.Fill.Color;
                    break;
            }

            switch (this.SettingViewModel.DeviceLayoutType)
            {
                case DeviceLayoutType.PC:
                    this.FillColorFlyout.ShowAt(button);
                    break;
                case DeviceLayoutType.Pad:
                case DeviceLayoutType.Phone:
                    this.FillColorFlyout.ShowAt(page);
                    break;
            }
        }
        private void ConstructFillColorFlyout()
        {
            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            if (this.FilColorPicker.HexPicker is TextBox textBox)
            {
                textBox.IsEnabled = false;
                this.FillColorFlyout.Opened += (s, e) => textBox.IsEnabled = true;
                this.FillColorFlyout.Closed += (s, e) => textBox.IsEnabled = false;
                textBox.GotFocus += (s, e) => this.SettingViewModel.UnRegisteKey();
                textBox.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            }

            this.FilColorPicker.ColorChanged += (s, value) => this.MethodViewModel.MethodFillColorChanged(value);

            this.FilColorPicker.ColorChangeStarted += (s, value) => this.MethodViewModel.MethodFillColorChangeStarted(value);
            this.FilColorPicker.ColorChangeDelta += (s, value) => this.MethodViewModel.MethodFillColorChangeDelta(value);
            this.FilColorPicker.ColorChangeCompleted += (s, value) => this.MethodViewModel.MethodFillColorChangeCompleted(value);
        }

        //Stroke
        private void ShowStrokeColorFlyout2(FrameworkElement page, FrameworkElement button)
        {
            switch (this.SelectionViewModel.Stroke.Type)
            {
                case BrushType.Color:
                    this.StrokeColorPicker.Color = this.SelectionViewModel.Stroke.Color;
                    break;
            }

            switch (this.SettingViewModel.DeviceLayoutType)
            {
                case DeviceLayoutType.PC:
                    this.StrokeColorFlyout.ShowAt(button);
                    break;
                case DeviceLayoutType.Pad:
                case DeviceLayoutType.Phone:
                    this.StrokeColorFlyout.ShowAt(page);
                    break;
            }
        }
        private void ConstructStrokeColorFlyout()
        {
            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            if (this.StrokeColorPicker.HexPicker is TextBox textBox)
            {
                textBox.IsEnabled = false;
                this.StrokeColorFlyout.Opened += (s, e) => textBox.IsEnabled = true;
                this.StrokeColorFlyout.Closed += (s, e) => textBox.IsEnabled = false;
                textBox.GotFocus += (s, e) => this.SettingViewModel.UnRegisteKey();
                textBox.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            }

            this.StrokeColorPicker.ColorChanged += (s, value) => this.MethodViewModel.MethodStrokeColorChanged(value);

            this.StrokeColorPicker.ColorChangeStarted += (s, value) => this.MethodViewModel.MethodStrokeColorChangeStarted(value);
            this.StrokeColorPicker.ColorChangeDelta += (s, value) => this.MethodViewModel.MethodStrokeColorChangeDelta(value);
            this.StrokeColorPicker.ColorChangeCompleted += (s, value) => this.MethodViewModel.MethodStrokeColorChangeCompleted(value);
        }


        //MoreTransform
        private void ShowMoreTransformFlyout(FrameworkElement page, FrameworkElement button)
        {
            switch (this.SettingViewModel.DeviceLayoutType)
            {
                case DeviceLayoutType.PC:
                    this.MoreTransformContent.Width = double.NaN;
                    this.MoreTransformFlyout.ShowAt(button);
                    break;
                case DeviceLayoutType.Pad:
                    this.MoreTransformContent.Width = double.NaN;
                    this.MoreTransformFlyout.ShowAt(page);
                    break;
                case DeviceLayoutType.Phone:
                    this.MoreTransformContent.Width = page.ActualWidth - 40;
                    this.MoreTransformFlyout.ShowAt(page);
                    break;
            }
        }

        //MoreCreate
        private void ShowMoreCreateFlyout(FrameworkElement page, FrameworkElement button)
        {
            switch (this.SettingViewModel.DeviceLayoutType)
            {
                case DeviceLayoutType.PC:
                    this.MoreCreateContent.Width = double.NaN;
                    this.MoreCreateFlyout.ShowAt(button);
                    break;
                case DeviceLayoutType.Pad:
                    this.MoreCreateContent.Width = double.NaN;
                    this.MoreCreateFlyout.ShowAt(page);
                    break;
                case DeviceLayoutType.Phone:
                    this.MoreCreateContent.Width = page.ActualWidth - 40;
                    this.MoreCreateFlyout.ShowAt(page);
                    break;
            }
        }

    }
}
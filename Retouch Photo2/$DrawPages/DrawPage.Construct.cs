using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Menus;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //Languages
        private void ConstructLanguages()
        {
            if (string.IsNullOrEmpty(ApplicationLanguages.PrimaryLanguageOverride) == false)
            {
                if (ApplicationLanguages.PrimaryLanguageOverride != base.Language)
                {
                    this.ConstructFlowDirection();
                    this.ConstructStrings();
                }
            }
        }

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
            this.DocumentUnSaveControl.Content = resource.GetString("$DrawPage_DocumentUnSave");
            {
                this.ExportToolTip.Content = this.OverflowExportControl.Content = resource.GetString("$DrawPage_Export");
                this.UndoToolTip.Content = this.OverflowUndoControl.Content = resource.GetString("$DrawPage_Undo");
                //this.RedoToolTip.Content = this.OverflowRedoControl.Content = resource.GetString("$DrawPage_Redo");
                this.SetupToolTip.Content = this.OverflowSetupControl.Content = resource.GetString("$DrawPage_Setup");
                this.SnapToolTip.Content = this.OverflowSnapControl.Content = resource.GetString("$DrawPage_Snap");
                //this.RulerToolTip.Content = this.OverflowRulerControl.Content = resource.GetString("$DrawPage_Ruler");
                this.FullScreenToolTip.Content = this.OverflowFullScreenControl.Content = resource.GetString("$DrawPage_FullScreen");
                this.OverflowTipControl.Content = resource.GetString("$DrawPage_Tip");
            }
            this.OverflowToolTip.Content = resource.GetString("$DrawPage_More");

            this.SetupDialog.Title = resource.GetString("$DrawPage_SetupDialog_Title");
            {
                this.SetupDialog.SecondaryButtonText = resource.GetString("$DrawPage_SetupDialog_Close");
                this.SetupDialog.PrimaryButtonText = resource.GetString("$DrawPage_SetupDialog_Primary");

                this.SetupSizePicker.WidthText = resource.GetString("$DrawPage_SetupDialog_SizePicker_Width");
                this.SetupSizePicker.HeightText = resource.GetString("$DrawPage_SetupDialog_SizePicker_Height");

                this.SetupAnchorCheckControl.Content = resource.GetString("$DrawPage_SetupDialog_Anchor");
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

            this.DrawLayout.GalleryToolTipContent = resource.GetString("$DrawPage_Gallery");
            this.DrawLayout.WidthToolTipContent = resource.GetString("$DrawPage_WidthTip");
            {
                this.TransformGroupHeader.Content = resource.GetString("More_Transform");
                this.RatioControl.Content = resource.GetString("More_Transform_Ratio");
                this.SnapToTickControl.Content = resource.GetString("More_Transform_SnapToTick");

                this.CreateGroupHeader.Content = resource.GetString("More_Create");
                this.SquareControl.Content = resource.GetString("More_Create_Square");
                this.CenterControl.Content = resource.GetString("More_Create_Center");

                this.OperateGroupHeader.Content = resource.GetString("More_Operate");
                this.WheelToRotateControl.Content = resource.GetString("More_Operate_WheelToRotate");
            }

            //Menus
            foreach (Expander expander in Expander.Dictionary)
            {
                MenuType type = expander.Type;

                expander.Title = resource.GetString($"Menus_{type}");
            }
        }


        //Menus
        private void ConstructMenus()
        {
            this.MenuListView.ItemClick += (s, e) =>
            {
                if (e.ClickedItem is Retouch_Photo2.Menus.Icon value)
                {
                    MenuType type = value.Type;
                    FrameworkElement placementTarget = value;
                    Expander.ShowAt(type, placementTarget);
                }
            };
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

                bool isSuccesful = await this.Export();

                this.LoadingControl.State = isSuccesful ? LoadingState.SaveSuccess : LoadingState.SaveFailed;
                await Task.Delay(400);

                this.LoadingControl.State = LoadingState.None;
            };
        }
        private void ShowExportDialog()
        {
            this.ExportSizePicker.SizeWith = this.ViewModel.CanvasTransformer.Width;
            this.ExportSizePicker.SizeHeight = this.ViewModel.CanvasTransformer.Height;
            this.ExportQuality = this.ExportQuality;
            this.ExportDialog.Show();
        }


        //Setup
        private void ConstructSetupDialog()
        {
            this.SetupIndicatorControl.Mode = IndicatorMode.LeftTop;

            this.SetupAnchorCheckControl.Tapped += (sender, args) =>
            {
                bool isChecked = this.SetupAnchorCheckControl.IsChecked;

                this.SetupAnchorCheckControl.IsChecked = !isChecked;
                this.SetupIndicatorControl.Visibility = isChecked ? Visibility.Collapsed : Visibility.Visible;
            };

            this.SetupDialog.SecondaryButtonClick += (sender, args) => this.SetupDialog.Hide();
            this.SetupDialog.PrimaryButtonClick += (_, __) =>
            {
                this.SetupDialog.Hide();

                BitmapSize size = this.SetupSizePicker.Size;

                if (this.SetupAnchorCheckControl.IsChecked == true)
                {
                    this.MethodViewModel.MethodSetup(size);
                }
                else
                {
                    IndicatorMode mode = this.SetupIndicatorControl.Mode;
                    this.MethodViewModel.MethodSetup(size, mode);
                }
            };
        }
        private void ShowSetupDialog()
        {
            this.SetupSizePicker.SizeWith = this.ViewModel.CanvasTransformer.Width;
            this.SetupSizePicker.SizeHeight = this.ViewModel.CanvasTransformer.Height;

            this.SetupDialog.Show();
        }


        /// <summary>
        /// Turn full-screen.
        /// </summary>
        public void FullScreenChanged()
        {
            this.DrawLayout.IsFullScreen = !this.DrawLayout.IsFullScreen;
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
            this.FilColorPicker.HexPicker.KeyDown += (s, e) => { if (e.Key == VirtualKey.Enter) this.FilColorPicker.Focus(FocusState.Programmatic); };
            this.FilColorPicker.HexPicker.GotFocus += (s, e) => this.SettingViewModel.UnregisteKey();
            this.FilColorPicker.HexPicker.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            this.FilColorPicker.EyedropperOpened += (s, e) => this.SettingViewModel.UnregisteKey();
            this.FilColorPicker.EyedropperClosed += (s, e) => this.SettingViewModel.RegisteKey();
            this.FilColorPicker.ColorChanged += (s, value) => this.MethodViewModel.MethodFillColorChanged(value);

            this.FilColorPicker.ColorChangedStarted += (s, value) => this.MethodViewModel.MethodFillColorChangeStarted(value);
            this.FilColorPicker.ColorChangedDelta += (s, value) => this.MethodViewModel.MethodFillColorChangeDelta(value);
            this.FilColorPicker.ColorChangedCompleted += (s, value) => this.MethodViewModel.MethodFillColorChangeCompleted(value);
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
            this.StrokeColorPicker.HexPicker.KeyDown += (s, e) => { if (e.Key == VirtualKey.Enter) this.StrokeColorPicker.Focus(FocusState.Programmatic); };
            this.StrokeColorPicker.HexPicker.GotFocus += (s, e) => this.SettingViewModel.UnregisteKey();
            this.StrokeColorPicker.HexPicker.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            this.StrokeColorPicker.EyedropperOpened += (s, e) => this.SettingViewModel.UnregisteKey();
            this.StrokeColorPicker.EyedropperClosed += (s, e) => this.SettingViewModel.RegisteKey();
            this.StrokeColorPicker.ColorChanged += (s, value) => this.MethodViewModel.MethodStrokeColorChanged(value);

            this.StrokeColorPicker.ColorChangedStarted += (s, value) => this.MethodViewModel.MethodStrokeColorChangeStarted(value);
            this.StrokeColorPicker.ColorChangedDelta += (s, value) => this.MethodViewModel.MethodStrokeColorChangeDelta(value);
            this.StrokeColorPicker.ColorChangedCompleted += (s, value) => this.MethodViewModel.MethodStrokeColorChangeCompleted(value);
        }


        //More
        private void ConstructMore()
        {
            this.RatioItem.Tapped += (s, e) => this.RatioItem.IsSelected = !this.RatioItem.IsSelected;
            this.SnapToTickItem.Tapped += (s, e) => this.SnapToTickItem.IsSelected = !this.SnapToTickItem.IsSelected;
            this.SquareItem.Tapped += (s, e) => this.SquareItem.IsSelected = !this.SquareItem.IsSelected;
            this.CenterItem.Tapped += (s, e) => this.CenterItem.IsSelected = !this.CenterItem.IsSelected;
            this.WheelToRotateItem.Tapped += (s, e) => this.WheelToRotateItem.IsSelected = !this.WheelToRotateItem.IsSelected;
        }

    }
}
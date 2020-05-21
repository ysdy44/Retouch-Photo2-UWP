using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
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
            
            this.ExportDialog.Title = resource.GetString("/$DrawPage/ExportDialog_Title");
            this.ExportDialog.CloseButton.Content = resource.GetString("/$DrawPage/ExportDialog_Close");
            this.ExportDialog.PrimaryButton.Content = resource.GetString("/$DrawPage/ExportDialog_Primary");

            this.RenameDialog.Title = resource.GetString("/$DrawPage/RenameDialog_Title");
            this.RenameDialog.CloseButton.Content = resource.GetString("/$DrawPage/RenameDialog_Close");
            this.RenameDialog.PrimaryButton.Content = resource.GetString("/$DrawPage/RenameDialog_Primary");
        }


        //Export
        private void ConstructExportDialog()
        {
            this.QualityPicker.Maximum = 1;
            this.QualityPicker.Value = 1;

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
        private void ShowExportDialog()
        {

            this.ExportDialog.Show();
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
        private void ShowSetupDialog()
        {
            float width = this.ViewModel.CanvasTransformer.Width;
            float height = this.ViewModel.CanvasTransformer.Height;
            this.SetupSizePicker.Width = width;
            this.SetupSizePicker.Height = height;

            this.SetupDialog.Show();
        }


        //Rename
        private void ConstructRenameDialog()
        {
            this.RenameDialog.CloseButton.Click += (sender, args) => this.RenameDialog.Hide();

            this.RenameDialog.PrimaryButton.Click += (_, __) =>
            {
                this.RenameDialog.Hide();
                string name = this.RenameTextBox.Text;

                //History
                IHistoryBase history = new IHistoryBase("Set name");
                this.ViewModel.Push(history);
                
                //Selection
                this.SelectionViewModel.LayerName = name;
                this.SelectionViewModel.SetValue((layer)=>
                {
                    if (layer.Name != name)
                    {
                        //History
                        var previous = layer.Name;
                        int index = layer.Control.Index;
                        history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                        Name = previous);
                        
                        layer.Name = name;
                    }
                });
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
            this.ConstructFillColorFlyout1();
            this.ConstructFillColorFlyout2();
            this.ConstructStrokeColorFlyout1();
            this.ConstructStrokeColorFlyout2();
        }


        //FillColor
        private void ConstructFillColorFlyout1()
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


            this.FillColorPicker.ColorChanged += (s, value) =>
            {
                //History
                IHistoryBase history = new IHistoryBase("Set fill");

                //Selection
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.SelectionViewModel.Color = value;
                        break;
                }
                this.SelectionViewModel.Fill = BrushBase.ColorBrush(value);
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    var previous = layer.Style.Fill.Clone();
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Style.Fill = previous.Clone());

                    layer.Style.Fill = BrushBase.ColorBrush(value);
                    this.SelectionViewModel.StyleLayer = layer;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructFillColorFlyout2()
        {
            //History
            IHistoryBase history = null;


            //Color
            this.FillColorPicker.ColorChangeStarted += (s, value) =>
            {
                history = new IHistoryBase("Set fill");

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Style.CacheFill();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.FillColorPicker.ColorChangeDelta += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Style.Fill = BrushBase.ColorBrush(value);
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.FillColorPicker.ColorChangeCompleted += (s, value) =>
            {
                //Selection
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.SelectionViewModel.Color = value;
                        break;
                }
                this.SelectionViewModel.Fill = BrushBase.ColorBrush(value);
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    var previous = layer.Style.StartingFill;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Style.Fill = previous.Clone());

                    layer.Style.Fill = BrushBase.ColorBrush(value);
                    this.SelectionViewModel.StyleLayer = layer;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }


        //StrokeColor
        private void ConstructStrokeColorFlyout1()
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


            this.StrokeColorPicker.ColorChanged += (s, value) =>
            {
                //History
                IHistoryBase history = new IHistoryBase("Set stroke");

                //Selection
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        this.SelectionViewModel.Color = value;
                        break;
                }
                this.SelectionViewModel.Stroke = BrushBase.ColorBrush(value);
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    var previous = layer.Style.Stroke.Clone();
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Style.Stroke = previous.Clone());

                    layer.Style.Stroke = BrushBase.ColorBrush(value);
                    this.SelectionViewModel.StyleLayer = layer;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }
        private void ConstructStrokeColorFlyout2()
        {
            //History
            IHistoryBase history = null;


            //Color
            this.StrokeColorPicker.ColorChangeStarted += (s, value) =>
            {
                history = new IHistoryBase("Set stroke");

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Style.CacheStroke();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.StrokeColorPicker.ColorChangeDelta += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Style.Stroke = BrushBase.ColorBrush(value);
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.StrokeColorPicker.ColorChangeCompleted += (s, value) =>
            {
                //Selection
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        this.SelectionViewModel.Color = value;
                        break;
                }
                this.SelectionViewModel.Stroke = BrushBase.ColorBrush(value);
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //History
                    var previous = layer.Style.StartingStroke;
                    int index = layer.Control.Index;
                    history.Undos.Push(() => this.ViewModel.LayerCollection.RootControls[index].Layer.
                    Style.Stroke = previous.Clone());

                    layer.Style.Stroke = BrushBase.ColorBrush(value);
                    this.SelectionViewModel.StyleLayer = layer;
                });

                //History
                this.ViewModel.Push(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate 
            };
        }
               

        #endregion


    }
}
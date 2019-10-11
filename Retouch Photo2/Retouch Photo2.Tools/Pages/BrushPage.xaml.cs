using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "BrushTool"/>.
    /// </summary>
    public sealed partial class BrushPage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        //@Converter
        private int FillOrStrokeToIndexConverter(FillOrStroke fillOrStroke) => (int)fillOrStroke;
        private int BrushTypeToIndexConverter(BrushType brushType)
        {
            switch (brushType)
            {
                case BrushType.Disabled: return 000;
                case BrushType.None: return 000;
                case BrushType.Color: return 001;
                case BrushType.LinearGradient: return 002;
                case BrushType.RadialGradient: return 003;
                case BrushType.EllipticalGradient: return 004;
                case BrushType.Image: return 005;
            }
            return 000;
        }
        private bool BrushTypeToIsEnabledConverter(BrushType brushType) => (brushType != BrushType.Disabled);

        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        
        //@Construct
        public BrushPage()
        {
            this.InitializeComponent();

            //FillOrStroke
            {
                this.FillComboBoxItem.Tapped += (s, e) =>
                {
                    this.SelectionViewModel.FillOrStroke = FillOrStroke.Fill;
                    this.SetFillOrStroke(FillOrStroke.Fill);
                    this.ViewModel.Invalidate();//Invalidate
                };
                this.StrokeComboBoxItem.Tapped += (s, e) =>
                {
                    this.SelectionViewModel.FillOrStroke = FillOrStroke.Stroke;
                    this.SetFillOrStroke(FillOrStroke.Stroke);
                    this.ViewModel.Invalidate();//Invalidate
                };
            }


            //GradientBrushType
            {
                this.StopsPicker.ComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)GradientBrushType.Linear);//ComboBox

                this.NoneComboBoxItem.Tapped += (s, e) => this.ToBrushTypeNone();
                this.ColorComboBoxItem.Tapped += (s, e) => this.ToBrushTypeColor();
                this.LinearGradientComboBoxItem.Tapped += (s, e) =>
                {
                    this.ToBrushTypeLinearGradient(isResetBrushArray: true);
                    this.StopsPicker.ComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)GradientBrushType.Linear);//ComboBox
                };
                this.RadialGradientComboBoxItem.Tapped += (s, e) =>
                {
                    this.ToBrushTypeRadialGradient(isResetBrushArray: true);
                    this.StopsPicker.ComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)GradientBrushType.Radial);//ComboBox
                };
                this.EllipticalGradientComboBoxItem.Tapped += (s, e) =>
                {
                    this.ToBrushTypeEllipticalGradient(isResetBrushArray: true);
                    this.StopsPicker.ComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)GradientBrushType.Elliptical);//ComboBox
                };
                this.ImageComboBoxItem.Tapped += (s, e) => this.ToBrushTypeImage();
            }


            //BrushType
            {
                this.StopsPicker.LinearGradientItem.Tapped += (s, e) =>
                {
                    this.ToBrushTypeLinearGradient(isResetBrushArray: false);
                    this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.LinearGradient);//ComboBox
                    this.EaseStoryboard.Begin();//Storyboard
                };
                this.StopsPicker.RadialGradientItem.Tapped += (s, e) =>
                {
                    this.ToBrushTypeRadialGradient(isResetBrushArray: false);
                    this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.RadialGradient);//ComboBox
                    this.EaseStoryboard.Begin();//Storyboard
                };
                this.StopsPicker.EllipticalGradientItem.Tapped += (s, e) =>
                {
                    this.ToBrushTypeEllipticalGradient(isResetBrushArray: false);
                    this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.EllipticalGradient);//ComboBox
                    this.EaseStoryboard.Begin();//Storyboard
                };
            }


            //Show
            {
                this.ShowControl.Tapped += (s, e) =>
                {
                    switch (this.SelectionViewModel.BrushType)
                    {
                        case BrushType.None: break;

                        case BrushType.Color:
                            {
                                switch (this.SelectionViewModel.FillOrStroke)
                                {
                                    case FillOrStroke.Fill:
                                        this.ColorPicker.Color = this.SelectionViewModel.FillColor;
                                        break;
                                    case FillOrStroke.Stroke:
                                        this.ColorPicker.Color = this.SelectionViewModel.StrokeColor;
                                        break;
                                }
                                this.ColorFlyout.ShowAt(this);//Flyout
                            }
                            break;

                        case BrushType.LinearGradient:
                        case BrushType.RadialGradient:
                        case BrushType.EllipticalGradient:
                            {
                                CanvasGradientStop[] array = this.SelectionViewModel.BrushArray;
                                this.StopsPicker.SetArray(array);
                                this.StopsFlyout.ShowAt(this);//Flyout
                            }
                            break;

                        case BrushType.Image: break;
                    }
                };

                this.ColorPicker.ColorChange += (s, value) =>
                {
                    this.SelectionViewModel.Color = value;

                    //Brush
                    this.SelectionViewModel.BrushType = BrushType.Color;

                    //Selection
                    this.SelectionViewModel.FillColor = value;
                    this.SelectionViewModel.SetValue((layer) =>
                    { 
                        //FillOrStroke
                        switch (this.SelectionViewModel.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                layer.FillColor = value;
                                break;
                            case FillOrStroke.Stroke:
                                layer.StrokeColor = value;
                                break;
                        }
                    });

                    this.ViewModel.Invalidate();//Invalidate
                };

                this.StopsPicker.StopsChanged += (s, array) =>
                {
                    //Selection
                    this.SelectionViewModel.BrushArray = (CanvasGradientStop[])array.Clone();

                    this.SelectionViewModel.SetValue(((layer) =>
                    {
                        if (layer is IGeometryLayer geometryLayer)
                        {
                            //FillOrStroke
                            switch (this.SelectionViewModel.FillOrStroke)
                            {
                                case FillOrStroke.Fill:
                                    geometryLayer.FillBrush.Array = (CanvasGradientStop[])array.Clone();
                                    break;
                                case FillOrStroke.Stroke:
                                    geometryLayer.StrokeBrush.Array = (CanvasGradientStop[])array.Clone();
                                    break;
                            }
                        }
                    }));

                    this.ViewModel.Invalidate();//Invalidate
                };
            }
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }
    }
}
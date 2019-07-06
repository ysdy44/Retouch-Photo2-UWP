using FanKit.Transformers;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.ILayer;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Numerics;
using Microsoft.Graphics.Canvas.Brushes;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers;

namespace Retouch_Photo2.Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "BrushTool"/>.
    /// </summary>
    public sealed partial class BrushPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Converter
        private int FillOrStrokeToIndexConverter(FillOrStroke fillOrStroke) => (int)fillOrStroke;
        private int BrushTypeToIndexConverter(BrushType brushType) => (int)brushType;
        private bool SelectionModeToBoolConverter(ListViewSelectionMode selectionMode) => selectionMode !=  ListViewSelectionMode.None;


        //@Construct
        public BrushPage()
        {
            this.InitializeComponent();

            //FillOrStroke
            this.FillComboBoxItem.Tapped += (s, e) =>
            {
                //ComboBox
                this.FillComboBoxItem.SetValue(ComboBox.SelectedIndexProperty, (int)FillOrStroke.Fill);

                //Brush
                this.SelectionViewModel.SetBrushFormSingleMode(FillOrStroke.Fill);

                this.ViewModel.Invalidate();//Invalidate
            };
            this.StrokeComboBoxItem.Tapped += (s, e) =>
            {
                //ComboBox
                this.FillComboBoxItem.SetValue(ComboBox.SelectedIndexProperty, (int)FillOrStroke.Stroke);

                //Brush
                this.SelectionViewModel.SetBrushFormSingleMode(FillOrStroke.Stroke);

                this.ViewModel.Invalidate();//Invalidate
            };


            //BrushType
            this.NoneComboBoxItem.Tapped += (s, e) => this.BrushTypeNone();
            this.ColorComboBoxItem.Tapped += (s, e) => this.BrushTypeColor();
            this.LinearGradientComboBoxItem.Tapped += (s, e) => this.BrushTypeLinearGradient();
            this.RadialGradientComboBoxItem.Tapped += (s, e) => this.BrushTypeRadialGradient();
            this.EllipticalGradientComboBoxItem.Tapped += (s, e) => this.BrushTypeEllipticalGradient();
            this.ImageComboBoxItem.Tapped += (s, e) => this.BrushTypeImage();

            this.StopsPicker.LinearGradientComboBoxItem.Tapped += (s, e) => this.BrushTypeLinearGradient();
            this.StopsPicker.RadialGradientComboBoxItem.Tapped += (s, e) => this.BrushTypeRadialGradient();
            this.StopsPicker.EllipticalGradientComboBoxItem.Tapped += (s, e) => this.BrushTypeEllipticalGradient();


            //Show
            this.ShowControl.Tapped += (s, e) =>
            {
                switch (this.SelectionViewModel.BrushType)
                {
                    case BrushType.None:
                        break;
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
                            this.ColorFlyout.ShowAt(this.ShowControl);//Flyout
                        }
                        break;
                    case BrushType.LinearGradient:
                    case BrushType.RadialGradient:
                    case BrushType.EllipticalGradient:
                        {
                            CanvasGradientStop[] array= this.SelectionViewModel.BrushArray;
                            this.StopsPicker.SetArray(array);

                            BrushType brushType = this.SelectionViewModel.BrushType;
                            this.StopsPicker.SetBrushType(brushType);

                            this.StopsFlyout.ShowAt(this.ShowControl);//Flyout
                        }
                        break;
                    case BrushType.Image:
                        break;
                    default:
                        break;
                }
            };

            this.ColorPicker.ColorChange += (s, value) =>
            {
                this.SelectionViewModel.Color = value;

                //Brush
                this.SelectionViewModel.BrushType = BrushType.Color;

                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        {
                            //Selection
                            this.SelectionViewModel.FillColor = value;
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.SetFillColor(value);
                            }, true);
                        }
                        break;
                    case FillOrStroke.Stroke:
                        {
                            //Selection
                            this.SelectionViewModel.StrokeColor = value;
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.SetStrokeColor(value);
                            }, true);
                        }
                        break;
                }

                this.ViewModel.Invalidate();//Invalidate
            };

            this.StopsPicker.StopsChanged += (s, array) =>
            {
                //Selection
                this.SelectionViewModel.BrushArray = (CanvasGradientStop[])array.Clone();

                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        {
                            //Selection
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                if (layer is IGeometryLayer geometryLayer)
                                {
                                    geometryLayer.FillBrush.Array= (CanvasGradientStop[])array.Clone();
                                }
                            }, true);
                        }
                        break;
                    case FillOrStroke.Stroke:
                        {
                            //Selection
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                if (layer is IGeometryLayer geometryLayer)
                                {
                                    geometryLayer.StrokeBrush.Array = (CanvasGradientStop[])array.Clone();
                                }
                            }, true);
                        }
                        break;
                }

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //BrushType
        private void BrushTypeNone()
        {
            //ComboBox
            this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.None);

            //Brush
            this.SelectionViewModel.BrushType = BrushType.None;

            //FillOrStroke
            switch (this.SelectionViewModel.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                geometryLayer.FillBrush.Type = BrushType.None;
                            }
                        }, true);
                    }
                    break;
                case FillOrStroke.Stroke:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                geometryLayer.StrokeBrush.Type = BrushType.None;
                            }
                        }, true);
                    }
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        
        private void BrushTypeColor()
        {
            //ComboBox
            this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.Color);

            //Brush
            this.SelectionViewModel.BrushType = BrushType.Color;
            switch (this.SelectionViewModel.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.SelectionViewModel.Color = this.SelectionViewModel.FillColor;
                    break;
                case FillOrStroke.Stroke:
                    this.SelectionViewModel.Color = this.SelectionViewModel.StrokeColor;
                    break;
            }

            //FillOrStroke
            switch (this.SelectionViewModel.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                geometryLayer.FillBrush.Type = BrushType.Color;
                                geometryLayer.FillBrush.Color = this.SelectionViewModel.FillColor;
                            }
                        }, true);
                    }
                    break;
                case FillOrStroke.Stroke:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                geometryLayer.StrokeBrush.Type = BrushType.Color;
                                geometryLayer.StrokeBrush.Color = this.SelectionViewModel.StrokeColor;
                            }
                        }, true);
                    }
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }

        private void BrushTypeLinearGradient()
        {
            //ComboBox
            this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.LinearGradient);

            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 startPoint = transformer.CenterTop;
            Vector2 endPoint = transformer.CenterBottom;

            this.SelectionViewModel.SetBrushToLinearGradient(startPoint, endPoint);//Initialize

            this.ViewModel.Invalidate();//Invalidate
        }

        private void BrushTypeRadialGradient()
        {
            //ComboBox
            this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.RadialGradient);

            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 center = transformer.Center;
            Vector2 point = transformer.CenterBottom;

            //Brush
            this.SelectionViewModel.BrushType = BrushType.RadialGradient;
            this.SelectionViewModel.BrushArray = Brush.GetNewArray();
            this.SelectionViewModel.BrushPoints.RadialGradientCenter = center;
            this.SelectionViewModel.BrushPoints.RadialGradientPoint = point;

            //FillOrStroke
            switch (this.SelectionViewModel.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                geometryLayer.FillBrush.Type = BrushType.RadialGradient;
                                geometryLayer.FillBrush.Array = Brush.GetNewArray();
                                geometryLayer.FillBrush.Points.RadialGradientCenter = center;
                                geometryLayer.FillBrush.Points.RadialGradientPoint = point;
                            }
                        }, true);
                    }
                    break;
                case FillOrStroke.Stroke:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                geometryLayer.StrokeBrush.Type = BrushType.RadialGradient;
                                geometryLayer.StrokeBrush.Array = Brush.GetNewArray();
                                geometryLayer.StrokeBrush.Points.RadialGradientCenter = center;
                                geometryLayer.StrokeBrush.Points.RadialGradientPoint = point;
                            }
                        }, true);
                    }
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }

        private void BrushTypeEllipticalGradient()
        {
            //ComboBox
            this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.EllipticalGradient);

            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 center = transformer.Center;
            Vector2 xPoint = transformer.CenterRight;
            Vector2 yPoint = transformer.CenterBottom;

            //Brush
            this.SelectionViewModel.BrushType = BrushType.EllipticalGradient;
            this.SelectionViewModel.BrushArray = Brush.GetNewArray();
            this.SelectionViewModel.BrushPoints.EllipticalGradientCenter = center;
            this.SelectionViewModel.BrushPoints.EllipticalGradientXPoint = xPoint;
            this.SelectionViewModel.BrushPoints.EllipticalGradientYPoint = yPoint;

            //FillOrStroke
            switch (this.SelectionViewModel.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                geometryLayer.FillBrush.Type = BrushType.EllipticalGradient;
                                geometryLayer.FillBrush.Array = Brush.GetNewArray();
                                geometryLayer.FillBrush.Points.EllipticalGradientCenter = center;
                                geometryLayer.FillBrush.Points.EllipticalGradientXPoint = xPoint;
                                geometryLayer.FillBrush.Points.EllipticalGradientYPoint = yPoint;
                            }
                        }, true);
                    }
                    break;
                case FillOrStroke.Stroke:
                    {
                        //Selection
                        this.SelectionViewModel.SetValue((layer) =>
                        {
                            if (layer is IGeometryLayer geometryLayer)
                            {
                                geometryLayer.StrokeBrush.Type = BrushType.EllipticalGradient;
                                geometryLayer.StrokeBrush.Array = Brush.GetNewArray();
                                geometryLayer.StrokeBrush.Points.EllipticalGradientCenter = center;
                                geometryLayer.StrokeBrush.Points.EllipticalGradientXPoint = xPoint;
                                geometryLayer.StrokeBrush.Points.EllipticalGradientYPoint = yPoint;
                            }
                        }, true);
                    }
                    break;
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        
        private void BrushTypeImage()
        {
            //BrushType
            this.SelectionViewModel.BrushType = BrushType.Image;

                this.ViewModel.Invalidate();//Invalidate
            }

    }
}
using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "BrushTool"/>.
    /// </summary>
    public sealed partial class BrushPage : Page
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        FillOrStroke FillOrStroke => this.SelectionViewModel.FillOrStroke;
        BrushType BrushType { get => this.SelectionViewModel.BrushType; set => this.SelectionViewModel.BrushType=value; }


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
        private bool BrushTypeToIsEnabledConverter(BrushType brushType)
        {
            switch (brushType)
            {
                case BrushType.Disabled: return false;
                case BrushType.None: return true;
                case BrushType.Color: return true;
                case BrushType.LinearGradient: return true;
                case BrushType.RadialGradient: return true;
                case BrushType.EllipticalGradient: return true;
                case BrushType.Image: return true;
            }
            return false;
        }

        private bool IsOpenConverter(bool isOpen) => isOpen && this.IsSelected;
        public bool IsSelected { private get; set; }


        //@Construct
        public BrushPage()
        {
            this.InitializeComponent();

            //FillOrStroke
            this.FillComboBoxItem.Tapped += (s, e) =>
            {
                this.SelectionViewModel.SetBrushFormSingleMode(FillOrStroke.Fill);//Brush
                this.ViewModel.Invalidate();//Invalidate
            };
            this.StrokeComboBoxItem.Tapped += (s, e) =>
            {
                this.SelectionViewModel.SetBrushFormSingleMode(FillOrStroke.Stroke);//Brush
                this.ViewModel.Invalidate();//Invalidate
            };


            //BrushType
            this.StopsPicker.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)GradientBrushType.LinearGradient);//ComboBox

            this.NoneComboBoxItem.Tapped += (s, e) => this.BrushTypeNone();
            this.ColorComboBoxItem.Tapped += (s, e) => this.BrushTypeColor();
            this.LinearGradientComboBoxItem.Tapped += (s, e) =>
            {
                this.BrushTypeLinearGradient();
                this.StopsPicker.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)GradientBrushType.LinearGradient);//ComboBox
            };
            this.RadialGradientComboBoxItem.Tapped += (s, e) =>
            {
                this.BrushTypeRadialGradient();
                this.StopsPicker.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)GradientBrushType.RadialGradient);//ComboBox
            };
            this.EllipticalGradientComboBoxItem.Tapped += (s, e) =>
            {
                this.BrushTypeEllipticalGradient();
                this.StopsPicker.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)GradientBrushType.EllipticalGradient);//ComboBox
            };
            this.ImageComboBoxItem.Tapped += (s, e) => this.BrushTypeImage();

            this.StopsPicker.LinearGradientComboBoxItem.Tapped += (s, e) =>
            {
                this.BrushTypeLinearGradient();
                this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.LinearGradient);//ComboBox
                this.EaseStoryboard.Begin();//Storyboard
            };
            this.StopsPicker.RadialGradientComboBoxItem.Tapped += (s, e) =>
            {
                this.BrushTypeRadialGradient();
                this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.RadialGradient);//ComboBox
                this.EaseStoryboard.Begin();//Storyboard
            };
            this.StopsPicker.EllipticalGradientComboBoxItem.Tapped += (s, e) =>
            {
                this.BrushTypeEllipticalGradient();
                this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.EllipticalGradient);//ComboBox
                this.EaseStoryboard.Begin();//Storyboard
            };


            //Show
            this.ShowControl.Tapped += (s, e) =>
            {
                switch (this.BrushType)
                {
                    case BrushType.None:
                        break;
                    case BrushType.Color:
                        {
                            switch (this.FillOrStroke)
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
                this.BrushType = BrushType.Color;

                //FillOrStroke
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        {
                            //Selection
                            this.SelectionViewModel.FillColor = value;
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.FillColor=value;
                            }, true);
                        }
                        break;
                    case FillOrStroke.Stroke:
                        {
                            //Selection
                            this.SelectionViewModel.StrokeColor = value;
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                layer.StrokeColor = value;
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
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        {
                            //Selection
                            this.SelectionViewModel.SetValue((layer) =>
                            {
                                if (layer is IGeometryLayer geometryLayer)
                                {
                                    geometryLayer.FillBrush.Array = (CanvasGradientStop[])array.Clone();
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
            //Brush
            this.BrushType = BrushType.None;

            //FillOrStroke
            switch (this.FillOrStroke)
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
            //Brush
            this.BrushType = BrushType.Color;
            switch (this.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.SelectionViewModel.Color = this.SelectionViewModel.FillColor;
                    break;
                case FillOrStroke.Stroke:
                    this.SelectionViewModel.Color = this.SelectionViewModel.StrokeColor;
                    break;
            }

            //FillOrStroke
            switch (this.FillOrStroke)
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
            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 startPoint = transformer.CenterTop;
            Vector2 endPoint = transformer.CenterBottom;

            this.SelectionViewModel.SetBrushToLinearGradient(startPoint, endPoint);//Initialize

            this.ViewModel.Invalidate();//Invalidate
        }

        private void BrushTypeRadialGradient()
        {
            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 center = transformer.Center;
            Vector2 point = transformer.CenterBottom;

            //Brush
            this.BrushType = BrushType.RadialGradient;
            this.SelectionViewModel.BrushArray = Brush.GetNewArray();
            this.SelectionViewModel.BrushPoints.RadialGradientCenter = center;
            this.SelectionViewModel.BrushPoints.RadialGradientPoint = point;

            //FillOrStroke
            switch (this.FillOrStroke)
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
            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 center = transformer.Center;
            Vector2 xPoint = transformer.CenterRight;
            Vector2 yPoint = transformer.CenterBottom;

            //Brush
            this.BrushType = BrushType.EllipticalGradient;
            this.SelectionViewModel.BrushArray = Brush.GetNewArray();
            this.SelectionViewModel.BrushPoints.EllipticalGradientCenter = center;
            this.SelectionViewModel.BrushPoints.EllipticalGradientXPoint = xPoint;
            this.SelectionViewModel.BrushPoints.EllipticalGradientYPoint = yPoint;

            //FillOrStroke
            switch (this.FillOrStroke)
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
            this.BrushType = BrushType.Image;

            this.ViewModel.Invalidate();//Invalidate
        }

    }
}
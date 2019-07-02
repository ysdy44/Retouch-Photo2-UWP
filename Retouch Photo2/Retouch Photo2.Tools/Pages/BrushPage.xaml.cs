using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers.ILayer;
using Retouch_Photo2.Tools.Models;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

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
                this.SelectionViewModel.FillOrStroke = FillOrStroke.Fill;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is IGeometryLayer geometryLayer)
                    {
                        //Brush
                        this.SelectionViewModel.BrushType = geometryLayer.FillBrush.Type;
                        this.SelectionViewModel.SetBrush(geometryLayer.FillBrush);
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.StrokeComboBoxItem.Tapped += (s, e) =>
            {
                //ComboBox
                this.StrokeComboBoxItem.SetValue(ComboBox.SelectedIndexProperty, (int)FillOrStroke.Stroke);

                //Brush
                this.SelectionViewModel.FillOrStroke = FillOrStroke.Stroke;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is IGeometryLayer geometryLayer)
                    {
                        //Brush
                        this.SelectionViewModel.BrushType = geometryLayer.StrokeBrush.Type;
                        this.SelectionViewModel.SetBrush(geometryLayer.StrokeBrush);
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };



            //BrushType
            this.NoneComboBoxItem.Tapped += (s, e) =>
            {
                //ComboBox
                this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.None);

                //Brush
                this.SelectionViewModel.BrushType = BrushType.None;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is IGeometryLayer geometryLayer)
                    {
                        switch (this.SelectionViewModel.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                geometryLayer.FillBrush.Type = BrushType.None;
                                break;
                            case FillOrStroke.Stroke:
                                geometryLayer.StrokeBrush.Type = BrushType.None;
                                break;
                        }
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.ColorComboBoxItem.Tapped += (s, e) =>
            {
                //ComboBox
                this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.Color);

                //Brush
                this.SelectionViewModel.BrushType = BrushType.Color;
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill: this.SelectionViewModel.BrushColor = this.SelectionViewModel.FillColor; break;
                    case FillOrStroke.Stroke: this.SelectionViewModel.BrushColor = this.SelectionViewModel.StrokeColor; break;
                }

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is IGeometryLayer geometryLayer)
                    {
                        switch (this.SelectionViewModel.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                {
                                    geometryLayer.FillBrush.Type = BrushType.Color;
                                    geometryLayer.FillBrush.Color = this.SelectionViewModel.FillColor;
                                }
                                break;
                            case FillOrStroke.Stroke:
                                {
                                    geometryLayer.StrokeBrush.Type = BrushType.Color;
                                    geometryLayer.FillBrush.Color = this.SelectionViewModel.StrokeColor;
                                }
                                break;
                        }
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.LinearGradientComboBoxItem.Tapped += (s, e) =>
            {
                //ComboBox
                this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.LinearGradient);

                Transformer transformer = this.SelectionViewModel.Transformer;
                Vector2 startPoint = transformer.CenterTop;
                Vector2 endPoint = transformer.CenterBottom;

                this.SelectionViewModel.InitializeLinearGradient(startPoint, endPoint);//Initialize
                
                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadialGradientComboBoxItem.Tapped += (s, e) =>
            {
                //ComboBox
                this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.RadialGradient);
                
                Transformer transformer = this.SelectionViewModel.Transformer;
                Vector2 center = transformer.Center;
                Vector2 point = transformer.CenterBottom;

                //Brush
                this.SelectionViewModel.BrushType = BrushType.RadialGradient;
                this.SelectionViewModel.BrushArray = new CanvasGradientStop[]
                {
                      new CanvasGradientStop{Color= Colors.White, Position=0.0f },
                      new CanvasGradientStop{Color= Colors.Gray, Position=1.0f }
                };
                this.SelectionViewModel.BrushRadialGradientCenter = center;
                this.SelectionViewModel.BrushRadialGradientPoint = point;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is IGeometryLayer geometryLayer)
                    {
                        switch (this.SelectionViewModel.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                {
                                    geometryLayer.FillBrush.Type = BrushType.RadialGradient;
                                    geometryLayer.FillBrush.RadialGradientCenter = center;
                                    geometryLayer.FillBrush.RadialGradientPoint = point;
                                }
                                break;
                            case FillOrStroke.Stroke:
                                {
                                    geometryLayer.StrokeBrush.Type = BrushType.RadialGradient;
                                    geometryLayer.StrokeBrush.RadialGradientCenter = center;
                                    geometryLayer.StrokeBrush.RadialGradientPoint = point;
                                }
                                break;
                        }
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.EllipticalGradientComboBoxItem.Tapped += (s, e) =>
            {
                //ComboBox
                this.BrushTypeComboBox.SetValue(ComboBox.SelectedIndexProperty, (int)BrushType.EllipticalGradient);
                
                Transformer transformer = this.SelectionViewModel.Transformer;
                Vector2 center = transformer.Center;
                Vector2 xPoint = transformer.CenterRight;
                Vector2 yPoint = transformer.CenterBottom;

                //Brush
                this.SelectionViewModel.BrushType = BrushType.EllipticalGradient;
                this.SelectionViewModel.BrushArray = new CanvasGradientStop[]
                {
                      new CanvasGradientStop{Color= Colors.White, Position=0.0f },
                      new CanvasGradientStop{Color= Colors.Gray, Position=1.0f }
                };
                this.SelectionViewModel.BrushEllipticalGradientCenter = center;
                this.SelectionViewModel.BrushEllipticalGradientXPoint = xPoint;
                this.SelectionViewModel.BrushEllipticalGradientYPoint = yPoint;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is IGeometryLayer geometryLayer)
                    {
                        switch (this.SelectionViewModel.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                {
                                    geometryLayer.FillBrush.Type = BrushType.EllipticalGradient;
                                    geometryLayer.FillBrush.EllipticalGradientCenter = center;
                                    geometryLayer.FillBrush.EllipticalGradientXPoint = xPoint;
                                    geometryLayer.FillBrush.EllipticalGradientYPoint = yPoint;
                                }
                                break;
                            case FillOrStroke.Stroke:
                                {
                                    geometryLayer.StrokeBrush.Type = BrushType.EllipticalGradient;
                                    geometryLayer.StrokeBrush.EllipticalGradientCenter = center;
                                    geometryLayer.StrokeBrush.EllipticalGradientXPoint = xPoint;
                                    geometryLayer.StrokeBrush.EllipticalGradientYPoint = yPoint;
                                }
                                break;
                        }
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.ImageComboBoxItem.Tapped += (s, e) =>
            {
                //BrushType
                this.SelectionViewModel.BrushType = BrushType.Image;

                this.ViewModel.Invalidate();//Invalidate
            };


            //Show
            this.ShowControl.Tapped += (s, e) =>
            {
                switch (this.SelectionViewModel.BrushType)
                {
                    case BrushType.None:
                        break;
                    case BrushType.Color:
                        {
                            this.ColorPicker.Color = this.SelectionViewModel.BrushColor;
                            this.ColorFlyout.ShowAt(this.ShowControl);
                        }
                        break;
                    case BrushType.LinearGradient:
                    case BrushType.RadialGradient:
                    case BrushType.EllipticalGradient:
                        {
                            this.BrushPicker.Array = this.SelectionViewModel.BrushArray;
                            this.BrushFlyout.ShowAt(this.ShowControl);
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
                //Brush
                this.SelectionViewModel.BrushType = BrushType.Color;
                this.SelectionViewModel.BrushColor = value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    if (layer is IGeometryLayer geometryLayer)
                    {
                        switch (this.SelectionViewModel.FillOrStroke)
                        {
                            case FillOrStroke.Fill:
                                {
                                    geometryLayer.FillBrush.Type = BrushType.Color;
                                    geometryLayer.FillBrush.Color = value;
                                }
                                break;
                            case FillOrStroke.Stroke:
                                {
                                    geometryLayer.StrokeBrush.Type = BrushType.Color;
                                    geometryLayer.StrokeBrush.Color = value;
                                }
                                break;
                        }
                    }
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }
}
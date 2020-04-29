using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public sealed partial class BrushTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        
        ListViewSelectionMode Mode => this.SelectionViewModel.SelectionMode;
        BrushType BrushType => this.SelectionViewModel.BrushType;

        //@Static
        /// <summary> Navigate to <see cref="PhotosPage"/> </summary>
        public static Action Image;


        //@Converter
        private int FillOrStrokeToIndexConverter(FillOrStroke fillOrStroke) => (int)fillOrStroke;
        private int BrushTypeToIndexConverter(BrushType brushType) => (int)brushType;

        
        //@Construct
        public BrushTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructFillOrStroke();
            this.ConstructBrushType();
            this.ConstructShowControl();
        }


        //FillOrStroke
        private void ConstructFillOrStroke()
        {
            this.FillOrStrokeComboBox.SelectionChanged += (s, e) =>
            {
                FillOrStroke fillOrStroke = (FillOrStroke)this.FillOrStrokeComboBox.SelectedIndex;
                if (this.SelectionViewModel.FillOrStroke == fillOrStroke) return;

                this.SelectionViewModel.SetFillOrStroke(fillOrStroke);
                this.ViewModel.Invalidate();//Invalidate
            };
        }
        

        //BrushType
        private void ConstructBrushType()
        {
            this.BrushTypeComboBox.SelectionChanged += (s, e) =>
            {
                BrushType brushType = (BrushType)this.BrushTypeComboBox.SelectedIndex;
                if (this.SelectionViewModel.BrushType == brushType) return;

                switch (brushType)
                {
                    case BrushType.None: this.ToBrushTypeNone(); break;
                    case BrushType.Color: this.ToBrushTypeColor(); break;
                    case BrushType.LinearGradient: this.ToBrushTypeLinearGradient(isResetBrushArray: true); break;
                    case BrushType.RadialGradient: this.ToBrushTypeRadialGradient(isResetBrushArray: true); break;
                    case BrushType.EllipticalGradient: this.ToBrushTypeEllipticalGradient(isResetBrushArray: true); break;
                    case BrushType.Image:
                        this.ToBrushTypeImage();
                        BrushTool.Image?.Invoke();
                        break;
                }
            };
            
        }


        //ShowControl
        private bool _isStopsFlyoutShowed;
        private void ConstructShowControl()
        {
            this.StopsFlyout.Opened += (s, e) => this._isStopsFlyoutShowed = true;
            this.StopsFlyout.Closed += (s, e) => this._isStopsFlyoutShowed = false;
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

                    case BrushType.Image:
                        BrushTool.Image?.Invoke();
                        break;
                }
            };

            this.ColorPicker.ColorChange += (s, value) =>
            {
                this.SetColor(value);
            };

            this.StopsPicker.StopsChanged += (s, array) =>
            {
                this.SetArray(array);
            };
        }


        public void OnNavigatedTo()
        {
            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.Single:
                    ILayer layer = this.SelectionViewModel.Layer;

                    switch (this.SelectionViewModel.FillOrStroke)
                    {
                        case FillOrStroke.Fill:
                            this.SelectionViewModel.BrushType = layer.StyleManager.FillBrush.Type;
                            this.SelectionViewModel.BrushPoints = layer.StyleManager.FillBrush.Points;
                            this.SelectionViewModel.BrushImageDestination = layer.StyleManager.FillBrush.PhotoDestination;
                            break;
                        case FillOrStroke.Stroke:
                            this.SelectionViewModel.BrushType = layer.StyleManager.StrokeBrush.Type;
                            this.SelectionViewModel.BrushPoints = layer.StyleManager.StrokeBrush.Points;
                            this.SelectionViewModel.BrushImageDestination = layer.StyleManager.StrokeBrush.PhotoDestination;
                            break;
                    }
                    break;
            }
        }
        public void OnNavigatedFrom() { }

    }

    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public sealed partial class BrushTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/Brush");

            this.FillOrStrokeTextBlock.Text = resource.GetString("/Tools/Brush_FillOrStroke");
            this.FillComboBoxItem.Content = resource.GetString("/ToolElements/Fill");
            this.StrokeComboBoxItem.Content = resource.GetString("/ToolElements/Stroke");

            this.BrushTypeTextBlock.Text = resource.GetString("/Tools/Brush_Type");
            this.NoneComboBoxItem.Content = resource.GetString("/Tools/Brush_TypeNone");
            this.ColorComboBoxItem.Content = resource.GetString("/Tools/Brush_TypeColor");
            this.LinearComboBoxItem.Content = resource.GetString("/Tools/Brush_TypeLinear");
            this.RadialComboBoxItem.Content = resource.GetString("/Tools/Brush_TypeRadial");
            this.EllipticalComboBoxItem.Content = resource.GetString("/Tools/Brush_TypeElliptical");
            this.ImageComboBoxItem.Content = resource.GetString("/Tools/Brush_TypeImage");

            this.BrushTextBlock.Text = resource.GetString("/Tools/Brush_Brush");
        }


        //@Content
        public ToolType Type => ToolType.Brush;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new BrushIcon();
        readonly ToolButton _button = new ToolButton(new BrushIcon());


        BrushPoints _startingBrushPoints;
        BrushOperateMode _operateMode = BrushOperateMode.None;

        public void Starting(Vector2 point)
        {
        }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            if (this.BrushType == BrushType.None || this.BrushType == BrushType.Color)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Vector2 startPoint = Vector2.Transform(startingPoint, inverseMatrix);
                Vector2 endPoint = Vector2.Transform(point, inverseMatrix);

                BrushPoints brushPoints = new BrushPoints
                {
                    LinearGradientStartPoint = startPoint,
                    LinearGradientEndPoint = endPoint,
                };
                this._startingBrushPoints = brushPoints;
                this.Gradient(GradientBrushType.Linear, brushPoints, isResetBrushArray: true);

                this._operateMode = BrushOperateMode.LinearEndPoint;
            }
            else
            {
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                BrushType brushType = this.BrushType;

                BrushPoints brushPoints = this.SelectionViewModel.BrushPoints;
                this._startingBrushPoints = brushPoints;

                this._operateMode = BrushOperateHelper.ContainsNodeMode(startingPoint, brushType, brushPoints, matrix);
            }

            this.ViewModel.Invalidate();//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;
            if (this._operateMode == BrushOperateMode.None) return;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();

            BrushPoints? brushPointsController = BrushOperateHelper.Controller(point, this._startingBrushPoints, this._operateMode, inverseMatrix);

            if (brushPointsController is BrushPoints brushPoints)
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    //FillOrStroke
                    switch (this.SelectionViewModel.FillOrStroke)
                    {
                        case FillOrStroke.Fill:
                            layer.StyleManager.FillBrush.Points = brushPoints;
                            break;
                        case FillOrStroke.Stroke:
                            layer.StyleManager.StrokeBrush.Points = brushPoints;
                            break;
                    }
                });

                this.SelectionViewModel.BrushPoints = brushPoints;//Selection
                this.ViewModel.Invalidate();//Invalidate
            }
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            //Selection
            if (this.Mode == ListViewSelectionMode.None) return;

            this._operateMode = BrushOperateMode.None;

            if (isSingleStarted == false)
            {
                //TransformerTool
                ITransformerTool transformerTool = this.TipViewModel.TransformerTool;
                transformerTool.SelectSingleLayer(startingPoint);
            }
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            BrushType brushType = this.BrushType;
            BrushPoints brushPoints = this.SelectionViewModel.BrushPoints;
            CanvasGradientStop[] brushArray = this.SelectionViewModel.BrushArray;
            Transformer imageDestination = this.SelectionViewModel.BrushImageDestination;

            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
            Windows.UI.Color accentColor = this.ViewModel.AccentColor;

            BrushOperateHelper.Draw(drawingSession, brushType, brushPoints, brushArray, imageDestination, matrix, accentColor);
        }

    }
    
    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public sealed partial class BrushTool : Page, ITool
    {

        private void SetColor(Color value)
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
                        layer.StyleManager.FillBrush.Color = value;
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Color = value;
                        break;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        private void SetArray(CanvasGradientStop[] array)
        {
            //Selection
            this.SelectionViewModel.BrushArray = (CanvasGradientStop[])array.Clone();

            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Array = (CanvasGradientStop[])array.Clone();
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Array = (CanvasGradientStop[])array.Clone();
                        break;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }


        /// <summary>
        /// To a gradient brush.
        /// </summary>
        /// <param name="brushPoints"> The brush-points </param>
        public void Gradient(GradientBrushType gradientBrushType, BrushPoints brushPoints, bool isResetBrushArray)
        {
            //GradientBrushType
            BrushType brushType = BrushType.LinearGradient;
            switch (gradientBrushType)
            {
                case GradientBrushType.Linear: brushType = BrushType.LinearGradient; break;
                case GradientBrushType.Radial: brushType = BrushType.RadialGradient; break;
                case GradientBrushType.Elliptical: brushType = BrushType.EllipticalGradient; break;
            }

            //Brush
            this.SelectionViewModel.BrushType = brushType;
            if (isResetBrushArray) this.SelectionViewModel.BrushArray = GreyWhiteMeshHelpher.GetGradientStopArray();
            this.SelectionViewModel.BrushPoints = brushPoints;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Type = brushType;
                        if (isResetBrushArray)
                            layer.StyleManager.StrokeBrush.Array = GreyWhiteMeshHelpher.GetGradientStopArray();
                        layer.StyleManager.StrokeBrush.Points = brushPoints;
                        break;

                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Type = brushType;
                        if (isResetBrushArray)
                            layer.StyleManager.FillBrush.Array = GreyWhiteMeshHelpher.GetGradientStopArray();
                        layer.StyleManager.FillBrush.Points = brushPoints;
                        break;
                }
            });
        }

    }
    
    /// <summary>
    /// <see cref="ITool"/>'s BrushTool.
    /// </summary>
    public sealed partial class BrushTool : Page, ITool
    {

        private void ToBrushTypeNone()
        {
            //Brush
            this.SelectionViewModel.BrushType = BrushType.None;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Type = BrushType.None;
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Type = BrushType.None;
                        break;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeColor()
        {
            //Brush
            this.SelectionViewModel.BrushType = BrushType.Color;

            //FillOrStroke
            switch (this.SelectionViewModel.FillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.SelectionViewModel.Color = this.SelectionViewModel.FillColor;
                    break;
                case FillOrStroke.Stroke:
                    this.SelectionViewModel.Color = this.SelectionViewModel.StrokeColor;
                    break;
            }

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Type = BrushType.Color;
                        layer.StyleManager.FillBrush.Color = this.SelectionViewModel.FillColor;
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Type = BrushType.Color;
                        layer.StyleManager.StrokeBrush.Color = this.SelectionViewModel.StrokeColor;
                        break;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeLinearGradient(bool isResetBrushArray)
        {
            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 startPoint = transformer.CenterTop;
            Vector2 endPoint = transformer.CenterBottom;

            BrushPoints brushPoints = new BrushPoints
            {
                LinearGradientStartPoint = startPoint,
                LinearGradientEndPoint = endPoint,
            };
            this.Gradient(GradientBrushType.Linear, brushPoints, isResetBrushArray: isResetBrushArray);

            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeRadialGradient(bool isResetBrushArray)
        {
            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 center = transformer.Center;
            Vector2 point = transformer.CenterBottom;

            BrushPoints brushPoints = new BrushPoints
            {
                RadialGradientCenter = center,
                RadialGradientPoint = point,
            };
            this.Gradient(GradientBrushType.Radial, brushPoints, isResetBrushArray: isResetBrushArray);

            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeEllipticalGradient(bool isResetBrushArray)
        {
            Transformer transformer = this.SelectionViewModel.Transformer;
            Vector2 center = transformer.Center;
            Vector2 xPoint = transformer.CenterRight;
            Vector2 yPoint = transformer.CenterBottom;

            BrushPoints brushPoints = new BrushPoints
            {
                EllipticalGradientCenter = center,
                EllipticalGradientXPoint = xPoint,
                EllipticalGradientYPoint = yPoint,
            };
            this.Gradient(GradientBrushType.Elliptical, brushPoints, isResetBrushArray: isResetBrushArray);

            this.ViewModel.Invalidate();//Invalidate
        }

        private void ToBrushTypeImage()
        {
            //BrushType
            this.SelectionViewModel.BrushType = BrushType.Image;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                //FillOrStroke
                switch (this.SelectionViewModel.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        layer.StyleManager.FillBrush.Type = BrushType.Image;
                        break;
                    case FillOrStroke.Stroke:
                        layer.StyleManager.StrokeBrush.Type = BrushType.Image;
                        break;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

    }
    
}
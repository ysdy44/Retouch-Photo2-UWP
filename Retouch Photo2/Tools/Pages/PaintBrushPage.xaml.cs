using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.ViewModels;
using System;
using System.Linq;
using System.Numerics;
using Windows.UI;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Tools.Pages
{
    public sealed partial class PaintBrushPage : ToolPage
    {
        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        //Converter
        public int FillOrStrokeConverter(Retouch_Photo2.Brushs.FillOrStroke fillOrStroke) => (int)fillOrStroke;
        public int BrushConverter(Retouch_Photo2.Brushs.Brush brush)
        {
            if (brush == null) return 0;

            return (int)brush.Type;
        }
        

        public PaintBrushPage()
        {
            this.InitializeComponent();

            //Color
            this.ColorPicker.ColorChange += (s, color) =>
            {
                if (this.ViewModel.Brush == null) return;
              
                this.ViewModel.Brush.Color = color;

                this.BrushShowControl.Invalidate();
                this.ViewModel.Invalidate();
            };
            //Brush
            this.BrushPicker.StopsChange += () =>
            {
                if (this.ViewModel.Brush == null) return;

                this.BrushShowControl.Invalidate();
                this.ViewModel.Invalidate();
            };


            //Show
            this.BrushShowControl.Tapped += (s, e) =>
            {
                if (this.ViewModel.Brush == null) return;

                switch (this.ViewModel.Brush.Type)
                {
                    case BrushType.None: break;
                    case BrushType.Color:
                        this.ColorPicker.Color = this.ViewModel.Brush.Color;
                        this.ColorFlyout.ShowAt(this.BrushShowControl);
                        break;
                    case BrushType.LinearGradient:
                    case BrushType.RadialGradient:
                    case BrushType.EllipticalGradient:
                        this.BrushPicker.Brush = this.ViewModel.Brush;
                        this.PickerFlyout.ShowAt(this.BrushShowControl);
                        break;
                    case BrushType.Image: break;
                    default: break;
                }
            };


            //FillOrStroke
            this.FillOrStrokeComboBox.ItemsSource = from FillOrStroke item in Enum.GetValues(typeof(FillOrStroke)) select item;
            this.FillOrStrokeComboBox.SelectionChanged += (s, e) =>
            {
                if (this.ViewModel.Brush == null) return;

                FillOrStroke type = (FillOrStroke)this.FillOrStrokeComboBox.SelectedIndex;
                FillOrStroke oldType = this.ViewModel.FillOrStroke;
                if (type == oldType) return;
                          
                if (this.ViewModel.GeometryLayer == null) return;
                switch (type)
                {
                    case FillOrStroke.Fill:
                        this.ViewModel.SetFillOrStroke(FillOrStroke.Fill, this.ViewModel.GeometryLayer.FillBrush);
                        break;

                    case FillOrStroke.Stroke:
                        this.ViewModel.SetFillOrStroke(FillOrStroke.Stroke, this.ViewModel.GeometryLayer.StrokeBrush);
                        break;
                    default:
                        break;
                }
            };


            //BrushType
            this.BrushTypeComboBox.ItemsSource = from BrushType item in Enum.GetValues(typeof(BrushType)) select item;
            this.BrushTypeComboBox.SelectionChanged += (s, e) =>
            {
                if (this.ViewModel.Brush == null) return;

                BrushType type = (BrushType)this.BrushTypeComboBox.SelectedIndex;
                BrushType oldType = this.ViewModel.Brush.Type;
                if (type == oldType) return;

                //Initialize
                switch (type)
                {
                    case BrushType.None: this.SetNone(); break;
                    case BrushType.Color: this.SetColor(oldType); break;
                    case BrushType.LinearGradient: this.SetLinearGradient(oldType); break;
                    case BrushType.RadialGradient: this.SetRadialGradient(oldType); break;
                    case BrushType.EllipticalGradient: this.SetEllipticalGradient(oldType); break;
                    case BrushType.Image: this.SetImage(); break;
                    default: break;
                }
            };
                       
        }



        private void SetNone()
        {
            this.ViewModel.Brush.Type = BrushType.None;
            this.BrushShowControl.Invalidate();
            this.ViewModel.Invalidate();
        }

        private void SetColor(BrushType oldType)
        {
            switch (oldType)
            {
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    CanvasGradientStop stop = this.ViewModel.Brush.Array.First();
                    this.ViewModel.Brush.Color = stop.Color;
                    break;
                default:
                    this.ViewModel.Brush.Color = Colors.Gray;
                    break;
            }

            this.ViewModel.Brush.Type = BrushType.Color;
            this.BrushShowControl.Invalidate();
            this.ViewModel.Invalidate();
        }



        private void SetLinearGradient(BrushType oldType)
        {
            if (oldType != BrushType.LinearGradient)
            {
                if (this.ViewModel.Transformer is Transformer transformer)
                {
                    this.ViewModel.Brush.LinearGradientManager.Initialize(transformer.DstLeftTop, transformer.DstRightBottom);

                    this.ViewModel.Brush.Type = BrushType.LinearGradient;
                    this.BrushShowControl.Invalidate();
                    this.ViewModel.Invalidate();
                }
            }
        }


        private void SetRadialGradient(BrushType oldType)
        {
            if (oldType != BrushType.RadialGradient)
            {
                if (this.ViewModel.Transformer is Transformer transformer)
                {
                    float radius = Vector2.Distance(transformer.DstLeftTop, transformer.DstLeftBottom) / 2;
                    this.ViewModel.Brush.RadialGradientManager.Initialize(transformer.DstCenter, radius);

                    this.ViewModel.Brush.Type = BrushType.RadialGradient;
                    this.BrushShowControl.Invalidate();
                    this.ViewModel.Invalidate();
                }
            }
        }


        private void SetEllipticalGradient(BrushType oldType)
        {
            if (oldType != BrushType.EllipticalGradient)
            {
                if (this.ViewModel.Transformer is Transformer transformer)
                {
                    float radiusX = Vector2.Distance(transformer.DstLeft, transformer.DstRight) / 2;
                    float radiusY = Vector2.Distance(transformer.DstTop, transformer.DstBottom) / 2;
                    this.ViewModel.Brush.EllipticalGradientManager.Initialize(transformer.DstCenter, radiusX, radiusY);

                    this.ViewModel.Brush.Type = BrushType.EllipticalGradient;
                    this.BrushShowControl.Invalidate();
                    this.ViewModel.Invalidate();
                    return;
                }
            }
        }

        private void SetImage()
        {
            this.ViewModel.Brush.Type = BrushType.Image;
            this.BrushShowControl.Invalidate();
            this.ViewModel.Invalidate();
        }

    }
}

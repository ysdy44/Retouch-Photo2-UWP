using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Models.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    public sealed partial class BrushControl : UserControl
    {
        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        float CanvasWidth, CanvasHieght;
        Vector2 CanvasCenter;
        
        #region DependencyProperty

        public GeometryLayer GeometryLayer
        {
            get { return (GeometryLayer)GetValue(GeometryLayerProperty); }
            set { SetValue(GeometryLayerProperty, value); }
        }
        public static readonly DependencyProperty GeometryLayerProperty = DependencyProperty.Register(nameof(GeometryLayer), typeof(GeometryLayer), typeof(BrushControl), new PropertyMetadata(null, (sender, e) =>
        {
            BrushControl con = (BrushControl)sender;

            if (e.NewValue is GeometryLayer value)
            {
                con.ComboBox.IsEnabled = true;
                con.ComboBox.SelectedIndex = (int)value.FillBrush.Type;
                con.Border.Visibility = Visibility.Visible;
            }
            else
            {
                con.ComboBox.IsEnabled = false;
                con.ComboBox.SelectedIndex = 0;
                con.Border.Visibility = Visibility.Collapsed;
            }
        }));

        #endregion

        public BrushControl()
        {
            this.InitializeComponent();
            this.ColorPicker.ColorChange += (s,color) =>
            {
                this.GeometryLayer.FillBrush.Color = color;

                this.CanvasControl.Invalidate();
                this.ViewModel.Invalidate();
            };
            this.PickerPicker.StopsChange += () =>
            {
                this.CanvasControl.Invalidate();
                this.ViewModel.Invalidate();
            };


            // Foreach:
            //     get enum0, enum1, enum2, enum3......
            this.ComboBox.ItemsSource = from BrushType item in Enum.GetValues(typeof(BrushType)) select item;
            this.ComboBox.SelectionChanged += this.SelectionChanged;


            this.Border.Tapped += (s, e) =>
            {
                if (this.GeometryLayer == null) return;

                switch (this.GeometryLayer.FillBrush.Type)
                {
                    case BrushType.None:
                        break;

                    case BrushType.Color:
                        this.ColorPicker.Color = this.GeometryLayer.FillBrush.Color;
                        this.ColorFlyout.ShowAt(this.Border);
                        break;

                    case BrushType.LinearGradient:
                        this.PickerPicker.Brush = this.GeometryLayer.FillBrush;
                        this.PickerFlyout.ShowAt(this.Border);
                        break;

                    case BrushType.RadialGradient:
                        this.PickerPicker.Brush = this.GeometryLayer.FillBrush;
                        this.PickerFlyout.ShowAt(this.Border);
                        break;

                    case BrushType.Image:
                        break;

                    default:
                        break;
                }
            };


            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                float width = (float)e.NewSize.Width;
                float height = (float)e.NewSize.Height;

                this.CanvasWidth = width;
                this.CanvasHieght = height;
                this.CanvasCenter = new Vector2(width/2,height/2);
            };
            this.CanvasControl.Draw += (s, arges) =>
            {
                if (this.GeometryLayer == null) return;

                switch (this.GeometryLayer.FillBrush.Type)
                {
                    case BrushType.None:
                        arges.DrawingSession.Clear(Colors.White);
                        arges.DrawingSession.DrawLine(0, 0, this.CanvasWidth, this.CanvasHieght, Colors.DodgerBlue);
                        arges.DrawingSession.DrawLine(0, this.CanvasHieght, this.CanvasWidth, 0, Colors.DodgerBlue);
                        break;

                    case BrushType.Color:
                        arges.DrawingSession.Clear(this.GeometryLayer.FillBrush.Color);
                        break;

                    case BrushType.LinearGradient:
                        arges.DrawingSession.FillRectangle(0, 0, this.CanvasWidth, this.CanvasHieght, new CanvasLinearGradientBrush(this.CanvasControl, this.GeometryLayer.FillBrush.Array)
                        {
                            StartPoint = new Vector2(0, this.CanvasCenter.Y),
                            EndPoint = new Vector2(this.CanvasWidth, this.CanvasCenter.Y)
                        });
                        break;

                    case BrushType.RadialGradient:
                        float radius = Math.Min(this.CanvasWidth, this.CanvasHieght) / 2;
                        arges.DrawingSession.FillRectangle(0, 0, this.CanvasWidth, this.CanvasHieght, new CanvasRadialGradientBrush(this.CanvasControl, this.GeometryLayer.FillBrush.Array)
                        {
                            Center = this.CanvasCenter,
                            RadiusX = radius,
                            RadiusY = radius
                        });
                        break;

                    case BrushType.EllipticalGradient:
                        arges.DrawingSession.FillRectangle(0, 0, this.CanvasWidth, this.CanvasHieght, new CanvasRadialGradientBrush(this.CanvasControl, this.GeometryLayer.FillBrush.Array)
                        {
                            Center = this.CanvasCenter,
                            RadiusX = this.CanvasWidth / 2,
                            RadiusY = this.CanvasHieght / 2
                        });
                        break;

                    case BrushType.Image:
                        break;

                    default:
                        break;
                }

            };
        }



        public void ChangeSelectedIndex(int index)
        {
            this.ComboBox.SelectionChanged -= this.SelectionChanged;
            this.ComboBox.SelectedIndex = index;
            this.ComboBox.SelectionChanged += this.SelectionChanged;

            this.CanvasControl.Invalidate();
        }
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.GeometryLayer == null) return;

            BrushType type = (BrushType)this.ComboBox.SelectedIndex;
            BrushType oldType = this.GeometryLayer.FillBrush.Type;

            //Initialize
            if (type == BrushType.Color && oldType != BrushType.Color)
            {
                this.GeometryLayer.FillBrush.Color = this.GeometryLayer.FillBrush.Array.First().Color;
            }

            //Initialize
            if (type == BrushType.LinearGradient&& oldType!= BrushType.LinearGradient)
            {
                this.GeometryLayer.FillBrush.LinearGradientManager.Initialize
                (
                    startPoint: this.GeometryLayer.Transformer.SrcLeftTop,
                    endPoint: this.GeometryLayer.Transformer.SrcRightBottom
                );
            }       

            //Initialize
            if (type == BrushType.RadialGradient&& oldType != BrushType.RadialGradient)
            {
                this.GeometryLayer.FillBrush.RadialGradientManager.Initialize
                (
                    center: Vector2.Add(this.GeometryLayer.Transformer.SrcLeftTop, this.GeometryLayer.Transformer.SrcRightBottom) / 2,
                    radius: Vector2.Distance(this.GeometryLayer.Transformer.SrcLeftTop, this.GeometryLayer.Transformer.SrcLeftBottom) / 2
                );
            }

            //Initialize
            if (type == BrushType.EllipticalGradient && oldType != BrushType.EllipticalGradient)
            {
                this.GeometryLayer.FillBrush.EllipticalGradientManager.Initialize
                (
                    center: Vector2.Add(this.GeometryLayer.Transformer.SrcLeftTop, this.GeometryLayer.Transformer.SrcRightBottom) / 2,
                    radiusX: Vector2.Distance(this.GeometryLayer.Transformer.SrcLeftTop, this.GeometryLayer.Transformer.SrcRightTop) / 2,
                    radiusY: Vector2.Distance(this.GeometryLayer.Transformer.SrcLeftTop, this.GeometryLayer.Transformer.SrcLeftBottom) / 2
                );
            }

            this.GeometryLayer.FillBrush.Type = type;
            this.CanvasControl.Invalidate();
            this.ViewModel.Invalidate();
        }

    }
}

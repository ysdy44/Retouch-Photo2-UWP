using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Pickers
{
    public sealed partial class AlphaPicker : UserControl
    {

        //Delegate
        public event AlphaChangeHandler AlphaChange;

        #region DependencyProperty


        private byte alpha = 255;
        private byte _Alpha
        {
            get => this.alpha;
            set
            {
                this.AlphaChange?.Invoke(this, value);
                CanvasControl.Invalidate();

                this.alpha = value;
            }
        }
        public byte Alpha
        {
            get => this.alpha;
            set
            {
                //A
                this.ASlider.Value = this.APicker.Value = (int)value;

                this.alpha = value;
            }
        }


        #endregion

        public AlphaPicker()
        {
            this.InitializeComponent();

            //Slider
            this.ASlider.ValueChangeDelta += (object sender, double value) => this.Alpha = this._Alpha = (byte)value;
            //Picker
            this.APicker.ValueChange += (object sender, int value) => this.Alpha = this._Alpha = (byte)value;
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            float width = (float)sender.Size.Width;
            float height = (float)sender.Size.Height;


            args.DrawingSession.DrawImage(new DpiCompensationEffect
            {
                Source = new ScaleEffect
                {
                    Scale = new Vector2(height / 3),
                    InterpolationMode = CanvasImageInterpolation.NearestNeighbor,
                    Source = new BorderEffect
                    {
                        ExtendX = CanvasEdgeBehavior.Wrap,
                        ExtendY = CanvasEdgeBehavior.Wrap,
                        Source = CanvasBitmap.CreateFromColors
                        (
                            resourceCreator: sender,
                            widthInPixels: 2,
                            heightInPixels: 2,
                            colors: new Color[]
                            {
                                 Windows.UI.Colors.LightGray,
                                 Windows.UI.Colors.White,
                                 Windows.UI.Colors.White,
                                 Windows.UI.Colors.LightGray
                            }
                         )
                    }
                }
            });


            args.DrawingSession.FillRectangle(0, 0, width, height, new CanvasLinearGradientBrush(sender, Windows.UI.Colors.Transparent, Windows.UI.Colors.DimGray)
            {
                StartPoint = new Vector2(0, height / 2),
                EndPoint = new Vector2(width, height / 2)
            });
        }

    }
}

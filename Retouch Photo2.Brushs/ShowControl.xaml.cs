using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs.Models;
using System.Numerics;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// A control used to show a brush.
    /// </summary>
    public sealed partial class ShowControl : UserControl
    {
        //Size
        float SizeWidth = 100;
        float SizeHeight = 50;
        Vector2 SizeCenter = new Vector2(25, 50);

        //Background
        CanvasRenderTarget GrayAndWhiteBackground;


        #region DependencyProperty
        

        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            set
            {
                this._vsFillOrStroke = value;
                this.Invalidate();//State
            }
        }

        /// <summary> Gets or sets the fill-brush. </summary>
        public IBrush FillBrush
        {
            set
            {
                this._vsFillBrush = value;
                this.Invalidate();//State
            }
        }

        /// <summary> Gets or sets the stroke-brush. </summary>
        public IBrush StrokeBrush
        {
            set
            {
                this._vsStrokeBrush = value;
                this.Invalidate();//State
            }
        }


        #endregion
        

        //@VisualState
        FillOrStroke _vsFillOrStroke;
        IBrush _vsFillBrush;
        IBrush _vsStrokeBrush;
        public void Invalidate() => this.CanvasControl.Invalidate();//State


        //@Construct
        public ShowControl()
        {
            this.InitializeComponent();

            //Canvas
            this.CanvasControl.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.SizeWidth = (float)e.NewSize.Width;
                this.SizeHeight = (float)e.NewSize.Height;
                this.SizeCenter = new Vector2(this.SizeWidth / 2, this.SizeHeight / 2);
            };
            this.CanvasControl.CreateResources += (sender, args) =>
            {
                float width = (float)sender.ActualWidth;
                float height = (float)sender.ActualHeight;
                this.GrayAndWhiteBackground = new CanvasRenderTarget(sender, width, height);

                using (CanvasDrawingSession drawingSession = this.GrayAndWhiteBackground.CreateDrawingSession())
                {
                    CanvasBitmap bitmap = GreyWhiteMeshHelpher.GetGreyWhiteMesh(sender);
                    ICanvasImage extendMesh = GreyWhiteMeshHelpher.GetBorderExtendMesh(height / 4, bitmap);
                    drawingSession.DrawImage(extendMesh);
                }
            };


            this.CanvasControl.Draw += (sender, args) =>
            {
                switch (this._vsFillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.Draw(this._vsFillBrush, args.DrawingSession);
                        break;
                    case FillOrStroke.Stroke:
                        this.Draw(this._vsStrokeBrush, args.DrawingSession);
                        break;
                }
            };
        }

        private void Draw(IBrush brush, CanvasDrawingSession drawingSession)
        {
            float sizeWidth = this.SizeWidth;
            float sizeHeight = this.SizeHeight;
            Vector2 sizeCenter = this.SizeCenter;

            if (this._vsFillBrush == null)
                NoneBrush.Show(drawingSession, sizeWidth, sizeHeight);
            else
                this._vsFillBrush.Show(this.CanvasControl, drawingSession, sizeWidth, sizeHeight, sizeCenter, this.GrayAndWhiteBackground);
        }
        
    }
}
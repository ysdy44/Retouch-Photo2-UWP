using FanKit.Transformers;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas;
using Windows.UI;

namespace Retouch_Photo2.TestApp.Pages
{
    public sealed partial class MainPage : Page
    {
        public CanvasTransformer CanvasTransformer { get; } = new CanvasTransformer();

        #region MyRegion

        //Single
        bool isSingleStarted;
        Vector2 singleStartingPoint;
        //Right
        Vector2 rightStartPoint;
        //Double
        Vector2 doubleStartCenter;
        Vector2 doubleStartPosition;
        float doubleStartScale;
        float doubleStartSpace;

        Vector2 StartPosition;


        public MainPage()
        {
            this.InitializeComponent();

            //Right
            this.CanvasOperator.Right_Start += (point) =>
            {
                this.rightStartPoint = point;

                this.StartPosition = this.CanvasTransformer.Position;
            };
            this.CanvasOperator.Right_Delta += (point) =>
            {
                this.CanvasTransformer.Position = this.StartPosition - this.rightStartPoint + point;
                this.CanvasTransformer.ReloadMatrix();

                this.CanvasControl.Invalidate();//Invalidate
            };
            this.CanvasOperator.Right_Complete += (point) =>
            {
                this.CanvasTransformer.Position = this.StartPosition - this.rightStartPoint + point;
                this.CanvasTransformer.ReloadMatrix();

                this.CanvasControl.Invalidate();//Invalidate
            };


            //Double
            this.CanvasOperator.Double_Start += (center, space) =>
            {
                this.doubleStartCenter = (center - this.CanvasTransformer.Position) / this.CanvasTransformer.Scale + this.CanvasTransformer.ControlCenter;
                this.doubleStartPosition = this.CanvasTransformer.Position;

                this.doubleStartSpace = space;
                this.doubleStartScale = this.CanvasTransformer.Scale;

                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Double_Delta += (center, space) =>
            {
                this.CanvasTransformer.Scale = this.doubleStartScale / this.doubleStartSpace * space;
                this.CanvasTransformer.Position = center - (this.doubleStartCenter - this.CanvasTransformer.ControlCenter) * this.CanvasTransformer.Scale;
                this.CanvasTransformer.ReloadMatrix();

                this.CanvasControl.Invalidate();
            };
            this.CanvasOperator.Double_Complete += (center, space) =>
            {
                this.CanvasControl.Invalidate();
            };

            //Wheel
            this.CanvasOperator.Wheel_Changed += (point, space) =>
            {
                if (space > 0)
                {
                    if (this.CanvasTransformer.Scale < 10f)
                    {
                        this.CanvasTransformer.Scale *= 1.1f;
                        this.CanvasTransformer.Position = point + (this.CanvasTransformer.Position - point) * 1.1f;
                    }
                }
                else
                {
                    if (this.CanvasTransformer.Scale > 0.1f)
                    {
                        this.CanvasTransformer.Scale /= 1.1f;
                        this.CanvasTransformer.Position = point + (this.CanvasTransformer.Position - point) / 1.1f;
                    }
                }

                this.CanvasTransformer.ReloadMatrix();
                this.CanvasControl.Invalidate();
            };
        }

        #endregion


        private void Page_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            this.CanvasTransformer.Size = e.NewSize;
            this.CanvasTransformer.ReloadMatrix();
        }
        private void CanvasOperator_Single_Start(System.Numerics.Vector2 point)
        {

        }

        private void CanvasOperator_Single_Delta(System.Numerics.Vector2 point)
        {

        }

        private void CanvasOperator_Single_Complete(System.Numerics.Vector2 point)
        {

        }

        private void CanvasControl_Draw( CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            Vector2 vector = new Vector2(100, 100);

            //DrawCrad
            {
                CanvasCommandList command = new CanvasCommandList(sender);
                using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
                {
                    drawingSession.Clear(Colors.White); 
                }
                args.DrawingSession.DrawCrad(command, this.CanvasTransformer);
            }

            //Vector2
            {
                Matrix3x2 matrix = this.CanvasTransformer.GetMatrix();
                args.DrawingSession.DrawNode(Vector2.Transform(vector, matrix));
            }
        }

    }
}
using Microsoft.Graphics.Canvas;
using Retouch_Photo.ViewModels;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo.Controls
{
    public sealed partial class MainCanvasControl : UserControl
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        #region DependencyProperty

        /// <summary>
        /// Brush of <see cref="Shadow"/>.
        /// </summary>
        public SolidColorBrush Brush
        {
            get { return (SolidColorBrush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Brush), typeof(SolidColorBrush), typeof(MainCanvasControl), new PropertyMetadata(new SolidColorBrush(Colors.Black), (sender, e) =>
        {
            MainCanvasControl con = (MainCanvasControl)sender;

            if (e.NewValue is SolidColorBrush value)
            {
                con.CanvasControl.Invalidate();
            }
        }));

        #endregion

        //Right
        Vector2 rightStartPoint;
        Vector2 rightStartPosition;

        //Double
        Vector2 doubleStartCenter;
        Vector2 doubleStartPosition;
        float doubleStartScale;
        float doubleStartSpace;


        public MainCanvasControl()
        {
            this.InitializeComponent();
            this.Drop += (s, e) => { };
            this.SizeChanged += (s, e) => this.ViewModel.MatrixTransformer.ControlSizeChanged(e.NewSize);


            //CanvasControl
            this.CanvasControl.CreateResources += (sender, args) => this.ViewModel.CanvasManger = new CanvasControlManger(sender);
            this.CanvasControl.Draw += (sender, args) =>
            {
                CanvasDrawingSession ds = args.DrawingSession;
                Matrix3x2 virtualToControlMatrix = this.ViewModel.MatrixTransformer.VirtualToControlMatrix;
                Color color = this.Brush.Color;

                //RenderLayer
                this.ViewModel.RenderLayer.Draw(ds, virtualToControlMatrix, color);
              
                //MatrixTransformer
                this.ViewModel.MatrixTransformer.RulerDraw(ds);

                //Tool
                this.ViewModel.Tool.Draw(ds);
            };


            //Single
            this.CanvasOperator.Single_Start += (point) => this.ViewModel.Tool.Start(point);
            this.CanvasOperator.Single_Delta += (point) => this.ViewModel.Tool.Delta(point);
            this.CanvasOperator.Single_Complete += (point) => this.ViewModel.Tool.Complete(point);


            //Right
            this.CanvasOperator.Right_Start += (point) =>
            {
                this.rightStartPoint = point;
                this.rightStartPosition = this.ViewModel.MatrixTransformer.Position;

                this.ViewModel.Invalidate(isThumbnail: true);
            };
            this.CanvasOperator.Right_Delta += (point) =>
            {
                this.ViewModel.MatrixTransformer.Position = this.rightStartPosition - this.rightStartPoint + point;

                this.ViewModel.Invalidate();
            };
            this.CanvasOperator.Right_Complete += (point) => this.ViewModel.Invalidate(isThumbnail: false);


            //Double
            this.CanvasOperator.Double_Start += (center, space) =>
            {
                this.doubleStartCenter = (center - this.ViewModel.MatrixTransformer.Position) / this.ViewModel.MatrixTransformer.Scale + new Vector2(this.ViewModel.MatrixTransformer.ControlWidth / 2, this.ViewModel.MatrixTransformer.ControlHeight / 2);
                this.doubleStartPosition = this.ViewModel.MatrixTransformer.Position;

                this.doubleStartSpace = space;
                this.doubleStartScale = this.ViewModel.MatrixTransformer.Scale;

                this.ViewModel.Invalidate(isThumbnail: true);
            };
            this.CanvasOperator.Double_Delta += (center, space) =>
            {
                this.ViewModel.MatrixTransformer.Scale = this.doubleStartScale / this.doubleStartSpace * space;

                this.ViewModel.MatrixTransformer.Position = center - (this.doubleStartCenter - new Vector2(this.ViewModel.MatrixTransformer.ControlWidth / 2, this.ViewModel.MatrixTransformer.ControlHeight / 2)) * this.ViewModel.MatrixTransformer.Scale;

                this.ViewModel.Invalidate();
            };
            this.CanvasOperator.Double_Complete += (center, space) => this.ViewModel.Invalidate(isThumbnail: false);


            //Wheel
            this.CanvasOperator.Wheel_Changed += (point, space) =>
            {
                if (space > 0)
                {
                    if (this.ViewModel.MatrixTransformer.Scale < 10f)
                    {
                        this.ViewModel.MatrixTransformer.Scale *= 1.1f;
                        this.ViewModel.MatrixTransformer.Position = point + (this.ViewModel.MatrixTransformer.Position - point) * 1.1f;
                    }
                }
                else
                {
                    if (this.ViewModel.MatrixTransformer.Scale > 0.1f)
                    {
                        this.ViewModel.MatrixTransformer.Scale /= 1.1f;
                        this.ViewModel.MatrixTransformer.Position = point + (this.ViewModel.MatrixTransformer.Position - point) / 1.1f;
                    }
                }

                this.ViewModel.Invalidate();
            };
        }
    }
}
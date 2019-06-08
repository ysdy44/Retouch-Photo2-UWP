using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Library;
using Retouch_Photo2.TestApp.ViewModels;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainCanvasControl" />. 
    /// </summary>
    public sealed partial class MainCanvasControl : UserControl
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        //Single
        bool isSingleStarted;
        Vector2 singleStartingPoint;
        //Right
        Vector2 rightStartPoint;
        Vector2 rightStartPosition;
        //Double
        Vector2 doubleStartCenter;
        Vector2 doubleStartPosition;
        float doubleStartScale;
        float doubleStartSpace;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "MainCanvasControl" />'s shadow color. </summary>
        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }
        /// <summary> Identifies the <see cref = "MainCanvasControl.ShadowColor" /> dependency property. </summary>
        public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register(nameof(ShadowColor), typeof(Color), typeof(MainCanvasControl), new PropertyMetadata(Colors.Black, (sender, e) =>
        {
            MainCanvasControl con = (MainCanvasControl)sender;

            if (e.NewValue is Color value)
            {
                con.CanvasControl.Invalidate();
            }
        }));


        #endregion

        public MainCanvasControl()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) => 
            {
                if (e.NewSize == e.PreviousSize) return;
                this.ViewModel.CanvasTransformer.Size = e.NewSize;
            };
               

            //Draw
            this.ViewModel.InvalidateAction = (mode) =>
            {
                switch (mode)
                {
                    case InvalidateMode.Thumbnail:
                        this.CanvasControl.DpiScale = 0.5f;
                        break;
                    case InvalidateMode.HD:
                        this.CanvasControl.DpiScale = 1.0f;
                        break;
                }

                this.CanvasControl.Invalidate();
            };
            this.CanvasControl.Draw += (sender, args) =>
            {
                //Render : Blank Image
                ICanvasImage previousImage = new ColorSourceEffect { Color = Colors.White };
                Matrix3x2 canvasToVirtualMatrix = this.ViewModel.CanvasTransformer.GetMatrix(MatrixTransformerMode.CanvasToVirtual);

               
                void aaa() =>
                  previousImage = Layer.Render(this.ViewModel.CanvasDevice, this.ViewModel.MezzanineLayer, previousImage, canvasToVirtualMatrix);

                void bbb(int i) =>
                    previousImage = Layer.Render(this.ViewModel.CanvasDevice, this.ViewModel.Layers[i], previousImage, canvasToVirtualMatrix);


                //Mezzanine :
                //   If the mezzanine is not **null**, 
                //   Insert a MezzanineLayer between the Layers
                if (this.ViewModel.MezzanineLayer != null)
                {
                    if (this.ViewModel.Layers.Count == 0) aaa();
                    else
                    {
                        for (int i = this.ViewModel.Layers.Count - 1; i >= 0; i--)
                        {
                            if (this.ViewModel.MezzanineIndex == i) aaa();

                            bbb(i);
                        }
                    }
                }
                else
                {
                    for (int i = this.ViewModel.Layers.Count - 1; i >= 0; i--)
                    {
                        bbb(i);
                    }
                }


                //Crop : Get the border from MatrixTransformer
                float width = this.ViewModel.CanvasTransformer.Width * this.ViewModel.CanvasTransformer.Scale;
                float height = this.ViewModel.CanvasTransformer.Height * this.ViewModel.CanvasTransformer.Scale;
                ICanvasImage cropRect = new CropEffect
                {
                    Source = previousImage,
                    SourceRectangle = new Rect(-width / 2, -height / 2, width, height),
                };


                //Final : Draw to Canvas
                ICanvasImage finalCanvas = new Transform2DEffect
                {
                    Source = cropRect,
                    TransformMatrix = this.ViewModel.CanvasTransformer.GetMatrix(MatrixTransformerMode.VirtualToControl)
                };
                ICanvasImage shadow = new ShadowEffect
                {
                    Source = finalCanvas,
                    ShadowColor = this.ShadowColor,
                    BlurAmount = 4.0f
                };
                args.DrawingSession.DrawImage(shadow, 5.0f, 5.0f);
                args.DrawingSession.DrawImage(finalCanvas);



                ///////////////////////////////////////////////////////////////////////////////////


                //Transformer
                switch (this.ViewModel.SelectionMode)
                {
                    case ListViewSelectionMode.None:
                        break;

                    case ListViewSelectionMode.Single:
                        {
                            Transformer transformer = this.ViewModel.SelectionLayer.TransformerMatrix.Destination;
                            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                            Transformer.DrawBoundNodes(args.DrawingSession, transformer, matrix);
                        }
                        break;

                    case ListViewSelectionMode.Multiple:
                        {
                            Transformer transformer = this.ViewModel.Transformer;
                            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                            Transformer.DrawBoundNodes(args.DrawingSession, transformer, matrix);
                        }
                        break;
                } 
            };


            //Single
            this.CanvasOperator.Single_Start += (point) =>
            {
                this.isSingleStarted = false;
                this.singleStartingPoint = point;
                this.ViewModel.Tool.Starting(point);//Starting
            };
            this.CanvasOperator.Single_Delta += (point) =>
            {
                //Delta
                if (this.isSingleStarted)
                {
                    this.ViewModel.Tool.Delta(this.singleStartingPoint, point);
                    return;
                }

                //Started
                if ((this.singleStartingPoint - point).LengthSquared() > 400.0f)
                {
                    this.isSingleStarted = true;
                    this.ViewModel.Tool.Started(this.singleStartingPoint, point);
                }
            };
            this.CanvasOperator.Single_Complete += (point) => this.ViewModel.Tool.Complete(this.singleStartingPoint, point, this.isSingleStarted);


            //Right
            this.CanvasOperator.Right_Start += (point) =>
            {
                this.rightStartPoint = point;
                this.rightStartPosition = this.ViewModel.CanvasTransformer.Position;

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
            };
            this.CanvasOperator.Right_Delta += (point) =>
            {
                this.ViewModel.CanvasTransformer.Position = this.rightStartPosition - this.rightStartPoint + point;

                this.ViewModel.Invalidate();
            };
            this.CanvasOperator.Right_Complete += (point) => this.ViewModel.Invalidate(InvalidateMode.HD);


            //Double
            this.CanvasOperator.Double_Start += (center, space) =>
            {
                this.doubleStartCenter = (center - this.ViewModel.CanvasTransformer.Position) / this.ViewModel.CanvasTransformer.Scale + new Vector2(this.ViewModel.CanvasTransformer.ControlWidth / 2, this.ViewModel.CanvasTransformer.ControlHeight / 2);
                this.doubleStartPosition = this.ViewModel.CanvasTransformer.Position;

                this.doubleStartSpace = space;
                this.doubleStartScale = this.ViewModel.CanvasTransformer.Scale;

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
            };
            this.CanvasOperator.Double_Delta += (center, space) =>
            {
                this.ViewModel.CanvasTransformer.Scale = this.doubleStartScale / this.doubleStartSpace * space;

                this.ViewModel.CanvasTransformer.Position = center - (this.doubleStartCenter - new Vector2(this.ViewModel.CanvasTransformer.ControlWidth / 2, this.ViewModel.CanvasTransformer.ControlHeight / 2)) * this.ViewModel.CanvasTransformer.Scale;

                this.ViewModel.Invalidate();
            };
            this.CanvasOperator.Double_Complete += (center, space) => this.ViewModel.Invalidate(InvalidateMode.HD);


            //Wheel
            this.CanvasOperator.Wheel_Changed += (point, space) =>
            {
                if (space > 0)
                {
                    if (this.ViewModel.CanvasTransformer.Scale < 10f)
                    {
                        this.ViewModel.CanvasTransformer.Scale *= 1.1f;
                        this.ViewModel.CanvasTransformer.Position = point + (this.ViewModel.CanvasTransformer.Position - point) * 1.1f;
                    }
                }
                else
                {
                    if (this.ViewModel.CanvasTransformer.Scale > 0.1f)
                    {
                        this.ViewModel.CanvasTransformer.Scale /= 1.1f;
                        this.ViewModel.CanvasTransformer.Position = point + (this.ViewModel.CanvasTransformer.Position - point) / 1.1f;
                    }
                }

                this.ViewModel.Invalidate();
            };
        }
    }
}
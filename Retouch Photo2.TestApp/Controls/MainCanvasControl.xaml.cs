using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Library;
using Retouch_Photo2.Library.Transformers;
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
               
        //@Construct
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
                  previousImage = Layer.Render(this.ViewModel.CanvasDevice, this.ViewModel.Mezzanine.Layer, previousImage, canvasToVirtualMatrix);

                void bbb(int i) =>
                    previousImage = Layer.Render(this.ViewModel.CanvasDevice, this.ViewModel.Layers[i], previousImage, canvasToVirtualMatrix);


                //Mezzanine 
                if (this.ViewModel.Mezzanine.Layer != null)
                {
                    if (this.ViewModel.Layers.Count == 0) aaa();
                    else
                    {
                        for (int i = this.ViewModel.Layers.Count - 1; i >= 0; i--)
                        {
                            if (this.ViewModel.Mezzanine.Index == i) aaa();

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
                float width = this.ViewModel.CanvasTransformer.Width * this.ViewModel.CanvasScale;
                float height = this.ViewModel.CanvasTransformer.Height * this.ViewModel.CanvasScale;
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

                
                //Mezzanine 
                if (this.ViewModel.Mezzanine.Layer == null)
                {
                    //Transformer
                    switch (this.ViewModel.Selection.Mode)
                    {
                        case ListViewSelectionMode.None:
                            break;

                        case ListViewSelectionMode.Single:
                            {
                                Transformer transformer = this.ViewModel.Selection.Layer.TransformerMatrix.Destination;
                                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                                args.DrawingSession.DrawBoundNodes(transformer, matrix);
                            }
                            break;

                        case ListViewSelectionMode.Multiple:
                            {
                                Transformer transformer = this.ViewModel.Selection.Transformer;
                                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                                args.DrawingSession.DrawBoundNodes(transformer, matrix);
                            }
                            break;
                    }
                }
                else
                {
                    Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                    args.DrawingSession.DrawBound(this.ViewModel.Mezzanine.Layer.TransformerMatrix.Destination, matrix);
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
                    this.ViewModel.Tool.Delta(this.singleStartingPoint, point);//Delta
                    return;
                }

                //Started
                if ((this.singleStartingPoint - point).LengthSquared() > 400.0f)
                {
                    this.isSingleStarted = true;

                    this.ViewModel.Tool.Started(this.singleStartingPoint, point);//Started
                }
            };
            this.CanvasOperator.Single_Complete += (point) => this.ViewModel.Tool.Complete(this.singleStartingPoint, point, this.isSingleStarted);//Started


            //Right
            this.CanvasOperator.Right_Start += (point) =>
            {
                this.singleStartingPoint = point;
                this.ViewModel.ViewTool.Started(this.singleStartingPoint, point);//Started
            };
            this.CanvasOperator.Right_Delta += (point) => this.ViewModel.ViewTool.Delta(this.singleStartingPoint, point);//Delta
            this.CanvasOperator.Right_Complete += (point) => this.ViewModel.ViewTool.Complete(this.singleStartingPoint, point, this.isSingleStarted);//Started


            //Double
            this.CanvasOperator.Double_Start += (center, space) =>
            {
                this.doubleStartCenter = (center - this.ViewModel.CanvasTransformer.Position) / this.ViewModel.CanvasScale + this.ViewModel.CanvasTransformer.ControlCenter;
                this.doubleStartPosition = this.ViewModel.CanvasTransformer.Position;

                this.doubleStartSpace = space;
                this.doubleStartScale = this.ViewModel.CanvasScale;

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
            };
            this.CanvasOperator.Double_Delta += (center, space) =>
            {
                this.ViewModel.CanvasScale = this.doubleStartScale / this.doubleStartSpace * space;
                this.ViewModel.CanvasTransformer.Position = center - (this.doubleStartCenter - this.ViewModel.CanvasTransformer.ControlCenter) * this.ViewModel.CanvasScale;
                this.ViewModel.CanvasTransformer.ReloadMatrix();

                this.ViewModel.Invalidate();
            };
            this.CanvasOperator.Double_Complete += (center, space) => this.ViewModel.Invalidate(InvalidateMode.HD);


            //Wheel
            this.CanvasOperator.Wheel_Changed += (point, space) =>
            {
                if (space > 0)
                {
                    if (this.ViewModel.CanvasScale < 10f)
                    {
                        this.ViewModel.CanvasScale *= 1.1f;
                        this.ViewModel.CanvasTransformer.Position = point + (this.ViewModel.CanvasTransformer.Position - point) * 1.1f;
                    }
                }
                else
                {
                    if (this.ViewModel.CanvasScale > 0.1f)
                    {
                        this.ViewModel.CanvasScale /= 1.1f;
                        this.ViewModel.CanvasTransformer.Position = point + (this.ViewModel.CanvasTransformer.Position - point) / 1.1f;
                    }
                }

                this.ViewModel.CanvasTransformer.ReloadMatrix();
                this.ViewModel.Invalidate();
            };
        }


    }
}
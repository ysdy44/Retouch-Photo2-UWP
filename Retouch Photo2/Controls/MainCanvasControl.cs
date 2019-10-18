using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "MainCanvasControl" />. 
    /// </summary>
    public partial class MainCanvasControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        bool _isSingleStarted;
        Vector2 _singleStartingPoint;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "MainCanvasControl" />'s accent color. </summary>
        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }
        /// <summary> Identifies the <see cref = "MainCanvasControl.AccentColor" /> dependency property. </summary>
        public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register(nameof(AccentColor), typeof(Color), typeof(MainCanvasControl), new PropertyMetadata(Colors.DodgerBlue, (sender, e) =>
        {
            MainCanvasControl con = (MainCanvasControl)sender;

            if (e.NewValue is Color value)
            {
                con.ViewModel.AccentColor = value;
                con.ViewModel.Invalidate();//Invalidate
            }
        }));


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
                con.ViewModel.Invalidate();//Invalidate
            }
        }));


        #endregion


        private void ConstructViewModel(CanvasControl canvasControl)
        {
            // if (this.ViewModel.InvalidateAction == null)
            {
                this.ViewModel.InvalidateAction = (mode) =>
                {
                    switch (mode)
                    {
                        case InvalidateMode.Thumbnail:
                            canvasControl.DpiScale = 0.5f;
                            break;
                        case InvalidateMode.HD:
                            canvasControl.DpiScale = 1.0f;
                            break;
                    }

                    canvasControl.Invalidate();//Invalidate
                };
            }
        }
        private void ConstructKeyboardViewModel()
        {
            if (this.KeyboardViewModel.Move == null)
            {
                this.KeyboardViewModel.Move += (value) =>
                {
                    this.ViewModel.CanvasTransformer.Position += value;
                    this.ViewModel.CanvasTransformer.ReloadMatrix();
                    this.ViewModel.Invalidate();//Invalidate
                };
            }
        }


        private void _drawRenderAndCrad(CanvasDrawingSession drawingSession)
        {
            ICanvasImage previousImage = new ColorSourceEffect { Color = Colors.White };

            Matrix3x2 canvasToVirtualMatrix = this.ViewModel.CanvasTransformer.GetMatrix(MatrixTransformerMode.CanvasToVirtual);

            for (int i = this.ViewModel.Layers.RootLayers.Count - 1; i >= 0; i--)
            {
                ILayer currentLayer = this.ViewModel.Layers.RootLayers[i];
                previousImage = LayerBase.Render(this.ViewModel.CanvasDevice, currentLayer, previousImage, canvasToVirtualMatrix);
            }

            //Crad
            drawingSession.DrawCrad(previousImage, this.ViewModel.CanvasTransformer, this.ShadowColor);
        }
        private void _drawToolAndBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession)
        {
            if (this.ViewModel.MezzanineLayer != null)
            {
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                drawingSession.DrawBound(this.SelectionViewModel.Transformer, matrix);
            }
            else
            {
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

                //Bound
                foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
                {
                    if (layer.SelectMode.ToBool())
                    {
                        layer.DrawBound(resourceCreator, drawingSession, matrix, this.ViewModel.AccentColor);
                    }
                }

                //Tool
                this.TipViewModel.Tool.Draw(drawingSession);
            }



        }

    }
}
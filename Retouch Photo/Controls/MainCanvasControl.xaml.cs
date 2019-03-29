using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Text;
using Windows.Foundation.Metadata;
using System.Globalization;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Retouch_Photo.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Retouch_Photo.ViewModels;

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

        public MainCanvasControl()
        {
            this.InitializeComponent();

            this.CanvasControl.CreateResources += (sender, args) => this.ViewModel.InitializeCanvasControl(sender);

            this.CanvasControl.Draw += (sender, args) =>
            {
                this.ViewModel.RenderLayer.Draw(args.DrawingSession, this.ViewModel.MatrixTransformer.VirtualToControlMatrix, this.Brush.Color);

                this.ViewModel.RenderLayer.RulerDraw(args.DrawingSession, this.ViewModel.MatrixTransformer);

                this.ViewModel.Tool.ViewModel.Draw(args.DrawingSession);

                Retouch_Photo.Library.HomographyController.Transformer.DrawNode(args.DrawingSession, App.ViewModel.AAA);
                Retouch_Photo.Library.HomographyController.Transformer.DrawNode(args.DrawingSession, App.ViewModel.BBB);
                Retouch_Photo.Library.HomographyController.Transformer.DrawNode(args.DrawingSession, App.ViewModel.CCC);
                Retouch_Photo.Library.HomographyController.Transformer.DrawNode(args.DrawingSession, App.ViewModel.DDD);
            };

            this.SizeChanged += (s, e) => this.ViewModel.MatrixTransformer.ControlSizeChanged(e.NewSize);
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {

        }

        #region Single


        //单指&&左键&&笔
        private void Single_Start(Vector2 point) =>this.ViewModel.Tool.ViewModel.Start(point);
        private void Single_Delta(Vector2 point) => this.ViewModel.Tool.ViewModel.Delta(point);
        private void Single_Complete(Vector2 point) => this.ViewModel.Tool.ViewModel.Complete(point);


        #endregion

        #region Right


        Vector2 rightStartPoint;
        Vector2 rightStartPosition;
        private void Right_Start(Vector2 point)
        {
            this.rightStartPoint = point;
            this.rightStartPosition = this.ViewModel.MatrixTransformer.Position;

            this.ViewModel.Invalidate(isThumbnail: true);
        }
        private void Right_Delta(Vector2 point)
        {
            this.ViewModel.MatrixTransformer.Position = this.rightStartPosition - this.rightStartPoint + point;

            this.ViewModel.Invalidate();
        }
        private void Right_Complete(Vector2 point)
        {
            this.ViewModel.Invalidate(isThumbnail: false);
        }


        #endregion

        #region Double


        Vector2 doubleStartCenter;
        Vector2 doubleStartPosition;
        float doubleStartScale;
        float doubleStartSpace;
        private void Double_Start(Vector2 center, float space)
        {
            this.doubleStartCenter = (center - this.ViewModel.MatrixTransformer.Position) / this.ViewModel.MatrixTransformer.Scale + new Vector2(this.ViewModel.MatrixTransformer.ControlWidth / 2, this.ViewModel.MatrixTransformer.ControlHeight / 2);
            this.doubleStartPosition = this.ViewModel.MatrixTransformer.Position;

            this.doubleStartSpace = space;
            this.doubleStartScale = this.ViewModel.MatrixTransformer.Scale;

            this.ViewModel.Invalidate(isThumbnail: true);
        }
        private void Double_Delta(Vector2 center, float space)
        {
            this.ViewModel.MatrixTransformer.Scale = this.doubleStartScale / this.doubleStartSpace * space;

            this.ViewModel.MatrixTransformer.Position = center - (this.doubleStartCenter - new Vector2(this.ViewModel.MatrixTransformer.ControlWidth / 2, this.ViewModel.MatrixTransformer.ControlHeight / 2)) * this.ViewModel.MatrixTransformer.Scale;

            this.ViewModel.Invalidate();
        }
        private void Double_Complete(Vector2 center, float space)
        {
            this.ViewModel.Invalidate(isThumbnail: false);
        }


        #endregion

        #region Wheel


        //Wheel Changed
        private void Wheel_Changed(Vector2 point, float space)
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
        }


        #endregion


    }
}

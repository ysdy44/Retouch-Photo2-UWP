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
using Windows.Foundation;
using Retouch_Photo.ViewModels;

namespace Retouch_Photo.Controls
{
    public sealed partial class MainCanvasControl : UserControl
    {

        //ViewModel
        public DrawViewModel ViewModel; 

        public MainCanvasControl()
        {
            this.InitializeComponent();

            //ViewModel
            this.ViewModel = App.ViewModel;
            this.CanvasControl.CreateResources += (sender, args) => this.ViewModel.InitializeCanvasControl(sender);
        }


        #region Single


        //单指&&左键&&笔
        private void Single_Start(Vector2 point)
        {
            this.ViewModel.Tool.ViewModel.Start(point, this.ViewModel);
            this.ViewModel.Invalidate();
        }
        private void Single_Delta(Vector2 point)
        {
            this.ViewModel.Tool.ViewModel.Delta(point, this.ViewModel);
            this.ViewModel.Invalidate();
        }
        private void Single_Complete(Vector2 point)
        {
            this.ViewModel.Tool.ViewModel.Complete(point, this.ViewModel);
            this.ViewModel.Invalidate();
        }


        #endregion

        #region Right


        //右键
        Vector2 rightStartPoint;
        Vector2 rightStartPosition;
        private void Right_Start(Vector2 point)
        {
            this.rightStartPoint = point;
            this.rightStartPosition = this.ViewModel.Position;

            this.ViewModel.Invalidate(true);
        }
        private void Right_Delta(Vector2 point)
        {
            this.ViewModel.Position = this.rightStartPosition - this.rightStartPoint + point;

            this.ViewModel.Invalidate(true);
        }
        private void Right_Complete(Vector2 point)
        {
            this.ViewModel.Invalidate(true);
        }


        #endregion

        #region Double
        

        Vector2 doubleStartCenter;
        Vector2 doubleStartPosition;
        float doubleStartScale;
        float doubleStartSpace;
        private void Double_Start(Vector2 center, float space)
        {
            this.doubleStartCenter = center;
            this.doubleStartPosition = this.ViewModel.Position;

            this.doubleStartSpace = space;
            this.doubleStartScale = this.ViewModel.Scale;

            this.ViewModel.Invalidate(true);
        }
        private void Double_Delta(Vector2 center, float space)
        {
            this.ViewModel.Position = this.doubleStartPosition - this.doubleStartCenter + center;

            this.ViewModel.Scale = this.doubleStartScale / this.doubleStartSpace * space;

            this.ViewModel.Invalidate(true);
        }
        private void Double_Complete(Vector2 center, float space)
        {
            this.ViewModel.Invalidate(true);
        }

        #endregion

        #region Wheel


        //Wheel Changed
        private void Wheel_Changed(Vector2 point, float space)
        {
            if (space > 0)
            {
                if (this.ViewModel.Scale < 10f)
                {
                    this.ViewModel.Scale *= 1.1f;
                    this.ViewModel.Position = point + (this.ViewModel.Position - point) * 1.1f;                         
                }
            }
            else
            {
                if (this.ViewModel.Scale > 0.1f)
                {
                    this.ViewModel.Scale /= 1.1f;
                    this.ViewModel.Position = point + (this.ViewModel.Position - point) / 1.1f;
                }
            }

            this.ViewModel.Invalidate(true);
        }


        #endregion

        #region CanvasControl
        

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (this.ViewModel.GrayWhiteGrid == null) return;
      
            ICanvasImage image = new Transform2DEffect
            {
                Source = this.ViewModel.ToRender(this.ViewModel.GrayWhiteGrid),
                TransformMatrix = this.ViewModel.GetMatrix()
            };
            ICanvasImage shadow = new OpacityEffect
            {
                Opacity = 0.2f,
                Source = new ShadowEffect
                {
                    Source = image,
                    ShadowColor = Colors.Black,
                }
            };
            args.DrawingSession.DrawImage(shadow, 5.0f, 5.0f);
            args.DrawingSession.DrawImage(image);

            this.ViewModel.MarqueeTool.Draw(sender, args.DrawingSession, this.ViewModel.GetMatrix());
            this.ViewModel.DottedLine.Update();

            this.ViewModel.DottedLine.Draw(sender, args.DrawingSession, new Rect(0, 0, sender.ActualWidth, sender.ActualHeight));
        }


        #endregion



    }
}

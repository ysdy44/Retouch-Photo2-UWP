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
            this.ViewModel.Invalidate(isDottedLineRender: true);
        }
        private void Single_Delta(Vector2 point)
        {
            this.ViewModel.Tool.ViewModel.Delta(point, this.ViewModel);
            this.ViewModel.Invalidate(isDottedLineRender: true);
        }
        private void Single_Complete(Vector2 point)
        {
            this.ViewModel.Tool.ViewModel.Complete(point, this.ViewModel);
            this.ViewModel.Invalidate(isDottedLineRender: true);
        }


        #endregion

        #region Right


        //右键
        Vector2 rightStartPoint;
        Vector2 rightStartPosition;
        private void Right_Start(Vector2 point)
        {
            this.rightStartPoint = point;
            this.rightStartPosition = this.ViewModel.Transformer.Position;

            this.ViewModel.Invalidate(isDottedLineRender: true);
        }
        private void Right_Delta(Vector2 point)
        {
            this.ViewModel.Transformer.Position = this.rightStartPosition - this.rightStartPoint + point;

            this.ViewModel.Invalidate(isDottedLineRender: true);
        }
        private void Right_Complete(Vector2 point)
        {
            this.ViewModel.Invalidate(isDottedLineRender: true);
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
            this.doubleStartPosition = this.ViewModel.Transformer.Position;

            this.doubleStartSpace = space;
            this.doubleStartScale = this.ViewModel.Transformer.Scale;

            this.ViewModel.Invalidate(isDottedLineRender: true);
        }
        private void Double_Delta(Vector2 center, float space)
        {
            this.ViewModel.Transformer.Position = this.doubleStartPosition - this.doubleStartCenter + center;

            this.ViewModel.Transformer.Scale = this.doubleStartScale / this.doubleStartSpace * space;

            this.ViewModel.Invalidate(isDottedLineRender: true);
        }
        private void Double_Complete(Vector2 center, float space)
        {
            this.ViewModel.Invalidate(isDottedLineRender: true);
        }

        #endregion

        #region Wheel


        //Wheel Changed
        private void Wheel_Changed(Vector2 point, float space)
        {
            if (space > 0)
            {
                if (this.ViewModel.Transformer.Scale < 10f)
                {
                    this.ViewModel.Transformer.Scale *= 1.1f;
                    this.ViewModel.Transformer.Position = point + (this.ViewModel.Transformer.Position - point) * 1.1f;                         
                }
            }
            else
            {
                if (this.ViewModel.Transformer.Scale > 0.1f)
                {
                    this.ViewModel.Transformer.Scale /= 1.1f;
                    this.ViewModel.Transformer.Position = point + (this.ViewModel.Transformer.Position - point) / 1.1f;
                }
            }

            this.ViewModel.Invalidate(isDottedLineRender: true);
        }


        #endregion

        #region CanvasControl


        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {    
        

                

             
            this.ViewModel.RenderLayer.Draw(sender, args.DrawingSession, this.ViewModel.Transformer.Matrix);

            this.ViewModel.MarqueeTool.Draw(sender, args.DrawingSession, this.ViewModel.Transformer.Matrix);

            this.ViewModel.DottedLine.Update();
            this.ViewModel.DottedLine.Draw(sender, args.DrawingSession, new Rect(0, 0, sender.ActualWidth, sender.ActualHeight));
        }


        #endregion



    }
}

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Models;
using Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels;
using Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels.ScaleViewModels.Scale1ViewModels;
using Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels.ScaleViewModels.Scale2ViewModels;
using Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels.SkewViewModels;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo.ViewModels.ToolViewModels
{
    /// <summary>
    /// The nodes mode of [CursorTool].
    /// </summary>
    public enum CursorMode
    {
        None,
        Translation,
        Rotation,

        SkewLeft,
        SkewTop,
        SkewRight,
        SkewBottom,

        ScaleLeft,
        ScaleTop,
        ScaleRight,
        ScaleBottom,

        ScaleLeftTop,
        ScaleRightTop,
        ScaleRightBottom,
        ScaleLeftBottom,
    }


    public class CursorViewModel : ToolViewModel
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;
        bool IsSkew => this.ViewModel.KeyAlt;
         

        Layer CurrentLayer;

        CursorMode Mode = CursorMode.None;
        readonly Dictionary<CursorMode, ToolViewModel2> ViewModelDictionary = new Dictionary<CursorMode, ToolViewModel2>
        {
            {CursorMode.None,  new NoneViewModel()},
            {CursorMode.Translation,  new TranslationViewModel()},
            {CursorMode.Rotation,  new RotationViewModel()},

            {CursorMode.SkewLeft,  new CursorViewModels.SkewViewModels.LeftViewModel()},
            {CursorMode.SkewTop,  new CursorViewModels.SkewViewModels.TopViewModel()},
            {CursorMode.SkewRight,  new CursorViewModels.SkewViewModels.RightViewModel()},
            {CursorMode.SkewBottom,  new CursorViewModels.SkewViewModels.BottomViewModel()},

            {CursorMode.ScaleLeft,  new CursorViewModels.ScaleViewModels.Scale1ViewModels.LeftViewModel()},
            {CursorMode.ScaleTop,  new CursorViewModels.ScaleViewModels.Scale1ViewModels.TopViewModel()},
            {CursorMode.ScaleRight,  new CursorViewModels.ScaleViewModels.Scale1ViewModels.RightViewModel()},
            {CursorMode.ScaleBottom,  new CursorViewModels.ScaleViewModels.Scale1ViewModels.BottomViewModel()},

            {CursorMode.ScaleLeftTop,  new LeftTopViewModel()},
            {CursorMode.ScaleRightTop,  new RightTopViewModel()},
            {CursorMode.ScaleRightBottom,  new RightBottomViewModel()},
            {CursorMode.ScaleLeftBottom,  new LeftBottomViewModel()},
        };

        bool IsCursorBox;
        Vector2 StartPoint;
        Vector2 EndPoint;



        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
            this.CurrentLayer = this.ViewModel.CurrentLayer;
            if (this.CurrentLayer == null) return;
            this.ViewModel.Invalidate();
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }

        public override void Start(Vector2 point)
        {
            this.CurrentLayer = this.ViewModel.CurrentLayer;



            //CursorMode
            if (this.CurrentLayer != null)
            {
                this.Mode = this.ContainsNodeMode(point, this.CurrentLayer.Transformer, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix, this.IsSkew);

                if (this.Mode!= CursorMode.None)
                {
                    this.ViewModelDictionary[this.Mode].Start(point, this.CurrentLayer);
                    return;
                }
            }


            //Translation                
            Vector2 canvasPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            foreach (Layer layer in this.ViewModel.RenderLayer.Layers)
            {
                if (layer.IsVisual == false || layer.Opacity == 0) continue;

                if (Transformer.ContainsBound(canvasPoint, layer.Transformer))
                {
                    this.CurrentLayer = layer;
                    this.ViewModel.CurrentLayer = layer;
                    this.Mode = CursorMode.Translation;

                    this.ViewModelDictionary[this.Mode].Start(point, this.CurrentLayer);
                    return;
                }
            }


            //CursorBox
            this.CurrentLayer = null;
            this.IsCursorBox = true;
            this.StartPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
        }

        public override void Delta(Vector2 point)
        {
            this.EndPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);

            if (this.IsCursorBox)
            {
                this.ViewModel.Invalidate();
                return;
            }

            if (this.CurrentLayer != null)
            {
                this.ViewModelDictionary[this.Mode].Delta(point, this.CurrentLayer);
                this.ViewModel.Invalidate();
                return;
            }
        }
               
        public override void Complete(Vector2 point)
        {
            this.EndPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);

            if (this.IsCursorBox)
            {
                this.IsCursorBox = false;

                this.ViewModel.CurrentLayer = null;
                this.CurrentLayer = null;

                this.ViewModel.Invalidate();
                return;
            }

            if (this.CurrentLayer == null)
            {
                this.ViewModel.CurrentLayer = null;
            }
            else
            {
                this.CurrentLayer.Invalidate();
                this.ViewModelDictionary[this.Mode].Complete(point, this.CurrentLayer);
            }

            this.Mode = CursorMode.None;
            this.ViewModel.Invalidate();
        }
        

        
        public override void Draw(CanvasDrawingSession ds)
        {            
            if (this.IsCursorBox)
            {
                Vector2[] points = new Vector2[4];
                points[0] = Vector2.Transform(this.StartPoint, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
                points[1] = Vector2.Transform(new Vector2(this.StartPoint.X, this.EndPoint.Y), this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
                points[2] = Vector2.Transform(this.EndPoint, this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
                points[3] = Vector2.Transform(new Vector2(this.EndPoint.X, this.StartPoint.Y), this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);
                CanvasGeometry geometry = CanvasGeometry.CreatePolygon(this.ViewModel.CanvasDevice, points);

                ds.FillGeometry(geometry, Color.FromArgb(128, 30, 144, 255));
                ds.DrawGeometry(geometry, Colors.DodgerBlue, 1);

                return;
            }            

            if (this.CurrentLayer != null)
            {
                this.ViewModelDictionary[this.Mode].Draw(ds, this.CurrentLayer);
            }
        }



        /// <summary>
        /// Returns whether the radian area filled by the skew node contains the specified point. 
        /// </summary>
        /// <param name="point"> Input point. </param>
        /// <param name="transformer"> Layer's transformer. </param>
        /// <param name="canvasToVirtualToControlMatrix"></param>
        /// <param name="isSkew"> Skew mode? </param>
        /// <returns></returns>
        public CursorMode ContainsNodeMode(Vector2 point, Transformer transformer, Matrix3x2 canvasToVirtualToControlMatrix, bool isSkew = false)
        {
            Matrix3x2 matrix = transformer.Matrix * canvasToVirtualToControlMatrix;

            //LTRB
            Vector2 leftTop = transformer.TransformLeftTop(matrix);
            Vector2 rightTop = transformer.TransformRightTop(matrix);
            Vector2 rightBottom = transformer.TransformRightBottom(matrix);
            Vector2 leftBottom = transformer.TransformLeftBottom(matrix);

            //Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            if (isSkew == false)
            {
                //Scale
                if (Transformer.InNodeRadius(leftTop, point)) return CursorMode.ScaleLeftTop;
                if (Transformer.InNodeRadius(rightTop, point)) return CursorMode.ScaleRightTop;
                if (Transformer.InNodeRadius(rightBottom, point)) return CursorMode.ScaleRightBottom;
                if (Transformer.InNodeRadius(leftBottom, point)) return CursorMode.ScaleLeftBottom;

                //Scale
                if (Transformer.InNodeRadius(centerLeft, point)) return CursorMode.ScaleLeft;
                if (Transformer.InNodeRadius(centerTop, point)) return CursorMode.ScaleTop;
                if (Transformer.InNodeRadius(centerRight, point)) return CursorMode.ScaleRight;
                if (Transformer.InNodeRadius(centerBottom, point)) return CursorMode.ScaleBottom;
            }

            if (isSkew == false && transformer.DisabledRadian == false)
            {
                //Rotation
                Vector2 radians = centerTop - Vector2.Normalize(centerBottom - centerTop) * Transformer.NodeDistanceDouble;
                if (Transformer.InNodeRadius(radians, point)) return CursorMode.Rotation;
            }

            if (isSkew && transformer.DisabledRadian == false)
            {
                //Skew
                if (Transformer.InNodeRadius(centerLeft, point)) return CursorMode.SkewLeft;
                if (Transformer.InNodeRadius(centerTop, point)) return CursorMode.SkewTop;
                if (Transformer.InNodeRadius(centerRight, point)) return CursorMode.SkewRight;
                if (Transformer.InNodeRadius(centerBottom, point)) return CursorMode.SkewBottom;
            }

            return CursorMode.None;
        }

    }
}



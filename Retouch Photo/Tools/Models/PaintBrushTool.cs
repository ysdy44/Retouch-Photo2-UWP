using Microsoft.Graphics.Canvas;
using Retouch_Photo.Brushs;
using Retouch_Photo.Brushs.LinearGradient;
using Retouch_Photo.Models.Layers;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;
using System.Numerics;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Tools.Models
{
    public class PaintBrushTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public PaintBrushTool()
        {
            base.Type = ToolType.PaintBrush;
            base.Icon = new PaintBrushControl();
            base.WorkIcon = new PaintBrushControl();
            base.Page = new PaintBrushPage();
        }


        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }


        #region Override


        public override void Start(Vector2 point)
        {
            //Transformer      
            GeometryLayer geometryLayer = this.ViewModel.CurrentGeometryLayer;
            if (geometryLayer != null)
            {
                switch (geometryLayer.FillBrush.Type)
                {
                    case BrushType.None:
                        geometryLayer.FillBrush.Type = BrushType.LinearGradient;
                         this.LinearGradientInitialize(geometryLayer.FillBrush.LinearGradientManager, geometryLayer.Transformer, point);//LinearGradient
                        return;

                    case BrushType.Color:
                        geometryLayer.FillBrush.Type = BrushType.LinearGradient;
                        this.LinearGradientInitialize(geometryLayer.FillBrush.LinearGradientManager, geometryLayer.Transformer, point);//LinearGradient
                        return;

                    case BrushType.LinearGradient:
                        this.LinearGradientStart(geometryLayer.FillBrush.LinearGradientManager, geometryLayer.Transformer, point);//LinearGradient
                        break;

                    case BrushType.RadialGradient:
                        return;

                    case BrushType.Image:
                        return;

                    default:
                        return;
                }
            }
        }
        public override void Delta(Vector2 point)
        {
            //Transformer      
            GeometryLayer geometryLayer = this.ViewModel.CurrentGeometryLayer;
            if (geometryLayer != null)
            {
                switch (geometryLayer.FillBrush.Type)
                {
                    case BrushType.None:
                        return;

                    case BrushType.Color:
                        return;

                    case BrushType.LinearGradient:
                        this.LinearGradientDelta(geometryLayer.FillBrush.LinearGradientManager, geometryLayer.Transformer,point);//LinearGradient
                        break;

                    case BrushType.RadialGradient:
                        return;

                    case BrushType.Image:
                        return;

                    default:
                        break;
                }
            }
        }
        public override void Complete(Vector2 point)
        {
            //Transformer      
            GeometryLayer geometryLayer = this.ViewModel.CurrentGeometryLayer;
            if (geometryLayer != null)
            {
                switch (geometryLayer.FillBrush.Type)
                {
                    case BrushType.None:
                   return;

                    case BrushType.Color:
                        return;

                    case BrushType.LinearGradient:
                        this.LinearGradientComplete(geometryLayer.FillBrush.LinearGradientManager);//LinearGradient
                        break;

                    case BrushType.RadialGradient:
                        return;

                    case BrushType.Image:
                        return;

                    default:
                        break;
                }
            }
        }

        public override void Draw(CanvasDrawingSession ds)
        {
            //Transformer      
            GeometryLayer geometryLayer = this.ViewModel.CurrentGeometryLayer;
            if (geometryLayer != null)
            {
                Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;
                geometryLayer.Draw(this.ViewModel.CanvasDevice, ds, matrix);


                switch (geometryLayer.FillBrush.Type)
                {
                    case BrushType.None:
                        break;

                    case BrushType.Color:
                        break;

                    case BrushType.LinearGradient:
                        Matrix3x2 matrix2 = geometryLayer.Transformer.Matrix * matrix;
                        this.LinearGradientDraw(ds, geometryLayer.FillBrush.LinearGradientManager, matrix2);//LinearGradient
                        break;

                    case BrushType.RadialGradient:

                        break;

                    case BrushType.Image:
                        break;

                    default:
                        break;
                }

                
                this.ViewModel.Invalidate();
            }
        }


        #endregion


        #region LinearGradient


        public void LinearGradientInitialize(LinearGradientManager manager, Transformer transformer, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix * transformer.InverseMatrix;
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            manager.StartPoint = canvasPoint;
            manager.EndPoint = canvasPoint;

            manager.Type = LinearGradientType.EndPoint;

             base.Page.Communication(2);//Communication
        }

        public void LinearGradientStart(LinearGradientManager manager, Transformer transformer, Vector2 point)
        {
            Matrix3x2 matrix = transformer.Matrix * this.ViewModel.MatrixTransformer.Matrix;

            Vector2 startPoint = Vector2.Transform(manager.StartPoint, matrix);
            if (Transformer.OutNodeDistance(point, startPoint) == false)
            {
                manager.Type = LinearGradientType.StartPoint;
                return;
            }

            Vector2 endPoint = Vector2.Transform(manager.EndPoint, matrix);
            if (Transformer.OutNodeDistance(point, endPoint) == false)
            {
                manager.Type = LinearGradientType.EndPoint;
                return;
            }

            this.ViewModel.Invalidate();
        }
        public void LinearGradientDelta(LinearGradientManager manager, Transformer transformer,Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix * transformer.InverseMatrix;
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (manager.Type)
            {
                case LinearGradientType.None:
                    return;

                case LinearGradientType.StartPoint:
                    manager.StartPoint = canvasPoint;
                    this.ViewModel.Invalidate();
                    break;

                case LinearGradientType.EndPoint:
                    manager.EndPoint = canvasPoint;
                    break;

                default:
                    return;
            }
        }
        public void LinearGradientComplete(LinearGradientManager manager)
        {
            manager.Type = LinearGradientType.None;
        }

        public void LinearGradientDraw(CanvasDrawingSession ds, LinearGradientManager manager, Matrix3x2 matrix)
        {
            Vector2 startPoint = Vector2.Transform(manager.StartPoint, matrix);
            Vector2 endPoint = Vector2.Transform(manager.EndPoint, matrix);

            ds.DrawLine(startPoint, endPoint, Colors.DodgerBlue);
            Transformer.DrawNode(ds, startPoint);
            Transformer.DrawNode(ds, endPoint);
        }

        #endregion

    }
}

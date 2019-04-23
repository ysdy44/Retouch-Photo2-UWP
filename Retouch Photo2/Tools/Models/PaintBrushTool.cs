using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.LinearGradient;
using Retouch_Photo2.Models.Layers;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.ITools;
using Retouch_Photo2.Tools.Models.PaintBrushTools;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Tools.Models
{
    public class PaintBrushTool : Tool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        readonly IClickedTool IClickedTool;
        readonly ILinearGradient ILinearGradient= new ILinearGradient();
        readonly IRadialGradient IRadialGradient= new IRadialGradient();
        readonly IEllipticalGradient IEllipticalGradient= new IEllipticalGradient();

        public PaintBrushTool()
        {
            base.Type = ToolType.PaintBrush;
            base.Icon = new PaintBrushControl();
            base.WorkIcon = new PaintBrushControl();
            base.Page = new PaintBrushPage();
            
            this.IClickedTool = new IClickedTool(this.PaintBrushStart, this.PaintBrushDelta, this.PaintBrushComplete);
        }

        //@Override
        public override void ToolOnNavigatedTo() { }//当前页面成为活动页面
        public override void ToolOnNavigatedFrom() { }//当前页面不再成为活动页面

        public override void Start(Vector2 point) => this.IClickedTool.Start(point);
        public override void Delta(Vector2 point) => this.IClickedTool.Delta(point);
        public override void Complete(Vector2 point) => this.IClickedTool.Complete(point);
        
        public override void Draw(CanvasDrawingSession ds)=>  this.PaintBrushDraw(ds);

        
        private bool PaintBrushStart(Vector2 point)
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
                        return true;

                    case BrushType.Color:
                        geometryLayer.FillBrush.Type = BrushType.LinearGradient;
                        this.LinearGradientInitialize(geometryLayer.FillBrush.LinearGradientManager, geometryLayer.Transformer, point);//LinearGradient
                        return true;

                    case BrushType.LinearGradient:
                        this.ILinearGradient.Start(geometryLayer.FillBrush.LinearGradientManager, geometryLayer.Transformer, point);//LinearGradient
                        break;

                    case BrushType.RadialGradient:
                        this.IRadialGradient.Start(geometryLayer.FillBrush.RadialGradientManager, geometryLayer.Transformer, point);//LinearGradient
                        return true;

                    case BrushType.EllipticalGradient:
                        this.IEllipticalGradient.Start(geometryLayer.FillBrush.EllipticalGradientManager, geometryLayer.Transformer, point);//LinearGradient
                        return true;

                    case BrushType.Image:
                        return true;

                    default:
                        return true;
                }
            }
            return true;

        }
        private bool PaintBrushDelta(Vector2 point)
        {
            //Transformer      
            GeometryLayer geometryLayer = this.ViewModel.CurrentGeometryLayer;
            if (geometryLayer != null)
            {
                switch (geometryLayer.FillBrush.Type)
                {
                    case BrushType.None:
                        return true;

                    case BrushType.Color:
                        return true;

                    case BrushType.LinearGradient:
                        this.ILinearGradient.Delta(geometryLayer.FillBrush.LinearGradientManager, geometryLayer.Transformer, point);//LinearGradient
                        break;

                    case BrushType.RadialGradient:
                        this.IRadialGradient.Delta(geometryLayer.FillBrush.RadialGradientManager, geometryLayer.Transformer, point);//LinearGradient
                        return true;

                    case BrushType.EllipticalGradient:
                        this.IEllipticalGradient.Delta(geometryLayer.FillBrush.EllipticalGradientManager, geometryLayer.Transformer, point);//LinearGradient
                        return true;

                    case BrushType.Image:
                        return true;

                    default:
                        break;
                }
            }
            return true;

        }
        private bool PaintBrushComplete(Vector2 point)
        {
            //Transformer      
            GeometryLayer geometryLayer = this.ViewModel.CurrentGeometryLayer;
            if (geometryLayer != null)
            {
                switch (geometryLayer.FillBrush.Type)
                {
                    case BrushType.None:
                        return true;

                    case BrushType.Color:
                        return true;

                    case BrushType.LinearGradient:
                        this.ILinearGradient.Complete(geometryLayer.FillBrush.LinearGradientManager);//LinearGradient
                        break;

                    case BrushType.RadialGradient:
                        this.IRadialGradient.Complete(geometryLayer.FillBrush.RadialGradientManager);//LinearGradient
                        return true;

                    case BrushType.EllipticalGradient:
                        this.IEllipticalGradient.Complete(geometryLayer.FillBrush.EllipticalGradientManager);//LinearGradient
                        return true;

                    case BrushType.Image:
                        return true;

                    default:
                        break;
                }
            }
            return true;

        }

        private bool PaintBrushDraw(CanvasDrawingSession ds)
        {
            //Transformer      
            GeometryLayer geometryLayer = this.ViewModel.CurrentGeometryLayer;
            if (geometryLayer == null)
            {
                return false;
            }
            else
            {
                Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;
                geometryLayer.Draw(this.ViewModel.CanvasDevice, ds, matrix);

                Matrix3x2 matrix2 = geometryLayer.Transformer.Matrix * matrix;


                switch (geometryLayer.FillBrush.Type)
                {
                    case BrushType.None:
                        break;

                    case BrushType.Color:
                        break;

                    case BrushType.LinearGradient:
                        this.ILinearGradient.Draw(ds, geometryLayer.FillBrush.LinearGradientManager, matrix2);//LinearGradient
                        break;

                    case BrushType.RadialGradient:
                        this.IRadialGradient.Draw(ds, geometryLayer.FillBrush.RadialGradientManager, matrix2);//LinearGradient
                        break;

                    case BrushType.EllipticalGradient:
                        this.IEllipticalGradient.Draw(ds, geometryLayer.FillBrush.EllipticalGradientManager, matrix2);//LinearGradient
                        break;

                    case BrushType.Image:
                        break;

                    default:
                        break;
                }

                this.ViewModel.Invalidate();
                return true;
            }
        }
        

        private void LinearGradientInitialize(LinearGradientManager manager, Transformer transformer, Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix * transformer.InverseMatrix;
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            manager.StartPoint = canvasPoint;
            manager.EndPoint = canvasPoint;

            manager.Type = LinearGradientType.EndPoint;

            base.Page.Communication(2);//Communication
        }
    }
}

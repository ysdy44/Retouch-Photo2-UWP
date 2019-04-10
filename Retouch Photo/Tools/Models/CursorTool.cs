using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Models;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;
using System.Numerics;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Tools.Models
{
    /// <summary> Show blue Box </summary>
    public class CursorBox
    {
        public bool IsCursorBox;

        public Vector2 StartPoint;
        public Vector2 EndPoint;

        public void Draw(ICanvasResourceCreator creator, CanvasDrawingSession ds, Matrix3x2 matrix)
        {
            Vector2[] points = new Vector2[4];
            points[0] = Vector2.Transform(this.StartPoint, matrix);
            points[1] = Vector2.Transform(new Vector2(this.StartPoint.X, this.EndPoint.Y), matrix);
            points[2] = Vector2.Transform(this.EndPoint, matrix);
            points[3] = Vector2.Transform(new Vector2(this.EndPoint.X, this.StartPoint.Y), matrix);
            CanvasGeometry geometry = CanvasGeometry.CreatePolygon(creator, points);

            ds.FillGeometry(geometry, Color.FromArgb(128, 30, 144, 255));
            ds.DrawGeometry(geometry, Colors.DodgerBlue, 1);
        }
    }


    public class CursorTool : ICursorTool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;
        
        readonly CursorBox Box = new CursorBox();
        
        public CursorTool()
        {
            base.Type = ToolType.Cursor;
            base.Icon = new CursorControl();
            base.WorkIcon = new CursorControl();
            base.Page = new CursorPage();
        }


        //@Override
        public override void ToolOnNavigatedTo()//当前页面成为活动页面
        {
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }
                

        public override bool OperatorStart(Vector2 point)
        {
            //Translation 
            Layer layer = this.GetTranslationLayer(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            if (layer != null)
            {
                this.ViewModel.CurrentLayer = layer;

                TransformerMode mode = TransformerMode.Translation;
                this.ViewModel.TransformerMode = mode;
                this.ViewModel.TransformerDictionary[mode].Start(point, layer, this.ViewModel.MatrixTransformer.Matrix, this.ViewModel.MatrixTransformer.InverseMatrix);
                return true;
            }

            //CursorBox
            this.Box.IsCursorBox = true;
            this.Box.StartPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            return true;
        }
        public override bool OperatorDelta(Vector2 point)
        {
            //CursorBox
            if (this.Box.IsCursorBox)
            {
                this.Box.EndPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
                this.ViewModel.Invalidate();
                return true;
            }
            return false;
        }
        public override bool OperatorComplete(Vector2 point)
        {
            //CursorBox
            if (this.Box.IsCursorBox)
            {
                this.Box.EndPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
                this.Box.IsCursorBox = false;

                this.ViewModel.CurrentLayer = null;
                this.ViewModel.Invalidate();
                return true;
            }
            return false;
        }
        
        public override bool OperatorDraw(CanvasDrawingSession ds)
        {    
            //CursorBox
            if (this.Box.IsCursorBox)
            {
                this.Box.Draw(this.ViewModel.CanvasDevice, ds, this.ViewModel.MatrixTransformer.Matrix);
                return true;
            }
            return false;
        }


        public Layer GetTranslationLayer(Vector2 point, Matrix3x2 inverseMatrix)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            foreach (Layer layer in this.ViewModel.RenderLayer.Layers)
            {
                if (layer.IsVisual == false || layer.Opacity == 0) continue;

                if (Transformer.ContainsBound(canvasPoint, layer.Transformer))
                {
                    return layer;
                }
            }

            return null;
        }
    }
}

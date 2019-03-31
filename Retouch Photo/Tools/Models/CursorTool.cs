using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Models;
using Retouch_Photo.Tools.Controls;
using Retouch_Photo.Tools.Pages;
using Retouch_Photo.ViewModels;
using System.Collections.Generic;
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


    public class CursorTool : Tool
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


        public override void Start(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;


            // Transformer
            if (this.ViewModel.TransformerStart(point)) return;


            //Translation 
            Layer layer2 = this.GetTranslationLayer(point, inverseMatrix);
            if (layer2 != null)
            {
                this.ViewModel.CurrentLayer = layer2;

                TransformerMode mode = TransformerMode.Translation;
                this.ViewModel.TransformerMode = mode;
                this.ViewModel.TransformerDictionary[mode].Start(point, layer2, matrix, inverseMatrix);
                return;
            }

            //CursorBox
            this.Box.IsCursorBox = true;
            this.Box.StartPoint = Vector2.Transform(point, inverseMatrix);
        }

        public override void Delta(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;

            //CursorBox
            if (this.Box.IsCursorBox)
            {
                this.Box.EndPoint = Vector2.Transform(point, inverseMatrix);
                this.ViewModel.Invalidate();
                return;
            }

            // Transformer
            if (this.ViewModel.TransformerDelta(point)) return;
        }

        public override void Complete(Vector2 point)
        {
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;

            //CursorBox
            if (this.Box.IsCursorBox)
            {
                this.Box.EndPoint = Vector2.Transform(point, inverseMatrix);
                this.Box.IsCursorBox = false;

                this.ViewModel.CurrentLayer = null;
                this.ViewModel.Invalidate();
                return;
            }

            // Transformer
            if (this.ViewModel.TransformerComplete(point)) return;
        }


        public override void Draw(CanvasDrawingSession ds)
        {
            Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;

            //CursorBox
            if (this.Box.IsCursorBox)
            {
                this.Box.Draw(this.ViewModel.CanvasDevice, ds, matrix);
                return;
            }

            //Transformer      
            if (this.ViewModel.TransformerDraw(ds)) return;
        }
    }
}

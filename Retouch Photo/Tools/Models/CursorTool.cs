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


        Layer CurrentLayer;

        readonly CursorBox Box = new CursorBox();

        TransformerMode Mode = TransformerMode.None;
        readonly Dictionary<TransformerMode, IController> Dictionary = new Dictionary<TransformerMode, IController>
            {
                {TransformerMode.None,  new NoneController()},
                {TransformerMode.Translation,  new TranslationController()},
                {TransformerMode.Rotation,  new RotationController()},

                {TransformerMode.SkewLeft,  new SkewLeftController()},
                {TransformerMode.SkewTop,  new SkewTopController()},
                {TransformerMode.SkewRight,  new SkewRightController()},
                {TransformerMode.SkewBottom,  new SkewBottomController()},

                {TransformerMode.ScaleLeft,  new ScaleLeftController()},
                {TransformerMode.ScaleTop,  new ScaleTopController()},
                {TransformerMode.ScaleRight,  new ScaleRightController()},
                {TransformerMode.ScaleBottom,  new ScaleBottomController()},

                {TransformerMode.ScaleLeftTop,  new ScaleLeftTopController()},
                {TransformerMode.ScaleRightTop,  new ScaleRightTopController()},
                {TransformerMode.ScaleRightBottom,  new ScaleRightBottomController()},
                {TransformerMode.ScaleLeftBottom,  new ScaleLeftBottomController()},
            };


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
            this.CurrentLayer = this.ViewModel.CurrentLayer;
            if (this.CurrentLayer == null) return;
            this.ViewModel.Invalidate();
        }
        public override void ToolOnNavigatedFrom()//当前页面不再成为活动页面
        {
        }


        public bool IsTransformer(TransformerMode mode)
        {
            if (mode == TransformerMode.None) return false;
            if (mode == TransformerMode.Translation) return false;
            return true;
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
            this.CurrentLayer = this.ViewModel.CurrentLayer;

            // Transformer
            if (this.CurrentLayer != null)
            {
                TransformerMode mode = Transformer.ContainsNodeMode(point, this.CurrentLayer.Transformer, matrix);
                if (this.IsTransformer(mode))
                {
                    this.Mode = mode;
                    this.Dictionary[mode].Start(point, this.CurrentLayer, matrix, inverseMatrix);
                    return;
                }
            }

            //Translation 
            this.CurrentLayer = this.GetTranslationLayer(point, inverseMatrix);
            if (this.CurrentLayer != null)
            {
                this.ViewModel.CurrentLayer = this.CurrentLayer;

                this.Mode = TransformerMode.Translation;
                this.Dictionary[this.Mode].Start(point, this.CurrentLayer, matrix, inverseMatrix);
                return;
            }

            //CursorBox
            this.Box.IsCursorBox = true;
            this.Box.StartPoint = Vector2.Transform(point, inverseMatrix);
        }

        public override void Delta(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;

            //CursorBox
            if (this.Box.IsCursorBox)
            {
                this.Box.EndPoint = Vector2.Transform(point, inverseMatrix);
                this.ViewModel.Invalidate();
                return;
            }

            // Transformer
            if (this.CurrentLayer != null)
            {
                TransformerMode mode = this.Mode;
                this.Dictionary[mode].Delta(point, this.CurrentLayer, matrix, inverseMatrix);

                this.ViewModel.Transformer = this.CurrentLayer.Transformer;//Transformer
                this.ViewModel.Invalidate();
                return;
            }
        }

        public override void Complete(Vector2 point)
        {
            Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;
            Matrix3x2 inverseMatrix = this.ViewModel.MatrixTransformer.InverseMatrix;

            //CursorBox
            if (this.Box.IsCursorBox)
            {
                this.Box.EndPoint = Vector2.Transform(point, inverseMatrix);
                this.Box.IsCursorBox = false;

                this.ViewModel.CurrentLayer = null;
                this.CurrentLayer = null;

                this.ViewModel.Invalidate();
                return;
            }

            // Transformer
            if (this.CurrentLayer != null)
            {
                TransformerMode mode = this.Mode;
                this.Dictionary[mode].Complete(point, this.CurrentLayer, matrix, inverseMatrix);

                this.Mode = TransformerMode.None;
                this.ViewModel.Invalidate();
                return;
            }

            this.ViewModel.CurrentLayer = null;
            this.Mode = TransformerMode.None;
            this.ViewModel.Invalidate();
        }
        

        public override void Draw(CanvasDrawingSession ds)
        {
            Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;
            Matrix3x2 canvasToVirtualMatrix = this.ViewModel.MatrixTransformer.CanvasToVirtualMatrix;

            //CursorBox
            if (this.Box.IsCursorBox)
            {
                this.Box.Draw(this.ViewModel.CanvasDevice, ds, matrix);
                return;
            }

            //Transformer
            if (this.CurrentLayer != null)
            {
                Transformer.DrawBoundNodes(ds, this.CurrentLayer.Transformer, matrix);
                this.CurrentLayer.Draw(this.ViewModel.CanvasDevice, ds, canvasToVirtualMatrix);
            }
        }
    }
}

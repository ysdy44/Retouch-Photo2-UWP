using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo.Models;
using System.Numerics;
using Windows.UI;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.ViewModels.ToolViewModels
{


    public class CursorViewModel : ToolViewModel
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;
        bool IsSkew => this.ViewModel.KeyAlt;
         

        Layer CurrentLayer;
        readonly Controller Controller = new Controller();
      
        /// <summary> 蓝色选框 </summary>
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
                Matrix3x2 matrix = this.ViewModel.MatrixTransformer.Matrix;

                this.Controller.Mode = Transformer.ContainsNodeMode(point, this.CurrentLayer.Transformer, matrix);

                if (!(this.Controller.Mode == TransformerMode.None || this.Controller.Mode == TransformerMode.Translation))
                {
                    this.Controller.ControllerDictionary[this.Controller.Mode].Start
                    (
                        point,
                        this.CurrentLayer,
                        matrix,
                        this.ViewModel.MatrixTransformer.InverseMatrix
                    );
                    return;
                }
            }


            //Translation                
            Vector2 canvasPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            foreach (Layer layer in this.ViewModel.RenderLayer.Layers)
            {
                if (layer.IsVisual == false || layer.Opacity == 0) continue;

                if (Transformer.ContainsBound(canvasPoint, layer.Transformer))
                {                       
                    this.CurrentLayer = layer;
                    this.ViewModel.CurrentLayer = layer;
                    this.Controller.Mode = TransformerMode.Translation;

                    this.Controller.ControllerDictionary[this.Controller.Mode].Start
                    (
                        point, 
                        this.CurrentLayer,
                        this.ViewModel.MatrixTransformer.Matrix,
                        this.ViewModel.MatrixTransformer.InverseMatrix
                    );
                    return;
                }
            }


            //CursorBox
            this.CurrentLayer = null;
            this.IsCursorBox = true;
            this.StartPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
        }

        public override void Delta(Vector2 point)
        {
            this.EndPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);

            if (this.IsCursorBox)
            {
                this.ViewModel.Invalidate();
                return;
            }

            if (this.CurrentLayer != null)
            {
                this.Controller.ControllerDictionary[this.Controller.Mode].Delta
                (
                     point,
                     this.CurrentLayer,
                     this.ViewModel.MatrixTransformer.Matrix,
                     this.ViewModel.MatrixTransformer.InverseMatrix
                );

                this.ViewModel.Transformer = this.CurrentLayer.Transformer;//Transformer
                this.ViewModel.Invalidate();
                return;
            }
        }
               
        public override void Complete(Vector2 point)
        {
            this.EndPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);

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
                this.Controller.ControllerDictionary[this.Controller.Mode].Complete
                (
                    point,
                    this.CurrentLayer,
                    this.ViewModel.MatrixTransformer.Matrix,
                    this.ViewModel.MatrixTransformer.InverseMatrix
                );
            }

            this.Controller.Mode = TransformerMode.None;
            this.ViewModel.Invalidate();
        }
        

        
        public override void Draw(CanvasDrawingSession ds)
        {            
            if (this.IsCursorBox)
            {
                Vector2[] points = new Vector2[4];
                points[0] = Vector2.Transform(this.StartPoint, this.ViewModel.MatrixTransformer.Matrix);
                points[1] = Vector2.Transform(new Vector2(this.StartPoint.X, this.EndPoint.Y), this.ViewModel.MatrixTransformer.Matrix);
                points[2] = Vector2.Transform(this.EndPoint, this.ViewModel.MatrixTransformer.Matrix);
                points[3] = Vector2.Transform(new Vector2(this.EndPoint.X, this.StartPoint.Y), this.ViewModel.MatrixTransformer.Matrix);
                CanvasGeometry geometry = CanvasGeometry.CreatePolygon(this.ViewModel.CanvasDevice, points);

                ds.FillGeometry(geometry, Color.FromArgb(128, 30, 144, 255));
                ds.DrawGeometry(geometry, Colors.DodgerBlue, 1);

                return;
            }            

            if (this.CurrentLayer != null)
            {
                Transformer.DrawBoundNodes(ds, this.CurrentLayer.Transformer, this.ViewModel.MatrixTransformer.Matrix);
            }
        }


    }
}



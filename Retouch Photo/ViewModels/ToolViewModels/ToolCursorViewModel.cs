using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers.GeometryLayers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.ViewModels.ToolViewModels
{

    enum CursorMode
    {
        None,
        Move,
        Rotation,
        Transform,
    }

    public class ToolCursorViewModel : ToolViewModel
    {
        CursorMode Mode = CursorMode.None;
        ToolCursorTranslationViewModel MoveViewModel = new ToolCursorTranslationViewModel();
        ToolCursorRotationViewModel RotationViewModel = new ToolCursorRotationViewModel();
        ToolCursorTransformViewModel TransformViewModel = new ToolCursorTransformViewModel();

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            Layer layer = viewModel.RenderLayer.CurrentLayer;
            if (layer!=null)
            {
                Vector2 radiansNode = Transformer.GetRadianNode(layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix);

                if (Transformer.NodeRadiusOut(radiansNode, point) == false)
                {
                    this.Mode = CursorMode.Rotation;
                    this.RotationViewModel.Start(point, viewModel);
                    return;
                }
            }
            
            this.Mode = CursorMode.Move;
            this.MoveViewModel.Start(point, viewModel);
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            switch (this.Mode)
            {
                case CursorMode.None:
                    break;
                case CursorMode.Move:
                    this.MoveViewModel.Delta(point, viewModel);
                    break;
                case CursorMode.Rotation:
                    this.RotationViewModel.Delta(point, viewModel);
                    break;
                case CursorMode.Transform:
                    break;
                default:
                    break;
            }
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            switch (this.Mode)
            {
                case CursorMode.None:
                    break;
                case CursorMode.Move:
                    this.MoveViewModel.Complete(point, viewModel);
                    break;
                case CursorMode.Rotation:
                    this.RotationViewModel.Complete(point, viewModel);
                    break;
                case CursorMode.Transform:
                    break;
                default:
                    break;
            }
            this.Mode = CursorMode.None;
        }


        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            Layer layer = viewModel.RenderLayer.CurrentLayer;

            if (layer == null) return;
            Transformer.DrawNodeLine(ds, layer.Transformer, viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix,true);
            
            switch (this.Mode)
            {
                case CursorMode.None:
                    break;
                case CursorMode.Move:
                    this.MoveViewModel.Draw(ds, viewModel);
                    break;
                case CursorMode.Rotation:
                    this.RotationViewModel.Draw(ds, viewModel);
                    break;
                case CursorMode.Transform:
                    break;
                default:
                    break;
            }
        }

         
    }


    public class ToolCursorTranslationViewModel : ToolViewModel
    {
        Layer CurrentLayer;

        Vector2 LayerStartPostion;
        Vector2 StartPoint;

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.StartPoint = Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
            this.CurrentLayer = viewModel.RenderLayer.CurrentLayer = this.GetLayerWhichFillContainsPoint
            (
                creator: viewModel.CanvasControl,
                layers: viewModel.RenderLayer.Layers,
                point: this.StartPoint
            );

            if (this.CurrentLayer == null) return;
            this.LayerStartPostion = this.CurrentLayer.Transformer.Postion;
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
            if (this.CurrentLayer == null) return;

            this.CurrentLayer.Transformer.Postion= this.LayerStartPostion - this.StartPoint + Vector2.Transform(point, viewModel.MatrixTransformer.ControlToVirtualToCanvasMatrix);
         
            viewModel.Invalidate();
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            this.CurrentLayer = null;
            viewModel.Invalidate();
        }

        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
        }

        private Layer GetLayerWhichFillContainsPoint(ICanvasResourceCreator creator, ObservableCollection<Layer> layers, Vector2 point)
        {
            foreach (Layer layer in layers)
            {
                if (layer.IsVisual == false || layer.Opacity == 0) continue;

                if (layer.Transformer.FillContainsPoint(point)) return layer;                
            }
            return null;
        }

    }


    public class ToolCursorRotationViewModel : ToolViewModel
    {

        Layer CurrentLayer;
        Vector2 Center;

        float LayerStartRadian;
        float StartRadian;

        float Radians;

        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
            this.CurrentLayer = viewModel.RenderLayer.CurrentLayer;

            if (this.CurrentLayer == null) return;

            this.Center = Vector2.Transform
            (
                new Vector2
                (
                    this.CurrentLayer.Transformer.Width / 2, 
                    this.CurrentLayer.Transformer.Height / 2
                 ) , 
                this.CurrentLayer.Transformer.Matrix * viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix
             );

            this.LayerStartRadian = this.CurrentLayer.Transformer.Radian;
            this.StartRadian = this.VectorToRadians(point-this.Center);

            viewModel.Invalidate();
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
          this.Radians = this.VectorToRadians(point - this.Center);

            if (this.CurrentLayer == null) return;
            this.CurrentLayer.Transformer.Radian = this.LayerStartRadian - this.StartRadian + this.Radians;

            viewModel.Invalidate();
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
            viewModel.Invalidate();

        }

        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
            ds.DrawLine(this.Center, this.RadiansToVector(this.StartRadian, this.Center), Colors.Gray);
            ds.DrawLine(this.Center, this.RadiansToVector(this.Radians, this.Center), Colors.Gray);
        }

        public float VectorToRadians(Vector2 vector)
        {
            float tan = (float)Math.Atan(Math.Abs(vector.Y / vector.X));

            //First Quantity
            if (vector.X > 0 && vector.Y > 0) return tan;
            //Second Quadrant
            else if (vector.X > 0 && vector.Y < 0) return -tan;
            //Third Quadrant  
            else if (vector.X < 0 && vector.Y > 0) return (float)Math.PI - tan;
            //Fourth Quadrant  
            else return tan - (float)Math.PI;
        }
        public Vector2 RadiansToVector(float radians,Vector2 center, float distance=40.0f)
        {
            return new Vector2((float)Math.Cos(radians) * distance + center.X, (float)Math.Sin(radians) * distance + center.Y);
        }

    }


    public class ToolCursorTransformViewModel : ToolViewModel
    {
        public override void Start(Vector2 point, DrawViewModel viewModel)
        {
        }
        public override void Delta(Vector2 point, DrawViewModel viewModel)
        {
        }
        public override void Complete(Vector2 point, DrawViewModel viewModel)
        {
        }
        public override void Draw(CanvasDrawingSession ds, DrawViewModel viewModel)
        {
        }
    }


}



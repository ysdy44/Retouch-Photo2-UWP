using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorScaleViewModels
{
    public abstract class ToolCursorScale1ViewModel : ToolCursorScaleViewModel
    {

        //@Override
        public abstract Orientation GetOrientation();
        public abstract Vector2 GetPoint(Layer layer, Matrix3x2 matrix);
        public abstract Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix);
        public abstract void SetPostion(Layer layer, Transformer startTransformer, float cos, float sin);


        Vector2 Point;
        VectorLine Line;

        public override void Start(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            base.Start(point,layer,viewModel);

            Matrix3x2 matrix = layer.Transformer.Matrix * viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix;
            this.Point = this.GetPoint(layer, matrix);

            Vector2 diagonal = this.GetDiagonal(layer, matrix);
            this.Line = new VectorLine
            {
                Diagonal = diagonal,
                Symmetric = diagonal + diagonal - this.Point,
                Center = layer.Transformer.TransformCenter(matrix)
            };
        }

        public override void Delta(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            Vector2 footPoint = Transformer.FootPoint(point, this.Line.Diagonal, this.Point);
            VectorDistance distance = new VectorDistance
            {
                FD = Vector2.Distance(footPoint, this.Line.Diagonal),
                FP = Vector2.Distance(footPoint, this.Point),
                FC = Vector2.Distance(footPoint, this.Line.Center),
                PC = Vector2.Distance(this.Point, this.Line.Center),
                FS = Vector2.Distance(footPoint, this.Line.Symmetric),
                PD = Vector2.Distance(this.Point, this.Line.Diagonal),
            };

            //Scale with Center
            if (viewModel.KeyCtrl)
            {
                //Scale
                float scale = distance.FC / distance.PC;
                this.SetScale(layer,scale, viewModel.KeyShift);

                //Flip
                bool isFlip = distance.FD > distance.FP;
                this.SetFlip(layer, isFlip);
            }

            //Scale with Side
            else
            {
                //Scale
                float scale = distance.FD / distance.PD;
                this.SetScale(layer, scale, viewModel.KeyShift);

                //Flip
                bool isFlip = distance.FS > distance.FP;
                this.SetFlip(layer,isFlip);

                //Postion
                float move = distance.FP / 2 / viewModel.MatrixTransformer.Scale;
                this.SetReversePostion(layer, base.Cos * move, base.Sin * move, distance);
            }
        }

        


        //Scale
        public void SetScale(Layer layer,  float scale, bool isRatio)
        {
            if (isRatio)
            {
                layer.Transformer.XScale = this.StartTransformer.XScale * scale;
                layer.Transformer.YScale = this.StartTransformer.YScale * scale;
            }
            else
            {
                if (this.GetOrientation() == Orientation.Horizontal)
                    layer.Transformer.XScale = this.StartTransformer.XScale * scale;
                else
                    layer.Transformer.YScale = this.StartTransformer.YScale * scale;
            }
        }


        //Flip
        public void SetFlip(Layer layer,  bool isFlip)
        {
            if (this.GetOrientation() == Orientation.Horizontal)
                layer.Transformer.FlipHorizontal = (this.StartTransformer.FlipHorizontal == isFlip);
            else
                layer.Transformer.FlipVertical = (this.StartTransformer.FlipVertical == isFlip);
        }


        //Postion: Reverse
        public void SetReversePostion(Layer layer, float cos, float sin, VectorDistance distance)
        {
            bool reverse = distance.FD < distance.PD ? true : distance.FD < distance.FP;//F in the left of the P ?

            if (reverse)
                this.SetOrientationPostion(layer,  cos, sin);
            else
                this.SetOrientationPostion(layer,  -cos, -sin);
        }

        //Postion: Orientation
        public void SetOrientationPostion(Layer layer, float cos, float sin)
        {
            if (this.GetOrientation() == Orientation.Horizontal)
                this.SetFlipPostion(layer, cos, sin, this.StartTransformer.FlipHorizontal);
            else
                this.SetFlipPostion(layer, -cos, -sin, this.StartTransformer.FlipHorizontal);
        }

        //Postion: Flip
        public void SetFlipPostion(Layer layer, float cos, float sin,bool flip)
        {
            if (flip)
                this.SetPostion(layer, this.StartTransformer, cos, sin);
            else
                this.SetPostion(layer, this.StartTransformer, -cos, -sin);
        }     
        

    }
}

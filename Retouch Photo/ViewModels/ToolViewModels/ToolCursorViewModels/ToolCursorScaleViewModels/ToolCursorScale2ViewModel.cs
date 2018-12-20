using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using System;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo.ViewModels.ToolViewModels.ToolCursorViewModels.ToolCursorScaleViewModels
{
    public abstract class ToolCursorScale2ViewModel : ToolCursorScaleViewModel
    {
        //@Override
        public abstract Vector2 GetPoint(Layer layer, Matrix3x2 matrix);
        public abstract Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix);
        public abstract Vector2 GetHorizontalDiagonal(Layer layer, Matrix3x2 matrix);
        public abstract Vector2 GetVerticalDiagonal(Layer layer, Matrix3x2 matrix);

        public abstract void SetPostion(Layer layer, Transformer startTransformer, 
            float xCos, float xSin, 
            float yCos, float ySin);


        Vector2 Point;

        VectorLine Line;
        VectorLine HorizontalLine;
        VectorLine VerticalLine;

        public override void Start(Vector2 point, Layer layer, DrawViewModel viewModel)
        {
            base.Start(point, layer, viewModel);

            Matrix3x2 matrix = layer.Transformer.Matrix * viewModel.MatrixTransformer.CanvasToVirtualToControlMatrix;
            this.Point = this.GetPoint(layer, matrix);

            Vector2 diagonal = this.GetDiagonal(layer, matrix);
            this.Line = new VectorLine
            {
                Diagonal = diagonal,
                Symmetric = diagonal + diagonal - this.Point,
                Center = layer.Transformer.TransformCenter(matrix)
            };

            Vector2 horizontalDiagonal = this.GetHorizontalDiagonal(layer, matrix);
            this.HorizontalLine = new VectorLine
            {
                Diagonal = horizontalDiagonal,
                Symmetric = horizontalDiagonal + horizontalDiagonal - this.Point,
                Center = (this.Point + horizontalDiagonal) / 2
            };
            
            Vector2 verticalDiagonal = this.GetVerticalDiagonal(layer, matrix);
            this.VerticalLine = new VectorLine
            {
                Diagonal = verticalDiagonal,
                Symmetric = verticalDiagonal + verticalDiagonal - this.Point,
                Center = (this.Point + verticalDiagonal) / 2,
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

            Vector2 point2 = viewModel.KeyShift ? footPoint : point;

            Vector2 horizontalFootPoint = Transformer.FootPoint(point2, this.HorizontalLine.Diagonal, this.Point);
            VectorDistance horizontalDistance = this.GetDistance(horizontalFootPoint, this.Point, this.HorizontalLine);

            Vector2 verticalFootPoint = Transformer.FootPoint(point2, this.VerticalLine.Diagonal, this.Point);
            VectorDistance verticalDistance = this.GetDistance(verticalFootPoint, this.Point, this.VerticalLine);
                                 

            //Scale with Center
            if (viewModel.KeyCtrl)
            {

                //Ratio
                if (viewModel.KeyShift)
                {
                    //Scale
                    float scale = distance.FC / distance.PC;
                    this.SetScale(layer, scale, scale);

                    //Flip              
                    bool isFlip = distance.FD > distance.FP;
                    this.SetFlip(layer, isFlip, isFlip);
                }
                //Free
                else
                {
                    //Scale
                    float xScale = horizontalDistance.FC / horizontalDistance.PC;
                    float yScale = verticalDistance.FC / verticalDistance.PC;
                    this.SetScale(layer, xScale, yScale);

                    //Flip
                    bool isFlipHorizontal = horizontalDistance.FD > horizontalDistance.FP;
                    bool isFlipVertical = verticalDistance.FD > verticalDistance.FP;
                    this.SetFlip(layer, isFlipHorizontal, isFlipVertical);
                }

            }


            //Scale with Side
            else
            {

                //Ratio
                if (viewModel.KeyShift)
                {
                    //Scale
                    float scale = distance.FD / distance.PD;
                    this.SetScale(layer, scale, scale);

                    //Flip
                    bool isFlip = distance.FS > distance.FP;
                    this.SetFlip(layer, isFlip, isFlip);
                }
                //Free
                else
                {
                    //Scale
                    float xScale = horizontalDistance.FD / horizontalDistance.PD;
                    float yScale = verticalDistance.FD / verticalDistance.PD;
                    this.SetScale(layer,xScale, yScale);

                    //Flip
                    bool isFlipHorizontal = horizontalDistance.FS > horizontalDistance.FP;
                    bool isFlipVertical = verticalDistance.FS > verticalDistance.FP;
                    this.SetFlip(layer, isFlipHorizontal, isFlipVertical);
                }

                //Postion
                float xMove = horizontalDistance.FP / 2 / viewModel.MatrixTransformer.Scale;
                float yMove = verticalDistance.FP / 2 / viewModel.MatrixTransformer.Scale;
                this.SetReversePostion(layer,
                    base.Cos * xMove, base.Sin * xMove,
                    base.Cos * yMove, base.Sin * yMove,
                    horizontalDistance, verticalDistance);

            }
        }


        public VectorDistance GetDistance(Vector2 footPoint, Vector2 point, VectorLine line)
        {
            return new VectorDistance
            {
                FD = Vector2.Distance(footPoint, line.Diagonal),
                FP = Vector2.Distance(footPoint, point),
                FC = Vector2.Distance(footPoint, line.Center),
                PC = Vector2.Distance(point, line.Center),
                FS = Vector2.Distance(footPoint, line.Symmetric),
                PD = Vector2.Distance(point, line.Diagonal),
            };
        }

        //Scale
        public void SetScale(Layer layer, float xScale, float yScale)
        {
            layer.Transformer.XScale = this.StartTransformer.XScale * xScale;
            layer.Transformer.YScale = this.StartTransformer.YScale * yScale;
        }


        //Flip
        public void SetFlip(Layer layer, bool isFlipHorizontal, bool isFlipVertical)
        {
            layer.Transformer.FlipHorizontal = (this.StartTransformer.FlipHorizontal == isFlipHorizontal);
            layer.Transformer.FlipVertical = (this.StartTransformer.FlipVertical == isFlipVertical);
        }


        //Postion: Reverse
        public void SetReversePostion(Layer layer,
            float xCos, float xSin,
            float yCos, float ySin,
            VectorDistance horizontalDistance, VectorDistance verticalDistance)
        {
            bool xReverse = horizontalDistance.FD < horizontalDistance.PD ? true : horizontalDistance.FD < horizontalDistance.FP;
            bool yReverse = verticalDistance.FD < verticalDistance.PD ? true : verticalDistance.FD < verticalDistance.FP;

            if (xReverse == false && yReverse == false)
                this.SetFlipPostion(layer, xCos, xSin, yCos, ySin);
            else if (xReverse == false && yReverse)
                this.SetFlipPostion(layer, xCos, xSin, -yCos, -ySin);
            else if (xReverse && yReverse == false)
                this.SetFlipPostion(layer, -xCos, -xSin, yCos, ySin);
            else
                this.SetFlipPostion(layer, -xCos, -xSin, -yCos, -ySin);
        }

        //Postion: Flip
        public void SetFlipPostion(Layer layer,
            float xCos, float xSin, 
            float yCos, float ySin)
        {
            bool flipHorizontal = this.StartTransformer.FlipHorizontal;
            bool flipVertical = this.StartTransformer.FlipVertical;

            if (flipHorizontal == false && flipVertical == false)
                this.SetPostion(layer, this.StartTransformer, xCos, xSin, yCos, ySin);
            else if (flipHorizontal == false && flipVertical)
                this.SetPostion(layer, this.StartTransformer, xCos, xSin, -yCos, -ySin);
            else if (flipHorizontal && flipVertical == false)
                this.SetPostion(layer, this.StartTransformer, -xCos, -xSin, yCos, ySin);
            else
                this.SetPostion(layer, this.StartTransformer, -xCos, -xSin, -yCos, -ySin);
        }


    }
}

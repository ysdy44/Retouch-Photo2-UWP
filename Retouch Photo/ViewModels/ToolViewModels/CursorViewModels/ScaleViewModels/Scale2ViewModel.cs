using Retouch_Photo.Library;
using Retouch_Photo.Models;
using System.Numerics;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels.ScaleViewModels
{
    public abstract class Scale2ViewModel : ScaleViewModel
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;
        bool IsCenter => (this.ViewModel.MarqueeMode == MarqueeMode.Center) || (this.ViewModel.MarqueeMode == MarqueeMode.SquareAndCenter);
        bool IsRatio => (this.ViewModel.MarqueeMode == MarqueeMode.Square) || (this.ViewModel.MarqueeMode == MarqueeMode.SquareAndCenter);


        //@Override
        public abstract Vector2 GetHorizontalDiagonal(Layer layer, Matrix3x2 matrix);
        public abstract Vector2 GetVerticalDiagonal(Layer layer, Matrix3x2 matrix);


        VectorLine HorizontalLine;
        VectorLine VerticalLine;

        public override void Start(Vector2 point, Layer layer)
        {
            base.Start(point, layer);

            Matrix3x2 matrix = layer.Transformer.Matrix * this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix;
            base.Point = this.GetPoint(layer, matrix);

            //Diagonal line
            Vector2 diagonal = this.GetDiagonal(layer, matrix);
            this.Line = new VectorLine
            {
                Diagonal = diagonal,
                Symmetric = diagonal + diagonal - base.Point,
                Center = layer.Transformer.TransformCenter(matrix)
            };

            //Horizontal line
            Vector2 horizontalDiagonal = this.GetHorizontalDiagonal(layer, matrix);
            this.HorizontalLine = new VectorLine
            {
                Diagonal = horizontalDiagonal,
                Symmetric = horizontalDiagonal + horizontalDiagonal - base.Point,
                Center = (base.Point + horizontalDiagonal) / 2
            };

            //Vertical line
            Vector2 verticalDiagonal = this.GetVerticalDiagonal(layer, matrix);
            this.VerticalLine = new VectorLine
            {
                Diagonal = verticalDiagonal,
                Symmetric = verticalDiagonal + verticalDiagonal - base.Point,
                Center = (base.Point + verticalDiagonal) / 2,
            };
        }

      
        public override void Delta(Vector2 point, Layer layer)
        {
            //Point on diagonal line
            Vector2 footPoint = Transformer.FootPoint(point, this.Line.Diagonal, base.Point);
            VectorDistance distance = base.GetVectorDistance(footPoint,base.Point, this.Line);

            Vector2 point2 = this.IsRatio ? footPoint : point;

            //Point on horizontal line
            Vector2 horizontalFootPoint = Transformer.IntersectionPoint(base.Point, this.HorizontalLine.Diagonal, point2, point2 + this.VerticalLine.Diagonal - base.Point);
            VectorDistance horizontalDistance = base.GetVectorDistance(horizontalFootPoint, base.Point, this.HorizontalLine);

            //Point on vertical line
            Vector2 verticalFootPoint = Transformer.IntersectionPoint(base.Point, this.VerticalLine.Diagonal, point2, point2 + this.HorizontalLine.Diagonal - base.Point);
            VectorDistance verticalDistance = base.GetVectorDistance(verticalFootPoint, base.Point, this.VerticalLine);


            //Scale with Center
            if (this.IsCenter)
            {

                //Ratio
                if (this.IsRatio)
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
                if (this.IsRatio)
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
                    this.SetScale(layer, xScale, yScale);

                    //Flip
                    bool isFlipHorizontal = horizontalDistance.FS > horizontalDistance.FP;
                    bool isFlipVertical = verticalDistance.FS > verticalDistance.FP;
                    this.SetFlip(layer, isFlipHorizontal, isFlipVertical);
                }

                //Postion
                float xMove = horizontalDistance.FP / 2 / this.ViewModel.MatrixTransformer.Scale;
                float yMove = verticalDistance.FP / 2 / this.ViewModel.MatrixTransformer.Scale;
                this.SetReversePostion(layer, horizontalDistance, verticalDistance,
                    base.XCos * xMove, base.XSin * xMove,
                    base.YCos * yMove, base.YSin * yMove);

            }
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
        public void SetReversePostion(Layer layer, VectorDistance horizontalDistance, VectorDistance verticalDistance,
            float xCos, float xSin,
            float yCos, float ySin
            )
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

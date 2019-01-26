using Microsoft.Graphics.Canvas;
using Retouch_Photo.Library;
using Retouch_Photo.Models;
using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.ViewModels.ToolViewModels.CursorViewModels.ScaleViewModels
{
    public abstract class Scale1ViewModel : ScaleViewModel, IToolViewModel
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;
        bool IsCenter => (this.ViewModel.MarqueeMode == MarqueeMode.Center) || (this.ViewModel.MarqueeMode == MarqueeMode.SquareAndCenter);
        bool IsRatio => (this.ViewModel.MarqueeMode == MarqueeMode.Square) || (this.ViewModel.MarqueeMode == MarqueeMode.SquareAndCenter);


        //@Override
        public abstract void SetScale(Layer layer, float scale, bool isRatio);
        public abstract void SetFlip(Layer layer, bool isFlip);
               

        public new void Start(Vector2 point, Layer layer)
        {
            base.Start(point, layer);

            Matrix3x2 matrix = layer.Transformer.Matrix * this.ViewModel.MatrixTransformer.CanvasToVirtualToControlMatrix;
            base.Point = this.GetPoint(layer, matrix);

            //Diagonal line
            Vector2 diagonal = this.GetDiagonal(layer, matrix);
            base.Line = new VectorLine
            {
                Diagonal = diagonal,
                Symmetric = diagonal + diagonal - base.Point,
                Center = layer.Transformer.TransformCenter(matrix)
            };
        }

        public new void Delta(Vector2 point, Layer layer)
        {
            //Point on diagonal line
            Vector2 footPoint = Transformer.FootPoint(point, base.Line.Diagonal, base.Point);
            VectorDistance distance = base.GetVectorDistance(footPoint, base.Point, this.Line);
            
            //Scale with Center
            if (this.IsCenter)
            {
                //Scale
                float scale = distance.FC / distance.PC;
                this.SetScale(layer, scale, this.IsRatio);

                //Flip
                bool isFlip = distance.FD > distance.FP;
                this.SetFlip(layer, isFlip);
            }

            //Scale with Side
            else
            {
                //Scale
                float scale = distance.FD / distance.PD;
                this.SetScale(layer, scale, this.IsRatio);

                //Flip
                bool isFlip = distance.FS > distance.FP;
                this.SetFlip(layer, isFlip);

                //Postion
                float move = distance.FP / 2 / this.ViewModel.MatrixTransformer.Scale;
                this.SetReversePostion(layer, distance, base.XCos * move, base.XSin * move, base.YCos * move, base.YSin * move);
            }
        }


        //Postion: Reverse
        public void SetReversePostion(Layer layer, VectorDistance distance, float xCos, float xSin, float yCos, float ySin)
        {
            bool reverse = distance.FD < distance.PD ? true : distance.FD < distance.FP;//F in the left of the P ?

            if (reverse)
                this.SetPostion(layer, this.StartTransformer, xCos, xSin, yCos, ySin);
            else
                this.SetPostion(layer, this.StartTransformer, -xCos, -xSin, -yCos, -ySin);
        }


    }
}

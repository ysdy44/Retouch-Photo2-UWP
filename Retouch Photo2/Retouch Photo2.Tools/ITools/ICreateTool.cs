using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Tools.ITools
{
    public class ICreateTool : ITool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;

        /// <summary>
        /// get new Layer by VectRect
        /// </summary>
        /// <param name="VectRect"></param>
        /// <return> Layer </summary>
        readonly Func<VectRect, Layer> GetLayerFunc;
        Vector2 StartPoint;
        Layer Layer;

        public ICreateTool(Func<VectRect, Layer> fun)
        {
            this.GetLayerFunc = fun;
        }
        
        public override bool Start(Vector2 point)
        {
            this.StartPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, this.StartPoint, this.ViewModel.MarqueeMode);

            this.Layer = this.GetLayerFunc(rect);//@Override
            this.ViewModel.InvalidateWithJumpedQueueLayer(this.Layer);
            return true;
        }
        public override bool Delta(Vector2 point)
        {
            Vector2 endPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);

            this.Layer.Transformer = Transformer.CreateFromSize(rect.Width, rect.Height, new Vector2(rect.X, rect.Y));
            this.ViewModel.InvalidateWithJumpedQueueLayer(this.Layer);
            this.ViewModel.SetLayer(null);
            return true;
        }
        public override bool Complete(Vector2 point)
        {
            Vector2 endPoint = Vector2.Transform(point, this.ViewModel.MatrixTransformer.InverseMatrix);
            VectRect rect = new VectRect(this.StartPoint, endPoint, this.ViewModel.MarqueeMode);

            Layer layer = this.GetLayerFunc(rect);//@Override
            this.ViewModel.RenderLayer.Insert(Layer);

            this.ViewModel.RenderLayer.Selected(Layer);
            this.ViewModel.SetLayer(Layer);

            this.Layer = null;
            this.ViewModel.Invalidate();
            return true;
        }

        public override bool Draw(CanvasDrawingSession ds)
        {
            if (this.Layer==null) return false;

            this.Layer.Draw(ds,this.ViewModel.MatrixTransformer.Matrix);
            return false;
        }
    }
}

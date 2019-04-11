using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System;
using System.Numerics;
using static Retouch_Photo.Library.HomographyController;

namespace Retouch_Photo.Tools.ITools
{
    public class IClickedTool : ITool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;
       
        Vector2 point;
        bool IsClicked=true;
        readonly Func<Vector2, bool> ClickedStart;
        readonly Func<Vector2, bool> ClickedDelta;
        readonly Func<Vector2, bool> ClickedComplete;


        public IClickedTool(Func<Vector2, bool> start, Func<Vector2, bool> delta, Func<Vector2, bool> complete)
        {
            this.ClickedStart = start;
            this.ClickedDelta = delta;
            this.ClickedComplete = complete;
        }


        public override bool Start(Vector2 point)
        {
            this.point = point;
            return true;
        }
        public override bool Delta(Vector2 point)
        {
            if (this.IsClicked)
            {
                bool isOut = Transformer.OutNodeDistance(this.point, point);
                if (isOut)
                {
                    this.IsClicked = false;
                    this.ClickedStart(this.point);//Clicked
                }
            }
            else
            {
                this.ClickedDelta(point);//Clicked
            }
            return true;
        }
        public override bool Complete(Vector2 point)
        {
            if (this.IsClicked)
            {
                Layer layer = this.ViewModel.RenderLayer.GetClickedLayer(this.point, this.ViewModel.MatrixTransformer.InverseMatrix);
                this.ViewModel.CurrentLayer = layer;
            }
            else
            {
                this.IsClicked = true;
                this.ClickedComplete(point);//Clicked
            }
            return true;
        }

        public override bool Draw(CanvasDrawingSession ds)
        {
            return false;
            }
    }
}

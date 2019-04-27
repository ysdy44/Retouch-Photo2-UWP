using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Models;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using static Retouch_Photo2.Library.HomographyController;

namespace Retouch_Photo2.Tools.ITools
{
    /// <summary>
    /// Clicked Tool Interface: 
    ///   Drag distance more than a certain distance to trigger <see cref = "ClickedStart" /> event.
    /// </summary>
    public class IClickedTool : ITool
    {
        //ViewModel
        DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;
       
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
                // Click on the layer
                Layer layer = this.ViewModel.RenderLayer.GetClickedLayer(this.point, this.ViewModel.MatrixTransformer.Matrix);
                this.ViewModel.CurrentLayer = layer;
            }
            else
            {
                this.IsClicked = true;
                this.ClickedComplete(point);//Clicked
            }
            return true;
        }

        public override bool Draw(CanvasDrawingSession ds)=> false;        
    }}

using Microsoft.Graphics.Canvas;
using Retouch_Photo2.TestApp.Tools.Controls;
using Retouch_Photo2.TestApp.Tools.Pages;
using Retouch_Photo2.TestApp.ViewModels;
using Retouch_Photo2.Transformers;
using System.Numerics;

namespace Retouch_Photo2.TestApp.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s CursorTool .
    /// </summary>
    public partial class CursorTool : Tool
    {
        //ViewModel
        ViewModel ViewModel => Retouch_Photo2.TestApp.App.ViewModel;

        //Transformer
        Transformer oldTransformer;
        TransformerMode TransformerMode;

        //Box
        bool isBox;
        TransformerRect boxCanvasRect;

        //@Construct
        public CursorTool()
        {
            base.Type = ToolType.Cursor;
            base.Icon = new CursorControl();
            base.ShowIcon = new CursorControl();
            base.Page = new CursorPage();
        }


        //@Override
        public override void Starting(Vector2 point)
        {
            this.isBox = false; //Box

            if (this.CursorStarting(point)) return; //Cursor

            this.isBox = true; //Box
        }
        public override void Started(Vector2 startingPoint, Vector2 point)
        {
            //Box
            if (this.isBox)
            {
                this.BoxDelta(startingPoint, point);//Box
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                return;
            }

            this.CursorStarted(startingPoint, false);//Cursor
        }
        public override void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Box
            if (this.isBox)
            {
                this.BoxDelta(startingPoint, point);//Box
                this.ViewModel.Invalidate();//Invalidate
                return;
            }

            this.CursorDelta(startingPoint, point);//Cursor
        }
        public override void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            //Box
            if (this.isBox)
            {
                this.isBox = false;

                if (isSingleStarted)
                {
                    this.BoxComplete();//Box
                    this.ViewModel.SetSelectionMode();//Selection
                    this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    return;
                }
            }

            this.CursorComplete(isSingleStarted);//Cursor            
        }


        public override void Draw(CanvasDrawingSession ds)
        {
            //Box
            if (this.isBox)
            {
                this.BoxDraw(ds);//Box
            }
        }
    }
}
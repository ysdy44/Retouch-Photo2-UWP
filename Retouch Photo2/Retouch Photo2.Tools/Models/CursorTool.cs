using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Tools.Controls;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Keyboards;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using System.Numerics;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="Tool"/>'s CursorTool.
    /// </summary>
    public partial class CursorTool : Tool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        MezzanineViewModel MezzanineViewModel => App.MezzanineViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

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

            if (this.TipViewModel.TransformerTool.Starting(point)) return; //TransformerToolBase

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

            this.TipViewModel.TransformerTool.Started(startingPoint, false);//TransformerToolBase
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

            this.TipViewModel.TransformerTool.Delta(startingPoint, point); //TransformerToolBase
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
                    this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                    this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    return;
                }
            }

            this.TipViewModel.TransformerTool.Complete(isSingleStarted); //TransformerToolBase
        }


        public override void Draw(CanvasDrawingSession drawingSession)
        {
            //Box
            if (this.isBox)
            {
                this.BoxDraw(drawingSession);//Box
                return;
            }

            this.TipViewModel.TransformerTool.Draw(drawingSession);//TransformerToolBase
        }
    }
}
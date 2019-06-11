using Microsoft.Graphics.Canvas;
using Retouch_Photo2.TestApp.Tools.Controls;
using Retouch_Photo2.TestApp.Tools.Pages;
using Retouch_Photo2.TestApp.ViewModels;
using Retouch_Photo2.Transformers;
using System.Numerics;

namespace Retouch_Photo2.TestApp.Tools.Models
{
    /// <summary> Mode of <see cref="CursorTool"/>. </summary>
    internal enum CursorAddMode
    {
        /// <summary> The current layer becomes the only selected layer. </summary>
        New,
        /// <summary> Add the current layer to the selected layer. </summary>
        Add,
        /// <summary> Subtract the current layer from the selected layer. </summary>
        Subtract
    }

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

        //Add
        CursorAddMode AddMode
        {
            get
            {
                if (this.ViewModel.KeyShift)
                    return CursorAddMode.Add;

                if (this.ViewModel.KeyCtrl)
                    return CursorAddMode.Subtract;

                return CursorAddMode.New;
            }
        }

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

            if (this.CursorComplete(isSingleStarted)) return; //Cursor

            {
                //Selection
                this.ViewModel.SelectionSetValue((layer) =>
                {
                    layer.IsChecked = false;
                });
                this.ViewModel.SetSelectionModeNone();//Selection
                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            }
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
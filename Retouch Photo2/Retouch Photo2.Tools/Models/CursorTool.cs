using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Buttons;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.Tools.Pages;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s CursorTool.
    /// </summary>
    public partial class CursorTool : ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;

        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;
        CompositeMode CompositeMode => this.KeyboardViewModel.CompositeMode;


        //Box
        bool _isBox;
        TransformerRect _boxCanvasRect;


        public bool IsSelected
        {
            set
            {
                this.Button.IsSelected = value;
                this._cursorPage.IsSelected = value;
            }
        }
        public ToolType Type=> ToolType.Cursor;
        public FrameworkElement Icon { get; }= new CursorIcon();
        public IToolButton Button { get; } = new CursorButton();
        public Page Page => this._cursorPage;
        CursorPage _cursorPage { get; } = new CursorPage();


        public void Starting(Vector2 point)
        {
            this._isBox = false; //Box

            if (this.TransformerTool.Starting(point)) return; //TransformerTool

            this._isBox = true; //Box
        }
        public void Started(Vector2 startingPoint, Vector2 point)
        {
            //Box
            if (this._isBox)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Vector2 pointA = Vector2.Transform(startingPoint, inverseMatrix);
                Vector2 pointB = Vector2.Transform(point, inverseMatrix);
                this._boxCanvasRect = new TransformerRect(pointA, pointB);

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                return;
            }

            this.TransformerTool.Started(startingPoint, point, isSetTransformerMode: false);//TransformerTool
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            //Box
            if (this._isBox)
            {
                Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                Vector2 pointA = Vector2.Transform(startingPoint, inverseMatrix);
                Vector2 pointB = Vector2.Transform(point, inverseMatrix);
                this._boxCanvasRect = new TransformerRect(pointA, pointB);

                this.ViewModel.Invalidate();//Invalidate
                return;
            }

            this.TransformerTool.Delta(startingPoint, point);//TransformerTool
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted)
        {
            //Box
            if (this._isBox)
            {
                this._isBox = false;

                if (isSingleStarted)
                {
                    this.BoxChoose();//Box

                    this.SelectionViewModel.SetMode(this.ViewModel.Layers.RootLayers);//Selection
                    this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    return;
                }
            }

            this.TransformerTool.Complete(startingPoint, point, isSingleStarted); //TransformerTool
        }
        
        public void Draw(CanvasDrawingSession drawingSession)
        {
            //Box
            if (this._isBox)
            {
                Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                CanvasGeometry geometry = this._boxCanvasRect.ToRectangle(this.ViewModel.CanvasDevice, matrix);
                drawingSession.DrawGeometryDodgerBlue(geometry);
                return;
            }

            this.TransformerTool.Draw(drawingSession);//TransformerTool
        }
        
        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }


        private void BoxChoose()
        {
            foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
            {
                Transformer transformer = layer.TransformManager.Destination;
                bool contained = transformer.Contained(this._boxCanvasRect);

                switch (this.CompositeMode)
                {
                    case CompositeMode.New:
                        layer.SelectMode = SelectMode.Selected;
                        break;
                    case CompositeMode.Add:
                        if (contained) layer.SelectMode = SelectMode.Selected;
                        break;
                    case CompositeMode.Subtract:
                        if (contained) layer.SelectMode = SelectMode.UnSelected;
                        break;
                    case CompositeMode.Intersect:
                        if (contained == false) layer.SelectMode = SelectMode.UnSelected;
                        break;
                }
            }
        }
    }
}
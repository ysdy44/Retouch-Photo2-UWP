using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// <see cref="ITool"/>'s CursorTool.
    /// </summary>
    public partial class CursorTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel ;
        TipViewModel TipViewModel => App.TipViewModel;

        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;
        MarqueeCompositeMode MarqueeCompositeMode => this.SettingViewModel.CompositeMode;


        //@Construct
        public CursorTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructToolTip();            
        }


        //Box
        private void BoxChoose(IList<ILayer> layers)
        {
            foreach (ILayer layer in layers)
            {
                Transformer transformer = layer.GetActualDestinationWithRefactoringTransformer;
                bool contained = transformer.Contained(this._boxCanvasRect);

                switch (this.MarqueeCompositeMode)
                {
                    case MarqueeCompositeMode.New:
                        layer.SelectMode = contained ?
                            SelectMode.Selected :
                            SelectMode.UnSelected;
                        break;
                    case MarqueeCompositeMode.Add:
                        if (contained) layer.SelectMode = SelectMode.Selected;
                        break;
                    case MarqueeCompositeMode.Subtract:
                        if (contained) layer.SelectMode = SelectMode.UnSelected;
                        break;
                    case MarqueeCompositeMode.Intersect:
                        if (contained == false) layer.SelectMode = SelectMode.UnSelected;
                        break;
                }
            }
        }


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom() { }

    }
    
    /// <summary>
    /// <see cref="ITool"/>'s CursorTool.
    /// </summary>
    public partial class CursorTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = resource.GetString("/Tools/Cursor");

            this.StepFrequencyToolTip.Content = resource.GetString("/Tools/Cursor_StepFrequency");
        }

        //ToolTip
        private void ConstructToolTip()
        {
            this._button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSelected)
                {
                    this.StepFrequencyToolTip.IsOpen = true;

                    this.ModeControl.IsOpen = true;
                }
            };
            this._button.ToolTip.Closed += (s, e) =>
            {
                this.StepFrequencyToolTip.IsOpen = false;

                this.ModeControl.IsOpen = false;
            };
        }


        //@Content
        public ToolType Type => ToolType.Cursor;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new CursorIcon();
        readonly ToolButton _button = new ToolButton(new CursorIcon());


        //Box
        bool _isBox;
        TransformerRect _boxCanvasRect;

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
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            //Box
            if (this._isBox)
            {
                this._isBox = false;

                if (isOutNodeDistance)
                {
                    //Select a layer of the same depth
                    bool isChildSingle = (this.SelectionViewModel.SelectionMode == ListViewSelectionMode.Single
                        && this.SelectionViewModel.Layer.Parents != null);
                    IList<ILayer> parentsChildren = isChildSingle ?
                        this.SelectionViewModel.Layer.Parents.Children :
                        this.ViewModel.Layers.RootLayers;

                    this.BoxChoose(parentsChildren);//Box 

                    this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                    this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                }
                return;
            }

            this.TransformerTool.Complete(startingPoint, point); //TransformerTool
        }
        public void Clicke(Vector2 point)
        {
            //Select single layer
            this.TipViewModel.TransformerTool.SelectSingleLayer(point);
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

    }
}
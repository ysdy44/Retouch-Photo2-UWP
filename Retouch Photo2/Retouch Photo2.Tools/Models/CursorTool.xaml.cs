using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Collections.Generic;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    internal enum CursorMode
    {
        None,

        Transformer,
        Move,

        BoxChoose
    }

    /// <summary>
    /// <see cref="ITool"/>'s CursorTool.
    /// </summary>
    public partial class CursorTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel ;
        TipViewModel TipViewModel => App.TipViewModel;

        IMoveTool MoveTool => this.TipViewModel.MoveTool;
        ITransformerTool TransformerTool => this.TipViewModel.TransformerTool;
        MarqueeCompositeMode MarqueeCompositeMode => this.SettingViewModel.CompositeMode;


        //@Construct
        /// <summary>
        /// Initializes a CursorTool. 
        /// </summary>
        public CursorTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructToolTip();

            this.CountButton.Click += (s, e) =>
            {
                this.TipViewModel.ShowMenuLayoutAt(MenuType.Operate, this.CountButton);
            };
        }


        public void OnNavigatedTo() => this.CursorMode = CursorMode.None;
        public void OnNavigatedFrom() => this.CursorMode = CursorMode.None;
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

            this._button.ToolTip.Content =
                this.Title = resource.GetString("/Tools/Cursor");
        }

        //ToolTip
        private void ConstructToolTip()
        {
            this._button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSelected)
                {
                    this.ModeControl.IsOpen = true;
                }
            };
            this._button.ToolTip.Closed += (s, e) =>
            {
                this.ModeControl.IsOpen = false;
            };
        }


        //@Content
        public ToolType Type => ToolType.Cursor;  
        public string Title { get; set; }
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => this._button.IsSelected; set => this._button.IsSelected = value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new CursorIcon();
        readonly ToolButton _button = new ToolButton(new CursorIcon());


        CursorMode CursorMode;
        TransformerRect BoxRect;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            this.CursorMode = CursorMode.None;

            if (this.TransformerTool.Started(startingPoint, point))//TransformerTool
            {
                this.CursorMode = CursorMode.Transformer;
                return;
            }

            if (this.MoveTool.Started(startingPoint, point))//MoveTool
            {
                this.CursorMode = CursorMode.Move;
                return;
            }

            //Box
            this.CursorMode = CursorMode.BoxChoose;

            Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
            Vector2 canavsStartingPoint = Vector2.Transform(startingPoint, inverseMatrix); 
             Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix); 
            this.BoxRect = new TransformerRect(canavsStartingPoint, canvasPoint);

            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        public void Delta(Vector2 startingPoint, Vector2 point)
        {
            switch (this.CursorMode)
            {
                case CursorMode.Transformer:
                    this.TransformerTool.Delta(startingPoint, point);//TransformerTool
                    break;
                case CursorMode.Move:
                    this.MoveTool.Delta(startingPoint, point);//MoveTool
                    break;
                case CursorMode.BoxChoose:
                    {
                        Matrix3x2 inverseMatrix = this.ViewModel.CanvasTransformer.GetInverseMatrix();
                        Vector2 canavsStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
                        Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
                        this.BoxRect = new TransformerRect(canavsStartingPoint, canvasPoint);

                        this.ViewModel.Invalidate();//Invalidate
                    }
                    break;
            }
        }
        public void Complete(Vector2 startingPoint, Vector2 point, bool isOutNodeDistance)
        {
            CursorMode cursorMode = this.CursorMode;
            this.CursorMode = CursorMode.None;

            switch (cursorMode)
            {
                case CursorMode.Transformer:
                    this.TransformerTool.Complete(startingPoint, point); //TransformerTool
                    break;
                case CursorMode.Move:
                    this.MoveTool.Complete(startingPoint, point);//MoveTool
                    break;
                case CursorMode.BoxChoose:
                    {
                        if (isOutNodeDistance)
                        {
                            //BoxChoose 
                            Layerage layerage = this.SelectionViewModel.GetFirstSelectedLayerage();
                            IList<Layerage> parentsChildren = this.ViewModel.LayerageCollection.GetParentsChildren(layerage);
                            this.BoxChoose(parentsChildren);

                            this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection

                            LayerageCollection.ArrangeLayersBackground(this.ViewModel.LayerageCollection);
                            
                            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                        }
                    }
                    break;
            }
        }
        public void Clicke(Vector2 point) => this.MoveTool.Clicke(point);


        public void Draw(CanvasDrawingSession drawingSession)
        {
            switch (this.CursorMode)
            {
                case CursorMode.None:
                case CursorMode.Transformer:
                    this.TransformerTool.Draw(drawingSession);//TransformerTool
                    break;
                case CursorMode.Move:
                    this.MoveTool.Draw(drawingSession);//MoveTool
                    break;
                case CursorMode.BoxChoose:
                    {
                        Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();
                        CanvasGeometry geometry = this.BoxRect.ToRectangle(this.ViewModel.CanvasDevice, matrix);
                        drawingSession.DrawGeometryDodgerBlue(geometry);
                    }
                    break;
            }
        }


        //Box
        private void BoxChoose(IList<Layerage> layerages)
        {
            foreach (Layerage layerage in layerages)
            {
                ILayer layer = layerage.Self;

                Transformer transformer = layerage.GetActualTransformer();
                bool contained = transformer.Contained(this.BoxRect);

                switch (this.MarqueeCompositeMode)
                {
                    case MarqueeCompositeMode.New:
                        layer.IsSelected = contained;
                        break;
                    case MarqueeCompositeMode.Add:
                        if (contained) layer.IsSelected = true;
                        break;
                    case MarqueeCompositeMode.Subtract:
                        if (contained) layer.IsSelected = false;
                        break;
                        //case MarqueeCompositeMode.Intersect:
                        //if (contained == false) layer.IsSelected = false;
                        //break;
                }
            }
        }

    }
}
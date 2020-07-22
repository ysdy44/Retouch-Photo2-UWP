using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
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
    public partial class CursorTool : ITool
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Content
        public ToolType Type => ToolType.Cursor;
        public FrameworkElement Icon { get; } = new CursorIcon();
        public IToolButton Button { get; } = new ToolButton
        {
            CenterContent = new CursorIcon()
        };
        public FrameworkElement Page => this.CursorPage;
        CursorPage CursorPage = new CursorPage();


        //@Construct
        /// <summary>
        /// Initializes a CursorTool. 
        /// </summary>
        public CursorTool()
        {
            this.ConstructStrings();
        }


        CursorMode CursorMode;
        TransformerRect BoxRect;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            this.CursorMode = CursorMode.None;

            if (ToolBase.TransformerTool.Started(startingPoint, point))//TransformerTool
            {
                this.CursorMode = CursorMode.Transformer;
                return;
            }

            if (ToolBase.MoveTool.Started(startingPoint, point))//MoveTool
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
                    ToolBase.TransformerTool.Delta(startingPoint, point);//TransformerTool
                    break;
                case CursorMode.Move:
                    ToolBase.MoveTool.Delta(startingPoint, point);//MoveTool
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
                    ToolBase.TransformerTool.Complete(startingPoint, point); //TransformerTool
                    break;
                case CursorMode.Move:
                    ToolBase.MoveTool.Complete(startingPoint, point);//MoveTool
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
        public void Clicke(Vector2 point) => ToolBase.MoveTool.Clicke(point);


        public void Draw(CanvasDrawingSession drawingSession)
        {
            Matrix3x2 matrix = this.ViewModel.CanvasTransformer.GetMatrix();

            //@DrawBound
            switch (this.SelectionViewModel.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    break;
                case ListViewSelectionMode.Single:
                    ILayer layer2 = this.SelectionViewModel.SelectionLayerage.Self;
                    drawingSession.DrawLayerBound(layer2, matrix, this.ViewModel.AccentColor);

                    ToolBase.TransformerTool.Draw(drawingSession); //TransformerTool
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.ViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;
                        drawingSession.DrawLayerBound(layer, matrix, this.ViewModel.AccentColor);
                    }

                    ToolBase.TransformerTool.Draw(drawingSession); //TransformerTool
                    break;
            }


            switch (this.CursorMode)
            {
                case CursorMode.None:
                case CursorMode.Transformer:
                    ToolBase.TransformerTool.Draw(drawingSession);//TransformerTool
                    break;
                case CursorMode.Move:
                    ToolBase.MoveTool.Draw(drawingSession);//MoveTool
                    break;
                case CursorMode.BoxChoose:
                    CanvasGeometry geometry = this.BoxRect.ToRectangle(this.ViewModel.CanvasDevice, matrix);
                    drawingSession.DrawGeometryDodgerBlue(geometry);
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

                switch (this.CursorPage.ModeSegmented.Mode)
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


        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.CursorMode = CursorMode.None;
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Title = resource.GetString("/Tools/Cursor");

            this.Button.ToolTip.Closed += (s, e) => this.CursorPage.ModeSegmented.IsOpen = false;
            this.Button.ToolTip.Opened += (s, e) =>
            {
                if (this.Button.IsSelected == false) return;

                this.CursorPage.ModeSegmented.IsOpen = true;
            };
        }

    }


    /// <summary>
    /// Page of <see cref="CursorTool"/>.
    /// </summary>
    internal partial class CursorPage : Page
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        
        public CompositeModeSegmented ModeSegmented => this._ModeSegmented;


        //@Construct
        /// <summary>
        /// Initializes a CursorPage. 
        /// </summary>
        public CursorPage()
        {
            this.InitializeComponent();
            //this.ConstructStrings();

            this.CountButton.Click += (s, e) =>
            {
                this.TipViewModel.ShowMenuLayoutAt(MenuType.Operate, this.CountButton);
            };
        }

    }
}
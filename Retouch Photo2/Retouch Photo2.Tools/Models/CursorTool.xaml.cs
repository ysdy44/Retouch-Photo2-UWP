// Core:              ★★★★
// Referenced:   
// Difficult:         ★★★★
// Only:              
// Complete:      ★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools.Elements;
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
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Converter
        private Visibility DeviceLayoutTypeConverter(DeviceLayoutType type) => type == DeviceLayoutType.Phone ? Visibility.Collapsed : Visibility.Visible;


        //@Content      
        public ToolType Type => ToolType.Cursor;
        public ControlTemplate Icon => this.IconContentControl.Template;
        public FrameworkElement Page => this;
        public bool IsSelected { get; set; }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "CursorTool" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "CursorTool.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(CursorTool), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a CursorTool. 
        /// </summary>
        public CursorTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.OperateButton.Tapped += (s, e) => Expander.ShowAt("Operate", this.OperateButton);

            this.MoreButton.Tapped += (s, e) => Retouch_Photo2.DrawPage.ShowMoreFlyout?.Invoke(this.MoreButton);
        }


        CursorMode CursorMode;
        TransformerRect BoxRect;

        public void Started(Vector2 startingPoint, Vector2 point)
        {
            this.CursorMode = CursorMode.None;

            if (this.ViewModel.TransformerTool.Started(startingPoint, point))//TransformerTool
            {
                //Cursor
                CoreCursorExtension.IsManipulationStarted = false;
                CoreCursorExtension.None();
                this.CursorMode = CursorMode.Transformer;
                return;
            }

            if (this.ViewModel.MoveTool.Started(startingPoint, point))//MoveTool
            {
                //Cursor
                CoreCursorExtension.IsManipulationStarted = false;
                CoreCursorExtension.None();
                this.CursorMode = CursorMode.Move;
                return;
            }

            //Cursor
            CoreCursorExtension.IsManipulationStarted = true;
            CoreCursorExtension.Cross();
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
                    this.ViewModel.TransformerTool.Delta(startingPoint, point);//TransformerTool
                    break;
                case CursorMode.Move:
                    this.ViewModel.MoveTool.Delta(startingPoint, point);//MoveTool
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

            //Cursor
            CoreCursorExtension.IsManipulationStarted = false;
            CoreCursorExtension.None();

            switch (cursorMode)
            {
                case CursorMode.Transformer:
                    this.ViewModel.TransformerTool.Complete(startingPoint, point); //TransformerTool
                    break;
                case CursorMode.Move:
                    this.ViewModel.MoveTool.Complete(startingPoint, point);//MoveTool
                    break;
                case CursorMode.BoxChoose:
                    {
                        if (isOutNodeDistance)
                        {
                            //BoxChoose 
                            Layerage layerage = this.SelectionViewModel.GetFirstSelectedLayerage();
                            Layerage parents = LayerManager.GetParentsChildren(layerage);
                            this.BoxChoose(parents.Children);

                            this.SelectionViewModel.SetMode();//Selection

                            LayerManager.ArrangeLayersBackground();

                            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                        }
                    }
                    break;
            }
        }
        public void Clicke(Vector2 point) => this.ViewModel.ClickeTool.Clicke(point);

        public void Cursor(Vector2 point) => this.ViewModel.CreateTool.Cursor(point);


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

                    this.ViewModel.TransformerTool.Draw(drawingSession); //TransformerTool
                    break;
                case ListViewSelectionMode.Multiple:
                    foreach (Layerage layerage in this.ViewModel.SelectionLayerages)
                    {
                        ILayer layer = layerage.Self;
                        drawingSession.DrawLayerBound(layer, matrix, this.ViewModel.AccentColor);
                    }

                    this.ViewModel.TransformerTool.Draw(drawingSession); //TransformerTool
                    break;
            }


            switch (this.CursorMode)
            {
                case CursorMode.None:
                case CursorMode.Transformer:
                    this.ViewModel.TransformerTool.Draw(drawingSession);//TransformerTool
                    break;
                case CursorMode.Move:
                    this.ViewModel.MoveTool.Draw(drawingSession);//MoveTool
                    break;
                case CursorMode.BoxChoose:
                    CanvasGeometry geometry = this.BoxRect.ToRectangle(LayerManager.CanvasDevice, matrix);
                    drawingSession.DrawGeometryDodgerBlue(geometry);
                    break;
            }
        }


        //Box
        private void BoxChoose(IList<Layerage> layerages)
        {
            //History 
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetIsSelected);

            foreach (Layerage layerage in layerages)
            {
                ILayer layer = layerage.Self;

                Transformer transformer = layerage.GetActualTransformer();
                bool contained = transformer.Contained(this.BoxRect);

                switch (this.ModeSegmented.Mode)
                {
                    case MarqueeCompositeMode.New:
                        if (layer.IsSelected != contained)
                        {
                            var previous = layer.IsSelected;
                            history.UndoAction += () =>
                            {
                                layer.IsSelected = previous;
                            };

                            layer.IsSelected = contained;
                        }
                        break;
                    case MarqueeCompositeMode.Add:
                        if (contained && layer.IsSelected == false)
                        {
                            var previous = false;
                            history.UndoAction += () =>
                            {
                                layer.IsSelected = previous;
                            };

                            layer.IsSelected = true;
                        }
                        break;
                    case MarqueeCompositeMode.Subtract:
                        if (contained && layer.IsSelected == true)
                        {
                            var previous = true;
                            history.UndoAction += () =>
                            {
                                layer.IsSelected = previous;
                            };

                            layer.IsSelected = false;
                        }
                        break;
                    case MarqueeCompositeMode.Intersect:
                        if (contained == false && layer.IsSelected == true)
                        {
                            layer.IsSelected = false;
                        }
                        break;
                }
            }

            //History 
            this.ViewModel.HistoryPush(history);
        }


        public void OnNavigatedTo()
        {
            this.ViewModel.Invalidate();//Invalidate
        }
        public void OnNavigatedFrom()
        {
            this.CursorMode = CursorMode.None;
        }

    }


    public partial class CursorTool : Page, ITool
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.OperateToolTip.Content = resource.GetString("Menus_Operate");

            this.MoreToolTip.Content = resource.GetString("Tools_More");
        }

    }
}
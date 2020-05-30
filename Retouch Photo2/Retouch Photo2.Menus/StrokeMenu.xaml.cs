using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Stroke;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    public sealed partial class StrokeMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        
        CanvasStrokeStyle StrokeStyle { get => this.SelectionViewModel.StrokeStyle; set => this.SelectionViewModel.StrokeStyle = value; }


        //@Converter
        private CanvasDashStyle DashConverter(CanvasStrokeStyle strokeStyle) => strokeStyle == null ? CanvasDashStyle.Solid : strokeStyle.DashStyle;
        private CanvasCapStyle CapConverter(CanvasStrokeStyle strokeStyle) => strokeStyle == null ? CanvasCapStyle.Flat : strokeStyle.DashCap;
        private CanvasLineJoin JoinConverter(CanvasStrokeStyle strokeStyle) => strokeStyle == null ? CanvasLineJoin.Miter : strokeStyle.LineJoin;
        private float OffsetConverter(CanvasStrokeStyle strokeStyle) => strokeStyle == null ? 0 : strokeStyle.DashOffset;


        //@Construct
        public StrokeMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructToolTip();
            this.ConstructMenu();
         
            this.ConstructDash();
            this.ConstructWidth();
            this.ConstructCap();
            this.ConstructJoin();
            this.ConstructOffset();
        }
    }

    public sealed partial class StrokeMenu : UserControl, IMenu
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content = 
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/Stroke");

            this.DashTextBlock.Text = resource.GetString("/Strokes/Dash");
            this.WidthTextBlock.Text = resource.GetString("/Strokes/Width");
            this.CapTextBlock.Text = resource.GetString("/Strokes/Cap");
            this.JoinTextBlock.Text = resource.GetString("/Strokes/Join");
            this.OffsetTextBlock.Text = resource.GetString("/Strokes/Offset");
        }

        //ToolTip
        private void ConstructToolTip()
        {
            this._button.ToolTip.Opened += (s, e) =>
            {
                if (this._Expander.IsSecondPage) return;

                if (this.Expander.State == ExpanderState.Overlay)
                {
                    this.DashSegmented.IsOpen = true;
                    this.CapSegmented.IsOpen = true;
                    this.JoinSegmented.IsOpen = true;
                }
            };
            this._button.ToolTip.Closed += (s, e) =>
            {
                this.DashSegmented.IsOpen = false;
                this.CapSegmented.IsOpen = false;
                this.JoinSegmented.IsOpen = false;
            };
        }


        //Menu
        public MenuType Type => MenuType.Stroke;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Strokes.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }
    

    public sealed partial class StrokeMenu : UserControl, IMenu
    {

        //Dash
        public void ConstructDash()
        {
            this.DashSegmented.DashChanged += (s, dash) =>
            {                        
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set stroke style");

                //Selection
                CanvasStrokeStyle strokeStyle = this.StrokeStyle.Clone();
                strokeStyle.DashStyle = dash;
                this.StrokeStyle = strokeStyle;
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Style.StrokeStyle.Clone();
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Style.StrokeStyle = previous.Clone();
                    });

                    layer.Style.StrokeStyle.DashStyle = dash;
                    this.SelectionViewModel.StyleLayerage = layerage;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Width
        public void ConstructWidth()
        {                     
            //History
            LayersPropertyHistory history = null;
            
            this.WidthPicker.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set stroke width");

                //Selection
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Style.CacheStrokeWidth();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.WidthPicker.ValueChangeDelta += (s, value) =>
            {
                float width = (float)value;

                //Selection
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Style.StrokeWidth = width;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.WidthPicker.ValueChangeCompleted += (s, value) =>
            {
                float width = (float)value;

                //Selection
                this.SelectionViewModel.StrokeWidth = width;
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Style.StartingStrokeWidth;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Style.StrokeWidth = previous;
                    });
                    
                    layer.Style.StrokeWidth = width;
                    this.SelectionViewModel.StyleLayerage = layerage;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }
        

        //Offset
        public void ConstructOffset()
        {
            //History
            LayersPropertyHistory history = null;

            this.OffsetPicker.Value = 0;
            this.OffsetPicker.Minimum = 0;
            this.OffsetPicker.Maximum = 10;
            this.OffsetPicker.ValueChangeStarted += (s, value) =>
            {
                history = new LayersPropertyHistory("Set stroke style");

                //Selection
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Style.CacheStrokeStyle();
                });

                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.OffsetPicker.ValueChangeDelta += (s, value) =>
            {
                float offset = (float)value;

                //Selection
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.Style.StrokeStyle.DashOffset = offset;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OffsetPicker.ValueChangeCompleted += (s, value) =>
            {
                float offset = (float)value;

                //Selection
                CanvasStrokeStyle strokeStyle = this.StrokeStyle.Clone();
                strokeStyle.DashOffset = offset;
                this.StrokeStyle = strokeStyle;
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Style.StartingStrokeStyle.Clone();
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Style.StrokeStyle = previous.Clone();
                    });

                    layer.Style.StrokeStyle.DashOffset = offset;
                    this.SelectionViewModel.StyleLayerage = layerage;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }
        

        //Cap
        public void ConstructCap()
        {       
            this.CapSegmented.CapChanged += (s, cap) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set stroke style");

                //Selection
                CanvasStrokeStyle strokeStyle = this.StrokeStyle.Clone();
                strokeStyle.DashCap = cap;
                strokeStyle.StartCap = cap;
                strokeStyle.EndCap = cap;
                this.StrokeStyle = strokeStyle;
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Style.StrokeStyle.Clone();
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Style.StrokeStyle = previous.Clone();
                    });

                    layer.Style.StrokeStyle.DashCap = cap;
                    layer.Style.StrokeStyle.StartCap = cap;
                    layer.Style.StrokeStyle.EndCap = cap;
                    this.SelectionViewModel.StyleLayerage = layerage;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Join
        public void ConstructJoin()
        {
            this.JoinSegmented.JoinChanged += (s, join) =>
            {
                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set stroke style");

                //Selection
                CanvasStrokeStyle strokeStyle = this.StrokeStyle.Clone();
                strokeStyle.LineJoin = join;
                this.StrokeStyle = strokeStyle;
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Style.StrokeStyle.Clone();
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Style.StrokeStyle = previous.Clone();
                    });

                    layer.Style.StrokeStyle.LineJoin = join;
                    this.SelectionViewModel.StyleLayerage = layerage;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }

    }
}
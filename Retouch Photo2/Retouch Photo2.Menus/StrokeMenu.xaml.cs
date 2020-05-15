using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
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
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

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

            this._Expander.Title = resource.GetString("/Menus/Stroke");

            this.DashTextBlock.Text = resource.GetString("/Strokes/Dash");
            this.WidthTextBlock.Text = resource.GetString("/Strokes/Width");
            this.CapTextBlock.Text = resource.GetString("/Strokes/Cap");
            this.JoinTextBlock.Text = resource.GetString("/Strokes/Join");
            this.OffsetTextBlock.Text = resource.GetString("/Strokes/Offset");
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
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.StyleManager.StrokeStyle.DashStyle = dash;
                    this.SelectionViewModel.StyleLayer = layer;
                });
                CanvasStrokeStyle strokeStyle = this.SelectionViewModel.StrokeStyle;
                strokeStyle.DashStyle = dash;
                this.SelectionViewModel.StrokeStyle = null;
                this.SelectionViewModel.StrokeStyle = strokeStyle;

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Width
        public void ConstructWidth()
        {
            this.WidthPicker.ValueChangeStarted += (s, value) =>
            {
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.WidthPicker.ValueChangeDelta += (s, value) =>
            {
                float width = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.StyleManager.StrokeWidth = width;
                    this.SelectionViewModel.StyleLayer = layer;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.WidthPicker.ValueChangeCompleted += (s, value) =>
            {
                float width = (float)value;

                this.SelectionViewModel.StrokeWidth = width;

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }
        

        //Offset
        public void ConstructOffset()
        {
            this.OffsetPicker.Maximum = 10;
            this.OffsetPicker.ValueChangeStarted += (s, value) =>
            {
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            };
            this.OffsetPicker.ValueChangeDelta += (s, value) =>
            {
                float offset = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.StyleManager.StrokeStyle.DashOffset = offset;
                    this.SelectionViewModel.StyleLayer = layer;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OffsetPicker.ValueChangeCompleted += (s, value) =>
            {
                float offset = (float)value;

                CanvasStrokeStyle strokeStyle = this.SelectionViewModel.StrokeStyle;
                strokeStyle.DashOffset = offset;
                this.SelectionViewModel.StrokeStyle = null;
                this.SelectionViewModel.StrokeStyle = strokeStyle;

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }
        

        //Cap
        public void ConstructCap()
        {
            this.CapSegmented.CapChanged += (s, cap) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.StyleManager.StrokeStyle.DashCap = cap;
                    layer.StyleManager.StrokeStyle.StartCap = cap;
                    layer.StyleManager.StrokeStyle.EndCap = cap;
                    this.SelectionViewModel.StyleLayer = layer;
                });
                CanvasStrokeStyle strokeStyle = this.SelectionViewModel.StrokeStyle;
                strokeStyle.DashCap = cap;
                strokeStyle.StartCap = cap;
                strokeStyle.EndCap = cap;
                this.SelectionViewModel.StrokeStyle = null;
                this.SelectionViewModel.StrokeStyle = strokeStyle;

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Join
        public void ConstructJoin()
        {
            this.JoinSegmented.JoinChanged += (s, join) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.StyleManager.StrokeStyle.LineJoin = join;
                    this.SelectionViewModel.StyleLayer = layer;
                });
                CanvasStrokeStyle strokeStyle = this.SelectionViewModel.StrokeStyle;
                strokeStyle.LineJoin = join;
                this.SelectionViewModel.StrokeStyle = null;
                this.SelectionViewModel.StrokeStyle = strokeStyle;

                this.ViewModel.Invalidate();//Invalidate
            };
        }

    }
}
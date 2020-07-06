using Microsoft.Graphics.Canvas.Geometry;
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
    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Strokes"/>.
    /// </summary>
    public sealed partial class StrokeMenu : Expander, IMenu 
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        StrokeMainPage StrokeMainPage = new StrokeMainPage();


        //@Construct
        /// <summary>
        /// Initializes a StrokeMenu. 
        /// </summary>
        public StrokeMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.MainPage = this.StrokeMainPage;
        }

    }

    /// <summary>
    /// Menu of <see cref = "Retouch_Photo2.Strokes"/>.
    /// </summary>
    public sealed partial class StrokeMenu : Expander, IMenu 
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.ToolTip.Content =
            this.Button.Title =
            this.Title = resource.GetString("/Menus/Stroke");

            this.Button.ToolTip.Closed += (s, e) => this.StrokeMainPage.IsOpen = false;
            this.Button.ToolTip.Opened += (s, e) =>
            {
                if (this.IsSecondPage) return;
                if (this.State != ExpanderState.Overlay) return;

                this.StrokeMainPage.IsOpen = true;
            };
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Stroke;
        /// <summary> Gets or sets the button. </summary>
        public override IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Strokes.Icon()
        };
        /// <summary> Reset Expander. </summary>
        public override void Reset() { }

    }



    /// <summary>
    /// MainPage of <see cref = "StrokeMenu"/>.
    /// </summary>
    public sealed partial class StrokeMainPage : UserControl
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


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "StrokeMainPage" />'s IsOpen. </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        /// <summary> Identifies the <see cref = "StrokeMainPage.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(StrokeMainPage), new PropertyMetadata(false));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a StrokeMainPage. 
        /// </summary>
        public StrokeMainPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();
         
            this.ConstructDash();
            this.ConstructWidth();
            this.ConstructCap();
            this.ConstructJoin();
            this.ConstructOffset();
        }
    }

    /// <summary>
    /// MainPage of <see cref = "StrokeMenu"/>.
    /// </summary>
    public sealed partial class StrokeMainPage : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.DashTextBlock.Text = resource.GetString("/Strokes/Dash");
            this.WidthTextBlock.Text = resource.GetString("/Strokes/Width");
            this.CapTextBlock.Text = resource.GetString("/Strokes/Cap");
            this.JoinTextBlock.Text = resource.GetString("/Strokes/Join");
            this.OffsetTextBlock.Text = resource.GetString("/Strokes/Offset");
        }
        
    }

    /// <summary>
    /// MainPage of <see cref = "StrokeMenu"/>.
    /// </summary>
    public sealed partial class StrokeMainPage : UserControl
    {

        //Dash
        private void ConstructDash()
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
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Style.StrokeStyle = previous.Clone();
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Style.StrokeStyle.DashStyle = dash;

                    this.SelectionViewModel.StandStyleLayerage = layerage;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Width
        private void ConstructWidth()
        {
            this.WidthPicker.Minimum = 0.0d;
            this.WidthPicker.Maximum = 128.0d;
            this.WidthPicker.ValueChangeStarted += (s, value) =>
            {             
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

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Style.StrokeWidth = width;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.WidthPicker.ValueChangeCompleted += (s, value) =>
            {
                float width = (float)value;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set stroke width");

                //Selection
                this.SelectionViewModel.StrokeWidth = width;
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Style.StartingStrokeWidth;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Style.StrokeWidth = previous;
                    };                    

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Style.StrokeWidth = width;

                    this.SelectionViewModel.StandStyleLayerage = layerage;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }
        

        //Offset
        private void ConstructOffset()
        {
            this.OffsetPicker.Minimum = 0.0d;
            this.OffsetPicker.Maximum = 10.0d;
            this.OffsetPicker.ValueChangeStarted += (s, value) =>
            {             
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

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Style.StrokeStyle.DashOffset = offset;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OffsetPicker.ValueChangeCompleted += (s, value) =>
            {
                float offset = (float)value;

                //History
                LayersPropertyHistory history = new LayersPropertyHistory("Set stroke style");
                
                //Selection
                CanvasStrokeStyle strokeStyle = this.StrokeStyle.Clone();
                strokeStyle.DashOffset = offset;
                this.StrokeStyle = strokeStyle;
                this.SelectionViewModel.SetValueWithChildrenOnlyGroup((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History
                    var previous = layer.Style.StartingStrokeStyle.Clone();
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Style.StrokeStyle = previous.Clone();
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Style.StrokeStyle.DashOffset = offset;

                    this.SelectionViewModel.StandStyleLayerage = layerage;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            };
        }
        

        //Cap
        private void ConstructCap()
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
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Style.StrokeStyle = previous.Clone();
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Style.StrokeStyle.DashCap = cap;
                    layer.Style.StrokeStyle.StartCap = cap;
                    layer.Style.StrokeStyle.EndCap = cap;

                    this.SelectionViewModel.StandStyleLayerage = layerage;
                });

                //History
                this.ViewModel.HistoryPush(history);

                this.ViewModel.Invalidate();//Invalidate
            };
        }


        //Join
        private void ConstructJoin()
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
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layer.Style.StrokeStyle = previous.Clone();
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Style.StrokeStyle.LineJoin = join;

                    this.SelectionViewModel.StandStyleLayerage = layerage;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }

    }
}
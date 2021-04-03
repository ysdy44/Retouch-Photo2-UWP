// Core:              ★★★★★
// Referenced:   ★★★
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★★
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Width state of <see cref="Expander"/>.
    /// </summary>
    public enum ExpanderWidth
    {
        Width200,
        Width250,
        Width300,
        Width350,
    }

    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Overlay), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Pin), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Width200), GroupName = nameof(WidthStates))]
    [TemplateVisualState(Name = nameof(Width250), GroupName = nameof(WidthStates))]
    [TemplateVisualState(Name = nameof(Width300), GroupName = nameof(WidthStates))]
    [TemplateVisualState(Name = nameof(Width350), GroupName = nameof(WidthStates))]
    [ContentProperty(Name = nameof(Content))]
    public sealed partial class Expander : ContentControl
    {

        //@VisualState
        bool _vsIsOverlay = false;
        bool _vsIsPin = false;
        ExpanderWidth _vsWidth = ExpanderWidth.Width300;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsPin) return this.Pin;
                if (this._vsIsOverlay) return this.Overlay;
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        public VisualState WidthVisualState
        {
            get
            {
                switch (this._vsWidth)
                {
                    case ExpanderWidth.Width200: return this.Width200;
                    case ExpanderWidth.Width250: return this.Width250;
                    case ExpanderWidth.Width300: return this.Width300;
                    case ExpanderWidth.Width350: return this.Width350;
                    default: return this.Width300;
                }

            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        /// <summary> VisualState's ExpanderWidth. </summary>
        public ExpanderWidth ExpanderWidth
        {
            set
            {
                this._vsWidth = value;
                this.WidthVisualState = this.WidthVisualState;//State
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "Expander" />'s title. </summary>
        public string Title
        {
            get => (string)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "Expander.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(Expander), new PropertyMetadata(string.Empty));

        public Flyout Flyout { get; set; }
        public Canvas OverlayCanvas { get; set; }
        public StackPanel PinStackPanel { get; set; }


        #endregion


        VisualStateGroup VisualStateGroup;
        VisualState Normal;
        VisualState Overlay;
        VisualState Pin;

        VisualStateGroup WidthStates;
        VisualState Width200;
        VisualState Width250;
        VisualState Width300;
        VisualState Width350;


        MenuFlyout WidthMenuFlyout;
        ToggleMenuFlyoutItem WidthFlyoutItem200;
        ToggleMenuFlyoutItem WidthFlyoutItem250;
        ToggleMenuFlyoutItem WidthFlyoutItem300;
        ToggleMenuFlyoutItem WidthFlyoutItem350;


        Grid TitleGrid;
        Button OverlayButton;
        Button PinButton;
        Button CloseButton;


        //@Construct     
        /// <summary>
        /// Initializes a Expander. 
        /// </summary>
        public Expander()
        {
            this.DefaultStyleKey = typeof(Expander);
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.MaxWidth = this.OverlayCanvas.ActualWidth;
                this.MaxHeight = this.OverlayCanvas.ActualHeight;
            };
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.VisualStateGroup = base.GetTemplateChild(nameof(VisualStateGroup)) as VisualStateGroup;
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.Overlay = base.GetTemplateChild(nameof(Overlay)) as VisualState;
            this.Pin = base.GetTemplateChild(nameof(Pin)) as VisualState;

            this.WidthStates = base.GetTemplateChild(nameof(WidthStates)) as VisualStateGroup;
            this.Width200 = base.GetTemplateChild(nameof(Width200)) as VisualState;
            this.Width250 = base.GetTemplateChild(nameof(Width250)) as VisualState;
            this.Width300 = base.GetTemplateChild(nameof(Width300)) as VisualState;
            this.Width350 = base.GetTemplateChild(nameof(Width350)) as VisualState;


            this.WidthMenuFlyout = base.GetTemplateChild(nameof(WidthMenuFlyout)) as MenuFlyout;

            if (this.WidthFlyoutItem200 != null) this.WidthFlyoutItem200.Click -= this.WidthFlyoutItem200_Click;
            this.WidthFlyoutItem200 = base.GetTemplateChild(nameof(WidthFlyoutItem200)) as ToggleMenuFlyoutItem;
            if (this.WidthFlyoutItem200 != null) this.WidthFlyoutItem200.Click += this.WidthFlyoutItem200_Click;

            if (this.WidthFlyoutItem250 != null) this.WidthFlyoutItem250.Click -= this.WidthFlyoutItem250_Click;
            this.WidthFlyoutItem250 = base.GetTemplateChild(nameof(WidthFlyoutItem250)) as ToggleMenuFlyoutItem;
            if (this.WidthFlyoutItem250 != null) this.WidthFlyoutItem250.Click += this.WidthFlyoutItem250_Click;

            if (this.WidthFlyoutItem300 != null) this.WidthFlyoutItem300.Click -= this.WidthFlyoutItem300_Click;
            this.WidthFlyoutItem300 = base.GetTemplateChild(nameof(WidthFlyoutItem300)) as ToggleMenuFlyoutItem;
            if (this.WidthFlyoutItem300 != null) this.WidthFlyoutItem300.Click += this.WidthFlyoutItem300_Click;

            if (this.WidthFlyoutItem350 != null) this.WidthFlyoutItem350.Click -= this.WidthFlyoutItem350_Click;
            this.WidthFlyoutItem350 = base.GetTemplateChild(nameof(WidthFlyoutItem350)) as ToggleMenuFlyoutItem;
            if (this.WidthFlyoutItem350 != null) this.WidthFlyoutItem350.Click += this.WidthFlyoutItem350_Click;


            if (this.TitleGrid != null)
            {
                this.TitleGrid.Holding -= this.TitleGrid_Holding;
                this.TitleGrid.DoubleTapped -= this.TitleGrid_DoubleTapped;
                this.TitleGrid.RightTapped -= this.TitleGrid_RightTapped;
                this.TitleGrid.ManipulationStarted -= this.TitleGrid_ManipulationStarted;
                this.TitleGrid.ManipulationDelta -= this.TitleGrid_ManipulationDelta;
                this.TitleGrid.ManipulationCompleted -= this.TitleGrid_ManipulationCompleted;
            }
            this.TitleGrid = base.GetTemplateChild(nameof(TitleGrid)) as Grid;
            if (this.TitleGrid != null)
            {
                this.TitleGrid.Holding += this.TitleGrid_Holding;
                this.TitleGrid.DoubleTapped += this.TitleGrid_DoubleTapped;
                this.TitleGrid.RightTapped += this.TitleGrid_RightTapped;
                this.TitleGrid.ManipulationStarted += this.TitleGrid_ManipulationStarted;
                this.TitleGrid.ManipulationDelta += this.TitleGrid_ManipulationDelta;
                this.TitleGrid.ManipulationCompleted += this.TitleGrid_ManipulationCompleted;
            }

            if (this.OverlayButton != null) this.OverlayButton.Tapped -= this.OverlayButton_Tapped;
            this.OverlayButton = base.GetTemplateChild(nameof(OverlayButton)) as Button;
            if (this.OverlayButton != null) this.OverlayButton.Tapped += this.OverlayButton_Tapped;

            if (this.PinButton != null) this.PinButton.Tapped -= this.PinButton_Tapped;
            this.PinButton = base.GetTemplateChild(nameof(PinButton)) as Button;
            if (this.PinButton != null) this.PinButton.Tapped += this.PinButton_Tapped;

            if (this.CloseButton != null) this.CloseButton.Tapped -= this.CloseButton_Tapped;
            this.CloseButton = base.GetTemplateChild(nameof(CloseButton)) as Button;
            if (this.CloseButton != null) this.CloseButton.Tapped += this.CloseButton_Tapped;
        }


        private void WidthFlyoutItem200_Click(object sender, RoutedEventArgs e) => this.ExpanderWidth = ExpanderWidth.Width200;
        private void WidthFlyoutItem250_Click(object sender, RoutedEventArgs e) => this.ExpanderWidth = ExpanderWidth.Width250;
        private void WidthFlyoutItem300_Click(object sender, RoutedEventArgs e) => this.ExpanderWidth = ExpanderWidth.Width300;
        private void WidthFlyoutItem350_Click(object sender, RoutedEventArgs e) => this.ExpanderWidth = ExpanderWidth.Width350;


        private void TitleGrid_Holding(object sender, HoldingRoutedEventArgs e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);
        private void TitleGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);
        private void TitleGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);

        double left, top;
        private void TitleGrid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (this._vsIsOverlay == false) return;

            this.left = Canvas.GetLeft(this);
            this.top = Canvas.GetTop(this);
            this.MoveToTop(); //Delegate
        }
        private void TitleGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (this._vsIsOverlay == false) return;

            switch (base.FlowDirection)
            {
                case FlowDirection.LeftToRight:
                    this.left += e.Delta.Translation.X;
                    break;
                case FlowDirection.RightToLeft:
                    this.left -= e.Delta.Translation.X;
                    break;
                default:
                    break;
            }
            this.top += e.Delta.Translation.Y;


            double left = this.GetBoundPostionX(this.left, base.ActualWidth, this.OverlayCanvas.ActualWidth);
            double top = this.GetBoundPostionY(this.top, base.ActualHeight, this.OverlayCanvas.ActualHeight);
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
        }
        private void TitleGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (this._vsIsOverlay == false) return;

            this.left = Canvas.GetLeft(this);
            this.top = Canvas.GetTop(this);
        }

        private void OverlayButton_Tapped(object sender, TappedRoutedEventArgs e) => this.AsOverlay();
        private void PinButton_Tapped(object sender, TappedRoutedEventArgs e) => this.AsPin();
        private void CloseButton_Tapped(object sender, TappedRoutedEventArgs e) => this.AsFlyout();
    }

    public sealed partial class Expander : ContentControl
    {

        public void FlyoutShowAt(FrameworkElement element)
        {
            this.AsFlyout();
            this.Flyout.ShowAt(element);
        }
        private void AsFlyout()
        {
            {
                this.OverlayCanvas.Children.Remove(this);
                this.Flyout.Content = null;
                this.PinStackPanel.Children.Remove(this);
            }
            this.Flyout.Content = this;


            this._vsIsOverlay = false;
            this._vsIsPin = false;
            this.VisualState = this.VisualState;//State
        }
        private void AsOverlay()
        {
            if (this._vsIsPin == false)
            {
                GeneralTransform ttv = base.TransformToVisual(this.OverlayCanvas);
                Point screenCoords = ttv.TransformPoint(new Point());
                Canvas.SetLeft(this, screenCoords.X);
                Canvas.SetTop(this, screenCoords.Y);
            }
            this.Flyout.Hide();


            {
                this.OverlayCanvas.Children.Remove(this);
                this.Flyout.Content = null;
                this.PinStackPanel.Children.Remove(this);
            }
            this.OverlayCanvas.Children.Add(this);


            this._vsIsOverlay = true;
            this._vsIsPin = false;
            this.VisualState = this.VisualState;//State
        }
        private void AsPin()
        {
            this.Flyout.Hide();

            {
                this.OverlayCanvas.Children.Remove(this);
                this.Flyout.Content = null;
                this.PinStackPanel.Children.Remove(this);
            }
            this.PinStackPanel.Children.Add(this);


            this._vsIsOverlay = false;
            this._vsIsPin = true;
            this.VisualState = this.VisualState;//State
        }


        private double GetBoundPostionX(double postionX, double width, double canvasWidth)
        {
            if (canvasWidth < 400) canvasWidth = 400;
            if (width < 200) width = 200;

            if (postionX < 0) return 0;
            if (width >= canvasWidth) return 0;

            double right = canvasWidth - width;
            if (postionX > right) return right;

            return postionX;
        }
        private double GetBoundPostionY(double postionY, double height, double canvasHeight)
        {
            if (canvasHeight < 400) canvasHeight = 400;
            if (height < 200) height = 200;

            if (postionY < 0) return 0;
            if (height >= canvasHeight) return 0;

            double bottom = canvasHeight - height;
            if (postionY > bottom) return bottom;

            return postionY;
        }

        private void MoveToTop()
        {
            if (this.Parent is Canvas canvas)
            {
                if (canvas.Children.Contains(this))
                {
                    int max = 0;

                    foreach (UIElement child in canvas.Children)
                    {
                        int index = Canvas.GetZIndex(child);
                        Canvas.SetZIndex(child, index - 1);

                        if (max > index) max = index;
                    }

                    Canvas.SetZIndex(this, max);
                }
            }
        }

    }
}
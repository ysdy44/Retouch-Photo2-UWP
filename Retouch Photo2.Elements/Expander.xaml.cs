// Core:              ★★★★★
// Referenced:   ★★★
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★★
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Width of <see cref="Expander"/>.
    /// </summary>
    public enum ExpanderWidth
    {
        Width222 = 222,
        Width272 = 272,
        Width322 = 322,
        Width372 = 372,
    }

    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Width222), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Width272), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Width322), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Width372), GroupName = nameof(VisualStateGroup))]
    [ContentProperty(Name = nameof(Content))]
    public sealed class Expander : ContentControl
    {

        //@VisualState
        ExpanderWidth _vsWidth;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsWidth)
                {
                    case ExpanderWidth.Width222: return this.Width222;
                    case ExpanderWidth.Width272: return this.Width272;
                    case ExpanderWidth.Width322: return this.Width322;
                    case ExpanderWidth.Width372: return this.Width372;
                    default: return this.Normal;
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
                this.VisualState = this.VisualState;//State
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


        /// <summary> Gets or sets <see cref = "Expander" />'s width. </summary>
        public double Width2
        {
            get => (double)base.GetValue(Width2Property);
            set => base.SetValue(Width2Property, value);
        }
        /// <summary> Identifies the <see cref = "Expander.Width2" /> dependency property. </summary>
        public static readonly DependencyProperty Width2Property = DependencyProperty.Register(nameof(Width2), typeof(double), typeof(Expander), new PropertyMetadata(200.0d));


        /// <summary> Gets or sets <see cref = "Expander" />'s height. </summary>
        public double Height2
        {
            get => (double)base.GetValue(Height2Property);
            set => base.SetValue(Height2Property, value);
        }
        /// <summary> Identifies the <see cref = "Expander.Height2" /> dependency property. </summary>
        public static readonly DependencyProperty Height2Property = DependencyProperty.Register(nameof(Height2), typeof(double), typeof(Expander), new PropertyMetadata(200.0d));


        /// <summary> Gets or sets <see cref = "Expander" />'s window width. </summary>
        public double WindowWidth { get => this.MaxWidth; set => this.MaxWidth = value; }
        /// <summary> Gets or sets <see cref = "Expander" />'s window height. </summary>
        public double WindowHeight { get => this.MaxHeight; set => this.MaxHeight = value; }


        #endregion


        VisualStateGroup VisualStateGroup;
        VisualState Normal;
        VisualState Width222;
        VisualState Width272;
        VisualState Width322;
        VisualState Width372;


        MenuFlyout WidthMenuFlyout;
        ToggleMenuFlyoutItem WidthFlyoutItem222;
        ToggleMenuFlyoutItem WidthFlyoutItem272;
        ToggleMenuFlyoutItem WidthFlyoutItem322;
        ToggleMenuFlyoutItem WidthFlyoutItem372;

        Grid TitleGrid;
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
                this.Width2 = e.NewSize.Width;
                this.Height2 = e.NewSize.Height;
            };
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.VisualStateGroup = base.GetTemplateChild(nameof(VisualStateGroup)) as VisualStateGroup;
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.Width222 = base.GetTemplateChild(nameof(Width222)) as VisualState;
            this.Width272 = base.GetTemplateChild(nameof(Width272)) as VisualState;
            this.Width322 = base.GetTemplateChild(nameof(Width322)) as VisualState;
            this.Width372 = base.GetTemplateChild(nameof(Width372)) as VisualState;

            this.WidthMenuFlyout = base.GetTemplateChild(nameof(WidthMenuFlyout)) as MenuFlyout;

            if (this.WidthFlyoutItem222 != null) this.WidthFlyoutItem222.Click -= this.WidthFlyoutItem222_Click;
            this.WidthFlyoutItem222 = base.GetTemplateChild(nameof(WidthFlyoutItem222)) as ToggleMenuFlyoutItem;
            if (this.WidthFlyoutItem222 != null) this.WidthFlyoutItem222.Click += this.WidthFlyoutItem222_Click;

            if (this.WidthFlyoutItem272 != null) this.WidthFlyoutItem272.Click -= this.WidthFlyoutItem272_Click;
            this.WidthFlyoutItem272 = base.GetTemplateChild(nameof(WidthFlyoutItem272)) as ToggleMenuFlyoutItem;
            if (this.WidthFlyoutItem272 != null) this.WidthFlyoutItem272.Click += this.WidthFlyoutItem272_Click;

            if (this.WidthFlyoutItem322 != null) this.WidthFlyoutItem322.Click -= this.WidthFlyoutItem322_Click;
            this.WidthFlyoutItem322 = base.GetTemplateChild(nameof(WidthFlyoutItem322)) as ToggleMenuFlyoutItem;
            if (this.WidthFlyoutItem322 != null) this.WidthFlyoutItem322.Click += this.WidthFlyoutItem322_Click;

            if (this.WidthFlyoutItem372 != null) this.WidthFlyoutItem372.Click -= this.WidthFlyoutItem372_Click;
            this.WidthFlyoutItem372 = base.GetTemplateChild(nameof(WidthFlyoutItem372)) as ToggleMenuFlyoutItem;
            if (this.WidthFlyoutItem372 != null) this.WidthFlyoutItem372.Click += this.WidthFlyoutItem372_Click;


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

            if (this.CloseButton != null) this.CloseButton.Tapped -= this.CloseButton_Tapped;
            this.CloseButton = base.GetTemplateChild(nameof(CloseButton)) as Button;
            if (this.CloseButton != null) this.CloseButton.Tapped += this.CloseButton_Tapped;
        }


        private void WidthFlyoutItem222_Click(object sender, RoutedEventArgs e) => this.ExpanderWidth = ExpanderWidth.Width222;
        private void WidthFlyoutItem272_Click(object sender, RoutedEventArgs e) => this.ExpanderWidth = ExpanderWidth.Width272;
        private void WidthFlyoutItem322_Click(object sender, RoutedEventArgs e) => this.ExpanderWidth = ExpanderWidth.Width322;
        private void WidthFlyoutItem372_Click(object sender, RoutedEventArgs e) => this.ExpanderWidth = ExpanderWidth.Width372;


        private void TitleGrid_Holding(object sender, HoldingRoutedEventArgs e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);
        private void TitleGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);
        private void TitleGrid_RightTapped(object sender, RightTappedRoutedEventArgs e) => this.WidthMenuFlyout.ShowAt(this.TitleGrid);

        double left, top;
        private void TitleGrid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.left = Canvas.GetLeft(this);
            this.top = Canvas.GetTop(this);
            this.Move(); //Delegate
        }
        private void TitleGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
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

            double left = ExpanderButton.GetBoundPostionX(this.left, this.Width2, this.WindowWidth);
            double top = ExpanderButton.GetBoundPostionY(this.top, this.Height2, this.WindowHeight);
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
        }
        private void TitleGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.left = Canvas.GetLeft(this);
            this.top = Canvas.GetTop(this);
        }

        private void CloseButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }


        /// <summary> 
        /// Occurs when the position changes, Move the menu to top.
        /// </summary>
        private void Move()
        {
            if (this.Parent is Canvas canvas)
            {
                if (canvas.Children.Contains(this))
                {
                    int index = canvas.Children.IndexOf(this);
                    int count = canvas.Children.Count;
                    canvas.Children.Move((uint)index, (uint)count - 1); ;
                }
            }
        }

        /// <summary>
        /// Occurs when the flyout opened, Disable all menus, except the current menu.
        /// </summary>
        private void Opened()
        {
            if (this.Parent is Canvas canvas)
            {
                foreach (UIElement menu in canvas.Children)
                {
                    menu.IsHitTestVisible = false;
                }
                this.IsHitTestVisible = true;

                this.Move();

                canvas.Background = new SolidColorBrush(Colors.Transparent);
                canvas.Tapped += this.Canvas_Tapped;
            }
        }

        private void Canvas_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                canvas.Tapped -= this.Canvas_Tapped;
                canvas.Background = null;

                this.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary> 
        /// Occurs when the flyout closed, Enable all menus.     
        /// </summary>
        private void Closed()
        {
            if (this.Parent is Canvas canvas)
            {
                foreach (UIElement menu in canvas.Children)
                {
                    menu.IsHitTestVisible = true;
                }

                this.Visibility = Visibility.Collapsed;
                canvas.Background = null;
            }
        }

        /// <summary>
        /// Occurs when the flyout overlaid, Enable all menus.  
        /// </summary>
        private void Overlaid()
        {
            if (this.Parent is Canvas canvas)
            {
                foreach (UIElement menu in canvas.Children)
                {
                    menu.IsHitTestVisible = true;
                }

                canvas.Background = null;
            }
        }

    }
}
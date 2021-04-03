// Core:              ★★★★★
// Referenced:   ★★★
// Difficult:         
// Only:              ★★
// Complete:      ★★
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Button of Menu.
    /// </summary>
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(PointerOver), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(Pressed), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(FlyoutShow), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(Overlay), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(OverlayPointerOver), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(OverlayPressed), GroupName = nameof(CommonStates))]
    [ContentProperty(Name = nameof(Content))]
    public sealed partial class ExpanderButton : ContentControl
    {

        //@VisualState
        ClickMode _vsClickMode;
        Visibility _vsVisibility = Visibility.Collapsed;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                switch (this._vsVisibility)
                {
                    case Visibility.Visible:
                        switch (this._vsClickMode)
                        {
                            case ClickMode.Release: return this.Overlay;
                            case ClickMode.Hover: return this.OverlayPointerOver;
                            case ClickMode.Press: return this.OverlayPressed;
                            default: return this.Normal;
                        }
                    case Visibility.Collapsed:
                        switch (this._vsClickMode)
                        {
                            case ClickMode.Release: return this.Normal;
                            case ClickMode.Hover: return this.PointerOver;
                            case ClickMode.Press: return this.Pressed;
                            default: return this.Normal;
                        }
                    default:
                        return this.Normal;
                }
            }
            set { if (value == null) return; VisualStateManager.GoToState(this, value.Name, false); }
        }
        /// <summary> VisualState's ClickMode. </summary>
        public ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "ExpanderButton" />'s PlacementMode. </summary>
        public FlyoutPlacementMode PlacementMode { get; set; } = FlyoutPlacementMode.Bottom;


        /// <summary> Gets or sets <see cref = "ExpanderButton" />'s visibility. </summary>
        public Visibility Visibility2
        {
            get => (Visibility)base.GetValue(Visibility2Property);
            set => base.SetValue(Visibility2Property, value);
        }
        /// <summary> Identifies the <see cref = "ExpanderButton.Visibility2" /> dependency property. </summary>
        public static readonly DependencyProperty Visibility2Property = DependencyProperty.Register(nameof(Visibility2), typeof(Visibility), typeof(ExpanderButton), new PropertyMetadata(Visibility.Collapsed, (sender, e) =>
        {
            ExpanderButton control = (ExpanderButton)sender;

            if (e.NewValue is Visibility value)
            {
                control._vsVisibility = value;
                control.VisualState = control.VisualState;//State     
            }
        }));


        /// <summary> Gets or sets <see cref = "ExpanderButton" />'s left. </summary>
        public double Left
        {
            get => (double)base.GetValue(LeftProperty);
            set => base.SetValue(LeftProperty, value);
        }
        /// <summary> Identifies the <see cref = "ExpanderButton.Left" /> dependency property. </summary>
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(nameof(Left), typeof(double), typeof(ExpanderButton), new PropertyMetadata(0.0d));

        /// <summary> Gets or sets <see cref = "ExpanderButton" />'s top. </summary>
        public double Top
        {
            get => (double)base.GetValue(TopProperty);
            set => base.SetValue(TopProperty, value);
        }
        /// <summary> Identifies the <see cref = "ExpanderButton.Top" /> dependency property. </summary>
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(nameof(Top), typeof(double), typeof(ExpanderButton), new PropertyMetadata(0.0d));


        /// <summary> Gets or sets <see cref = "ExpanderButton" />'s width. </summary>
        public double Width2
        {
            get => (double)base.GetValue(Width2Property);
            set => base.SetValue(Width2Property, value);
        }
        /// <summary> Identifies the <see cref = "ExpanderButton.Width2" /> dependency property. </summary>
        public static readonly DependencyProperty Width2Property = DependencyProperty.Register(nameof(Width2), typeof(double), typeof(ExpanderButton), new PropertyMetadata(200.0d));


        /// <summary> Gets or sets <see cref = "ExpanderButton" />'s height. </summary>
        public double Height2
        {
            get => (double)base.GetValue(Height2Property);
            set => base.SetValue(Height2Property, value);
        }
        /// <summary> Identifies the <see cref = "ExpanderButton.Height2" /> dependency property. </summary>
        public static readonly DependencyProperty Height2Property = DependencyProperty.Register(nameof(Height2), typeof(double), typeof(ExpanderButton), new PropertyMetadata(200.0d));


        /// <summary> Gets or sets <see cref = "ExpanderButton" />'s window width. </summary>
        public double WindowWidth { get; set; } = 400.0d;
        /// <summary> Gets or sets <see cref = "ExpanderButton" />'s window height. </summary>
        public double WindowHeight { get; set; } = 400.0d;


        #endregion


        VisualStateGroup CommonStates;
        VisualState Normal;
        VisualState PointerOver;
        VisualState Pressed;
        VisualState FlyoutShow;
        VisualState Overlay;
        VisualState OverlayPointerOver;
        VisualState OverlayPressed;


        //@Construct
        /// <summary>
        /// Initializes a MenuButton.
        /// </summary>
        public ExpanderButton()
        {
            this.DefaultStyleKey = typeof(ExpanderButton);
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
            //Button
            this.Tapped += (s, e) =>
            {
                switch (this.Visibility2)
                {
                    case Visibility.Visible:
                        this.Visibility2 = Visibility.Collapsed;
                        break;
                    case Visibility.Collapsed:
                        this.Left = ExpanderButton.CalculatePostionX(this, this.Width2, this.WindowWidth, this.PlacementMode);
                        this.Top = ExpanderButton.CalculatePostionY(this, this.Height2, this.WindowHeight, this.PlacementMode);
                        this.Visibility2 = Visibility.Visible;
                        break;
                }
            };

        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CommonStates = base.GetTemplateChild(nameof(CommonStates)) as VisualStateGroup;
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.PointerOver = base.GetTemplateChild(nameof(PointerOver)) as VisualState;
            this.Pressed = base.GetTemplateChild(nameof(Pressed)) as VisualState;
            this.FlyoutShow = base.GetTemplateChild(nameof(FlyoutShow)) as VisualState;
            this.Overlay = base.GetTemplateChild(nameof(Overlay)) as VisualState;
            this.OverlayPointerOver = base.GetTemplateChild(nameof(OverlayPointerOver)) as VisualState;
            this.OverlayPressed = base.GetTemplateChild(nameof(OverlayPressed)) as VisualState;
            this.VisualState = this.VisualState;//State
        }


        /// <summary>
        /// Gets flyout-postion X on canvas by placement target.
        /// </summary>  
        public static double CalculatePostionX(FrameworkElement placementTarget, double width, double windowWidth, FlyoutPlacementMode placementMode)
        {
            //Gets visual-postion in windows.
            Point buttonPostion = placementTarget.TransformToVisual(Window.Current.Content).TransformPoint(new Point());//@VisualPostion
            double buttonWidth = placementTarget.ActualWidth;

            switch (placementTarget.FlowDirection)
            {
                case FlowDirection.LeftToRight:
                    break;
                case FlowDirection.RightToLeft:
                    buttonPostion.X = windowWidth - buttonPostion.X;
                    break;
                default:
                    break;
            }
            return ExpanderButton.GetBoundPostionX(ExpanderButton.GetFlyoutPostionX(buttonPostion, buttonWidth, width, placementMode), width, windowWidth);
        }
        /// <summary>
        /// Gets flyout-postion Y on canvas by placement target.
        /// </summary>  
        public static double CalculatePostionY(FrameworkElement placementTarget, double height, double windowHeight, FlyoutPlacementMode placementMode)
        {
            //Gets visual-postion in windows.
            Point buttonPostion = placementTarget.TransformToVisual(Window.Current.Content).TransformPoint(new Point());//@VisualPostion
            double buttonHeight = placementTarget.ActualHeight;

            return ExpanderButton.GetBoundPostionY(ExpanderButton.GetFlyoutPostionY(buttonPostion, buttonHeight, height, placementMode), height, windowHeight);
        }

        /// <summary>
        /// Gets flyout-postion X on canvas.
        /// </summary>
        private static double GetFlyoutPostionX(Point buttonPostion, double buttonWidth, double width, FlyoutPlacementMode placementMode)
        {
            if (buttonWidth < 20) buttonWidth = 20;
            if (width < 222) width = 222;

            switch (placementMode)
            {
                case FlyoutPlacementMode.Top:
                case FlyoutPlacementMode.Bottom:
                    return buttonPostion.X + buttonWidth / 2 - width / 2;
                case FlyoutPlacementMode.Left:
                    return buttonPostion.X - width;
                case FlyoutPlacementMode.Right:
                    return buttonPostion.X + buttonWidth;
                default: return 0;
            }
        }

        /// <summary>
        /// Gets flyout-postion Y on canvas.
        /// </summary>
        private static double GetFlyoutPostionY(Point buttonPostion, double buttonHeight, double height, FlyoutPlacementMode placementMode)
        {
            if (buttonHeight < 20) buttonHeight = 20;
            if (height < 50) height = 50;

            switch (placementMode)
            {
                case FlyoutPlacementMode.Top:
                    return buttonPostion.Y - height;
                case FlyoutPlacementMode.Bottom:
                    return buttonPostion.Y + buttonHeight;
                case FlyoutPlacementMode.Left:
                case FlyoutPlacementMode.Right:
                    return buttonPostion.Y + buttonHeight / 2 - height / 2;
                default: return 0;
            }
        }

        /// <summary>
        /// Gets bound-postion X in windows.
        /// </summary>
        /// <param name="postionX"> The source postion X. </param>
        /// <returns> The croped postion. </returns>
        public static double GetBoundPostionX(double postionX, double width, double windowWidth)
        {
            if (windowWidth < 400) windowWidth = 400;
            if (width < 200) width = 200;

            if (postionX < 0) return 0;
            if (width >= windowWidth) return 0;

            double right = windowWidth - width;
            if (postionX > right) return right;

            return postionX;
        }
        /// <summary>
        /// Gets bound-postion Y in windows.
        /// </summary>
        /// <param name="postionY"> The source postion Y. </param>
        /// <returns> The croped postion. </returns>
        public static double GetBoundPostionY(double postionY, double height, double windowHeight)
        {
            if (windowHeight < 400) windowHeight = 400;
            if (height < 200) height = 200;

            if (postionY < 0) return 0;
            if (height >= windowHeight) return 0;

            double bottom = windowHeight - height;
            if (postionY > bottom) return bottom;

            return postionY;
        }

    }
}
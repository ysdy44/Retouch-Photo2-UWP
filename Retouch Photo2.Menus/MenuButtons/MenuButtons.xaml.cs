// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Button of <see cref="IMenu "/>.
    /// </summary>
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(PointerOver), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(Pressed), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(FlyoutShow), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(Overlay), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(OverlayPointerOver), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(OverlayPressed), GroupName = nameof(CommonStates))]
    [ContentProperty(Name = nameof(Content))]
    public sealed partial class MenuButton : ContentControl, IExpanderButton
    {

        //@Content   
        /// <summary> Gets or sets the state. </summary>
        public ExpanderState ExpanderState
        {
            set
            {
                this._vsMenuState = value;
                this.VisualState = this.VisualState;//State         
            }
        }
        /// <summary> Get the self. </summary>
        public FrameworkElement Self => this;


        //@VisualState
        ClickMode _vsClickMode;
        ExpanderState _vsMenuState = ExpanderState.Hide;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsMenuState == ExpanderState.FlyoutShow) return this.FlyoutShow;

                if (this._vsMenuState == ExpanderState.Hide)
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Normal;
                        case ClickMode.Hover: return this.PointerOver;
                        case ClickMode.Press: return this.Pressed;
                    }
                }

                if (this._vsMenuState == ExpanderState.Overlay || this._vsMenuState == ExpanderState.OverlayNotExpanded)
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Overlay;
                        case ClickMode.Hover: return this.OverlayPointerOver;
                        case ClickMode.Press: return this.OverlayPressed;
                    }
                }
                return this.Normal;
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


        /// <summary> Gets or sets the IsOpen of <see cref = "MenuButton" />. </summary>
        public bool IsOpen
        {
            get => (bool)base.GetValue(IsOpenProperty);
            set => base.SetValue(IsOpenProperty, value);
        }
        /// <summary> Identifies the <see cref = "MenuButton.IsOpen" /> dependency property. </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(MenuButton), new PropertyMetadata(false));

        /// <summary> Gets or sets the title of <see cref = "MenuButton" />. </summary>
        public string Title
        {
            get => (string)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "MenuButton.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(MenuButton), new PropertyMetadata(null));


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
        public MenuButton()
        {
            this.DefaultStyleKey = typeof(MenuButton);
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
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

    }
}
// Core:              ★★
// Referenced:   
// Difficult:         ★*
// Only:              ★★
// Complete:      ★★
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents a control that the user can select (select) or clear (not select).
    /// </summary>  
    [TemplateVisualState(Name = nameof(UncheckedNormal), GroupName = nameof(CombinedStates))]
    [TemplateVisualState(Name = nameof(UncheckedPointerOver), GroupName = nameof(CombinedStates))]
    [TemplateVisualState(Name = nameof(UncheckedPressed), GroupName = nameof(CombinedStates))]
    [TemplateVisualState(Name = nameof(CheckedNormal), GroupName = nameof(CombinedStates))]
    [TemplateVisualState(Name = nameof(CheckedPointerOver), GroupName = nameof(CombinedStates))]
    [TemplateVisualState(Name = nameof(CheckedPressed), GroupName = nameof(CombinedStates))]
    [ContentProperty(Name = nameof(Content))]
    public sealed class CheckControl : ContentControl
    {

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "CheckControl" />'s IsChecked. </summary>
        public bool IsChecked
        {
            get => (bool)base.GetValue(IsCheckedProperty);
            set => base.SetValue(IsCheckedProperty, value);
        }
        /// <summary> Identifies the <see cref = "CheckControl.IsChecked" /> dependency property. </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(CheckControl), new PropertyMetadata(false, (sender, e)=>
        {
            CheckControl control = (CheckControl)sender;

            if (e.NewValue is bool value)
            {
                control._vsIsChecked = value;
                control.VisualState = control.VisualState;//State
            }
        }));


        #endregion


        VisualStateGroup CombinedStates;
        VisualState UncheckedNormal;
        VisualState UncheckedPointerOver;
        VisualState UncheckedPressed;
        VisualState CheckedNormal;
        VisualState CheckedPointerOver;
        VisualState CheckedPressed;

        //@VisualState
        bool _vsIsChecked;
        ClickMode _vsClickMode;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsChecked)
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.CheckedNormal;
                        case ClickMode.Hover: return this.CheckedPointerOver;
                        case ClickMode.Press: return this.CheckedPressed;
                        default: return this.UncheckedNormal;
                    }
                }
                else
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.UncheckedNormal;
                        case ClickMode.Hover: return this.UncheckedPointerOver;
                        case ClickMode.Press: return this.UncheckedPressed;
                        default: return this.UncheckedNormal;
                    }
                }
            }
            set => VisualStateManager.GoToState(this, value?.Name ?? "UncheckedNormal", true);
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


        //@Construct
        /// <summary>
        /// Initializes a CheckControl.
        /// </summary>
        public CheckControl()
        {
            this.DefaultStyleKey = typeof(CheckControl);
            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CombinedStates = base.GetTemplateChild(nameof(CombinedStates)) as VisualStateGroup;
            this.UncheckedNormal = base.GetTemplateChild(nameof(UncheckedNormal)) as VisualState;
            this.UncheckedPointerOver = base.GetTemplateChild(nameof(UncheckedPointerOver)) as VisualState;
            this.UncheckedPressed = base.GetTemplateChild(nameof(UncheckedPressed)) as VisualState;
            this.CheckedNormal = base.GetTemplateChild(nameof(CheckedNormal)) as VisualState;
            this.CheckedPointerOver = base.GetTemplateChild(nameof(CheckedPointerOver)) as VisualState;
            this.CheckedPressed = base.GetTemplateChild(nameof(CheckedPressed)) as VisualState;
        }

    }
}
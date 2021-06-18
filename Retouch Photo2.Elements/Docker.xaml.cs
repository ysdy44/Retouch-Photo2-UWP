// Core:              ★★★★★
// Referenced:   ★★★
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★★
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents a docker panel that contains check boxes, hyperlinks, buttons, and other XAML content that you can customize.
    /// </summary>
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Phone), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(Pad), GroupName = nameof(VisualStateGroup))]
    [TemplateVisualState(Name = nameof(PC), GroupName = nameof(VisualStateGroup))]
    [TemplatePart(Name = nameof(PrimaryButton), Type = typeof(Button))]
    [ContentProperty(Name = nameof(Content))]
    public sealed partial class Docker : ContentControl
    {

        //@Delegate
        /// <summary> Occurs when the clicking the s secondary button. </summary>
        public event RoutedEventHandler SecondaryButtonClick;
        /// <summary> Occurs when the clicking the s primary button. </summary>
        public event RoutedEventHandler PrimaryButtonClick;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "Docker" />'s title. </summary>
        public string Title
        {
            get => (string)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "Docker.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(Docker), new PropertyMetadata(string.Empty));


        /// <summary> Gets or sets <see cref = "Docker" />'s icon template. </summary>
        public ControlTemplate IconTemplate
        {
            get => (ControlTemplate)base.GetValue(IconTemplateProperty);
            set => base.SetValue(IconTemplateProperty, value);
        }
        /// <summary> Identifies the <see cref = "Docker.IconTemplate" /> dependency property. </summary>
        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(nameof(IconTemplate), typeof(ControlTemplate), typeof(Docker), new PropertyMetadata(null));


        /// <summary> Gets or sets <see cref = "Docker.PrimaryButton" />'s content. </summary>
        public object PrimaryButtonContent
        {
            get => (object)base.GetValue(PrimaryButtonContentProperty);
            set => base.SetValue(PrimaryButtonContentProperty, value);
        }
        /// <summary> Identifies the <see cref = "Docker.PrimaryButtonContent" /> dependency property. </summary>
        public static readonly DependencyProperty PrimaryButtonContentProperty = DependencyProperty.Register(nameof(PrimaryButtonContent), typeof(object), typeof(Docker), new PropertyMetadata("OK"));


        #endregion


        VisualStateGroup VisualStateGroup;
        VisualState Normal;
        VisualState Phone;
        VisualState Pad;
        VisualState PC;
        Border LayoutBorder;
        Grid RootGrid;
        Button PrimaryButton;


        //@VisualState
        bool _vsIsShow = false;
        DeviceLayoutType _vsDeviceLayoutType = DeviceLayoutType.PC;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsShow == false) return this.Normal;

                switch (this._vsDeviceLayoutType)
                {
                    case DeviceLayoutType.PC: return this.PC;
                    case DeviceLayoutType.Pad: return this.Pad;
                    case DeviceLayoutType.Phone: return this.Phone;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value?.Name ?? "Normal", true);
        }
        /// <summary> Gets or sets the state. </summary>
        public bool IsShow
        {
            get => this._vsIsShow;
            private set
            {
                this._vsIsShow = value;
                this.VisualState = this.VisualState; // State
            }
        }
        /// <summary> Gets or sets the device layout type. </summary>
        public DeviceLayoutType DeviceLayoutType
        {
            set
            {
                this._vsDeviceLayoutType = value;
                this.VisualState = this.VisualState; // State
            }
        }


        //@Construct     
        /// <summary>
        /// Initializes a Docker. 
        /// </summary>
        public Docker()
        {
            this.DefaultStyleKey = typeof(Docker);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.VisualStateGroup = base.GetTemplateChild(nameof(VisualStateGroup)) as VisualStateGroup;
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.Phone = base.GetTemplateChild(nameof(Phone)) as VisualState;
            this.Pad = base.GetTemplateChild(nameof(Pad)) as VisualState;
            this.PC = base.GetTemplateChild(nameof(PC)) as VisualState;

            if (this.LayoutBorder != null) this.LayoutBorder.Tapped -= this.LayoutBorder_Tapped;
            this.LayoutBorder = base.GetTemplateChild(nameof(LayoutBorder)) as Border;
            if (this.LayoutBorder != null) this.LayoutBorder.Tapped += this.LayoutBorder_Tapped;

            if (this.RootGrid != null) this.RootGrid.Tapped -= this.RootGrid_Tapped;
            this.RootGrid = base.GetTemplateChild(nameof(RootGrid)) as Grid;
            if (this.RootGrid != null) this.RootGrid.Tapped += this.RootGrid_Tapped;

            if (this.PrimaryButton != null) this.PrimaryButton.Click -= this.PrimaryButtonClick;
            this.PrimaryButton = base.GetTemplateChild(nameof(PrimaryButton)) as Button;
            if (this.PrimaryButton != null) this.PrimaryButton.Click += this.PrimaryButtonClick;

            this.VisualState = this.VisualState; // State
        }
        private void LayoutBorder_Tapped(object sender, TappedRoutedEventArgs e) => this.Hide();
        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e) => e.Handled = true;
    }
}
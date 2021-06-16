// Core:              ★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents a dialog box that contains check boxes, hyperlinks, buttons, and other XAML content that you can customize.
    /// </summary>
    [TemplateVisualState(Name = nameof(DialogShowed), GroupName = nameof(DialogShowingStates))]
    [TemplateVisualState(Name = nameof(DialogHidden), GroupName = nameof(DialogShowingStates))]
    [TemplatePart(Name = nameof(LayoutRoot), Type = typeof(Border))]
    [TemplatePart(Name = nameof(RootGrid), Type = typeof(Grid))]
    [TemplatePart(Name = nameof(SecondaryButton), Type = typeof(Button))]
    [TemplatePart(Name = nameof(PrimaryButton), Type = typeof(Button))]
    [ContentProperty(Name = nameof(Content))]
    public sealed partial class Dialog : ContentControl
    {

        //@Delegate
        /// <summary> Occurs when the clicking the s secondary button. </summary>
        public event RoutedEventHandler SecondaryButtonClick;
        /// <summary> Occurs when the clicking the s primary button. </summary>
        public event RoutedEventHandler PrimaryButtonClick;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "Dialog" />'s title. </summary>
        public object Title
        {
            get => (object)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "Dialog.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(object), typeof(Dialog), new PropertyMetadata(null));


        /// <summary> Gets or sets <see cref = "Dialog.SecondaryButton" />'s text. </summary>
        public string SecondaryButtonText
        {
            get => (string)base.GetValue(SecondaryButtonTextProperty);
            set => base.SetValue(SecondaryButtonTextProperty, value);
        }
        /// <summary> Identifies the <see cref = "Dialog.PrimaryButtonContent" /> dependency property. </summary>
        public static readonly DependencyProperty SecondaryButtonTextProperty = DependencyProperty.Register(nameof(SecondaryButtonText), typeof(string), typeof(Dialog), new PropertyMetadata("Cancel"));


        /// <summary> Gets or sets <see cref = "Dialog.PrimaryButton" />'s text. </summary>
        public string PrimaryButtonText
        {
            get => (string)base.GetValue(PrimaryButtonTextProperty);
            set => base.SetValue(PrimaryButtonTextProperty, value);
        }
        /// <summary> Identifies the <see cref = "Dialog.PrimaryButtonText" /> dependency property. </summary>
        public static readonly DependencyProperty PrimaryButtonTextProperty = DependencyProperty.Register(nameof(PrimaryButtonText), typeof(string), typeof(Dialog), new PropertyMetadata("OK"));


        #endregion


        VisualStateGroup DialogShowingStates;
        VisualState DialogShowed;
        VisualState DialogHidden;
        Border LayoutRoot;
        Border RootGrid;
        Button SecondaryButton;
        Button PrimaryButton;


        //@VisualState
        bool _vsIsShow;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get => this._vsIsShow ? this.DialogShowed : this.DialogHidden;
            set => VisualStateManager.GoToState(this, value.Name, true);
        }


        //@Construct
        /// <summary>
        /// Initializes a Dialog. 
        /// </summary>
        public Dialog()
        {
            this.DefaultStyleKey = typeof(Dialog);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.DialogShowingStates = base.GetTemplateChild(nameof(DialogShowingStates)) as VisualStateGroup;
            this.DialogShowed = base.GetTemplateChild(nameof(DialogShowed)) as VisualState;
            this.DialogHidden = base.GetTemplateChild(nameof(DialogHidden)) as VisualState;
            this.VisualState = this.VisualState; // State

            if (this.LayoutRoot != null) this.LayoutRoot.Tapped -= this.LayoutRoot_Tapped;
            this.LayoutRoot = base.GetTemplateChild(nameof(LayoutRoot)) as Border;
            if (this.LayoutRoot != null) this.LayoutRoot.Tapped += this.LayoutRoot_Tapped;

            if (this.RootGrid != null) this.RootGrid.Tapped -= this.RootGrid_Tapped;
            this.RootGrid = base.GetTemplateChild(nameof(RootGrid)) as Border;
            if (this.RootGrid != null) this.RootGrid.Tapped += this.RootGrid_Tapped;

            if (this.SecondaryButton != null) this.SecondaryButton.Click -= this.SecondaryButtonClick;
            this.SecondaryButton = base.GetTemplateChild(nameof(SecondaryButton)) as Button;
            if (this.SecondaryButton != null) this.SecondaryButton.Click += this.SecondaryButtonClick;

            if (this.PrimaryButton != null) this.PrimaryButton.Click -= this.PrimaryButtonClick;
            this.PrimaryButton = base.GetTemplateChild(nameof(PrimaryButton)) as Button;
            if (this.PrimaryButton != null) this.PrimaryButton.Click += this.PrimaryButtonClick;
        }

        private void LayoutRoot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Hide();
        }
        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

    }
}
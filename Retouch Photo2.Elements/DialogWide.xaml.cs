﻿// Core:              ★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Represents a dialog box that contains grid view that you can customize.
    /// </summary>
    [TemplateVisualState(Name = nameof(DialogShowed), GroupName = nameof(DialogShowingStates))]
    [TemplateVisualState(Name = nameof(DialogHidden), GroupName = nameof(DialogShowingStates))]
    [TemplatePart(Name = nameof(LayoutRoot), Type = typeof(Border))]
    [TemplatePart(Name = nameof(RootGrid), Type = typeof(Grid))]
    [TemplatePart(Name = nameof(CloseButton), Type = typeof(Button))]
    [TemplatePart(Name = nameof(PrimaryButton), Type = typeof(Button))]
    [ContentProperty(Name = nameof(Content))]
    public sealed partial class DialogWide : ContentControl
    {

        //@Delegate
        /// <summary> Occurs when the tapped the s close button. </summary>
        public event TappedEventHandler CloseButtonTapped;
        /// <summary> Occurs when the clicking the s primary button. </summary>
        public event RoutedEventHandler PrimaryButtonClick;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "DialogWide" />'s title. </summary>
        public object Title
        {
            get => (object)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "DialogWide.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(object), typeof(DialogWide), new PropertyMetadata(null));


        /// <summary> Gets or sets <see cref = "DialogWide.PrimaryButton" />'s title. </summary>
        public string PrimaryButtonText
        {
            get => (string)base.GetValue(PrimaryButtonTextProperty);
            set => base.SetValue(PrimaryButtonTextProperty, value);
        }
        /// <summary> Identifies the <see cref = "DialogWide.PrimaryButtonText" /> dependency property. </summary>
        public static readonly DependencyProperty PrimaryButtonTextProperty = DependencyProperty.Register(nameof(PrimaryButtonText), typeof(string), typeof(DialogWide), new PropertyMetadata("OK"));


        #endregion


        VisualStateGroup DialogShowingStates;
        VisualState DialogShowed;
        VisualState DialogHidden;
        Border LayoutRoot;
        Border RootGrid;
        ListViewItem CloseButton;
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


        public bool IsShow => this._vsIsShow;


        //@Construct
        /// <summary>
        /// Initializes a DialogWide. 
        /// </summary>
        public DialogWide()
        {
            this.DefaultStyleKey = typeof(DialogWide);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.DialogShowingStates = base.GetTemplateChild(nameof(DialogShowingStates)) as VisualStateGroup;
            this.DialogShowed = base.GetTemplateChild(nameof(DialogShowed)) as VisualState;
            this.DialogHidden = base.GetTemplateChild(nameof(DialogHidden)) as VisualState;
            this.VisualState = this.VisualState; // State

            if ((this.LayoutRoot is null) == false) this.LayoutRoot.Tapped -= this.Root_Tapped;
            this.LayoutRoot = base.GetTemplateChild(nameof(LayoutRoot)) as Border;
            if ((this.LayoutRoot is null) == false) this.LayoutRoot.Tapped += this.Root_Tapped;

            if ((this.RootGrid is null) == false) this.RootGrid.Tapped -= this.Root_Tapped;
            this.RootGrid = base.GetTemplateChild(nameof(RootGrid)) as Border;
            if ((this.RootGrid is null) == false) this.RootGrid.Tapped += this.Root_Tapped;

            if ((this.CloseButton is null) == false) this.CloseButton.Tapped -= this.CloseButtonTapped;
            this.CloseButton = base.GetTemplateChild(nameof(CloseButton)) as ListViewItem;
            if ((this.CloseButton is null) == false) this.CloseButton.Tapped += this.CloseButtonTapped;

            if ((this.PrimaryButton is null) == false) this.PrimaryButton.Click -= this.PrimaryButtonClick;
            this.PrimaryButton = base.GetTemplateChild(nameof(PrimaryButton)) as Button;
            if ((this.PrimaryButton is null) == false) this.PrimaryButton.Click += this.PrimaryButtonClick;
        }

        private void Root_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

    }
}
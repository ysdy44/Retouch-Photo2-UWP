// Core:              
// Referenced:   
// Difficult:         ★★★
// Only:              ★★
// Complete:      ★★★
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Grid control that display and adjust parameters for adaptive width.
    /// </summary>
    [TemplateVisualState(Name = nameof(Normal), GroupName = nameof(CommonStates))]
    [TemplateVisualState(Name = nameof(Disable), GroupName = nameof(CommonStates))]
    [TemplatePart(Name = nameof(PhoneText), Type = typeof(string))]
    [TemplatePart(Name = nameof(PadText), Type = typeof(string))]
    [TemplatePart(Name = nameof(PCText), Type = typeof(string))]
    public sealed partial class AdaptiveWidthGrid : ContentControl
    {
        //@Delegate  
        /// <summary> Occurs when the scroll-mode changes. </summary>
        public EventHandler<ScrollMode> ScrollModeChanged;
        /// <summary> Occurs when the phone-width value changes. </summary>
        public EventHandler<int> PhoneWidthChanged;
        /// <summary> Occurs when the pad-width value changes. </summary>
        public EventHandler<int> PadWidthChanged;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "AdaptiveWidthGrid" />'s phone width. </summary>
        public string PhoneText
        {
            get => (string)base.GetValue(PhoneTextProperty);
            set => base.SetValue(PhoneTextProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdaptiveWidthGrid.PhoneText" /> dependency property. </summary>
        public static readonly DependencyProperty PhoneTextProperty = DependencyProperty.Register(nameof(PhoneText), typeof(string), typeof(AdaptiveWidthGrid), new PropertyMetadata("Phone"));


        /// <summary> Gets or sets <see cref = "AdaptiveWidthGrid" />'s Pad width. </summary>
        public string PadText
        {
            get => (string)base.GetValue(PadTextProperty);
            set => base.SetValue(PadTextProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdaptiveWidthGrid.PadText" /> dependency property. </summary>
        public static readonly DependencyProperty PadTextProperty = DependencyProperty.Register(nameof(PadText), typeof(string), typeof(AdaptiveWidthGrid), new PropertyMetadata("Pad"));


        /// <summary> Gets or sets <see cref = "AdaptiveWidthGrid" />'s PC width. </summary>
        public string PCText
        {
            get => (string)base.GetValue(PCTextProperty);
            set => base.SetValue(PCTextProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdaptiveWidthGrid.PCText" /> dependency property. </summary>
        public static readonly DependencyProperty PCTextProperty = DependencyProperty.Register(nameof(PCText), typeof(string), typeof(AdaptiveWidthGrid), new PropertyMetadata("PC"));
                

        #endregion


        VisualStateGroup CommonStates;
        VisualState Normal;
        VisualState Disable;
        TextBlock PhoneTextBlock;
        TextBlock PadTextBlock;
        ColumnDefinition PhoneGridLength;
        ColumnDefinition PadGridLength;
        ColumnDefinition PCGridLength;
        Thumb PhoneThumb;
        Thumb PadThumb;


        //@VisualState
        VisualState VisualState
        {
            get => base.IsEnabled ? this.Normal : this.Disable;
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        #region Width


        /// <summary> Layout phone width. </summary>
        public int PhoneWidth = 600;

        /// <summary> Layout pad width. </summary>
        public int PadWidth = 900;

        /// <summary>
        /// Change the layout based on width.
        /// </summary>
        public void SetWidth() => this.SetWidthCore(this.PhoneWidth, this.PadWidth);

        private void SetWidthCore(int phoneWidth, int padWidth, int pcWidth = 2000)
        {
            if (this.PhoneTextBlock != null) this.PhoneTextBlock.Text = phoneWidth.ToString();
            if (this.PadTextBlock != null) this.PadTextBlock.Text = padWidth.ToString();

            double phoneLength = phoneWidth;
            double padLength = padWidth - phoneWidth;
            double pcLength = pcWidth - padWidth;

            if (this.PhoneGridLength != null) this.PhoneGridLength.Width = new GridLength(phoneLength < 1 ? 1 : phoneLength, GridUnitType.Star);
            if (this.PadGridLength != null) this.PadGridLength.Width = new GridLength(padLength < 1 ? 1 : padLength, GridUnitType.Star);
            if (this.PCGridLength != null) this.PCGridLength.Width = new GridLength(pcLength < 1 ? 1 : pcLength, GridUnitType.Star);
        }


        #endregion


        #region Drag


        double _horizontal;

        int _startingPhoneWidth;
        int _startingPadWidth;

        double _startingPhoneLength;
        double _startingPadLength;
        double _startingLength;


        private void DragStarted()
        {
            this._horizontal = 0;

            this._startingPhoneWidth = this.PhoneWidth;
            this._startingPadWidth = this.PadWidth;

            this._startingPhoneLength = this.PhoneGridLength.Width.Value;
            this._startingPadLength = this.PadGridLength.Width.Value;
            double startingPCLength = this.PCGridLength.Width.Value;
            this._startingLength = this._startingPhoneLength + this._startingPadLength + startingPCLength;
        }
        private double DragDelta(double horizontalChange)
        {
            this._horizontal += horizontalChange;

            double scale = this._horizontal / this.ActualWidth;
            double changedLength = scale * this._startingLength;
            return changedLength;
        }


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a AdaptiveWidthGrid. 
        /// </summary>
        public AdaptiveWidthGrid()
        {
            this.DefaultStyleKey = typeof(AdaptiveWidthGrid);
            base.IsEnabledChanged += (s, e) => this.VisualState = this.VisualState;//State
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CommonStates = base.GetTemplateChild(nameof(CommonStates)) as VisualStateGroup;
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.Disable = base.GetTemplateChild(nameof(Disable)) as VisualState;
            this.VisualState = this.VisualState;//State

            this.PhoneTextBlock = base.GetTemplateChild(nameof(PhoneTextBlock)) as TextBlock;
            this.PadTextBlock = base.GetTemplateChild(nameof(PadTextBlock)) as TextBlock;
            this.PhoneGridLength = base.GetTemplateChild(nameof(PhoneGridLength)) as ColumnDefinition;
            this.PadGridLength = base.GetTemplateChild(nameof(PadGridLength)) as ColumnDefinition;
            this.PCGridLength = base.GetTemplateChild(nameof(PCGridLength)) as ColumnDefinition;
            this.SetWidth();

            //Phone
            this.PhoneThumb = base.GetTemplateChild(nameof(PhoneThumb)) as Thumb;
            this.PhoneThumb.DragStarted += (s, e) =>
            {
                this.DragStarted();
                this.ScrollModeChanged?.Invoke(this, ScrollMode.Disabled);//Delegate
            };
            this.PhoneThumb.DragDelta += (s, e) =>
            {
                double changedLength = this.DragDelta(e.HorizontalChange);
                double phoneLength = this._startingPhoneLength + changedLength;
                double scale = phoneLength / this._startingPhoneLength;

                this.PhoneWidth = (int)(this._startingPhoneWidth * scale);
                if (this.PhoneWidth < 300) this.PhoneWidth = 300;
                if (this.PhoneWidth > this.PadWidth - 100) this.PhoneWidth = this.PadWidth - 100;
                this.SetWidth();
            };
            this.PhoneThumb.DragCompleted += (s, e) =>
            {
                this.ScrollModeChanged?.Invoke(this, ScrollMode.Enabled);//Delegate
                this.PhoneWidthChanged?.Invoke(this, this.PhoneWidth);//Delegate
            };

            //Pad
            this.PadThumb = base.GetTemplateChild(nameof(PadThumb)) as Thumb;
            this.PadThumb.DragStarted += (s, e) =>
            {
                this.DragStarted();
                this.ScrollModeChanged?.Invoke(this, ScrollMode.Disabled);//Delegate
            };
            this.PadThumb.DragDelta += (s, e) =>
            {
                double changedLength = this.DragDelta(e.HorizontalChange);
                double padLength = this._startingPadLength + changedLength;
                double scale = padLength / this._startingPadLength;

                this.PadWidth = (int)((this._startingPadWidth - this._startingPhoneWidth) * scale) + this._startingPhoneWidth;
                if (this.PadWidth < this.PhoneWidth + 100) this.PadWidth = this.PhoneWidth + 100;
                if (this.PadWidth > 1800) this.PadWidth = 1800;
                this.SetWidth();
            };
            this.PadThumb.DragCompleted += (s, e) =>
            {
                this.ScrollModeChanged?.Invoke(this, ScrollMode.Enabled);//Delegate
                this.PadWidthChanged?.Invoke(this, this.PadWidth);//Delegate
            };
        }

    }
}
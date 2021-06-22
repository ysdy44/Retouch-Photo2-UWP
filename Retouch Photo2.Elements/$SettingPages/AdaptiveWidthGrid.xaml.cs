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
        public event EventHandler<ScrollMode> ScrollModeChanged;
        /// <summary> Occurs when the phone-width value changes. </summary>
        public event EventHandler<int> PhoneWidthChanged;
        /// <summary> Occurs when the pad-width value changes. </summary>
        public event EventHandler<int> PadWidthChanged;


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


        /// <summary> Gets or sets <see cref = "AdaptiveWidthGrid" />'s phone width. </summary>
        public string PhoneCount
        {
            get => (string)base.GetValue(PhoneCountProperty);
            set => base.SetValue(PhoneCountProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdaptiveWidthGrid.PhoneCount" /> dependency property. </summary>
        public static readonly DependencyProperty PhoneCountProperty = DependencyProperty.Register(nameof(PhoneCount), typeof(string), typeof(AdaptiveWidthGrid), new PropertyMetadata("0"));


        /// <summary> Gets or sets <see cref = "AdaptiveWidthGrid" />'s Pad width. </summary>
        public string PadCount
        {
            get => (string)base.GetValue(PadCountProperty);
            set => base.SetValue(PadCountProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdaptiveWidthGrid.PadCount" /> dependency property. </summary>
        public static readonly DependencyProperty PadCountProperty = DependencyProperty.Register(nameof(PadCount), typeof(string), typeof(AdaptiveWidthGrid), new PropertyMetadata("0"));


        /// <summary> Gets or sets <see cref = "AdaptiveLengthGrid" />'s phone length. </summary>
        public GridLength PhoneLength
        {
            get => (GridLength)base.GetValue(PhoneLengthProperty);
            set => base.SetValue(PhoneLengthProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdaptiveLengthGrid.PhoneLength" /> dependency property. </summary>
        public static readonly DependencyProperty PhoneLengthProperty = DependencyProperty.Register(nameof(PhoneLength), typeof(GridLength), typeof(AdaptiveWidthGrid), new PropertyMetadata(new GridLength(1, GridUnitType.Star)));


        /// <summary> Gets or sets <see cref = "AdaptiveLengthGrid" />'s Pad length. </summary>
        public GridLength PadLength
        {
            get => (GridLength)base.GetValue(PadLengthProperty);
            set => base.SetValue(PadLengthProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdaptiveLengthGrid.PadLength" /> dependency property. </summary>
        public static readonly DependencyProperty PadLengthProperty = DependencyProperty.Register(nameof(PadLength), typeof(GridLength), typeof(AdaptiveWidthGrid), new PropertyMetadata(new GridLength(1, GridUnitType.Star)));


        /// <summary> Gets or sets <see cref = "AdaptiveLengthGrid" />'s PC length. </summary>
        public GridLength PCLength
        {
            get => (GridLength)base.GetValue(PCLengthProperty);
            set => base.SetValue(PCLengthProperty, value);
        }
        /// <summary> Identifies the <see cref = "AdaptiveLengthGrid.PCLength" /> dependency property. </summary>
        public static readonly DependencyProperty PCLengthProperty = DependencyProperty.Register(nameof(PCLength), typeof(GridLength), typeof(AdaptiveWidthGrid), new PropertyMetadata(new GridLength(1, GridUnitType.Star)));


        #endregion


        VisualStateGroup CommonStates;
        VisualState Normal;
        VisualState Disable;
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
            this.PhoneCount = phoneWidth.ToString();
            this.PadCount = padWidth.ToString();

            double phoneLength = phoneWidth;
            double padLength = padWidth - phoneWidth;
            double pcLength = pcWidth - padWidth;

            if ((this.PhoneGridLength is null) == false) this.PhoneGridLength.Width = new GridLength(phoneLength < 1 ? 1 : phoneLength, GridUnitType.Star);
            if ((this.PadGridLength is null) == false) this.PadGridLength.Width = new GridLength(padLength < 1 ? 1 : padLength, GridUnitType.Star);
            if ((this.PCGridLength is null) == false) this.PCGridLength.Width = new GridLength(pcLength < 1 ? 1 : pcLength, GridUnitType.Star);
        }


        #endregion


        #region Drag


        double _horizontal;

        int _startingPhoneWidthCore;
        int _startingPadWidthCore;

        double _startingPhoneLength;
        double _startingPadLength;
        double _startingLength;


        private void DragStarted()
        {
            this._horizontal = 0;

            this._startingPhoneWidthCore = this.PhoneWidth;
            this._startingPadWidthCore = this.PadWidth;

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
            base.IsEnabledChanged += (s, e) => this.VisualState = this.VisualState; // State
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.CommonStates = base.GetTemplateChild(nameof(CommonStates)) as VisualStateGroup;
            this.Normal = base.GetTemplateChild(nameof(Normal)) as VisualState;
            this.Disable = base.GetTemplateChild(nameof(Disable)) as VisualState;
            this.VisualState = this.VisualState; // State

            this.PhoneGridLength = base.GetTemplateChild(nameof(PhoneGridLength)) as ColumnDefinition;
            this.PadGridLength = base.GetTemplateChild(nameof(PadGridLength)) as ColumnDefinition;
            this.PCGridLength = base.GetTemplateChild(nameof(PCGridLength)) as ColumnDefinition;
            this.SetWidth();

            // Phone
            if ((this.PhoneThumb is null) == false)
            {
                this.PhoneThumb.DragStarted -= this.PhoneThumb_DragStarted;
                this.PhoneThumb.DragDelta -= this.PhoneThumb_DragDelta;
                this.PhoneThumb.DragCompleted -= this.PhoneThumb_DragCompleted;
            }
            this.PhoneThumb = base.GetTemplateChild(nameof(PhoneThumb)) as Thumb;
            if (this.PhoneThumb!=null)
            {
                this.PhoneThumb.DragStarted += this.PhoneThumb_DragStarted;
                this.PhoneThumb.DragDelta += this.PhoneThumb_DragDelta;
                this.PhoneThumb.DragCompleted += this.PhoneThumb_DragCompleted;
            }

            // Pad
            if ((this.PadThumb is null) == false)
            {
                this.PadThumb.DragStarted -= this.PadThumb_DragStarted;
                this.PadThumb.DragDelta -= this.PadThumb_DragDelta;
                this.PadThumb.DragCompleted -= this.PadThumb_DragCompleted;
            }
            this.PadThumb = base.GetTemplateChild(nameof(PadThumb)) as Thumb;
            if ((this.PadThumb is null) == false)
            {
                this.PadThumb.DragStarted += this.PadThumb_DragStarted;
                this.PadThumb.DragDelta += this.PadThumb_DragDelta;
                this.PadThumb.DragCompleted += this.PadThumb_DragCompleted;
            }
        }


        private void PhoneThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.DragStarted();
            this.ScrollModeChanged?.Invoke(this, ScrollMode.Disabled); // Delegate
        }
        private void PhoneThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double changedLength = this.DragDelta(e.HorizontalChange);
            double phoneLength = this._startingPhoneLength + changedLength;
            double scale = phoneLength / this._startingPhoneLength;

            this.PhoneWidth = (int)(this._startingPhoneWidthCore * scale);
            if (this.PhoneWidth < 300) this.PhoneWidth = 300;
            if (this.PhoneWidth > this.PadWidth - 100) this.PhoneWidth = this.PadWidth - 100;
            this.SetWidth();
        }
        private void PhoneThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.ScrollModeChanged?.Invoke(this, ScrollMode.Enabled); // Delegate
            this.PhoneWidthChanged?.Invoke(this, this.PhoneWidth); // Delegate
        }

        private void PadThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.DragStarted();
            this.ScrollModeChanged?.Invoke(this, ScrollMode.Disabled); // Delegate
        }
        private void PadThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double changedLength = this.DragDelta(e.HorizontalChange);
            double padLength = this._startingPadLength + changedLength;
            double scale = padLength / this._startingPadLength;

            this.PadWidth = (int)((this._startingPadWidthCore - this._startingPhoneWidthCore) * scale) + this._startingPhoneWidthCore;
            if (this.PadWidth < this.PhoneWidth + 100) this.PadWidth = this.PhoneWidth + 100;
            if (this.PadWidth > 1800) this.PadWidth = 1800;
            this.SetWidth();
        }
        private void PadThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.ScrollModeChanged?.Invoke(this, ScrollMode.Enabled); // Delegate
            this.PadWidthChanged?.Invoke(this, this.PadWidth); // Delegate
        }



    }
}
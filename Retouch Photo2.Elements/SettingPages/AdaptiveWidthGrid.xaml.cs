using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.SettingPages
{
    /// <summary>
    /// Grid control that display and adjust parameters for adaptive width.
    /// </summary>
    public sealed partial class AdaptiveWidthGrid : UserControl
    {
        //@Delegate  
        /// <summary> Occurs when the scroll-mode changes. </summary>
        public EventHandler<ScrollMode> ScrollModeChanged;
        /// <summary> Occurs when the phone-width value changes. </summary>
        public EventHandler<int> PhoneWidthChanged;
        /// <summary> Occurs when the pad-width value changes. </summary>
        public EventHandler<int> PadWidthChanged;


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
        public void SetWidth() => this._SetWidth(this.PhoneWidth, this.PadWidth);

        private void _SetWidth(int phoneWidth, int padWidth, int pcWidth = 2000)
        {
            this.PhoneTextBlock.Text = $"{phoneWidth}";
            this.PadTextBlock.Text = $"{padWidth}";

            double phoneLength = phoneWidth;
            double padLength = padWidth - phoneWidth;
            double pcLength = pcWidth - padWidth;

            this.PhoneGridLength.Width = new GridLength(phoneLength < 1 ? 1 : phoneLength, GridUnitType.Star);
            this.PadGridLength.Width = new GridLength(padLength < 1 ? 1 : padLength, GridUnitType.Star);
            this.PCGridLength.Width = new GridLength(pcLength < 1 ? 1 : pcLength, GridUnitType.Star);
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
        public AdaptiveWidthGrid()
        {
            this.InitializeComponent();
            base.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            base.IsEnabledChanged += (s, e) => this.VisualState = this.VisualState;//State

            //Phone
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
using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "GammaTransferAdjustment"/>.
    /// </summary>
    public sealed partial class GammaTransferPage : IAdjustmentPage
    {

        //@Content
        bool _alphaLock;
        private bool AlphaDisable
        {
            set
            {
                this._alphaLock = true;
                this.AlphaToggleSwitch.IsOn = !value;
                this._alphaLock = false;
            }
        }
        private float AlphaOffset
        {
            set
            {
                this.AlphaOffsetPicker.Value = (int)(value * 100.0f);
                this.AlphaOffsetSlider.Value = value;
            }
        }
        private float AlphaExponent
        {
            set
            {
                this.AlphaExponentPicker.Value = (int)(value * 100.0f);
                this.AlphaExponentSlider.Value = value;
            }
        }
        private float AlphaAmplitude
        {
            set
            {
                this.AlphaAmplitudePicker.Value = (int)(value * 100.0f);
                this.AlphaAmplitudeSlider.Value = value;
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s alpha visibility. </summary>
        public Visibility AlphaIsExpaned
        {
            get { return (Visibility)GetValue(AlphaIsExpanedProperty); }
            set { SetValue(AlphaIsExpanedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.AlphaIsExpaned" /> dependency property. </summary>
        public static readonly DependencyProperty AlphaIsExpanedProperty = DependencyProperty.Register(nameof(AlphaIsExpaned), typeof(Visibility), typeof(GammaTransferPage), new PropertyMetadata(Visibility.Collapsed));


        #endregion


        private void ResetAlpha()
        {
            this.AlphaDisable = false;
            this.AlphaOffset = 0.0f;
            this.AlphaExponent = 1.0f;
            this.AlphaAmplitude = 1.0f;
        }
        private void FollowAlpha(GammaTransferAdjustment adjustment)
        {
            this.AlphaDisable = adjustment.AlphaDisable;
            this.AlphaOffset = adjustment.AlphaOffset;
            this.AlphaExponent = adjustment.AlphaExponent;
            this.AlphaAmplitude = adjustment.AlphaAmplitude;
        }

        private void ConstructStringsAlpha(string title, string offset, string exponent, string amplitude)
        {
            this.AlphaTextBlock.Text = title;
            this.AlphaOffsetTextBlock.Text = offset;
            this.AlphaExponentTextBlock.Text = exponent;
            this.AlphaAmplitudeTextBlock.Text = amplitude;
        }


        //AlphaDisable
        private void ConstructAlphaDisable()
        {
            this.AlphaTitleGrid.Tapped += (s, e) => this.AlphaIsExpaned = this.AlphaIsExpaned == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            this.AlphaToggleSwitch.Toggled += (s, e) =>
            {
                if (this._alphaLock) return;
                bool alphaDisable = !this.AlphaToggleSwitch.IsOn;
                //this.AlphaDisable = alphaDisable;

                this.MethodViewModel.TAdjustmentChanged<bool, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.AlphaDisable = alphaDisable,

                    historyTitle: "Set gamma transfer adjustment alpha disable",
                    getHistory: (tAdjustment) => tAdjustment.AlphaDisable,
                    setHistory: (tAdjustment, previous) => tAdjustment.AlphaDisable = previous
                );
            };
        }


        //AlphaOffset
        private void ConstructAlphaOffset1()
        {
            this.AlphaOffsetPicker.Unit = "%";
            this.AlphaOffsetPicker.Minimum = 0;
            this.AlphaOffsetPicker.Maximum = 100;
            this.AlphaOffsetPicker.ValueChanged += (s, value) =>
            {
                float alphaOffset = (float)value / 100.0f;
                this.AlphaOffset = alphaOffset;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.AlphaOffset = alphaOffset,

                    historyTitle: "Set gamma transfer adjustment alpha offset",
                    getHistory: (tAdjustment) => tAdjustment.AlphaOffset,
                    setHistory: (tAdjustment, previous) => tAdjustment.AlphaOffset = previous
                );
            };
        }

        private void ConstructAlphaOffset2()
        {
            this.AlphaOffsetSlider.SliderBrush = this.AlphaLeftBrush;
            this.AlphaOffsetSlider.Minimum = 0.0d;
            this.AlphaOffsetSlider.Maximum = 1.0d;
            this.AlphaOffsetSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheAlphaOffset());
            this.AlphaOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                float alphaOffset = (float)value;
                this.AlphaOffset = alphaOffset;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.AlphaOffset = alphaOffset);
            };
            this.AlphaOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                float alphaOffset = (float)value;
                this.AlphaOffset = alphaOffset;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.AlphaOffset = alphaOffset,

                    historyTitle: "Set gamma transfer adjustment alpha offset",
                    getHistory: (tAdjustment) => tAdjustment.StartingAlphaOffset,
                    setHistory: (tAdjustment, previous) => tAdjustment.AlphaOffset = previous
                );
            };
        }


        //AlphaExponent
        private void ConstructAlphaExponent1()
        {
            this.AlphaExponentPicker.Unit = "%";
            this.AlphaExponentPicker.Minimum = 0;
            this.AlphaExponentPicker.Maximum = 100;
            this.AlphaExponentPicker.ValueChanged += (s, value) =>
            {
                float alphaExponent = (float)value / 100.0f;
                this.AlphaExponent = alphaExponent;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.AlphaExponent = alphaExponent,

                    historyTitle: "Set gamma transfer adjustment alpha exponent",
                    getHistory: (tAdjustment) => tAdjustment.AlphaExponent,
                    setHistory: (tAdjustment, previous) => tAdjustment.AlphaExponent = previous
                );
            };
        }

        private void ConstructAlphaExponent2()
        {
            this.AlphaExponentSlider.SliderBrush = this.AlphaLeftBrush;
            this.AlphaExponentSlider.Minimum = 0.0d;
            this.AlphaExponentSlider.Maximum = 1.0d;
            this.AlphaExponentSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheAlphaExponent());
            this.AlphaExponentSlider.ValueChangeDelta += (s, value) =>
            {
                float alphaExponent = (float)value;
                this.AlphaExponent = alphaExponent;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.AlphaExponent = alphaExponent);
            };
            this.AlphaExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                float alphaExponent = (float)value;
                this.AlphaExponent = alphaExponent;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.AlphaExponent = alphaExponent,

                    historyTitle: "Set gamma transfer adjustment alpha exponent",
                    getHistory: (tAdjustment) => tAdjustment.StartingAlphaExponent,
                    setHistory: (tAdjustment, previous) => tAdjustment.AlphaExponent = previous
                );
            };
        }


        //AlphaAmplitude
        private void ConstructAlphaAmplitude1()
        {
            this.AlphaAmplitudePicker.Unit = "%";
            this.AlphaAmplitudePicker.Minimum = 0;
            this.AlphaAmplitudePicker.Maximum = 100;
            this.AlphaAmplitudePicker.ValueChanged += (s, value) =>
            {
                float alphaAmplitude = (float)value / 100.0f;
                this.AlphaAmplitude = alphaAmplitude;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.AlphaAmplitude = alphaAmplitude,

                    historyTitle: "Set gamma transfer adjustment alpha amplitude",
                    getHistory: (tAdjustment) => tAdjustment.AlphaAmplitude,
                    setHistory: (tAdjustment, previous) => tAdjustment.AlphaAmplitude = previous
                );
            };
        }

        private void ConstructAlphaAmplitude2()
        {
            this.AlphaAmplitudeSlider.SliderBrush = this.AlphaLeftBrush;
            this.AlphaAmplitudeSlider.Minimum = 0.0d;
            this.AlphaAmplitudeSlider.Maximum = 1.0d;
            this.AlphaAmplitudeSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheAlphaAmplitude());
            this.AlphaAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                float alphaAmplitude = (float)value;
                this.AlphaAmplitude = alphaAmplitude;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.AlphaAmplitude = alphaAmplitude);
            };
            this.AlphaAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                float alphaAmplitude = (float)value;
                this.AlphaAmplitude = alphaAmplitude;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.AlphaAmplitude = alphaAmplitude,

                    historyTitle: "Set gamma transfer adjustment alpha amplitude",
                    getHistory: (tAdjustment) => tAdjustment.StartingAlphaAmplitude,
                    setHistory: (tAdjustment, previous) => tAdjustment.AlphaAmplitude = previous
                );
            };
        }

    }
}
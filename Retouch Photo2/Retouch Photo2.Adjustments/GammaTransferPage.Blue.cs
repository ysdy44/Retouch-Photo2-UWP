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
        bool _blueLock;
        private bool BlueDisable
        {
            set
            {
                this._blueLock = true;
                this.BlueToggleSwitch.IsOn = !value;
                this._blueLock = false;
            }
        }
        private float BlueOffset
        {
            set
            {
                this.BlueOffsetPicker.Value = (int)(value * 100.0f);
                this.BlueOffsetSlider.Value = value;
            }
        }
        private float BlueExponent
        {
            set
            {
                this.BlueExponentPicker.Value = (int)(value * 100.0f);
                this.BlueExponentSlider.Value = value;
            }
        }
        private float BlueAmplitude
        {
            set
            {
                this.BlueAmplitudePicker.Value = (int)(value * 100.0f);
                this.BlueAmplitudeSlider.Value = value;
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s blue visibility. </summary>
        public Visibility BlueIsExpaned
        {
            get => (Visibility)base.GetValue(BlueIsExpanedProperty);
            set => base.SetValue(BlueIsExpanedProperty, value);
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.BlueIsExpaned" /> dependency property. </summary>
        public static readonly DependencyProperty BlueIsExpanedProperty = DependencyProperty.Register(nameof(BlueIsExpaned), typeof(Visibility), typeof(GammaTransferPage), new PropertyMetadata(Visibility.Collapsed));


        #endregion


        private void ResetBlue()
        {
            this.BlueDisable = false;
            this.BlueOffset = 0.0f;
            this.BlueExponent = 1.0f;
            this.BlueAmplitude = 1.0f;
        }
        private void FollowBlue(GammaTransferAdjustment adjustment)
        {
            this.BlueDisable = adjustment.BlueDisable;
            this.BlueOffset = adjustment.BlueOffset;
            this.BlueExponent = adjustment.BlueExponent;
            this.BlueAmplitude = adjustment.BlueAmplitude;
        }

        private void ConstructStringsBlue(string title, string offset, string exponent, string amplitude)
        {
            this.BlueTextBlock.Text = title;
            this.BlueOffsetTextBlock.Text = offset;
            this.BlueExponentTextBlock.Text = exponent;
            this.BlueAmplitudeTextBlock.Text = amplitude;
        }


        //BlueDisable
        private void ConstructBlueDisable()
        {
            this.BlueTitleGrid.Tapped += (s, e) => this.BlueIsExpaned = this.BlueIsExpaned == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            this.BlueToggleSwitch.Toggled += (s, e) =>
            {
                if (this._blueLock) return;
                bool blueDisable = !this.BlueToggleSwitch.IsOn;
                //this.BlueDisable = blueDisable;

                this.MethodViewModel.TAdjustmentChanged<bool, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueDisable = blueDisable,

                    historyTitle: "Set gamma transfer adjustment blue disable",
                    getHistory: (tAdjustment) => tAdjustment.BlueDisable,
                    setHistory: (tAdjustment, previous) => tAdjustment.BlueDisable = previous
                );
            };
        }


        //BlueOffset
        private void ConstructBlueOffset1()
        {
            this.BlueOffsetPicker.Unit = "%";
            this.BlueOffsetPicker.Minimum = 0;
            this.BlueOffsetPicker.Maximum = 100;
            this.BlueOffsetPicker.ValueChanged += (s, value) =>
            {
                float blueOffset = (float)value / 100.0f;
                this.BlueOffset = blueOffset;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueOffset = blueOffset,

                    historyTitle: "Set gamma transfer adjustment blue offset",
                    getHistory: (tAdjustment) => tAdjustment.BlueOffset,
                    setHistory: (tAdjustment, previous) => tAdjustment.BlueOffset = previous
                );
            };
        }

        private void ConstructBlueOffset2()
        {
            this.BlueOffsetSlider.SliderBrush = this.BlueLeftBrush;
            this.BlueOffsetSlider.Minimum = 0.0d;
            this.BlueOffsetSlider.Maximum = 1.0d;
            this.BlueOffsetSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheBlueOffset());
            this.BlueOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                float blueOffset = (float)value;
                this.BlueOffset = blueOffset;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.BlueOffset = blueOffset);
            };
            this.BlueOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                float blueOffset = (float)value;
                this.BlueOffset = blueOffset;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueOffset = blueOffset,

                    historyTitle: "Set gamma transfer adjustment blue offset",
                    getHistory: (tAdjustment) => tAdjustment.StartingBlueOffset,
                    setHistory: (tAdjustment, previous) => tAdjustment.BlueOffset = previous
                );
            };
        }


        //BlueExponent
        private void ConstructBlueExponent1()
        {
            this.BlueExponentPicker.Unit = "%";
            this.BlueExponentPicker.Minimum = 0;
            this.BlueExponentPicker.Maximum = 100;
            this.BlueExponentPicker.ValueChanged += (s, value) =>
            {
                float blueExponent = (float)value / 100.0f;
                this.BlueExponent = blueExponent;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueExponent = blueExponent,

                    historyTitle: "Set gamma transfer adjustment blue exponent",
                    getHistory: (tAdjustment) => tAdjustment.BlueExponent,
                    setHistory: (tAdjustment, previous) => tAdjustment.BlueExponent = previous
                );
            };
        }

        private void ConstructBlueExponent2()
        {
            this.BlueExponentSlider.SliderBrush = this.BlueLeftBrush;
            this.BlueExponentSlider.Minimum = 0.0d;
            this.BlueExponentSlider.Maximum = 1.0d;
            this.BlueExponentSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheBlueExponent());
            this.BlueExponentSlider.ValueChangeDelta += (s, value) =>
            {
                float blueExponent = (float)value;
                this.BlueExponent = blueExponent;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.BlueExponent = blueExponent);
            };
            this.BlueExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                float blueExponent = (float)value;
                this.BlueExponent = blueExponent;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueExponent = blueExponent,

                    historyTitle: "Set gamma transfer adjustment blue exponent",
                    getHistory: (tAdjustment) => tAdjustment.StartingBlueExponent,
                    setHistory: (tAdjustment, previous) => tAdjustment.BlueExponent = previous
                );
            };
        }


        //BlueAmplitude
        private void ConstructBlueAmplitude1()
        {
            this.BlueAmplitudePicker.Unit = "%";
            this.BlueAmplitudePicker.Minimum = 0;
            this.BlueAmplitudePicker.Maximum = 100;
            this.BlueAmplitudePicker.ValueChanged += (s, value) =>
            {
                float blueAmplitude = (float)value / 100.0f;
                this.BlueAmplitude = blueAmplitude;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueAmplitude = blueAmplitude,

                    historyTitle: "Set gamma transfer adjustment blue amplitude",
                    getHistory: (tAdjustment) => tAdjustment.BlueAmplitude,
                    setHistory: (tAdjustment, previous) => tAdjustment.BlueAmplitude = previous
                );
            };
        }

        private void ConstructBlueAmplitude2()
        {
            this.BlueAmplitudeSlider.SliderBrush = this.BlueLeftBrush;
            this.BlueAmplitudeSlider.Minimum = 0.0d;
            this.BlueAmplitudeSlider.Maximum = 1.0d;
            this.BlueAmplitudeSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheBlueAmplitude());
            this.BlueAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                float blueAmplitude = (float)value;
                this.BlueAmplitude = blueAmplitude;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.BlueAmplitude = blueAmplitude);
            };
            this.BlueAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                float blueAmplitude = (float)value;
                this.BlueAmplitude = blueAmplitude;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.BlueAmplitude = blueAmplitude,

                    historyTitle: "Set gamma transfer adjustment blue amplitude",
                    getHistory: (tAdjustment) => tAdjustment.StartingBlueAmplitude,
                    setHistory: (tAdjustment, previous) => tAdjustment.BlueAmplitude = previous
                );
            };
        }

    }
}
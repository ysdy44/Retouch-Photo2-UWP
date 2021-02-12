using Retouch_Photo2.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    public sealed partial class GammaTransferPage : IAdjustmentPage
    {

        //@Content
        bool _greenLock;
        private bool GreenDisable
        {
            set
            {
                this._greenLock = true;
                this.GreenToggleSwitch.IsOn = !value;
                this._greenLock = false;
            }
        }
        private float GreenOffset
        {
            set
            {
                this.GreenOffsetPicker.Value = (int)(value * 100.0f);
                this.GreenOffsetSlider.Value = value;
            }
        }
        private float GreenExponent
        {
            set
            {
                this.GreenExponentPicker.Value = (int)(value * 100.0f);
                this.GreenExponentSlider.Value = value;
            }
        }
        private float GreenAmplitude
        {
            set
            {
                this.GreenAmplitudePicker.Value = (int)(value * 100.0f);
                this.GreenAmplitudeSlider.Value = value;
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s green visibility. </summary>
        public Visibility GreenIsExpaned
        {
            get => (Visibility)base.GetValue(GreenIsExpanedProperty);
            set => base.SetValue(GreenIsExpanedProperty, value);
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.GreenIsExpaned" /> dependency property. </summary>
        public static readonly DependencyProperty GreenIsExpanedProperty = DependencyProperty.Register(nameof(GreenIsExpaned), typeof(Visibility), typeof(GammaTransferPage), new PropertyMetadata(Visibility.Collapsed));


        #endregion


        private void ResetGreen()
        {
            this.GreenDisable = false;
            this.GreenOffset = 0.0f;
            this.GreenExponent = 1.0f;
            this.GreenAmplitude = 1.0f;
        }
        private void FollowGreen(GammaTransferAdjustment adjustment)
        {
            this.GreenDisable = adjustment.GreenDisable;
            this.GreenOffset = adjustment.GreenOffset;
            this.GreenExponent = adjustment.GreenExponent;
            this.GreenAmplitude = adjustment.GreenAmplitude;
        }

        private void ConstructStringsGreen(string title, string offset, string exponent, string amplitude)
        {
            this.GreenTextBlock.Text = title;
            this.GreenOffsetTextBlock.Text = offset;
            this.GreenExponentTextBlock.Text = exponent;
            this.GreenAmplitudeTextBlock.Text = amplitude;
        }


        //GreenDisable
        private void ConstructGreenDisable()
        {
            this.GreenTitleGrid.Tapped += (s, e) => this.GreenIsExpaned = this.GreenIsExpaned == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            this.GreenToggleSwitch.Toggled += (s, e) =>
            {
                if (this._greenLock) return;
                bool greenDisable = !this.GreenToggleSwitch.IsOn;
                //this.GreenDisable = greenDisable;

                this.MethodViewModel.TAdjustmentChanged<bool, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.GreenDisable = greenDisable,

                    historyTitle: "Set gamma transfer adjustment green disable",
                    getHistory: (tAdjustment) => tAdjustment.GreenDisable,
                    setHistory: (tAdjustment, previous) => tAdjustment.GreenDisable = previous
                );
            };
        }


        //GreenOffset
        private void ConstructGreenOffset1()
        {
            this.GreenOffsetPicker.Unit = "%";
            this.GreenOffsetPicker.Minimum = 0;
            this.GreenOffsetPicker.Maximum = 100;
            this.GreenOffsetPicker.ValueChanged += (s, value) =>
            {
                float greenOffset = (float)value / 100.0f;
                this.GreenOffset = greenOffset;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.GreenOffset = greenOffset,

                    historyTitle: "Set gamma transfer adjustment green offset",
                    getHistory: (tAdjustment) => tAdjustment.GreenOffset,
                    setHistory: (tAdjustment, previous) => tAdjustment.GreenOffset = previous
                );
            };
        }

        private void ConstructGreenOffset2()
        {
            this.GreenOffsetSlider.SliderBrush = this.GreenLeftBrush;
            this.GreenOffsetSlider.Minimum = 0.0d;
            this.GreenOffsetSlider.Maximum = 1.0d;
            this.GreenOffsetSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheGreenOffset());
            this.GreenOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                float greenOffset = (float)value;
                this.GreenOffset = greenOffset;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.GreenOffset = greenOffset);
            };
            this.GreenOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                float greenOffset = (float)value;
                this.GreenOffset = greenOffset;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.GreenOffset = greenOffset,

                    historyTitle: "Set gamma transfer adjustment green offset",
                    getHistory: (tAdjustment) => tAdjustment.StartingGreenOffset,
                    setHistory: (tAdjustment, previous) => tAdjustment.GreenOffset = previous
                );
            };
        }


        //GreenExponent
        private void ConstructGreenExponent1()
        {
            this.GreenExponentPicker.Unit = "%";
            this.GreenExponentPicker.Minimum = 0;
            this.GreenExponentPicker.Maximum = 100;
            this.GreenExponentPicker.ValueChanged += (s, value) =>
            {
                float greenExponent = (float)value / 100.0f;
                this.GreenExponent = greenExponent;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.GreenExponent = greenExponent,

                    historyTitle: "Set gamma transfer adjustment green exponent",
                    getHistory: (tAdjustment) => tAdjustment.GreenExponent,
                    setHistory: (tAdjustment, previous) => tAdjustment.GreenExponent = previous
                );
            };
        }

        private void ConstructGreenExponent2()
        {
            this.GreenExponentSlider.SliderBrush = this.GreenLeftBrush;
            this.GreenExponentSlider.Minimum = 0.0d;
            this.GreenExponentSlider.Maximum = 1.0d;
            this.GreenExponentSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheGreenExponent());
            this.GreenExponentSlider.ValueChangeDelta += (s, value) =>
            {
                float greenExponent = (float)value;
                this.GreenExponent = greenExponent;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.GreenExponent = greenExponent);
            };
            this.GreenExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                float greenExponent = (float)value;
                this.GreenExponent = greenExponent;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.GreenExponent = greenExponent,

                    historyTitle: "Set gamma transfer adjustment green exponent",
                    getHistory: (tAdjustment) => tAdjustment.StartingGreenExponent,
                    setHistory: (tAdjustment, previous) => tAdjustment.GreenExponent = previous
                );
            };
        }


        //GreenAmplitude
        private void ConstructGreenAmplitude1()
        {
            this.GreenAmplitudePicker.Unit = "%";
            this.GreenAmplitudePicker.Minimum = 0;
            this.GreenAmplitudePicker.Maximum = 100;
            this.GreenAmplitudePicker.ValueChanged += (s, value) =>
            {
                float greenAmplitude = (float)value / 100.0f;
                this.GreenAmplitude = greenAmplitude;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.GreenAmplitude = greenAmplitude,

                    historyTitle: "Set gamma transfer adjustment green amplitude",
                    getHistory: (tAdjustment) => tAdjustment.GreenAmplitude,
                    setHistory: (tAdjustment, previous) => tAdjustment.GreenAmplitude = previous
                );
            };
        }

        private void ConstructGreenAmplitude2()
        {
            this.GreenAmplitudeSlider.SliderBrush = this.GreenLeftBrush;
            this.GreenAmplitudeSlider.Minimum = 0.0d;
            this.GreenAmplitudeSlider.Maximum = 1.0d;
            this.GreenAmplitudeSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheGreenAmplitude());
            this.GreenAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                float greenAmplitude = (float)value;
                this.GreenAmplitude = greenAmplitude;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.GreenAmplitude = greenAmplitude);
            };
            this.GreenAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                float greenAmplitude = (float)value;
                this.GreenAmplitude = greenAmplitude;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.GreenAmplitude = greenAmplitude,

                    historyTitle: "Set gamma transfer adjustment green amplitude",
                    getHistory: (tAdjustment) => tAdjustment.StartingGreenAmplitude,
                    setHistory: (tAdjustment, previous) => tAdjustment.GreenAmplitude = previous
                );
            };
        }

    }
}
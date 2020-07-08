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
        bool _redLock;
        private bool RedDisable
        {
            set
            {
                this._redLock = true;
                this.RedToggleSwitch.IsOn = !value;
                this._redLock = false;
            }
        }
        private float RedOffset
        {
            set
            {
                this.RedOffsetPicker.Value = (int)(value * 100.0f);
                this.RedOffsetSlider.Value = value;
            }
        }
        private float RedExponent
        {
            set
            {
                this.RedExponentPicker.Value = (int)(value * 100.0f);
                this.RedExponentSlider.Value = value;
            }
        }
        private float RedAmplitude
        {
            set
            {
                this.RedAmplitudePicker.Value = (int)(value * 100.0f);
                this.RedAmplitudeSlider.Value = value;
            }
        }


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s red visibility. </summary>
        public Visibility RedIsExpaned
        {
            get { return (Visibility)GetValue(RedIsExpanedProperty); }
            set { SetValue(RedIsExpanedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.RedIsExpaned" /> dependency property. </summary>
        public static readonly DependencyProperty RedIsExpanedProperty = DependencyProperty.Register(nameof(RedIsExpaned), typeof(Visibility), typeof(GammaTransferPage), new PropertyMetadata(Visibility.Collapsed));


        #endregion


        private void ResetRed()
        {
            this.RedDisable = false;
            this.RedOffset = 0.0f;
            this.RedExponent = 1.0f;
            this.RedAmplitude = 1.0f;
        }
        private void FollowRed(GammaTransferAdjustment adjustment)
        {
            this.RedDisable = adjustment.RedDisable;
            this.RedOffset = adjustment.RedOffset;
            this.RedExponent = adjustment.RedExponent;
            this.RedAmplitude = adjustment.RedAmplitude;
        }

        private void ConstructStringsRed(string title, string offset, string exponent, string amplitude)
        {
            this.RedTextBlock.Text = title;
            this.RedOffsetTextBlock.Text = offset;
            this.RedExponentTextBlock.Text = exponent;
            this.RedAmplitudeTextBlock.Text = amplitude;
        }


        //RedDisable
        private void ConstructRedDisable()
        {
            this.RedTitleGrid.Tapped += (s, e) => this.RedIsExpaned = this.RedIsExpaned == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            this.RedToggleSwitch.Toggled += (s, e) =>
            {
                if (this._redLock) return;
                bool redDisable = !this.RedToggleSwitch.IsOn;
                //this.RedDisable = redDisable;

                this.MethodViewModel.TAdjustmentChanged<bool, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedDisable = redDisable,

                    historyTitle: "Set gamma transfer adjustment red disable",
                    getHistory: (tAdjustment) => tAdjustment.RedDisable,
                    setHistory: (tAdjustment, previous) => tAdjustment.RedDisable = previous
                );
            };
        }


        //RedOffset
        private void ConstructRedOffset1()
        {
            this.RedOffsetPicker.Unit = null;
            this.RedOffsetPicker.Minimum = 0;
            this.RedOffsetPicker.Maximum = 100;
            this.RedOffsetPicker.ValueChange += (s, value) =>
            {
                float redOffset = (float)value / 100.0f;
                this.RedOffset = redOffset;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedOffset = redOffset,

                    historyTitle: "Set gamma transfer adjustment red offset",
                    getHistory: (tAdjustment) => tAdjustment.RedOffset,
                    setHistory: (tAdjustment, previous) => tAdjustment.RedOffset = previous
                );
            };
        }

        private void ConstructRedOffset2()
        {
            this.RedOffsetSlider.Minimum = 0.0d;
            this.RedOffsetSlider.Maximum = 1.0d;
            this.RedOffsetSlider.SliderBrush = this.RedLeftBrush;
            this.RedOffsetSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheRedOffset());
            this.RedOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                float redOffset = (float)value;
                this.RedOffset = redOffset;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.RedOffset = redOffset);
            };
            this.RedOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                float redOffset = (float)value;
                this.RedOffset = redOffset;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedOffset = redOffset,

                    historyTitle: "Set gamma transfer adjustment red offset",
                    getHistory: (tAdjustment) => tAdjustment.StartingRedOffset,
                    setHistory: (tAdjustment, previous) => tAdjustment.RedOffset = previous
                );
            };
        }


        //RedExponent
        private void ConstructRedExponent1()
        {
            this.RedExponentPicker.Unit = null;
            this.RedExponentPicker.Minimum = 0;
            this.RedExponentPicker.Maximum = 100;
            this.RedExponentPicker.ValueChange += (s, value) =>
            {
                float redExponent = (float)value / 100.0f;
                this.RedExponent = redExponent;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedExponent = redExponent,

                    historyTitle: "Set gamma transfer adjustment red exponent",
                    getHistory: (tAdjustment) => tAdjustment.RedExponent,
                    setHistory: (tAdjustment, previous) => tAdjustment.RedExponent = previous
                );
            };
        }

        private void ConstructRedExponent2()
        {
            this.RedExponentSlider.Minimum = 0.0d;
            this.RedExponentSlider.Maximum = 1.0d;
            this.RedExponentSlider.SliderBrush = this.RedLeftBrush;
            this.RedExponentSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheRedExponent());
            this.RedExponentSlider.ValueChangeDelta += (s, value) =>
            {
                float redExponent = (float)value;
                this.RedExponent = redExponent;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.RedExponent = redExponent);
            };
            this.RedExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                float redExponent = (float)value;
                this.RedExponent = redExponent;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedExponent = redExponent,

                    historyTitle: "Set gamma transfer adjustment red exponent",
                    getHistory: (tAdjustment) => tAdjustment.StartingRedExponent,
                    setHistory: (tAdjustment, previous) => tAdjustment.RedExponent = previous
                );
            };
        }


        //RedAmplitude
        private void ConstructRedAmplitude1()
        {
            this.RedAmplitudePicker.Unit = null;
            this.RedAmplitudePicker.Minimum = 0;
            this.RedAmplitudePicker.Maximum = 100;
            this.RedAmplitudePicker.ValueChange += (s, value) =>
            {
                float redAmplitude = (float)value / 100.0f;
                this.RedAmplitude = redAmplitude;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedAmplitude = redAmplitude,

                    historyTitle: "Set gamma transfer adjustment red amplitude",
                    getHistory: (tAdjustment) => tAdjustment.RedAmplitude,
                    setHistory: (tAdjustment, previous) => tAdjustment.RedAmplitude = previous
                );
            };
        }

        private void ConstructRedAmplitude2()
        {
            this.RedAmplitudeSlider.Minimum = 0.0d;
            this.RedAmplitudeSlider.Maximum = 1.0d;
            this.RedAmplitudeSlider.SliderBrush = this.RedLeftBrush;
            this.RedAmplitudeSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.TAdjustmentChangeStarted<GammaTransferAdjustment>(index: this.Index, cache: (tAdjustment) => tAdjustment.CacheRedAmplitude());
            this.RedAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                float redAmplitude = (float)value;
                this.RedAmplitude = redAmplitude;

                this.MethodViewModel.TAdjustmentChangeDelta<GammaTransferAdjustment>(index: this.Index, set: (tAdjustment) => tAdjustment.RedAmplitude = redAmplitude);
            };
            this.RedAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                float redAmplitude = (float)value;
                this.RedAmplitude = redAmplitude;

                this.MethodViewModel.TAdjustmentChangeCompleted<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedAmplitude = redAmplitude,

                    historyTitle: "Set gamma transfer adjustment red amplitude",
                    getHistory: (tAdjustment) => tAdjustment.StartingRedAmplitude,
                    setHistory: (tAdjustment, previous) => tAdjustment.RedAmplitude = previous
                );
            };
        }

    }
}
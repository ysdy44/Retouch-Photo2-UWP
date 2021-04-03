using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    public sealed partial class GammaTransferPage : IAdjustmentPage
    {

        //@Content
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


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s red IsEnabled. </summary>
        public bool RedIsEnabled
        {
            get => (bool)base.GetValue(RedIsEnabledProperty);
            set => base.SetValue(RedIsEnabledProperty, value);
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.RedIsEnabled" /> dependency property. </summary>
        public static readonly DependencyProperty RedIsEnabledProperty = DependencyProperty.Register(nameof(RedIsEnabled), typeof(bool), typeof(GammaTransferPage), new PropertyMetadata(false));


        #endregion


        private void ResetRed()
        {
            this.RedIsEnabled = true;
            this.RedOffset = 0.0f;
            this.RedExponent = 1.0f;
            this.RedAmplitude = 1.0f;
        }
        private void FollowRed(GammaTransferAdjustment adjustment)
        {
            this.RedIsEnabled = !adjustment.RedDisable;
            this.RedOffset = adjustment.RedOffset;
            this.RedExponent = adjustment.RedExponent;
            this.RedAmplitude = adjustment.RedAmplitude;
        }

        private void ConstructStringsRed(string title, string offset, string exponent, string amplitude)
        {
            this.RedCheckControl.Content = title;
            this.RedOffsetTextBlock.Text = offset;
            this.RedExponentTextBlock.Text = exponent;
            this.RedAmplitudeTextBlock.Text = amplitude;
        }


        //RedDisable
        private void ConstructRedDisable()
        {
            this.RedCheckControl.Tapped += (s, e) =>
            {
                bool disable = this.RedIsEnabled;
                this.RedIsEnabled = !disable;
                
                this.MethodViewModel.TAdjustmentChanged<bool, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedDisable = disable,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_RedDisable,
                    getUndo: (tAdjustment) => tAdjustment.RedDisable,
                    setUndo: (tAdjustment, previous) => tAdjustment.RedDisable = previous
                );
            };
        }


        //RedOffset
        private void ConstructRedOffset1()
        {
            this.RedOffsetPicker.Unit = "%";
            this.RedOffsetPicker.Minimum = 0;
            this.RedOffsetPicker.Maximum = 100;
            this.RedOffsetPicker.ValueChanged += (s, value) =>
            {
                float redOffset = (float)value / 100.0f;
                this.RedOffset = redOffset;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedOffset = redOffset,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_RedOffset,
                    getUndo: (tAdjustment) => tAdjustment.RedOffset,
                    setUndo: (tAdjustment, previous) => tAdjustment.RedOffset = previous
                );
            };
        }

        private void ConstructRedOffset2()
        {
            this.RedOffsetSlider.SliderBrush = this.RedLeftBrush;
            this.RedOffsetSlider.Minimum = 0.0d;
            this.RedOffsetSlider.Maximum = 1.0d;
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

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_RedOffset,
                    getUndo: (tAdjustment) => tAdjustment.StartingRedOffset,
                    setUndo: (tAdjustment, previous) => tAdjustment.RedOffset = previous
                );
            };
        }


        //RedExponent
        private void ConstructRedExponent1()
        {
            this.RedExponentPicker.Unit = "%";
            this.RedExponentPicker.Minimum = 0;
            this.RedExponentPicker.Maximum = 100;
            this.RedExponentPicker.ValueChanged += (s, value) =>
            {
                float redExponent = (float)value / 100.0f;
                this.RedExponent = redExponent;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedExponent = redExponent,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_RedExponent,
                    getUndo: (tAdjustment) => tAdjustment.RedExponent,
                    setUndo: (tAdjustment, previous) => tAdjustment.RedExponent = previous
                );
            };
        }

        private void ConstructRedExponent2()
        {
            this.RedExponentSlider.SliderBrush = this.RedLeftBrush;
            this.RedExponentSlider.Minimum = 0.0d;
            this.RedExponentSlider.Maximum = 1.0d;
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

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_RedExponent,
                    getUndo: (tAdjustment) => tAdjustment.StartingRedExponent,
                    setUndo: (tAdjustment, previous) => tAdjustment.RedExponent = previous
                );
            };
        }


        //RedAmplitude
        private void ConstructRedAmplitude1()
        {
            this.RedAmplitudePicker.Unit = "%";
            this.RedAmplitudePicker.Minimum = 0;
            this.RedAmplitudePicker.Maximum = 100;
            this.RedAmplitudePicker.ValueChanged += (s, value) =>
            {
                float redAmplitude = (float)value / 100.0f;
                this.RedAmplitude = redAmplitude;

                this.MethodViewModel.TAdjustmentChanged<float, GammaTransferAdjustment>
                (
                    index: this.Index,
                    set: (tAdjustment) => tAdjustment.RedAmplitude = redAmplitude,

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_RedAmplitude,
                    getUndo: (tAdjustment) => tAdjustment.RedAmplitude,
                    setUndo: (tAdjustment, previous) => tAdjustment.RedAmplitude = previous
                );
            };
        }

        private void ConstructRedAmplitude2()
        {
            this.RedAmplitudeSlider.SliderBrush = this.RedLeftBrush;
            this.RedAmplitudeSlider.Minimum = 0.0d;
            this.RedAmplitudeSlider.Maximum = 1.0d;
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

                    type: HistoryType.LayersProperty_SetAdjustment_GammaTransfer_RedAmplitude,
                    getUndo: (tAdjustment) => tAdjustment.StartingRedAmplitude,
                    setUndo: (tAdjustment, previous) => tAdjustment.RedAmplitude = previous
                );
            };
        }

    }
}
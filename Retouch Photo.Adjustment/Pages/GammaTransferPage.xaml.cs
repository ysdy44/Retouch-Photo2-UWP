using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo.Adjustments.Pages
{
    public enum GammaTransferPageState
    {
        Alpha,
        Red,
        Green,
        Blue
    }

    public sealed partial class GammaTransferPage : AdjustmentPage
    {

        public GammaTransferAdjustment GammaTransferAdjustment;
        
        //State
        public GammaTransferPageState State
        {
            set
            {
                //Alpha
                bool alpha = (value == GammaTransferPageState.Alpha);
                this.AlphaSegmented.Background = alpha ? this.AccentColor : this.UnAccentColor;
                this.AlphaCheckBox.Visibility = this.AlphaOffsetTextBlock.Visibility = this.AlphaOffsetSlider.Visibility = this.AlphaExponentTextBlock.Visibility = this.AlphaExponentSlider.Visibility = this.AlphaAmplitudeTextBlock.Visibility = this.AlphaAmplitudeSlider.Visibility = alpha ? Visibility.Visible : Visibility.Collapsed;
                if (alpha)
                {
                    this.AlphaOffsetSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaOffset * 100.0f;
                    this.AlphaExponentSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaExponent * 100.0f;
                    this.AlphaAmplitudeSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaAmplitude * 100.0f;
                }

                //Red
                bool red = (value == GammaTransferPageState.Red);
                this.RedSegmented.Background = red ? this.AccentColor : this.UnAccentColor;
                this.RedCheckBox.Visibility = this.RedOffsetTextBlock.Visibility = this.RedOffsetSlider.Visibility = this.RedExponentTextBlock.Visibility = this.RedExponentSlider.Visibility = this.RedAmplitudeTextBlock.Visibility = this.RedAmplitudeSlider.Visibility = red ? Visibility.Visible : Visibility.Collapsed;
                if (red)
                {
                    this.RedOffsetSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedOffset * 100.0f;
                    this.RedExponentSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedExponent * 100.0f;
                    this.RedAmplitudeSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedAmplitude * 100.0f;
                }

                //Green
                bool green = (value == GammaTransferPageState.Green);
                this.GreenSegmented.Background = green ? this.AccentColor : this.UnAccentColor;
                this.GreenCheckBox.Visibility = this.GreenOffsetTextBlock.Visibility = this.GreenOffsetSlider.Visibility = this.GreenExponentTextBlock.Visibility = this.GreenExponentSlider.Visibility = this.GreenAmplitudeTextBlock.Visibility = this.GreenAmplitudeSlider.Visibility = green ? Visibility.Visible : Visibility.Collapsed;
                if (green)
                {
                    this.GreenOffsetSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenOffset * 100.0f;
                    this.GreenExponentSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenExponent * 100.0f;
                    this.GreenAmplitudeSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenAmplitude * 100.0f;
                }

                //Blue
                bool blue = (value == GammaTransferPageState.Blue);
                this.BlueSegmented.Background = blue ? this.AccentColor : this.UnAccentColor;
                this.BlueCheckBox.Visibility = this.BlueOffsetTextBlock.Visibility = this.BlueOffsetSlider.Visibility = this.BlueExponentTextBlock.Visibility = this.BlueExponentSlider.Visibility = this.BlueAmplitudeTextBlock.Visibility = this.BlueAmplitudeSlider.Visibility = blue ? Visibility.Visible : Visibility.Collapsed;
                if (blue)
                {
                    this.BlueOffsetSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueOffset * 100.0f;
                    this.BlueExponentSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueExponent * 100.0f;
                    this.BlueAmplitudeSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueAmplitude * 100.0f;
                }
            }
        }


        //Alpha
        public bool AlphaDisable
        {
            set
            {
                this.AlphaOffsetSlider.IsEnabled = this.AlphaExponentSlider.IsEnabled = this.AlphaAmplitudeSlider.IsEnabled = !value;
                this.AlphaOffsetSlider.Opacity = this.AlphaExponentSlider.Opacity = this.AlphaAmplitudeSlider.Opacity = value ? 0.5 : 1.0;
            }
        }
        //Red
        public bool RedDisable
        {
            set
            {
                this.RedOffsetSlider.IsEnabled = this.RedExponentSlider.IsEnabled = this.RedAmplitudeSlider.IsEnabled = !value;
                this.RedOffsetSlider.Opacity = this.RedExponentSlider.Opacity = this.RedAmplitudeSlider.Opacity = value ? 0.5 : 1.0;
            }
        }
        //Green
        public bool GreenDisable
        {
            set
            {
                this.GreenOffsetSlider.IsEnabled = this.GreenExponentSlider.IsEnabled = this.GreenAmplitudeSlider.IsEnabled = !value;
                this.GreenOffsetSlider.Opacity = this.GreenExponentSlider.Opacity = this.GreenAmplitudeSlider.Opacity = value ? 0.5 : 1.0;
            }
        }
        //Blue   
        public bool BlueDisable
        {
            set
            {
                this.BlueOffsetSlider.IsEnabled = this.BlueExponentSlider.IsEnabled = this.BlueAmplitudeSlider.IsEnabled = !value;
                this.BlueOffsetSlider.Opacity = this.BlueExponentSlider.Opacity = this.BlueAmplitudeSlider.Opacity = value ? 0.5 : 1.0;
            }
        }


        public GammaTransferPage()
        {
            base.Type = AdjustmentType.GammaTransfer;
            base.Icon = new GammaTransferControl();
            this.InitializeComponent();

            this.Loaded += (s, e) =>
            {
                this.State = GammaTransferPageState.Alpha;
                this.AlphaDisable = this.RedDisable = this.GreenDisable = this.BlueDisable = true;
            };

            //Index
            this.AlphaSegmented.Tapped += (s, e) => this.State = GammaTransferPageState.Alpha;
            this.RedSegmented.Tapped += (s, e) => this.State = GammaTransferPageState.Red;
            this.GreenSegmented.Tapped += (s, e) => this.State = GammaTransferPageState.Green;
            this.BlueSegmented.Tapped += (s, e) => this.State = GammaTransferPageState.Blue;

            //Alpha
            this.AlphaCheckBox.Toggled += (s, e) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.AlphaDisable = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaDisable = !this.AlphaCheckBox.IsOn;
                Adjustment.Invalidate?.Invoke();
            };
            this.AlphaOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaOffset = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };
            this.AlphaExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaExponent = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };
            this.AlphaAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaAmplitude = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };

            //Red
            this.RedCheckBox.Toggled += (s, e) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.RedDisable = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedDisable = !this.RedCheckBox.IsOn;
                Adjustment.Invalidate?.Invoke();
            };
            this.RedOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedOffset = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };
            this.RedExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedExponent = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };
            this.RedAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedAmplitude = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };

            //Green
            this.GreenCheckBox.Toggled += (s, e) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GreenDisable = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenDisable = !this.GreenCheckBox.IsOn;
                Adjustment.Invalidate?.Invoke();
            };
            this.GreenOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenOffset = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };
            this.GreenExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenExponent = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };
            this.GreenAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenAmplitude = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };

            //Blue
            this.BlueCheckBox.Toggled += (s, e) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.BlueDisable = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueDisable = !this.BlueCheckBox.IsOn;
                Adjustment.Invalidate?.Invoke();
            };

            this.BlueOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueOffset = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };
            this.BlueExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueExponent = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };
            this.BlueAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.GammaTransferAdjustment == null) return;
                this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueAmplitude = (float)(value / 100.0f);
                Adjustment.Invalidate?.Invoke();
            };
        }

        //@override
        public override Adjustment GetNewAdjustment() => new GammaTransferAdjustment();
        public override Adjustment GetAdjustment() => this.GammaTransferAdjustment;
        public override void SetAdjustment(Adjustment value)
        {
            if (value is GammaTransferAdjustment adjustment)
            {
                this.GammaTransferAdjustment = adjustment;
                this.Invalidate(adjustment);
            }
        }

        public override void Close() => this.GammaTransferAdjustment = null;
        public override void Reset()
        {
            if (this.GammaTransferAdjustment == null) return;

            this.GammaTransferAdjustment.Item.Reset();
            this.Invalidate(this.GammaTransferAdjustment);
        }

        public void Invalidate(GammaTransferAdjustment adjustment)
        {
            //Alpha
            this.AlphaDisable = this.AlphaCheckBox.IsOn = !adjustment.GammaTransferAdjustmentItem.AlphaDisable;
            this.AlphaOffsetSlider.Value = adjustment.GammaTransferAdjustmentItem.AlphaOffset * 100.0f;
            this.AlphaExponentSlider.Value = adjustment.GammaTransferAdjustmentItem.AlphaExponent * 100.0f;
            this.AlphaAmplitudeSlider.Value = adjustment.GammaTransferAdjustmentItem.AlphaAmplitude * 100.0f;

            //Red
            this.RedDisable = this.RedCheckBox.IsOn = !adjustment.GammaTransferAdjustmentItem.RedDisable;
            this.RedOffsetSlider.Value = adjustment.GammaTransferAdjustmentItem.RedOffset * 100.0f;
            this.RedExponentSlider.Value = adjustment.GammaTransferAdjustmentItem.RedExponent * 100.0f;
            this.RedAmplitudeSlider.Value = adjustment.GammaTransferAdjustmentItem.RedAmplitude * 100.0f;

            //Green
            this.GreenDisable = this.GreenCheckBox.IsOn = !adjustment.GammaTransferAdjustmentItem.GreenDisable;
            this.GreenOffsetSlider.Value = adjustment.GammaTransferAdjustmentItem.GreenOffset * 100.0f;
            this.GreenExponentSlider.Value = adjustment.GammaTransferAdjustmentItem.GreenExponent * 100.0f;
            this.GreenAmplitudeSlider.Value = adjustment.GammaTransferAdjustmentItem.GreenAmplitude * 100.0f;

            //Blue
            this.BlueDisable = this.BlueCheckBox.IsOn = !adjustment.GammaTransferAdjustmentItem.BlueDisable;
            this.BlueOffsetSlider.Value = adjustment.GammaTransferAdjustmentItem.BlueOffset * 100.0f;
            this.BlueExponentSlider.Value = adjustment.GammaTransferAdjustmentItem.BlueExponent * 100.0f;
            this.BlueAmplitudeSlider.Value = adjustment.GammaTransferAdjustmentItem.BlueAmplitude * 100.0f;
        }
    }
}
using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    public enum GammaTransferPageState
    {
        Alpha,
        Red,
        Green,
        Blue
    }

    /// <summary>
    /// Page of <see cref = "Adjustment"/>.
    /// </summary>
    public sealed partial class GammaTransferPage : IAdjustmentPage
    {
        public GammaTransferAdjustment Adjustment;

        //@Content
        public AdjustmentType Type { get; } = AdjustmentType.GammaTransfer;
        public FrameworkElement Icon { get; } = new GammaTransferIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


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
                    this.AlphaOffsetSlider.Value = this.Adjustment.AlphaOffset * 100.0f;
                    this.AlphaExponentSlider.Value = this.Adjustment.AlphaExponent * 100.0f;
                    this.AlphaAmplitudeSlider.Value = this.Adjustment.AlphaAmplitude * 100.0f;
                }

                //Red
                bool red = (value == GammaTransferPageState.Red);
                this.RedSegmented.Background = red ? this.AccentColor : this.UnAccentColor;
                this.RedCheckBox.Visibility = this.RedOffsetTextBlock.Visibility = this.RedOffsetSlider.Visibility = this.RedExponentTextBlock.Visibility = this.RedExponentSlider.Visibility = this.RedAmplitudeTextBlock.Visibility = this.RedAmplitudeSlider.Visibility = red ? Visibility.Visible : Visibility.Collapsed;
                if (red)
                {
                    this.RedOffsetSlider.Value = this.Adjustment.RedOffset * 100.0f;
                    this.RedExponentSlider.Value = this.Adjustment.RedExponent * 100.0f;
                    this.RedAmplitudeSlider.Value = this.Adjustment.RedAmplitude * 100.0f;
                }

                //Green
                bool green = (value == GammaTransferPageState.Green);
                this.GreenSegmented.Background = green ? this.AccentColor : this.UnAccentColor;
                this.GreenCheckBox.Visibility = this.GreenOffsetTextBlock.Visibility = this.GreenOffsetSlider.Visibility = this.GreenExponentTextBlock.Visibility = this.GreenExponentSlider.Visibility = this.GreenAmplitudeTextBlock.Visibility = this.GreenAmplitudeSlider.Visibility = green ? Visibility.Visible : Visibility.Collapsed;
                if (green)
                {
                    this.GreenOffsetSlider.Value = this.Adjustment.GreenOffset * 100.0f;
                    this.GreenExponentSlider.Value = this.Adjustment.GreenExponent * 100.0f;
                    this.GreenAmplitudeSlider.Value = this.Adjustment.GreenAmplitude * 100.0f;
                }

                //Blue
                bool blue = (value == GammaTransferPageState.Blue);
                this.BlueSegmented.Background = blue ? this.AccentColor : this.UnAccentColor;
                this.BlueCheckBox.Visibility = this.BlueOffsetTextBlock.Visibility = this.BlueOffsetSlider.Visibility = this.BlueExponentTextBlock.Visibility = this.BlueExponentSlider.Visibility = this.BlueAmplitudeTextBlock.Visibility = this.BlueAmplitudeSlider.Visibility = blue ? Visibility.Visible : Visibility.Collapsed;
                if (blue)
                {
                    this.BlueOffsetSlider.Value = this.Adjustment.BlueOffset * 100.0f;
                    this.BlueExponentSlider.Value = this.Adjustment.BlueExponent * 100.0f;
                    this.BlueAmplitudeSlider.Value = this.Adjustment.BlueAmplitude * 100.0f;
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


        //@Construct
        public GammaTransferPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

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
                if (this.Adjustment == null) return;
                this.AlphaDisable = this.Adjustment.AlphaDisable = !this.AlphaCheckBox.IsOn;
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.AlphaOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.AlphaOffset = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.AlphaExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.AlphaExponent = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.AlphaAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.AlphaAmplitude = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };

            //Red
            this.RedCheckBox.Toggled += (s, e) =>
            {
                if (this.Adjustment == null) return;
                this.RedDisable = this.Adjustment.RedDisable = !this.RedCheckBox.IsOn;
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.RedOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.RedOffset = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.RedExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.RedExponent = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.RedAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.RedAmplitude = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };

            //Green
            this.GreenCheckBox.Toggled += (s, e) =>
            {
                if (this.Adjustment == null) return;
                this.GreenDisable = this.Adjustment.GreenDisable = !this.GreenCheckBox.IsOn;
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.GreenOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.GreenOffset = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.GreenExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.GreenExponent = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.GreenAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.GreenAmplitude = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };

            //Blue
            this.BlueCheckBox.Toggled += (s, e) =>
            {
                if (this.Adjustment == null) return;
                this.BlueDisable = this.Adjustment.BlueDisable = !this.BlueCheckBox.IsOn;
                AdjustmentManager.Invalidate?.Invoke();
            };

            this.BlueOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.BlueOffset = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.BlueExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.BlueExponent = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
            this.BlueAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment == null) return;
                this.Adjustment.BlueAmplitude = (float)(value / 100.0f);
                AdjustmentManager.Invalidate?.Invoke();
            };
        }

        
        public IAdjustment GetNewAdjustment() => new GammaTransferAdjustment();
        public void Follow(GammaTransferAdjustment adjustment)
        {
            //Alpha
            this.AlphaDisable = this.AlphaCheckBox.IsOn = !adjustment.AlphaDisable;
            this.AlphaOffsetSlider.Value = adjustment.AlphaOffset * 100.0f;
            this.AlphaExponentSlider.Value = adjustment.AlphaExponent * 100.0f;
            this.AlphaAmplitudeSlider.Value = adjustment.AlphaAmplitude * 100.0f;

            //Red
            this.RedDisable = this.RedCheckBox.IsOn = !adjustment.RedDisable;
            this.RedOffsetSlider.Value = adjustment.RedOffset * 100.0f;
            this.RedExponentSlider.Value = adjustment.RedExponent * 100.0f;
            this.RedAmplitudeSlider.Value = adjustment.RedAmplitude * 100.0f;

            //Green
            this.GreenDisable = this.GreenCheckBox.IsOn = !adjustment.GreenDisable;
            this.GreenOffsetSlider.Value = adjustment.GreenOffset * 100.0f;
            this.GreenExponentSlider.Value = adjustment.GreenExponent * 100.0f;
            this.GreenAmplitudeSlider.Value = adjustment.GreenAmplitude * 100.0f;

            //Blue
            this.BlueDisable = this.BlueCheckBox.IsOn = !adjustment.BlueDisable;
            this.BlueOffsetSlider.Value = adjustment.BlueOffset * 100.0f;
            this.BlueExponentSlider.Value = adjustment.BlueExponent * 100.0f;
            this.BlueAmplitudeSlider.Value = adjustment.BlueAmplitude * 100.0f;
        }


        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/GammaTransfer");

            this.AlphaOffsetTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Offset");
            this.AlphaExponentTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Exponent");
            this.AlphaAmplitudeTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Amplitude");

            this.RedOffsetTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Offset");
            this.RedExponentTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Exponent");
            this.RedAmplitudeTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Amplitude");

            this.GreenOffsetTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Offset");
            this.GreenExponentTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Exponent");
            this.GreenAmplitudeTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Amplitude");

            this.BlueOffsetTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Offset");
            this.BlueExponentTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Exponent");
            this.BlueAmplitudeTextBlock.Text = resource.GetString("/Adjustments/GammaTransfer_Amplitude");
        }

    }
}
using Retouch_Photo.Adjustments.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Adjustments.Pages
{
    public sealed partial class GammaTransferPage : Page
    {
        #region DependencyProperty


        public GammaTransferAdjustment GammaTransferAdjustment
        {
            get { return (GammaTransferAdjustment)GetValue(GammaTransferAdjustmentProperty); }
            set { SetValue(GammaTransferAdjustmentProperty, value); }
        }
        public static readonly DependencyProperty GammaTransferAdjustmentProperty = DependencyProperty.Register(nameof(GammaTransferAdjustment), typeof(GammaTransferAdjustment), typeof(GammaTransferAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            GammaTransferPage con = (GammaTransferPage)sender;

            if (e.NewValue is GammaTransferAdjustment adjustment)
            {

                //GammaTransfer Alpha
                bool alpha = !adjustment.GammaTransferAdjustmentItem.AlphaDisable;
                con.AlphaDisable = alpha;
                con.AlphaCheckBox.IsOn = alpha;

                con.AlphaOffsetSlider.Value = adjustment.GammaTransferAdjustmentItem.AlphaOffset * 100.0f;
                con.AlphaExponentSlider.Value = adjustment.GammaTransferAdjustmentItem.AlphaExponent * 100.0f;
                con.AlphaAmplitudeSlider.Value = adjustment.GammaTransferAdjustmentItem.AlphaAmplitude * 100.0f;


                //GammaTransfer Red
                bool red = !adjustment.GammaTransferAdjustmentItem.RedDisable;
                con.RedDisable = red;
                con.RedCheckBox.IsOn = red;

                con.RedOffsetSlider.Value = adjustment.GammaTransferAdjustmentItem.RedOffset * 100.0f;
                con.RedExponentSlider.Value = adjustment.GammaTransferAdjustmentItem.RedExponent * 100.0f;
                con.RedAmplitudeSlider.Value = adjustment.GammaTransferAdjustmentItem.RedAmplitude * 100.0f;


                //GammaTransfer Green
                bool green = !adjustment.GammaTransferAdjustmentItem.GreenDisable;
                con.GreenDisable = green;
                con.GreenCheckBox.IsOn = green;

                con.GreenOffsetSlider.Value = adjustment.GammaTransferAdjustmentItem.GreenOffset * 100.0f;
                con.GreenExponentSlider.Value = adjustment.GammaTransferAdjustmentItem.GreenExponent * 100.0f;
                con.GreenAmplitudeSlider.Value = adjustment.GammaTransferAdjustmentItem.GreenAmplitude * 100.0f;


                //GammaTransfer Blue
                bool blue = !adjustment.GammaTransferAdjustmentItem.BlueDisable;
                con.BlueDisable = blue;
                con.BlueCheckBox.IsOn = blue;

                con.BlueOffsetSlider.Value = adjustment.GammaTransferAdjustmentItem.BlueOffset * 100.0f;
                con.BlueExponentSlider.Value = adjustment.GammaTransferAdjustmentItem.BlueExponent * 100.0f;
                con.BlueAmplitudeSlider.Value = adjustment.GammaTransferAdjustmentItem.BlueAmplitude * 100.0f;
            }
        }));

        #endregion


        public GammaTransferPage()
        {
            this.InitializeComponent();

            this.Loaded += (s, e) =>
            {
                this.Index = 0;

                this.AlphaDisable =
                this.RedDisable =
                this.GreenDisable =
                this.BlueDisable = true;
            };
        }



        // GammaTransfer Alpha
        public bool AlphaVisibility
        {
            set
            {
                this.AlphaCheckBox.Visibility =
                this.AlphaOffsetTextBlock.Visibility =
                this.AlphaOffsetSlider.Visibility =
                this.AlphaExponentTextBlock.Visibility =
                this.AlphaExponentSlider.Visibility =
                this.AlphaAmplitudeTextBlock.Visibility =
                this.AlphaAmplitudeSlider.Visibility =
                value ? Visibility.Visible : Visibility.Collapsed;

                if (value)
                {
                    this.AlphaOffsetSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaOffset * 100.0f;
                    this.AlphaExponentSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaExponent * 100.0f;
                    this.AlphaAmplitudeSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaAmplitude * 100.0f;
                }
            }
        }

        public bool AlphaDisable
        {
            set
            {
                this.AlphaOffsetSlider.IsEnabled =
                this.AlphaExponentSlider.IsEnabled =
                this.AlphaAmplitudeSlider.IsEnabled =
                !value;

                this.AlphaOffsetSlider.Opacity =
                this.AlphaExponentSlider.Opacity =
                this.AlphaAmplitudeSlider.Opacity =
                value ? 0.5 : 1.0;
            }
        }
        private void AlphaCheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            bool value = !this.AlphaCheckBox.IsOn;

            this.AlphaDisable =
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaDisable = value;
            Adjustment.Invalidate?.Invoke();
        }

        private void AlphaOffsetSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaOffset = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }
        private void AlphaExponentSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaExponent = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }
        private void AlphaAmplitudeSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.AlphaAmplitude = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }



        //GammaTransfer Red
        public bool RedVisibility
        {
            set
            {
                this.RedCheckBox.Visibility =
                this.RedOffsetTextBlock.Visibility =
                this.RedOffsetSlider.Visibility =
                this.RedExponentTextBlock.Visibility =
                this.RedExponentSlider.Visibility =
                this.RedAmplitudeTextBlock.Visibility =
                this.RedAmplitudeSlider.Visibility =
                value ? Visibility.Visible : Visibility.Collapsed;

                if (value)
                {
                    this.RedOffsetSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedOffset * 100.0f;
                    this.RedExponentSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedExponent * 100.0f;
                    this.RedAmplitudeSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedAmplitude * 100.0f;
                }
            }
        }

        public bool RedDisable
        {
            set
            {
                this.RedOffsetSlider.IsEnabled =
                this.RedExponentSlider.IsEnabled =
                this.RedAmplitudeSlider.IsEnabled =
                !value;

                this.RedOffsetSlider.Opacity =
                this.RedExponentSlider.Opacity =
                this.RedAmplitudeSlider.Opacity =
                value ? 0.5 : 1.0;
            }
        }
        private void RedCheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            bool value = !this.RedCheckBox.IsOn;

            this.RedDisable =
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedDisable = value;
            Adjustment.Invalidate?.Invoke();
        }

        private void RedOffsetSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedOffset = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }
        private void RedExponentSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedExponent = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }
        private void RedAmplitudeSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.RedAmplitude = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }



        //GammaTransfer Green
        public bool GreenVisibility
        {
            set
            {
                this.GreenCheckBox.Visibility =
                this.GreenOffsetTextBlock.Visibility =
                this.GreenOffsetSlider.Visibility =
                this.GreenExponentTextBlock.Visibility =
                this.GreenExponentSlider.Visibility =
                this.GreenAmplitudeTextBlock.Visibility =
                this.GreenAmplitudeSlider.Visibility =
                value ? Visibility.Visible : Visibility.Collapsed;

                if (value)
                {
                    this.GreenOffsetSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenOffset * 100.0f;
                    this.GreenExponentSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenExponent * 100.0f;
                    this.GreenAmplitudeSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenAmplitude * 100.0f;
                }
            }
        }

        public bool GreenDisable
        {
            set
            {
                this.GreenOffsetSlider.IsEnabled =
                this.GreenExponentSlider.IsEnabled =
                this.GreenAmplitudeSlider.IsEnabled =
                !value;

                this.GreenOffsetSlider.Opacity =
                this.GreenExponentSlider.Opacity =
                this.GreenAmplitudeSlider.Opacity =
                value ? 0.5 : 1.0;
            }
        }
        private void GreenCheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            bool value = !this.GreenCheckBox.IsOn;

            this.GreenDisable =
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenDisable = value;
            Adjustment.Invalidate?.Invoke();
        }

        private void GreenOffsetSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenOffset = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }
        private void GreenExponentSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenExponent = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }
        private void GreenAmplitudeSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.GreenAmplitude = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }



        //GammaTransfer Blue   
        public bool BlueVisibility
        {
            set
            {
                this.BlueCheckBox.Visibility =
                this.BlueOffsetTextBlock.Visibility =
                this.BlueOffsetSlider.Visibility =
                this.BlueExponentTextBlock.Visibility =
                this.BlueExponentSlider.Visibility =
                this.BlueAmplitudeTextBlock.Visibility =
                this.BlueAmplitudeSlider.Visibility =
                 value ? Visibility.Visible : Visibility.Collapsed;

                if (value)
                {
                    this.BlueOffsetSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueOffset * 100.0f;
                    this.BlueExponentSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueExponent * 100.0f;
                    this.BlueAmplitudeSlider.Value = this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueAmplitude * 100.0f;
                }
            }
        }

        public bool BlueDisable
        {
            set
            {
                this.BlueOffsetSlider.IsEnabled =
                this.BlueExponentSlider.IsEnabled =
                this.BlueAmplitudeSlider.IsEnabled =
                !value;

                this.BlueOffsetSlider.Opacity =
                this.BlueExponentSlider.Opacity =
                this.BlueAmplitudeSlider.Opacity =
                value ? 0.5 : 1.0;
            }
        }
        private void BlueCheckBox_Toggled(object sender, RoutedEventArgs e)
        {
            bool value = !this.BlueCheckBox.IsOn;

            this.BlueDisable =
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueDisable = value;
            Adjustment.Invalidate?.Invoke();
        }

        private void BlueOffsetSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueOffset = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }
        private void BlueExponentSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueExponent = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }
        private void BlueAmplitudeSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GammaTransferAdjustmentItem.BlueAmplitude = (float)(value / 100.0f);
            Adjustment.Invalidate?.Invoke();
        }



        //Segmented
        public int Index
        {
            set
            {
                //GammaTransfer Alpha
                bool alpha = (value == 0);
                this.AlphaSegmented.Background = alpha ? this.AccentColor : this.UnAccentColor;
                this.AlphaVisibility = alpha;

                //GammaTransfer Red
                bool red = (value == 1);
                this.RedSegmented.Background = red ? this.AccentColor : this.UnAccentColor;
                this.RedVisibility = red;

                //GammaTransfer Green
                bool green = (value == 2);
                this.GreenSegmented.Background = green ? this.AccentColor : this.UnAccentColor;
                this.GreenVisibility = green;

                //GammaTransfer Blue
                bool blue = (value == 3);
                this.BlueSegmented.Background = blue ? this.AccentColor : this.UnAccentColor;
                this.BlueVisibility = blue;
            }
        }
        private void AlphaSegmented_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) => this.Index = 0;
        private void RedSegmented_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) => this.Index = 1;
        private void GreenSegmented_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) => this.Index = 2;
        private void BlueSegmented_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) => this.Index = 3;


    }
}
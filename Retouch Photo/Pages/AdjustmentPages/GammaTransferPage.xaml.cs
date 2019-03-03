using Retouch_Photo.Models.Adjustments;
using Retouch_Photo.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Pages.AdjustmentPages
{
    public sealed partial class GammaTransferPage : Page
    {

        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        #region DependencyProperty


        public GammaTransferAdjustment GammaTransferAdjustment
        {
            get { return (GammaTransferAdjustment)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(GammaTransferAdjustment), typeof(GammaTransferAdjustment), typeof(GammaTransferAdjustment), new PropertyMetadata(null, (sender, e) =>
        {
            GammaTransferPage con = (GammaTransferPage)sender;

            if (e.NewValue is GammaTransferAdjustment adjustment)
            {

                //GammaTransfer Alpha
                bool alpha = !adjustment.AlphaDisable;
                con.AlphaDisable = alpha;
                con.AlphaCheckBox.IsOn = alpha;

                con.AlphaOffsetSlider.Value = adjustment.AlphaOffset * 100.0f;
                con.AlphaExponentSlider.Value = adjustment.AlphaExponent * 100.0f;
                con.AlphaAmplitudeSlider.Value = adjustment.AlphaAmplitude * 100.0f;


                //GammaTransfer Red
                bool red = !adjustment.RedDisable;
                con.RedDisable = red;
                con.RedCheckBox.IsOn = red;

                con.RedOffsetSlider.Value = adjustment.RedOffset * 100.0f;
                con.RedExponentSlider.Value = adjustment.RedExponent * 100.0f;
                con.RedAmplitudeSlider.Value = adjustment.RedAmplitude * 100.0f;


                //GammaTransfer Green
                bool green = !adjustment.GreenDisable;
                con.GreenDisable = green;
                con.GreenCheckBox.IsOn = green;

                con.GreenOffsetSlider.Value = adjustment.GreenOffset * 100.0f;
                con.GreenExponentSlider.Value = adjustment.GreenExponent * 100.0f;
                con.GreenAmplitudeSlider.Value = adjustment.GreenAmplitude * 100.0f;


                //GammaTransfer Blue
                bool blue = !adjustment.BlueDisable;
                con.BlueDisable = blue;
                con.BlueCheckBox.IsOn = blue;

                con.BlueOffsetSlider.Value = adjustment.BlueOffset * 100.0f;
                con.BlueExponentSlider.Value = adjustment.BlueExponent * 100.0f;
                con.BlueAmplitudeSlider.Value = adjustment.BlueAmplitude * 100.0f;
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
                    this.AlphaOffsetSlider.Value = this.GammaTransferAdjustment.AlphaOffset * 100.0f;
                    this.AlphaExponentSlider.Value = this.GammaTransferAdjustment.AlphaExponent * 100.0f;
                    this.AlphaAmplitudeSlider.Value = this.GammaTransferAdjustment.AlphaAmplitude * 100.0f;
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
            this.GammaTransferAdjustment.AlphaDisable = value;
            this.ViewModel.Invalidate();
        }

        private void AlphaOffsetSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.AlphaOffset = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
        }
        private void AlphaExponentSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.AlphaExponent = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
        }
        private void AlphaAmplitudeSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.AlphaAmplitude = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
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
                    this.RedOffsetSlider.Value = this.GammaTransferAdjustment.RedOffset * 100.0f;
                    this.RedExponentSlider.Value = this.GammaTransferAdjustment.RedExponent * 100.0f;
                    this.RedAmplitudeSlider.Value = this.GammaTransferAdjustment.RedAmplitude * 100.0f;
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
            this.GammaTransferAdjustment.RedDisable = value;
            this.ViewModel.Invalidate();
        }

        private void RedOffsetSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.RedOffset = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
        }
        private void RedExponentSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.RedExponent = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
        }
        private void RedAmplitudeSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.RedAmplitude = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
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
                    this.GreenOffsetSlider.Value = this.GammaTransferAdjustment.GreenOffset * 100.0f;
                    this.GreenExponentSlider.Value = this.GammaTransferAdjustment.GreenExponent * 100.0f;
                    this.GreenAmplitudeSlider.Value = this.GammaTransferAdjustment.GreenAmplitude * 100.0f;
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
            this.GammaTransferAdjustment.GreenDisable = value;
            this.ViewModel.Invalidate();
        }

        private void GreenOffsetSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GreenOffset = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
        }
        private void GreenExponentSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GreenExponent = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
        }
        private void GreenAmplitudeSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.GreenAmplitude = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
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
                    this.BlueOffsetSlider.Value = this.GammaTransferAdjustment.BlueOffset * 100.0f;
                    this.BlueExponentSlider.Value = this.GammaTransferAdjustment.BlueExponent * 100.0f;
                    this.BlueAmplitudeSlider.Value = this.GammaTransferAdjustment.BlueAmplitude * 100.0f;
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
            this.GammaTransferAdjustment.BlueDisable = value;
            this.ViewModel.Invalidate();
        }

        private void BlueOffsetSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.BlueOffset = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
        }
        private void BlueExponentSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.BlueExponent = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
        }
        private void BlueAmplitudeSlider_ValueChangeDelta(object sender, double value)
        {
            this.GammaTransferAdjustment.BlueAmplitude = (float)(value / 100.0f);
            this.ViewModel.Invalidate();
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
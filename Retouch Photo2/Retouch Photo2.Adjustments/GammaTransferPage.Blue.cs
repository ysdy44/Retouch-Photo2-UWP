using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "GammaTransferAdjustment"/>.
    /// </summary>
    public sealed partial class GammaTransferPage : IAdjustmentGenericPage<GammaTransferAdjustment>
    {

        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s blue visibility. </summary>
        public Visibility BlueIsExpaned
        {
            get { return (Visibility)GetValue(BlueIsExpanedProperty); }
            set { SetValue(BlueIsExpanedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.BlueIsExpaned" /> dependency property. </summary>
        public static readonly DependencyProperty BlueIsExpanedProperty = DependencyProperty.Register(nameof(BlueIsExpaned), typeof(Visibility), typeof(GammaTransferPage), new PropertyMetadata(Visibility.Collapsed));


        #endregion


        public void ResetBlue()
        {
            this.BlueToggleSwitch.IsOn = false;
            this.BlueOffsetSlider.Value = 0;
            this.BlueExponentSlider.Value = 100;
            this.BlueAmplitudeSlider.Value = 100;
        }
        public void FollowBlue(GammaTransferAdjustment adjustment)
        {
            this.BlueToggleSwitch.IsOn = !adjustment.BlueDisable;
            this.BlueOffsetSlider.Value = adjustment.BlueOffset * 100.0f;
            this.BlueExponentSlider.Value = adjustment.BlueExponent * 100.0f;
            this.BlueAmplitudeSlider.Value = adjustment.BlueAmplitude * 100.0f;
        }

        public void ConstructStringsBlue(string title, string offset, string exponent, string amplitude)
        {
            this.BlueTextBlock.Text = title;
            this.BlueOffsetTextBlock.Text = offset;
            this.BlueExponentTextBlock.Text = exponent;
            this.BlueAmplitudeTextBlock.Text = amplitude;
        }


        public void ConstructBlueDisable()
        {
            this.BlueTitleGrid.Tapped += (s, e) =>
            {
                switch (this.BlueIsExpaned)
                {
                    case Visibility.Visible:
                        this.BlueIsExpaned = Visibility.Collapsed;
                        break;
                    case Visibility.Collapsed:
                        this.BlueIsExpaned = Visibility.Visible;
                        break;
                }
            };

            this.BlueToggleSwitch.Toggled += (s, e) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    bool disable = !this.BlueToggleSwitch.IsOn;
                    if (adjustment.BlueDisable == disable) return;


                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment blue disable");

                    var previous = adjustment.BlueDisable;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.BlueDisable = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.BlueDisable = disable;
                    this.ViewModel.Invalidate();
                }
            };
        }


        public void ConstructBlueOffset()
        {
            this.BlueOffsetSlider.Value = 0;
            this.BlueOffsetSlider.Minimum = 0;
            this.BlueOffsetSlider.Maximum = 100;

            this.BlueOffsetSlider.SliderBrush = this.BlueLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.BlueOffsetSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment blue offset");

                    adjustment.CacheBlueOffset();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.BlueOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float offset = (float)value / 100.0f;

                    adjustment.BlueOffset = offset;
                    this.ViewModel.Invalidate();
                }
            };
            this.BlueOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float offset = (float)value / 100.0f;


                    var previous = adjustment.StartingBlueOffset;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.BlueOffset = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.BlueOffset = offset;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }


        public void ConstructBlueExponent()
        {
            this.BlueExponentSlider.Value = 100;
            this.BlueExponentSlider.Minimum = 0;
            this.BlueExponentSlider.Maximum = 100;

            this.BlueExponentSlider.SliderBrush = this.BlueLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.BlueExponentSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment blue exponent");

                    adjustment.CacheBlueExponent();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.BlueExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float exponent = (float)value / 100.0f;

                    adjustment.BlueExponent = exponent;
                    this.ViewModel.Invalidate();
                }
            };
            this.BlueExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float exponent = (float)value / 100.0f;


                    var previous = adjustment.StartingBlueExponent;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.BlueExponent = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.BlueExponent = exponent;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }


        public void ConstructBlueAmplitude()
        {
            this.BlueAmplitudeSlider.Value = 100;
            this.BlueAmplitudeSlider.Minimum = 0;
            this.BlueAmplitudeSlider.Maximum = 100;

            this.BlueAmplitudeSlider.SliderBrush = this.BlueLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.BlueAmplitudeSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment blue amplitude");

                    adjustment.CacheBlueAmplitude();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.BlueAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float amplitude = (float)value / 100.0f;

                    adjustment.BlueAmplitude = amplitude;
                    this.ViewModel.Invalidate();
                }
            };
            this.BlueAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float amplitude = (float)value / 100.0f;


                    var previous = adjustment.StartingBlueAmplitude;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.BlueAmplitude = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.BlueAmplitude = amplitude;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

    }
}
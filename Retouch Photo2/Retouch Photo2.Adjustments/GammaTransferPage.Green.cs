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


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s green visibility. </summary>
        public Visibility GreenIsExpaned
        {
            get { return (Visibility)GetValue(GreenIsExpanedProperty); }
            set { SetValue(GreenIsExpanedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.GreenIsExpaned" /> dependency property. </summary>
        public static readonly DependencyProperty GreenIsExpanedProperty = DependencyProperty.Register(nameof(GreenIsExpaned), typeof(Visibility), typeof(GammaTransferPage), new PropertyMetadata(Visibility.Collapsed));


        #endregion


        public void ResetGreen()
        {
            this.GreenToggleSwitch.IsOn = false;
            this.GreenOffsetSlider.Value = 0;
            this.GreenExponentSlider.Value = 100;
            this.GreenAmplitudeSlider.Value = 100;
        }
        public void FollowGreen(GammaTransferAdjustment adjustment)
        {
            this.GreenToggleSwitch.IsOn = !adjustment.GreenDisable;
            this.GreenOffsetSlider.Value = adjustment.GreenOffset * 100.0f;
            this.GreenExponentSlider.Value = adjustment.GreenExponent * 100.0f;
            this.GreenAmplitudeSlider.Value = adjustment.GreenAmplitude * 100.0f;
        }

        public void ConstructStringsGreen(string title, string offset, string exponent, string amplitude)
        {
            this.GreenTextBlock.Text = title;
            this.GreenOffsetTextBlock.Text = offset;
            this.GreenExponentTextBlock.Text = exponent;
            this.GreenAmplitudeTextBlock.Text = amplitude;
        }


        public void ConstructGreenDisable()
        {
            this.GreenTitleGrid.Tapped += (s, e) =>
            {
                switch (this.GreenIsExpaned)
                {
                    case Visibility.Visible:
                        this.GreenIsExpaned = Visibility.Collapsed;
                        break;
                    case Visibility.Collapsed:
                        this.GreenIsExpaned = Visibility.Visible;
                        break;
                }
            };

            this.GreenToggleSwitch.Toggled += (s, e) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    bool disable = !this.GreenToggleSwitch.IsOn;
                    if (adjustment.GreenDisable == disable) return;


                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment green disable");

                    var previous = adjustment.GreenDisable;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.GreenDisable = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.GreenDisable = disable;
                    this.ViewModel.Invalidate();
                }
            };
        }


        public void ConstructGreenOffset()
        {
            this.GreenOffsetSlider.Value = 0;
            this.GreenOffsetSlider.Minimum = 0;
            this.GreenOffsetSlider.Maximum = 100;

            this.GreenOffsetSlider.SliderBrush = this.GreenLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.GreenOffsetSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment green offset");

                    adjustment.CacheGreenOffset();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.GreenOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float offset = (float)value / 100.0f;

                    adjustment.GreenOffset = offset;
                    this.ViewModel.Invalidate();
                }
            };
            this.GreenOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float offset = (float)value / 100.0f;


                    var previous = adjustment.StartingGreenOffset;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.GreenOffset = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.GreenOffset = offset;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }


        public void ConstructGreenExponent()
        {
            this.GreenExponentSlider.Value = 100;
            this.GreenExponentSlider.Minimum = 0;
            this.GreenExponentSlider.Maximum = 100;

            this.GreenExponentSlider.SliderBrush = this.GreenLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.GreenExponentSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment green exponent");

                    adjustment.CacheGreenExponent();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.GreenExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float exponent = (float)value / 100.0f;

                    adjustment.GreenExponent = exponent;
                    this.ViewModel.Invalidate();
                }
            };
            this.GreenExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float exponent = (float)value / 100.0f;


                    var previous = adjustment.StartingGreenExponent;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.GreenExponent = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.GreenExponent = exponent;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }


        public void ConstructGreenAmplitude()
        {
            this.GreenAmplitudeSlider.Value = 100;
            this.GreenAmplitudeSlider.Minimum = 0;
            this.GreenAmplitudeSlider.Maximum = 100;

            this.GreenAmplitudeSlider.SliderBrush = this.GreenLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.GreenAmplitudeSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment green amplitude");

                    adjustment.CacheGreenAmplitude();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.GreenAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float amplitude = (float)value / 100.0f;

                    adjustment.GreenAmplitude = amplitude;
                    this.ViewModel.Invalidate();
                }
            };
            this.GreenAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float amplitude = (float)value / 100.0f;


                    var previous = adjustment.StartingGreenAmplitude;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.GreenAmplitude = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.GreenAmplitude = amplitude;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

    }
}
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


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s alpha visibility. </summary>
        public Visibility AlphaIsExpaned
        {
            get { return (Visibility)GetValue(AlphaIsExpanedProperty); }
            set { SetValue(AlphaIsExpanedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.AlphaIsExpaned" /> dependency property. </summary>
        public static readonly DependencyProperty AlphaIsExpanedProperty = DependencyProperty.Register(nameof(AlphaIsExpaned), typeof(Visibility), typeof(GammaTransferPage), new PropertyMetadata(Visibility.Collapsed));


        #endregion


        public void ResetAlpha()
        {
            this.AlphaToggleSwitch.IsOn = false;
            this.AlphaOffsetSlider.Value = 0;
            this.AlphaExponentSlider.Value = 100;
            this.AlphaAmplitudeSlider.Value = 100;
        }
        public void FollowAlpha(GammaTransferAdjustment adjustment)
        {
            this.AlphaToggleSwitch.IsOn = !adjustment.AlphaDisable;
            this.AlphaOffsetSlider.Value = adjustment.AlphaOffset * 100.0f;
            this.AlphaExponentSlider.Value = adjustment.AlphaExponent * 100.0f;
            this.AlphaAmplitudeSlider.Value = adjustment.AlphaAmplitude * 100.0f;
        }

        public void ConstructStringsAlpha(string title, string offset, string exponent, string amplitude)
        {
            this.AlphaTextBlock.Text = offset;
            this.AlphaOffsetTextBlock.Text = offset;
            this.AlphaExponentTextBlock.Text = exponent;
            this.AlphaAmplitudeTextBlock.Text = amplitude;
        }


        public void ConstructAlphaDisable()
        {
            this.AlphaTitleGrid.Tapped += (s, e) =>
            {
                switch (this.AlphaIsExpaned)
                {
                    case Visibility.Visible:
                        this.AlphaIsExpaned = Visibility.Collapsed;
                        break;
                    case Visibility.Collapsed:
                        this.AlphaIsExpaned = Visibility.Visible;
                        break;
                }
            };

            this.AlphaToggleSwitch.Toggled += (s, e) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    bool disable = !this.AlphaToggleSwitch.IsOn;
                    if (adjustment.AlphaDisable == disable) return;


                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment alpha disable");

                    var previous = adjustment.AlphaDisable;
                    history.UndoActions.Push(()=> 
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.AlphaDisable = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.AlphaDisable = disable;
                    this.ViewModel.Invalidate();
                }
            };
        }


        public void ConstructAlphaOffset()
        {
            this.AlphaOffsetSlider.Value = 0;
            this.AlphaOffsetSlider.Minimum = 0;
            this.AlphaOffsetSlider.Maximum = 100;

            this.AlphaOffsetSlider.SliderBrush = this.AlphaLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.AlphaOffsetSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment alpha offset");

                    adjustment.CacheAlphaOffset();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.AlphaOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float offset = (float)value / 100.0f;

                    adjustment.AlphaOffset = offset;
                    this.ViewModel.Invalidate();
                }
            };
            this.AlphaOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float offset = (float)value / 100.0f;


                    var previous = adjustment.StartingAlphaOffset;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.AlphaOffset = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.AlphaOffset = offset;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

               
        public void ConstructAlphaExponent()
        {
            this.AlphaExponentSlider.Value = 100;
            this.AlphaExponentSlider.Minimum = 0;
            this.AlphaExponentSlider.Maximum = 100;

            this.AlphaExponentSlider.SliderBrush = this.AlphaLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.AlphaExponentSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment alpha exponent");

                    adjustment.CacheAlphaExponent();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.AlphaExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float exponent = (float)value / 100.0f;

                    adjustment.AlphaExponent = exponent;
                    this.ViewModel.Invalidate();
                }
            };
            this.AlphaExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float exponent = (float)value / 100.0f;


                    var previous = adjustment.StartingAlphaExponent;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.AlphaExponent = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.AlphaExponent = exponent;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }


        public void ConstructAlphaAmplitude()
        {
            this.AlphaAmplitudeSlider.Value = 100;
            this.AlphaAmplitudeSlider.Minimum = 0;
            this.AlphaAmplitudeSlider.Maximum = 100;

            this.AlphaAmplitudeSlider.SliderBrush = this.AlphaLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.AlphaAmplitudeSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment alpha amplitude");

                    adjustment.CacheAlphaAmplitude();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.AlphaAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float amplitude = (float)value / 100.0f;

                    adjustment.AlphaAmplitude = amplitude;
                    this.ViewModel.Invalidate();
                }
            };
            this.AlphaAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float amplitude = (float)value / 100.0f;


                    var previous = adjustment.StartingAlphaAmplitude;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.AlphaAmplitude = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.AlphaAmplitude = amplitude;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

    }
}
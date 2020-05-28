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


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s red visibility. </summary>
        public Visibility RedIsExpaned
        {
            get { return (Visibility)GetValue(RedIsExpanedProperty); }
            set { SetValue(RedIsExpanedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.RedIsExpaned" /> dependency property. </summary>
        public static readonly DependencyProperty RedIsExpanedProperty = DependencyProperty.Register(nameof(RedIsExpaned), typeof(Visibility), typeof(GammaTransferPage), new PropertyMetadata(Visibility.Collapsed));


        #endregion


        public void ResetRed()
        {
            this.RedCheckBox.IsOn = true;
            this.RedOffsetSlider.Value = 0;
            this.RedExponentSlider.Value = 100;
            this.RedAmplitudeSlider.Value = 100;
        }
        public void FollowRed(GammaTransferAdjustment adjustment)
        {
            this.RedCheckBox.IsOn = !adjustment.RedDisable;
            this.RedOffsetSlider.Value = adjustment.RedOffset * 100.0f;
            this.RedExponentSlider.Value = adjustment.RedExponent * 100.0f;
            this.RedAmplitudeSlider.Value = adjustment.RedAmplitude * 100.0f;
        }

        public void ConstructStringsRed(string offset, string exponent, string amplitude)
        {
            this.RedOffsetTextBlock.Text = offset;
            this.RedExponentTextBlock.Text = exponent;
            this.RedAmplitudeTextBlock.Text = amplitude;
        }


        public void ConstructRedDisable()
        {
            this.RedRelativePanel.Tapped += (s, e) =>
            {
                switch (this.RedIsExpaned)
                {
                    case Visibility.Visible:
                        this.RedIsExpaned = Visibility.Collapsed;
                        break;
                    case Visibility.Collapsed:
                        this.RedIsExpaned = Visibility.Visible;
                        break;
                }
            };

            this.RedCheckBox.Toggled += (s, e) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    bool disable = !this.RedCheckBox.IsOn;
                    if (adjustment.RedDisable == disable) return;


                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment red disable");

                    var previous = adjustment.RedDisable;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.RedDisable = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.RedDisable = disable;
                    this.ViewModel.Invalidate();
                }
            };
        }


        public void ConstructRedOffset()
        {
            this.RedOffsetSlider.Value = 0;
            this.RedOffsetSlider.Minimum = 0;
            this.RedOffsetSlider.Maximum = 100;

            this.RedOffsetSlider.SliderBrush = this.RedLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.RedOffsetSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment red offset");

                    adjustment.CacheRedOffset();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.RedOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float offset = (float)value / 100.0f;

                    adjustment.RedOffset = offset;
                    this.ViewModel.Invalidate();
                }
            };
            this.RedOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float offset = (float)value / 100.0f;


                    var previous = adjustment.StartingRedOffset;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.RedOffset = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.RedOffset = offset;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }


        public void ConstructRedExponent()
        {
            this.RedExponentSlider.Value = 100;
            this.RedExponentSlider.Minimum = 0;
            this.RedExponentSlider.Maximum = 100;

            this.RedExponentSlider.SliderBrush = this.RedLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.RedExponentSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment red exponent");

                    adjustment.CacheRedExponent();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.RedExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float exponent = (float)value / 100.0f;

                    adjustment.RedExponent = exponent;
                    this.ViewModel.Invalidate();
                }
            };
            this.RedExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float exponent = (float)value / 100.0f;


                    var previous = adjustment.StartingRedExponent;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.RedExponent = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.RedExponent = exponent;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }


        public void ConstructRedAmplitude()
        {
            this.RedAmplitudeSlider.Value = 100;
            this.RedAmplitudeSlider.Minimum = 0;
            this.RedAmplitudeSlider.Maximum = 100;

            this.RedAmplitudeSlider.SliderBrush = this.RedLeftBrush;


            //History
            LayersPropertyHistory history = null;


            this.RedAmplitudeSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    history = new LayersPropertyHistory("Set gamma transfer adjustment red amplitude");

                    adjustment.CacheRedAmplitude();
                    this.ViewModel.Invalidate(InvalidateMode.Thumbnail);
                }
            };
            this.RedAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float amplitude = (float)value / 100.0f;

                    adjustment.RedAmplitude = amplitude;
                    this.ViewModel.Invalidate();
                }
            };
            this.RedAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.Adjustment is GammaTransferAdjustment adjustment)
                {
                    float amplitude = (float)value / 100.0f;


                    var previous = adjustment.StartingRedAmplitude;
                    history.UndoActions.Push(() =>
                    {
                        GammaTransferAdjustment adjustment2 = adjustment;

                        adjustment2.RedAmplitude = previous;
                    });

                    this.ViewModel.HistoryPush(history);


                    adjustment.RedAmplitude = amplitude;
                    this.ViewModel.Invalidate(InvalidateMode.HD);
                }
            };
        }

    }
}
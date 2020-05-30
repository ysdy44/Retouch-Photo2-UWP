using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
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
            this.RedToggleSwitch.IsOn = false;
            this.RedOffsetSlider.Value = 0;
            this.RedExponentSlider.Value = 100;
            this.RedAmplitudeSlider.Value = 100;
        }
        public void FollowRed(GammaTransferAdjustment adjustment)
        {
            this.RedToggleSwitch.IsOn = !adjustment.RedDisable;
            this.RedOffsetSlider.Value = adjustment.RedOffset * 100.0f;
            this.RedExponentSlider.Value = adjustment.RedExponent * 100.0f;
            this.RedAmplitudeSlider.Value = adjustment.RedAmplitude * 100.0f;
        }

        public void ConstructStringsRed(string title, string offset, string exponent, string amplitude)
        {
            this.RedTextBlock.Text = offset;
            this.RedOffsetTextBlock.Text = offset;
            this.RedExponentTextBlock.Text = exponent;
            this.RedAmplitudeTextBlock.Text = amplitude;
        }


        public void ConstructRedDisable()
        {
            this.RedTitleGrid.Tapped += (s, e) =>
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

            this.RedToggleSwitch.Toggled += (s, e) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        bool disable = !this.RedToggleSwitch.IsOn;
                        if (adjustment.RedDisable == disable) return;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment red disable");

                        var previous = adjustment.RedDisable;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.RedDisable = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.RedDisable = disable;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
        }


        public void ConstructRedOffset()
        {
            this.RedOffsetSlider.Value = 0;
            this.RedOffsetSlider.Minimum = 0;
            this.RedOffsetSlider.Maximum = 100;

            this.RedOffsetSlider.SliderBrush = this.RedLeftBrush;

            this.RedOffsetSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheRedOffset();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.RedOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float offset = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.RedOffset = offset;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.RedOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float offset = (float)value / 100.0f;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment red offset");

                        var previous = adjustment.StartingRedOffset;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.RedOffset = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.RedOffset = offset;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }


        public void ConstructRedExponent()
        {
            this.RedExponentSlider.Value = 100;
            this.RedExponentSlider.Minimum = 0;
            this.RedExponentSlider.Maximum = 100;

            this.RedExponentSlider.SliderBrush = this.RedLeftBrush;

            this.RedExponentSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheRedExponent();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.RedExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float exponent = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.RedExponent = exponent;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.RedExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float exponent = (float)value / 100.0f;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment red exponent");

                        var previous = adjustment.StartingRedExponent;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.RedExponent = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.RedExponent = exponent;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }


        public void ConstructRedAmplitude()
        {
            this.RedAmplitudeSlider.Value = 100;
            this.RedAmplitudeSlider.Minimum = 0;
            this.RedAmplitudeSlider.Maximum = 100;

            this.RedAmplitudeSlider.SliderBrush = this.RedLeftBrush;

            this.RedAmplitudeSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheRedAmplitude();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.RedAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float amplitude = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.RedAmplitude = amplitude;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.RedAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float amplitude = (float)value / 100.0f;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment red amplitude");

                        var previous = adjustment.StartingRedAmplitude;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.RedAmplitude = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.RedAmplitude = amplitude;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}
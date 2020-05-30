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
            this.GreenTextBlock.Text = offset;
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
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        bool disable = !this.GreenToggleSwitch.IsOn;
                        if (adjustment.GreenDisable == disable) return;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment green disable");

                        var previous = adjustment.GreenDisable;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.GreenDisable = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.GreenDisable = disable;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
        }


        public void ConstructGreenOffset()
        {
            this.GreenOffsetSlider.Value = 0;
            this.GreenOffsetSlider.Minimum = 0;
            this.GreenOffsetSlider.Maximum = 100;

            this.GreenOffsetSlider.SliderBrush = this.GreenLeftBrush;

            this.GreenOffsetSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheGreenOffset();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.GreenOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float offset = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.GreenOffset = offset;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.GreenOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float offset = (float)value / 100.0f;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment green offset");

                        var previous = adjustment.StartingGreenOffset;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.GreenOffset = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.GreenOffset = offset;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }


        public void ConstructGreenExponent()
        {
            this.GreenExponentSlider.Value = 100;
            this.GreenExponentSlider.Minimum = 0;
            this.GreenExponentSlider.Maximum = 100;

            this.GreenExponentSlider.SliderBrush = this.GreenLeftBrush;

            this.GreenExponentSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheGreenExponent();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.GreenExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float exponent = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.GreenExponent = exponent;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.GreenExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float exponent = (float)value / 100.0f;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment green exponent");

                        var previous = adjustment.StartingGreenExponent;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.GreenExponent = previous;
                        });
                        
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.GreenExponent = exponent;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }


        public void ConstructGreenAmplitude()
        {
            this.GreenAmplitudeSlider.Value = 100;
            this.GreenAmplitudeSlider.Minimum = 0;
            this.GreenAmplitudeSlider.Maximum = 100;

            this.GreenAmplitudeSlider.SliderBrush = this.GreenLeftBrush;

            this.GreenAmplitudeSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheGreenAmplitude();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.GreenAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float amplitude = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.GreenAmplitude = amplitude;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.GreenAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float amplitude = (float)value / 100.0f;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment green amplitude");

                        var previous = adjustment.StartingGreenAmplitude;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.GreenAmplitude = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.GreenAmplitude = amplitude;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}
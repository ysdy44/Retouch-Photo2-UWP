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


        /// <summary> Gets or sets <see cref = "GammaTransferPage" />'s alpha visibility. </summary>
        public Visibility AlphaIsExpaned
        {
            get { return (Visibility)GetValue(AlphaIsExpanedProperty); }
            set { SetValue(AlphaIsExpanedProperty, value); }
        }
        /// <summary> Identifies the <see cref = "GammaTransferPage.AlphaIsExpaned" /> dependency property. </summary>
        public static readonly DependencyProperty AlphaIsExpanedProperty = DependencyProperty.Register(nameof(AlphaIsExpaned), typeof(Visibility), typeof(GammaTransferPage), new PropertyMetadata(Visibility.Collapsed));


        #endregion


        private void ResetAlpha()
        {
            this.AlphaToggleSwitch.IsOn = false;
            this.AlphaOffsetSlider.Value = 0;
            this.AlphaExponentSlider.Value = 100;
            this.AlphaAmplitudeSlider.Value = 100;
        }
        private void FollowAlpha(GammaTransferAdjustment adjustment)
        {
            this.AlphaToggleSwitch.IsOn = !adjustment.AlphaDisable;
            this.AlphaOffsetSlider.Value = adjustment.AlphaOffset * 100.0f;
            this.AlphaExponentSlider.Value = adjustment.AlphaExponent * 100.0f;
            this.AlphaAmplitudeSlider.Value = adjustment.AlphaAmplitude * 100.0f;
        }

        private void ConstructStringsAlpha(string title, string offset, string exponent, string amplitude)
        {
            this.AlphaTextBlock.Text = offset;
            this.AlphaOffsetTextBlock.Text = offset;
            this.AlphaExponentTextBlock.Text = exponent;
            this.AlphaAmplitudeTextBlock.Text = amplitude;
        }


        private void ConstructAlphaDisable()
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
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        bool disable = !this.AlphaToggleSwitch.IsOn;
                        if (adjustment.AlphaDisable == disable) return;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment alpha disable");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.AlphaDisable;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is GammaTransferAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.AlphaDisable = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.AlphaDisable = disable;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
        }


        private void ConstructAlphaOffset()
        {
            this.AlphaOffsetSlider.Value = 0;
            this.AlphaOffsetSlider.Minimum = 0;
            this.AlphaOffsetSlider.Maximum = 100;

            this.AlphaOffsetSlider.SliderBrush = this.AlphaLeftBrush;

            this.AlphaOffsetSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheAlphaOffset();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.AlphaOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float offset = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.AlphaOffset = offset;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.AlphaOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float offset = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment alpha offset");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingAlphaOffset;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is GammaTransferAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.AlphaOffset = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.AlphaOffset = offset;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }


        private void ConstructAlphaExponent()
        {
            this.AlphaExponentSlider.Value = 100;
            this.AlphaExponentSlider.Minimum = 0;
            this.AlphaExponentSlider.Maximum = 100;

            this.AlphaExponentSlider.SliderBrush = this.AlphaLeftBrush;

            this.AlphaExponentSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheAlphaExponent();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.AlphaExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float exponent = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.AlphaExponent = exponent;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.AlphaExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float exponent = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment alpha exponent");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingAlphaExponent;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is GammaTransferAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.AlphaExponent = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.AlphaExponent = exponent;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }


        private void ConstructAlphaAmplitude()
        {
            this.AlphaAmplitudeSlider.Value = 100;
            this.AlphaAmplitudeSlider.Minimum = 0;
            this.AlphaAmplitudeSlider.Maximum = 100;

            this.AlphaAmplitudeSlider.SliderBrush = this.AlphaLeftBrush;

            this.AlphaAmplitudeSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheAlphaAmplitude();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.AlphaAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float amplitude = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layerage.RefactoringParentsRender();
                        adjustment.AlphaAmplitude = amplitude;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.AlphaAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float amplitude = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment alpha amplitude");

                        var previous = layer.Filter.Adjustments.IndexOf(adjustment);
                        var previous1 = adjustment.StartingAlphaAmplitude;
                        history.UndoAction += () =>
                        {
                            if (previous < 0) return;
                            if (previous > layer.Filter.Adjustments.Count - 1) return;
                            if (layer.Filter.Adjustments[previous] is GammaTransferAdjustment adjustment2)
                            {
                                //Refactoring
                                layer.IsRefactoringRender = true;
                                layer.IsRefactoringIconRender = true;
                                adjustment2.AlphaAmplitude = previous1;
                            }
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        layerage.RefactoringParentsRender();
                        layerage.RefactoringParentsIconRender();
                        adjustment.AlphaAmplitude = amplitude;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}
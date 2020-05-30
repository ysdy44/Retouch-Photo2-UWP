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
            this.BlueTextBlock.Text = offset;
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
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        bool disable = !this.BlueToggleSwitch.IsOn;
                        if (adjustment.BlueDisable == disable) return;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment blue disable");

                        var previous = adjustment.BlueDisable;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.BlueDisable = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.BlueDisable = disable;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
        }


        public void ConstructBlueOffset()
        {
            this.BlueOffsetSlider.Value = 0;
            this.BlueOffsetSlider.Minimum = 0;
            this.BlueOffsetSlider.Maximum = 100;

            this.BlueOffsetSlider.SliderBrush = this.BlueLeftBrush;

            this.BlueOffsetSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheBlueOffset();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.BlueOffsetSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float offset = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.BlueOffset = offset;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.BlueOffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float offset = (float)value / 100.0f;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment blue offset");

                        var previous = adjustment.StartingBlueOffset;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.BlueOffset = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.BlueOffset = offset;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }


        public void ConstructBlueExponent()
        {
            this.BlueExponentSlider.Value = 100;
            this.BlueExponentSlider.Minimum = 0;
            this.BlueExponentSlider.Maximum = 100;

            this.BlueExponentSlider.SliderBrush = this.BlueLeftBrush;

            this.BlueExponentSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheBlueExponent();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.BlueExponentSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float exponent = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.BlueExponent = exponent;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.BlueExponentSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float exponent = (float)value / 100.0f;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment blue exponent");

                        var previous = adjustment.StartingBlueExponent;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.BlueExponent = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.BlueExponent = exponent;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }


        public void ConstructBlueAmplitude()
        {
            this.BlueAmplitudeSlider.Value = 100;
            this.BlueAmplitudeSlider.Minimum = 0;
            this.BlueAmplitudeSlider.Maximum = 100;

            this.BlueAmplitudeSlider.SliderBrush = this.BlueLeftBrush;

            this.BlueAmplitudeSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        adjustment.CacheBlueAmplitude();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.BlueAmplitudeSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float amplitude = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.BlueAmplitude = amplitude;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.BlueAmplitudeSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is GammaTransferAdjustment adjustment)
                    {
                        float amplitude = (float)value / 100.0f;
                        
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set gamma transfer adjustment blue amplitude");

                        var previous = adjustment.StartingBlueAmplitude;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.BlueAmplitude = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.BlueAmplitude = amplitude;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}
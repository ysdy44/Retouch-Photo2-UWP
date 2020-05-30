using Retouch_Photo2.Adjustments.Icons;
using Retouch_Photo2.Adjustments.Models;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Adjustments.Pages
{
    /// <summary>
    /// Page of <see cref = "BrightnessAdjustment"/>.
    /// </summary>
    public sealed partial class BrightnessPage : IAdjustmentGenericPage<BrightnessAdjustment>
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;

        //@Generic
        public BrightnessAdjustment Adjustment { get; set; }

        //@Construct
        public BrightnessPage()
        {
            this.InitializeComponent();
            this.ConstructStrings();

            this.ConstructWhiteLight();
            this.ConstructWhiteDark();

            this.ConstructBlackLight();
            this.ConstructBlackDark();
        }
    }

    /// <summary>
    /// Page of <see cref = "BrightnessAdjustment"/>.
    /// </summary>
    public sealed partial class BrightnessPage : IAdjustmentGenericPage<BrightnessAdjustment>
    {

        //Strings
        public void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Text = resource.GetString("/Adjustments/Brightness");

            this.WhiteToLightTextBlock.Text = resource.GetString("/Adjustments/Brightness_WhiteToLight");
            this.WhiteToDarkTextBlock.Text = resource.GetString("/Adjustments/Brightness_WhiteToDark");

            this.BlackToLightTextBlock.Text = resource.GetString("/Adjustments/Brightness_BlackToLight");
            this.BlackToDarkTextBlock.Text = resource.GetString("/Adjustments/Brightness_BlackToDark");
        }


        //@Content
        public AdjustmentType Type => AdjustmentType.Brightness;
        public FrameworkElement Icon { get; } = new BrightnessIcon();
        public FrameworkElement Self => this;
        public string Text { get; private set; }


        public IAdjustment GetNewAdjustment() => new BrightnessAdjustment();


        public void Reset()
        {
            this.WhiteLightSlider.Value = 100;
            this.WhiteDarkSlider.Value = 100;
            this.BlackLightSlider.Value = 0;
            this.BlackDarkSlider.Value = 0;

            if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
            {
                ILayer layer = layerage.Self;

                if (this.Adjustment is BrightnessAdjustment adjustment)
                {
                    //History
                    LayersPropertyHistory history = new LayersPropertyHistory("Set brightness adjustment");

                    var previous1 = adjustment.WhiteLight;
                    var previous2 = adjustment.WhiteDark;
                    var previous3 = adjustment.BlackLight;
                    var previous4 = adjustment.BlackDark;
                    history.UndoActions.Push(() =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.WhiteLight = previous1;
                        adjustment.WhiteDark = previous2;
                        adjustment.BlackLight = previous3;
                        adjustment.BlackDark = previous4;
                    });

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    adjustment.WhiteLight = 1.0f;
                    adjustment.WhiteDark = 1.0f;
                    adjustment.BlackLight = 0.0f;
                    adjustment.BlackDark = 0.0f;

                    //History
                    this.ViewModel.HistoryPush(history);

                    this.ViewModel.Invalidate();//Invalidate
                }
            }
        }
        public void Follow(BrightnessAdjustment adjustment)
        {
            this.WhiteLightSlider.Value = adjustment.WhiteLight * 100;
            this.WhiteDarkSlider.Value = adjustment.WhiteDark * 100;

            this.BlackLightSlider.Value = adjustment.BlackLight * 100;
            this.BlackDarkSlider.Value = adjustment.BlackDark * 100;
        }

    }

    /// <summary>
    /// Page of <see cref = "BrightnessAdjustment"/>.
    /// </summary>
    public sealed partial class BrightnessPage : IAdjustmentGenericPage<BrightnessAdjustment>
    { 

        public void ConstructWhiteLight()
        {
            this.WhiteLightSlider.Value = 100;
            this.WhiteLightSlider.Minimum = 50;
            this.WhiteLightSlider.Maximum = 100;

            this.WhiteLightSlider.SliderBrush = this.WhiteLightBrush;

            this.WhiteLightSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        adjustment.CacheWhiteLight();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.WhiteLightSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        float light = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.WhiteLight = light;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.WhiteLightSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        float light = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set brightness adjustment white light");

                        var previous = adjustment.StartingWhiteLight;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.WhiteLight = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.WhiteLight = light;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

        public void ConstructWhiteDark()
        {
            this.WhiteDarkSlider.Value = 100;
            this.WhiteDarkSlider.Minimum = 50;
            this.WhiteDarkSlider.Maximum = 100;

            this.WhiteDarkSlider.SliderBrush = this.WhiteDarkBrush;
            
            this.WhiteDarkSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        adjustment.CacheWhiteDark();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.WhiteDarkSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        float dark = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.WhiteDark = dark;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.WhiteDarkSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        float dark = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set brightness adjustment white dark");

                        var previous = adjustment.StartingWhiteDark;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.WhiteDark = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.WhiteDark = dark;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }


        public void ConstructBlackLight()
        {
            this.BlackLightSlider.Value = 0;
            this.BlackLightSlider.Minimum = 0;
            this.BlackLightSlider.Maximum = 50;

            this.BlackLightSlider.SliderBrush = this.BlackLightBrush;

            this.BlackLightSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        adjustment.CacheBlackLight();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.BlackLightSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        float light = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.BlackLight = light;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.BlackLightSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        float light = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set brightness adjustment black light");

                        var previous = adjustment.StartingBlackLight;
                        history.UndoActions.Push(() =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.BlackLight = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.BlackLight = light;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

        public void ConstructBlackDark()
        {
            this.BlackDarkSlider.Value = 0;
            this.BlackDarkSlider.Minimum = 0;
            this.BlackDarkSlider.Maximum = 50;

            this.BlackDarkSlider.SliderBrush = this.BlackDarkBrush;

            this.BlackDarkSlider.ValueChangeStarted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;

                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        adjustment.CacheBlackDark();
                        this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
                    }
                }
            };
            this.BlackDarkSlider.ValueChangeDelta += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;
                    
                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        float dark = (float)value / 100.0f;

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        adjustment.BlackDark = dark;

                        this.ViewModel.Invalidate();//Invalidate
                    }
                }
            };
            this.BlackDarkSlider.ValueChangeCompleted += (s, value) =>
            {
                if (this.SelectionViewModel.SelectionLayerage is Layerage layerage)
                {
                    ILayer layer = layerage.Self;
                    
                    if (this.Adjustment is BrightnessAdjustment adjustment)
                    {
                        float dark = (float)value / 100.0f;

                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set brightness adjustment black dark");

                        var previous = adjustment.StartingBlackDark;
                        history.UndoActions.Push(() =>
                        {  
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            adjustment.BlackDark = previous;
                        });

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        adjustment.BlackDark = dark;

                        //History
                        this.ViewModel.HistoryPush(history);
                        
                        this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
                    }
                }
            };
        }

    }
}
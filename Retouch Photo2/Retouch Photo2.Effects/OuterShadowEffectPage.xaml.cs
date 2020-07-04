using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers;
using Windows.UI;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        
        //@Construct
        /// <summary>
        /// Initializes a OuterShadowEffectPage. 
        /// </summary>
        public OuterShadowEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();

            this.ConstructOuterShadow_Radius();
            this.ConstructOuterShadow_Opacity();
            this.ConstructOuterShadow_Offset();
            this.ConstructOuterShadow_Angle();
            this.ConstructColor1();
            this.ConstructColor2();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/OuterShadow");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/OuterShadow_Radius");
            this.OpacityTextBlock.Text = resource.GetString("/Effects/OuterShadow_Opacity");
            this.OffsetTextBlock.Text = resource.GetString("/Effects/OuterShadow_Offset");

            this.AngleTextBlock.Text = resource.GetString("/Effects/OuterShadow_Angle");
            this.ColorTextBlock.Text = resource.GetString("/Effects/OuterShadow_Color");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.OuterShadow;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new OuterShadowIcon()
        };
        
        public void Reset()
        {
            this.RadiusSlider.Value = 12;
            this.OpacitySlider.Value = 50;
            this.SolidColorBrush.Color = Windows.UI.Colors.Black;

            this.OffsetSlider.Value = 0;
            this.AnglePicker.Radians = 0.78539816339744830961566084581988f;// 1/4 π

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect outer shadow");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous1 = layer.Effect.OuterShadow_Radius;
                var previous2 = layer.Effect.OuterShadow_Opacity;
                var previous3 = layer.Effect.OuterShadow_Color;
                var previous4 = layer.Effect.OuterShadow_Offset;
                var previous5 = layer.Effect.OuterShadow_Angle;
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Effect.OuterShadow_Radius = previous1;
                    layer.Effect.OuterShadow_Opacity = previous2;
                    layer.Effect.OuterShadow_Color = previous3;
                    layer.Effect.OuterShadow_Offset = previous4;
                    layer.Effect.OuterShadow_Angle = previous5;
                };
                
                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Effect.OuterShadow_Radius = 12.0f;
                layer.Effect.OuterShadow_Opacity = 0.5f;
                layer.Effect.OuterShadow_Color = Windows.UI.Colors.Black;
                layer.Effect.OuterShadow_Offset = 0;
                layer.Effect.OuterShadow_Angle = 0.78539816339744830961566084581988f;// 1/4 π
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.OuterShadow_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.RadiusSlider.Value = effect.OuterShadow_Radius;
            this.OpacitySlider.Value = effect.OuterShadow_Opacity * 100.0f;
            this.SolidColorBrush.Color = effect.OuterShadow_Color;

            this.OffsetSlider.Value = effect.OuterShadow_Offset;
            this.AnglePicker.Radians = effect.OuterShadow_Angle;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructButton()
        {
            this.Button.Toggled += (isOn) =>
            {
                this.MethodViewModel.EffectChanged<bool>
                (
                    set: (effect) => effect.OuterShadow_IsOn = isOn,

                    historyTitle: "Set effect outer shadow is on",
                    getHistory: (effect) => effect.OuterShadow_IsOn,
                    setHistory: (effect, previous) => effect.OuterShadow_IsOn = previous
                );
            };
        }


        //OuterShadow_Radius
        private void ConstructOuterShadow_Radius()
        {
            this.RadiusSlider.Minimum = 0;
            this.RadiusSlider.Maximum = 100;
            this.RadiusSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow());
            this.RadiusSlider.ValueChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Radius = (float)value);
            this.RadiusSlider.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.OuterShadow_Radius = (float)value,

                historyTitle: "Set effect outer shadow radius",
                getHistory: (effect) => effect.StartingOuterShadow_Radius,
                setHistory: (effect, previous) => effect.OuterShadow_Radius = previous
            );
        }


        //OuterShadow_Opacity
        private void ConstructOuterShadow_Opacity()
        {
            this.OpacitySlider.Minimum = 0;
            this.OpacitySlider.Maximum = 100;
            this.OpacitySlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow());
            this.OpacitySlider.ValueChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Opacity = (float)value / 100.0f);
            this.OpacitySlider.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.OuterShadow_Opacity = (float)value / 100.0f,

                historyTitle: "Set effect outer shadow opacity",
                getHistory: (effect) => effect.StartingOuterShadow_Opacity,
                setHistory: (effect, previous) => effect.OuterShadow_Opacity = previous
            );
        }


        //OuterShadow_Offset
        private void ConstructOuterShadow_Offset()
        {
            this.OffsetSlider.Minimum = 0;
            this.OffsetSlider.Maximum = 100;
            this.OffsetSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow());
            this.OffsetSlider.ValueChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Offset = (float)value);
            this.OffsetSlider.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.OuterShadow_Offset = (float)value,

                historyTitle: "Set effect outer shadow offset",
                getHistory: (effect) => effect.StartingOuterShadow_Offset,
                setHistory: (effect, previous) => effect.OuterShadow_Offset = previous
            );
        }


        //Angle
        private void ConstructOuterShadow_Angle()
        {
            //this.AnglePicker.Minimum = 0;
            //this.AnglePicker.Maximum = FanKit.Math.PiTwice;
            this.AnglePicker.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow());
            this.AnglePicker.ValueChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Angle = (float)value);
            this.AnglePicker.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.OuterShadow_Angle = (float)value,

                historyTitle: "Set effect outer shadow angle",
                getHistory: (effect) => effect.StartingOuterShadow_Angle,
                setHistory: (effect, previous) => effect.OuterShadow_Angle = previous
            );
        }


        //Color
        private void ConstructColor1()
        {
            this.ColorBorder.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorBorder);
                this.ColorPicker.Color = this.SolidColorBrush.Color;
            };
            this.ColorPicker.ColorChanged += (s, value) => this.MethodViewModel.EffectChangeCompleted<Color>
            (
                set: (effect) => effect.OuterShadow_Color = (Color)value,

                historyTitle: "Set effect outer shadow color",
                getHistory: (effect) => effect.OuterShadow_Color,
                setHistory: (effect, previous) => effect.OuterShadow_Color = previous
            );
        }

        private void ConstructColor2()
        {
            this.ColorPicker.ColorChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow()); 
            this.ColorPicker.ColorChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Color = (Color)value);
            this.ColorPicker.ColorChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<Color>
            (
                set: (effect) => effect.OuterShadow_Color = (Color)value,

                historyTitle: "Set effect outer shadow color",
                getHistory: (effect) => effect.StartingOuterShadow_Color,
                setHistory: (effect, previous) => effect.OuterShadow_Color = previous
            );
        }


    }
}
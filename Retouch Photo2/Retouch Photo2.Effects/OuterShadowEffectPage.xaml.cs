using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public OuterShadowEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();


            //Radius
            this.RadiusSlider.ValueChangeStarted += (s, value) => { };
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.OuterShadow_Radius = radius;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) => { };


            //Opacity
            this.OpacitySlider.Maximum = 1;
            this.OpacitySlider.ValueChangeStarted += (s, value) => { };
            this.OpacitySlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.OuterShadow_Opacity = radius;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OpacitySlider.ValueChangeCompleted += (s, value) => { };



            //Radius
            this.OffsetSlider.ValueChangeStarted += (s, value) => { };
            this.OffsetSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.OuterShadow_Offset = radius;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OffsetSlider.ValueChangeCompleted += (s, value) => { };
                       

            //Angle
            this.AnglePicker.ValueChangeStarted += (s, value) => { };
            this.AnglePicker.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.OuterShadow_Angle = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AnglePicker.ValueChangeCompleted += (s, value) => { };
            

            this.ColorBorder.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorBorder);
                this.ColorPicker.Color = this.SolidColorBrush.Color;
            };
            this.ColorPicker.ColorChange += (s, value) =>
            {
                this.SolidColorBrush.Color = value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.OuterShadow_Color = value;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
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
        public EffectType Type => EffectType.OuterShadow;
        public FrameworkElement Page => this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new OuterShadowIcon()
        };


        public void Reset()
        {
            this.RadiusSlider.Value = 0;
            this.OpacitySlider.Value = 0.5f;
            this.SolidColorBrush.Color = Windows.UI.Colors.Black;

            this.OffsetSlider.Value = 0;
            this.AnglePicker.Radians = 0.78539816339744830961566084581988f;// 1/4 π
        }
        public void ResetEffect(Effect effect)
        {
            effect.OuterShadow_Radius = 0;
            effect.OuterShadow_Opacity = 0.5f;
            effect.OuterShadow_Color = Windows.UI.Colors.Black;

            effect.OuterShadow_Offset = 0;
            effect.OuterShadow_Angle = 0.78539816339744830961566084581988f;// 1/4 π
        }
        public void FollowEffect(Effect effect)
        {
            this.RadiusSlider.Value = effect.OuterShadow_Radius;
            this.OpacitySlider.Value = effect.OuterShadow_Opacity;
            this.SolidColorBrush.Color = effect.OuterShadow_Color;

            this.OffsetSlider.Value = effect.OuterShadow_Offset;
            this.AnglePicker.Radians = effect.OuterShadow_Angle;

            this.ToggleSwitch.IsOn = effect.OuterShadow_IsOn;
        }
        public void OverwritingEffect(Effect effect)
        {
            effect.OuterShadow_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}
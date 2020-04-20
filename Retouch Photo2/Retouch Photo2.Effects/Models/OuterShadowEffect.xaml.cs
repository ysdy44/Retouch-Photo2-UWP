using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "OuterShadowEffect"/>.
    /// </summary>
    public sealed partial class OuterShadowEffect : Page, IEffect
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public OuterShadowEffect()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.RadiusSlider.ValueChanged += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.EffectManager.OuterShadow_Radius = (float)e.NewValue;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OpacitySlider.ValueChanged += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.EffectManager.OuterShadow_Opacity = (float)(e.NewValue / 100.0f);
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.OffsetSlider.ValueChanged += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.EffectManager.OuterShadow_Offset = (float)e.NewValue;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AnglePicker.RadiansChange += (s, radians) =>
            {  
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.EffectManager.OuterShadow_Angle = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };

            this.ColorButton.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorButton);
                this.ColorPicker.Color = this.SolidColorBrush.Color;
            };
            this.ColorPicker.ColorChange += (s, value) =>
            {
                this.SolidColorBrush.Color = value;

                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.EffectManager.OuterShadow_Color = value;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }

    /// <summary>
    /// Page of <see cref = "OuterShadowEffect"/>.
    /// </summary>
    public sealed partial class OuterShadowEffect : Page, IEffect
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Text = resource.GetString("/Effects/OuterShadow");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/OuterShadow_Radius");
            this.OpacityTextBlock.Text = resource.GetString("/Effects/OuterShadow_Opacity");
            this.OffsetTextBlock.Text = resource.GetString("/Effects/OuterShadow_Offset");

            this.AngleTextBlock.Text = resource.GetString("/Effects/OuterShadow_Angle");
            this.ColorTextBlock.Text = resource.GetString("/Effects/OuterShadow_Color");
        }

        //@Content
        public EffectType Type => EffectType.OuterShadow;
        public FrameworkElement Page => this;
        public Control Button => this._button;
        public ToggleSwitch ToggleSwitch => this._button.ToggleSwitch;
        private EffectButton _button = new EffectButton
        {
            Icon = new OuterShadowIcon()
        };


        public void Reset()
        {
            this.RadiusSlider.Value = 0;
            this.OpacitySlider.Value = 50f;
            this.SolidColorBrush.Color = Windows.UI.Colors.Black;

            this.OffsetSlider.Value = 0;
            this.AnglePicker.Radians = 0.78539816339744830961566084581988f;// 1/4 π
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.OuterShadow_Radius = 0;
            effectManager.OuterShadow_Opacity = 0.5f;
            effectManager.OuterShadow_Color = Windows.UI.Colors.Black;

            effectManager.OuterShadow_Offset = 0;
            effectManager.OuterShadow_Angle = 0.78539816339744830961566084581988f;// 1/4 π
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.RadiusSlider.Value = effectManager.OuterShadow_Radius;
            this.OpacitySlider.Value = effectManager.OuterShadow_Opacity * 100.0f;
            this.SolidColorBrush.Color = effectManager.OuterShadow_Color;

            this.OffsetSlider.Value = effectManager.OuterShadow_Offset;
            this.AnglePicker.Radians = effectManager.OuterShadow_Angle;

            this.ToggleSwitch.IsOn = effectManager.OuterShadow_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.OuterShadow_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}
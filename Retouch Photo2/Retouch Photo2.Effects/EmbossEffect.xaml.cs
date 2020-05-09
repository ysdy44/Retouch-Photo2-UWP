using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// <see cref="IEffect"/> of EmbossEffect.
    /// </summary>
    public sealed partial class EmbossEffect : Page, IEffect
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public EmbossEffect()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.RadiusSlider.ValueChanged += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.EffectManager.Emboss_Radius = (float)e.NewValue;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AnglePicker.RadiansChange += (s, radians) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.EffectManager.Emboss_Angle = radians;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }

    /// <summary>
    /// <see cref="IEffect"/> of EmbossEffect.
    /// </summary>
    public sealed partial class EmbossEffect : Page, IEffect
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Text = resource.GetString("/Effects/Emboss");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/Emboss_Radius");
            this.AngleTextBlock.Text = resource.GetString("/Effects/Emboss_Angle");
        }

        //@Content
        public EffectType Type => EffectType.Emboss;
        public FrameworkElement Page => this;
        public Control Button => this._button;
        public ToggleSwitch ToggleSwitch => this._button.ToggleSwitch;
        private EffectButton _button = new EffectButton
        {
            Icon = new EmbossIcon()
        };


        public void Reset()
        {
            this.RadiusSlider.Value = 0;
            this.AnglePicker.Radians = 0;
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.Emboss_Radius = 0;
            effectManager.Emboss_Angle = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.RadiusSlider.Value = effectManager.Emboss_Radius;
            this.AnglePicker.Radians = effectManager.Emboss_Angle;

            this.ToggleSwitch.IsOn = effectManager.Emboss_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.Emboss_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}
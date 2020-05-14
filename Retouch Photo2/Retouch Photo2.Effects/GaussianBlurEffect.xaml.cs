using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "GaussianBlurEffect"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffect : Page, IEffect
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public GaussianBlurEffect()
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
                    layer.EffectManager.GaussianBlur_Radius = radius;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) => { };
            
        }
    }

    /// <summary>
    /// Page of <see cref = "GaussianBlurEffect"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffect : Page, IEffect
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Text = resource.GetString("/Effects/GaussianBlur");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/GaussianBlur_Radius");
        }

        //@Content
        public EffectType Type => EffectType.GaussianBlur;
        public FrameworkElement Page => this;
        public Control Button => this._button;
        public ToggleSwitch ToggleSwitch => this._button.ToggleSwitch;
        private EffectButton _button = new EffectButton
        {
            Icon = new GaussianBlurIcon()
        };


        public void Reset()
        {
            this.RadiusSlider.Value = 0;
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.GaussianBlur_Radius = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.RadiusSlider.Value = effectManager.GaussianBlur_Radius;

            this.ToggleSwitch.IsOn = effectManager.GaussianBlur_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.GaussianBlur_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}
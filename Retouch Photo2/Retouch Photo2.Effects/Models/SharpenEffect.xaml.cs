using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "SharpenEffect"/>.
    /// </summary>
    public sealed partial class SharpenEffect : Page, IEffect
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public SharpenEffect()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.AmountSlider.ValueChanged += (s, e) =>
            {
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.EffectManager.Sharpen_Amount = (float)e.NewValue / 10.0f;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }

    /// <summary>
    /// Page of <see cref = "SharpenEffect"/>.
    /// </summary>
    public sealed partial class SharpenEffect : Page, IEffect
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Text = resource.GetString("/Effects/Sharpen");

            this.AmountTextBlock.Text = resource.GetString("/Effects/Sharpen_Amount");
        }

        //@Content
        public EffectType Type => EffectType.Sharpen;
        public FrameworkElement Page => this;
        public Control Button => this._button;
        public ToggleSwitch ToggleSwitch => this._button.ToggleSwitch;
        private EffectButton _button = new EffectButton
        {
            Icon = new SharpenIcon()
        };


        public void Reset()
        {
            this.AmountSlider.Value = 0;
        }
        public void ResetEffectManager(EffectManager effectManager)
        {
            effectManager.Sharpen_Amount = 0;
        }
        public void FollowEffectManager(EffectManager effectManager)
        {
            this.AmountSlider.Value = effectManager.Sharpen_Amount * 10.0f;

            this.ToggleSwitch.IsOn = effectManager.Sharpen_IsOn;
        }
        public void OverwritingEffectManager(EffectManager effectManager)
        {
            effectManager.Sharpen_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}
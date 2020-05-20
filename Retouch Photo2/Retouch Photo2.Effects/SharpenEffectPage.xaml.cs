using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Construct
        public SharpenEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();


            //Radius
            this.AmountSlider.Maximum = 10;
            this.AmountSlider.ValueChangeStarted += (s, value) => { };
            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                float amount = (float)value;
                 
                //Selection
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.Effect.Sharpen_Amount = amount;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
            this.AmountSlider.ValueChangeCompleted += (s, value) => { };

        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/Sharpen");

            this.AmountTextBlock.Text = resource.GetString("/Effects/Sharpen_Amount");
        }

        //@Content
        public EffectType Type => EffectType.Sharpen;
        public FrameworkElement Page => this;
        public ToggleSwitch ToggleSwitch => this.Button.ToggleSwitch;
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new SharpenIcon()
        };


        public void Reset()
        {
            this.AmountSlider.Value = 0;
        }
        public void ResetEffect(Effect effect)
        {
            effect.Sharpen_Amount = 0;
        }
        public void FollowEffect(Effect effect)
        {
            this.AmountSlider.Value = effect.Sharpen_Amount;

            this.ToggleSwitch.IsOn = effect.Sharpen_IsOn;
        }
        public void OverwritingEffect(Effect effect)
        {
            effect.Sharpen_IsOn = this.ToggleSwitch.IsOn;
        }
    }
}
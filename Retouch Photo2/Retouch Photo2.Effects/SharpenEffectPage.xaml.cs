using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        
        //@Construct
        /// <summary>
        /// Initializes a SharpenEffectPage. 
        /// </summary>
        public SharpenEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();
            this.ConstructButton();
            this.ConstructSharpen_Amount();
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
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.Sharpen;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new SharpenIcon()
        };
        
        public void Reset()
        {
            this.AmountSlider.Value = 0;

            this.MethodViewModel.EffectChanged<float>
            (
                set: (effect) => effect.Sharpen_Amount = 0,

                historyTitle: "Set effect sharpen",
                getHistory: (effect) => effect.Sharpen_Amount,
                setHistory: (effect, previous) => effect.Sharpen_Amount = previous
            );
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.Sharpen_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.AmountSlider.Value = effect.Sharpen_Amount;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructButton()
        {
            this.Button.Toggled += (isOn) => this.MethodViewModel.EffectChanged<bool>
            (
                set: (effect) => effect.Straighten_IsOn = isOn,

                historyTitle: "Set effect sharpen is on",
                getHistory: (effect) => effect.Sharpen_IsOn,
                setHistory: (effect, previous) => effect.Sharpen_IsOn = previous
            );
        }


        //Sharpen_Amount
        private void ConstructSharpen_Amount()
        {
            this.AmountSlider.Minimum = 0;
            this.AmountSlider.Maximum = 10;
            this.AmountSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheSharpen());
            this.AmountSlider.ValueChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Sharpen_Amount = (float)value);
            this.AmountSlider.ValueChangeCompleted += (s, value) => this.MethodViewModel.EffectChangeCompleted<float>
            (
                set: (effect) => effect.Sharpen_Amount = (float)value,

                historyTitle: "Set effect sharpen amount",
                getHistory: (effect) => effect.StartingSharpen_Amount,
                setHistory: (effect, previous) => effect.Sharpen_Amount = previous
            );
        }

    }
}
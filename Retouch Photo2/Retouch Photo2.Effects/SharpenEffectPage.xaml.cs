// Core:              ★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.Historys;
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
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        private float Amount
        {
            set
            {
                this.AmountPicker.Value = (int)(value * 10.0f);
                this.AmountSlider.Value = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a SharpenEffectPage. 
        /// </summary>
        public SharpenEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructIsOn();

            this.ConstructAmount1();
            this.ConstructAmount2();
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

            this.Button.Text = resource.GetString("Effects_Sharpen");

            this.AmountTextBlock.Text = resource.GetString("Effects_Sharpen_Amount");
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
            this.Amount = 0.0f;

            this.MethodViewModel.EffectChanged<float>
            (
                set: (effect) => effect.Sharpen_Amount = 0.0f,

                type: HistoryType.LayersProperty_ResetEffect_Sharpen,
                getUndo: (effect) => effect.Sharpen_Amount,
                setUndo: (effect, previous) => effect.Sharpen_Amount = previous
            );
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.Sharpen_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.Amount = effect.Sharpen_Amount;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Sharpen_IsOn"/>.
    /// </summary>
    public sealed partial class SharpenEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructIsOn()
        {
            this.Button.Toggled += (isOn) => this.MethodViewModel.EffectChanged<bool>
            (
                set: (effect) => effect.Straighten_IsOn = isOn,

                type: HistoryType.LayersProperty_SwitchEffect_Sharpen,
                getUndo: (effect) => effect.Sharpen_IsOn,
                setUndo: (effect, previous) => effect.Sharpen_IsOn = previous
            );
        }


        //Amount
        private void ConstructAmount1()
        {
            this.AmountPicker.Unit = null;
            this.AmountPicker.Minimum = 0;
            this.AmountPicker.Maximum = 100;
            this.AmountPicker.ValueChanged += (s, value) =>
            {
                float amount = (float)value / 10.0f;
                this.Amount = amount;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.Sharpen_Amount = amount,

                    type: HistoryType.LayersProperty_SetEffect_Sharpen_Amount,
                    getUndo: (effect) => effect.Sharpen_Amount,
                    setUndo: (effect, previous) => effect.Sharpen_Amount = previous
                );
            };
        }

        private void ConstructAmount2()
        {
            this.AmountSlider.Minimum = 0.0d;
            this.AmountSlider.Maximum = 10.0d;
            this.AmountSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheSharpen());
            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                float amount = (float)value;
                this.Amount = amount;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Sharpen_Amount = amount);
            };
            this.AmountSlider.ValueChangeCompleted += (s, value) =>
            {
                float amount = (float)value;
                this.Amount = amount;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.Sharpen_Amount = amount,

                    type: HistoryType.LayersProperty_SetEffect_Sharpen_Amount,
                    getUndo: (effect) => effect.StartingSharpen_Amount,
                    setUndo: (effect, previous) => effect.Sharpen_Amount = previous
                );
            };
        }

    }
}
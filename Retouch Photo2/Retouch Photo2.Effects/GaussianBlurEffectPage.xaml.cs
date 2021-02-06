// Core:              ★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Retouch_Photo2.Historys;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffectPage : Page, IEffectPage
    {

        //@ViewModel
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        private float Radius
        {
            set
            {
                this.RadiusPicker.Value = (int)value;
                this.RadiusSlider.Value = value;
            }
        }
        

        //@Construct
        /// <summary>
        /// Initializes a GaussianBlurEffectPage. 
        /// </summary>
        public GaussianBlurEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructIsOn();

            this.ConstructRadius1();
            this.ConstructRadius2();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("Effects_GaussianBlur");

            this.RadiusTextBlock.Text = resource.GetString("Effects_GaussianBlur_Radius");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.GaussianBlur;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new GaussianBlurIcon()
        };
        
        public void Reset()
        {
            this.Radius = 0.0f;

            this.MethodViewModel.EffectChanged<float>
            (
                set: (effect) => effect.GaussianBlur_Radius = 0.0f,

                historyTitle: "Set effect gaussian blur",
                getHistory: (effect) => effect.GaussianBlur_Radius,
                setHistory: (effect, previous) => effect.GaussianBlur_Radius = previous
            );
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.GaussianBlur_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.RadiusSlider.Value = effect.GaussianBlur_Radius;
        }
    }
    
    /// <summary>
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructIsOn()
        {
            this.Button.Toggled += (isOn) => this.MethodViewModel.EffectChanged<bool>
            (
                set: (effect) => effect.GaussianBlur_IsOn = isOn,

                historyTitle: "Set effect gaussian blur is on",
                getHistory: (effect) => effect.GaussianBlur_IsOn,
                setHistory: (effect, previous) => effect.GaussianBlur_IsOn = previous
            );
        }


        //Radius
        private void ConstructRadius1()
        {
            this.RadiusPicker.Unit = null;
            this.RadiusPicker.Minimum = 0;
            this.RadiusPicker.Maximum = 100;
            this.RadiusPicker.ValueChanged += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.GaussianBlur_Radius = radius,

                    historyTitle: "Set effect gaussian blur amount",
                    getHistory: (effect) => effect.GaussianBlur_Radius,
                    setHistory: (effect, previous) => effect.GaussianBlur_Radius = previous
                );
            };
        }

        private void ConstructRadius2()
        {
            this.RadiusSlider.Minimum = 0.0d;
            this.RadiusSlider.Maximum = 100.0d;
            this.RadiusSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheGaussianBlur());
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.GaussianBlur_Radius = radius);
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.GaussianBlur_Radius = radius,

                    historyTitle: "Set effect gaussian blur amount",
                    getHistory: (effect) => effect.StartingGaussianBlur_Radius,
                    setHistory: (effect, previous) => effect.GaussianBlur_Radius = previous
                );
            };
        }

    }
}
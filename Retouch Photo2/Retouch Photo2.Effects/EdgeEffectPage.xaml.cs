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
    /// Page of <see cref = "Effect.Edge_IsOn"/>.
    /// </summary>
    public sealed partial class EdgeEffectPage : Page, IEffectPage
    {

        //@ViewModel
        ViewModel MethodViewModel => App.MethodViewModel;

        
        //@Content
        private float Amount
        {
            set
            {
                this.AmountPicker.Value = (int)(value * 100.0f);
                this.AmountSlider.Value = value;
            }
        }
        private float Radius
        {
            set
            {
                this.RadiusPicker.Value = (int)(value * 10.0f);
                this.RadiusSlider.Value = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a EdgeEffectPage. 
        /// </summary>
        public EdgeEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructIsOn();

            this.ConstructAmount1();
            this.ConstructAmount2();

            this.ConstructRadius1();
            this.ConstructRadius2();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Edge_IsOn"/>.
    /// </summary>
    public sealed partial class EdgeEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("Effects_Edge");

            this.AmountTextBlock.Text = resource.GetString("Effects_Edge_Amount");
            this.RadiusTextBlock.Text = resource.GetString("Effects_Edge_Radius");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.Edge;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new EdgeIcon()
        };
        
        public void Reset()
        {
            this.Amount = 0.5f;
            this.Radius = 0.0f;

            this.MethodViewModel.EffectChangeCompleted<(float, float)>
            (
                set: (effect) =>
                {
                    effect.Edge_Amount = 0.5f;
                    effect.Edge_Radius = 0.0f;
                },
                historyTitle: "Set effect outline",
                getHistory: (effect) =>
                (
                    effect.Edge_Amount,
                    effect.Edge_Radius
                ),
                setHistory: (effect, previous) =>
                {
                    effect.Edge_Amount = previous.Item1;
                    effect.Edge_Radius = previous.Item2;
                }
            );
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.Edge_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.Amount = effect.Edge_Amount;
            this.Radius = effect.Edge_Radius;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Edge_IsOn"/>.
    /// </summary>
    public sealed partial class EdgeEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructIsOn()
        {
            this.Button.Toggled += (isOn) =>  this.MethodViewModel.EffectChanged<bool>
            (
                set: (effect) => effect.Edge_IsOn = isOn,

                historyTitle: "Set effect edge is on",
                getHistory: (effect) => effect.Edge_IsOn,
                setHistory: (effect, previous) => effect.Edge_IsOn = previous
            );
        }


        //Amount
        private void ConstructAmount1()
        {
            this.AmountPicker.Unit = "%";
            this.AmountPicker.Minimum = 0;
            this.AmountPicker.Maximum = 100;
            this.AmountPicker.ValueChanged += (s, value) =>
            {
                float amount = (float)value / 100.0f;
                this.Amount = amount;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.Edge_Amount = amount,

                    historyTitle: "Set effect edge amount",
                    getHistory: (effect) => effect.Edge_Amount,
                    setHistory: (effect, previous) => effect.Edge_Amount = previous
               );
            };
        }

        private void ConstructAmount2()
        {
            this.AmountSlider.Minimum = 0.0d;
            this.AmountSlider.Maximum = 1.0d;
            this.AmountSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheEdge());
            this.AmountSlider.ValueChangeDelta += (s, value) =>
            {
                float amount = (float)value;
                this.Amount = amount;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Edge_Amount = amount);
            };
            this.AmountSlider.ValueChangeCompleted += (s, value) =>
            {
                float amount = (float)value;
                this.Amount = amount;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.Edge_Amount = amount,

                    historyTitle: "Set effect edge amount",
                    getHistory: (effect) => effect.StartingEdge_Amount,
                    setHistory: (effect, previous) => effect.Edge_Amount = previous
                );
            };
        }


        //Radius
        private void ConstructRadius1()
        {
            this.RadiusPicker.Unit = null;
            this.RadiusPicker.Minimum = 0;
            this.RadiusPicker.Maximum = 100;
            this.RadiusPicker.ValueChanged += (s, value) =>
            {
                float radius = (float)value / 10.0f;
                this.Radius = radius;

                this.MethodViewModel.EffectChanged<float>
               (
                   set: (effect) => effect.Edge_Radius = radius,

                   historyTitle: "Set effect edge radius",
                   getHistory: (effect) => effect.Edge_Radius,
                   setHistory: (effect, previous) => effect.Edge_Radius = previous
               );
            };
        }

        private void ConstructRadius2()
        {
            this.RadiusSlider.Minimum = 0.0d;
            this.RadiusSlider.Maximum = 10.0d;
            this.RadiusSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheEdge());
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Edge_Radius = radius);
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChangeCompleted<float>
               (
                   set: (effect) => effect.Edge_Radius = radius,

                   historyTitle: "Set effect edge radius",
                   getHistory: (effect) => effect.StartingEdge_Radius,
                   setHistory: (effect, previous) => effect.Edge_Radius = previous
               );
            };
        }

    }
}
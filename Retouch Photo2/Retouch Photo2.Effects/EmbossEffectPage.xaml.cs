using Retouch_Photo2.Effects.Icons;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.Emboss_IsOn"/>.
    /// </summary>
    public sealed partial class EmbossEffectPage : Page, IEffectPage
    {

        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        

        //@Content
        private float Radius
        {
            set
            {
                this.RadiusPicker.Value = (int)(value * 10.0f);
                this.RadiusSlider.Value = value;
            }
        }
        private float Angle
        {
            set
            {
                this.AnglePicker.Value = (int)(value * 180.0f / FanKit.Math.Pi);
                this.AnglePicker2.Radians = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a EmbossEffectPage. 
        /// </summary>
        public EmbossEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructIsOn();

            this.ConstructRadius1();
            this.ConstructRadius2();

            this.ConstructAngle1();
            this.ConstructAngle2();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Emboss_IsOn"/>.
    /// </summary>
    public sealed partial class EmbossEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("/Effects/Emboss");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/Emboss_Radius");
            this.AngleTextBlock.Text = resource.GetString("/Effects/Emboss_Angle");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.Emboss;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new EmbossIcon()
        };
        
        public void Reset()
        {
            this.Radius = 1.0f;
            this.Angle = 0.0f;

            this.MethodViewModel.EffectChangeCompleted<(float, float)>
            (
                set: (effect) =>
                {
                    effect.Emboss_Radius = 1.0f;
                    effect.Emboss_Angle = 0.0f;
                },
                historyTitle: "Set effect emboss",
                getHistory: (effect) =>
                (
                    effect.Emboss_Radius,
                    effect.Emboss_Angle
                ),
                setHistory: (effect, previous) =>
                {
                    effect.Emboss_Radius = previous.Item1;
                    effect.Emboss_Angle = previous.Item2;
                }
            );
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.Emboss_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.Radius = effect.Emboss_Radius;
            this.Angle = effect.Emboss_Angle;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Emboss_IsOn"/>.
    /// </summary>
    public sealed partial class EmbossEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructIsOn()
        {
            this.Button.Toggled += (isOn) => this.MethodViewModel.EffectChanged<bool>
            (
                set: (effect) => effect.Emboss_IsOn = isOn,

                historyTitle: "Set effect emboss is on",
                getHistory: (effect) => effect.Emboss_IsOn,
                setHistory: (effect, previous) => effect.Emboss_IsOn = previous
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
                float radius = (float)value / 10.0f;
                this.Radius = radius;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.Emboss_Radius = (float)value,

                    historyTitle: "Set effect emboss radius",
                    getHistory: (effect) => effect.Emboss_Radius,
                    setHistory: (effect, previous) => effect.Emboss_Radius = previous
                );
            };
        }

        private void ConstructRadius2()
        {
            this.RadiusSlider.Minimum = 0.0d;
            this.RadiusSlider.Maximum = 10.0d;
            this.RadiusSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheEmboss());
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Emboss_Radius = radius);
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.Emboss_Radius = (float)value,

                    historyTitle: "Set effect emboss radius",
                    getHistory: (effect) => effect.StartingEmboss_Radius,
                    setHistory: (effect, previous) => effect.Emboss_Radius = previous
                );
            };
        }


        //Angle
        private void ConstructAngle1()
        {
            this.AnglePicker.Unit = "º";
            this.AnglePicker.Minimum = -360;
            this.AnglePicker.Maximum = 360;
            this.AnglePicker.ValueChanged += (s, value) =>
            {
                float radians = (float)value * 180.0f / FanKit.Math.Pi;
                this.Angle = radians;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.Emboss_Angle = (float)value,

                    historyTitle: "Set effect emboss angle",
                    getHistory: (effect) => effect.Emboss_Angle,
                    setHistory: (effect, previous) => effect.Emboss_Angle = previous
                );
            };
        }

        private void ConstructAngle2()
        {
            //this.AnglePicker2.Minimum = 0;
            //this.AnglePicker2.Maximum = FanKit.Math.PiTwice;
            this.AnglePicker2.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheEmboss());
            this.AnglePicker2.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Emboss_Angle = radians);
            };
            this.AnglePicker2.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.Emboss_Angle = (float)value,

                    historyTitle: "Set effect emboss angle",
                    getHistory: (effect) => effect.StartingEmboss_Angle,
                    setHistory: (effect, previous) => effect.Emboss_Angle = previous
                );
            };
        }

    }
}
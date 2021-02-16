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
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurEffectPage : Page, IEffectPage
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
        /// Initializes a DirectionalBlurEffectPage. 
        /// </summary>
        public DirectionalBlurEffectPage()
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
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("Effects_DirectionalBlur");

            this.RadiusTextBlock.Text = resource.GetString("Effects_DirectionalBlur_Radius");
            this.AngleTextBlock.Text = resource.GetString("Effects_DirectionalBlur_Angle");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.DirectionalBlur;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new DirectionalBlurIcon()
        };
        
        public void Reset()
        {
            this.Radius = 0.0f;
            this.Angle = 0.0f;

            this.MethodViewModel.EffectChanged<(float, float)>
            (
                set: (effect) =>
                {
                    effect.DirectionalBlur_Radius = 0.0f;
                    effect.DirectionalBlur_Angle = 0.5f;
                },
                type: HistoryType.LayersProperty_ResetEffect_DirectionalBlur,
                getUndo: (effect) =>
                (
                    effect.DirectionalBlur_Radius,
                    effect.DirectionalBlur_Angle
                ),
                setUndo: (effect, previous) =>
                {
                    effect.DirectionalBlur_Radius = previous.Item1;
                    effect.DirectionalBlur_Angle = previous.Item2;
                }
            );
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.DirectionalBlur_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.Radius = effect.DirectionalBlur_Radius;
            this.Angle = effect.DirectionalBlur_Angle;
        }
    }
    
    /// <summary>
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructIsOn()
        {
            this.Button.Toggled += (isOn) => this.MethodViewModel.EffectChanged<bool>
            (
                set: (effect) => effect.DirectionalBlur_IsOn = isOn,

                type: HistoryType.LayersProperty_SwitchEffect_DirectionalBlur,
                getUndo: (effect) => effect.DirectionalBlur_IsOn,
                setUndo: (effect, previous) => effect.DirectionalBlur_IsOn = previous
            );            
        }


        //Radius
        private void ConstructRadius1()
        {
            this.RadiusPicker.Minimum = 0;
            this.RadiusPicker.Maximum = 100;
            this.RadiusPicker.ValueChanged += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.DirectionalBlur_Radius = radius,
                               
                    type: HistoryType.LayersProperty_SetEffect_DirectionalBlur_Radius,
                    getUndo: (effect) => effect.DirectionalBlur_Radius,
                    setUndo: (effect, previous) => effect.DirectionalBlur_Radius = previous
                );
            };
        }
        
        private void ConstructRadius2()
        {
            this.RadiusSlider.Minimum = 0.0d;
            this.RadiusSlider.Maximum = 100.0d;
            this.RadiusSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheDirectionalBlur());
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.DirectionalBlur_Radius = radius);
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.DirectionalBlur_Radius = radius,

                    type: HistoryType.LayersProperty_SetEffect_DirectionalBlur_Radius,
                    getUndo: (effect) => effect.StartingDirectionalBlur_Radius,
                    setUndo: (effect, previous) => effect.DirectionalBlur_Radius = previous
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
                float angle = (float)value * 180 / FanKit.Math.Pi;
                this.Angle = angle;

                this.MethodViewModel.EffectChanged<float>
               (
                   set: (effect) => effect.DirectionalBlur_Angle = (float)value,

                   type: HistoryType.LayersProperty_SetEffect_DirectionalBlur_Angle,
                   getUndo: (effect) => effect.StartingDirectionalBlur_Angle,
                   setUndo: (effect, previous) => effect.DirectionalBlur_Angle = previous
               );
            };
        }

        private void ConstructAngle2()
        {
            //this.AnglePicker2.Minimum = 0;
            //this.AnglePicker2.Maximum = FanKit.Math.PiTwice;
            this.AnglePicker2.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheDirectionalBlur());
            this.AnglePicker2.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.DirectionalBlur_Angle = radians);
            };
            this.AnglePicker2.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeCompleted<float>
               (
                   set: (effect) => effect.DirectionalBlur_Angle = (float)value,

                   type: HistoryType.LayersProperty_SetEffect_DirectionalBlur_Angle,
                   getUndo: (effect) => effect.StartingDirectionalBlur_Angle,
                   setUndo: (effect, previous) => effect.DirectionalBlur_Angle = previous
               );
            };
        }

    }
}
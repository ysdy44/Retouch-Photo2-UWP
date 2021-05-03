// Core:              ★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurPage : Page, IEffectPage
    {

        //@ViewModel
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.DirectionalBlur;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Self => this;

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
        private EffectBorderMode BorderMode
        {
            set
            {
                switch (value)
                {
                    case EffectBorderMode.Soft:
                        this.IsHardBorderCheckBox.IsChecked = false;
                        break;
                    case EffectBorderMode.Hard:
                        this.IsHardBorderCheckBox.IsChecked = true;
                        break;
                    default:
                        break;
                }
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a DirectionalBlurPage.
        /// </summary>
        public DirectionalBlurPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructRadius1();
            this.ConstructRadius2();

            this.ConstructAngle1();
            this.ConstructAngle2();

            this.ConstructIsHard();
        }
    }

    public sealed partial class DirectionalBlurPage : Page, IEffectPage
    {

        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RadiusTextBlock.Text = resource.GetString("Effects_DirectionalBlur_Radius");
            this.AngleTextBlock.Text = resource.GetString("Effects_DirectionalBlur_Angle");

            this.IsHardBorderCheckBox.Content = resource.GetString("Effects_DirectionalBlur_IsHardBorder");
        }

        public void Reset()
        {
            this.Radius = 0.0f;
            this.Angle = 0.0f;
            this.BorderMode = EffectBorderMode.Soft;

            this.MethodViewModel.EffectChanged<(float, float, EffectBorderMode)>
            (
                set: (effect) =>
                {
                    effect.DirectionalBlur_Radius = 0.0f;
                    effect.DirectionalBlur_Angle = 0.0f;
                    effect.DirectionalBlur_BorderMode = EffectBorderMode.Soft;
                },
                type: HistoryType.LayersProperty_ResetEffect_DirectionalBlur,
                getUndo: (effect) =>
                (
                    effect.DirectionalBlur_Radius,
                    effect.DirectionalBlur_Angle,
                    effect.DirectionalBlur_BorderMode
                ),
                setUndo: (effect, previous) =>
                {
                    effect.DirectionalBlur_Radius = previous.Item1;
                    effect.DirectionalBlur_Angle = previous.Item2;
                    effect.DirectionalBlur_BorderMode = previous.Item3;
                }
            );
        }
        public void Switch(bool isOn)
        {
            this.MethodViewModel.EffectChanged<bool>
            (
               set: (effect) => effect.DirectionalBlur_IsOn = isOn,

               type: HistoryType.LayersProperty_SwitchEffect_DirectionalBlur,
               getUndo: (effect) => effect.DirectionalBlur_IsOn,
               setUndo: (effect, previous) => effect.DirectionalBlur_IsOn = previous
            );
        }
        public bool FollowButton(Effect effect) => effect.DirectionalBlur_IsOn;
        public void FollowPage(Effect effect)
        {
            this.Radius = effect.DirectionalBlur_Radius;
            this.Angle = effect.DirectionalBlur_Angle;
            this.BorderMode = effect.DirectionalBlur_BorderMode;
        }

    }

    /// <summary>
    /// Page of <see cref = "Effect.DirectionalBlur_IsOn"/>.
    /// </summary>
    public sealed partial class DirectionalBlurPage : Page, IEffectPage
    {

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
            this.AnglePicker2.ValueChangedStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheDirectionalBlur());
            this.AnglePicker2.ValueChangedDelta += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.DirectionalBlur_Angle = radians);
            };
            this.AnglePicker2.ValueChangedCompleted += (s, value) =>
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

        //IsHardBorder
        private void ConstructIsHard()
        {
            this.IsHardBorderCheckBox.Tapped += (s, e) =>
            {
                EffectBorderMode borderMode = this.IsHardBorderCheckBox.IsChecked == true ? EffectBorderMode.Soft : EffectBorderMode.Hard;
                this.BorderMode = borderMode;

                this.MethodViewModel.EffectChangeCompleted<EffectBorderMode>
                (
                   set: (effect) => effect.DirectionalBlur_BorderMode = borderMode,

                   type: HistoryType.LayersProperty_SetEffect_DirectionalBlur_BoderMode,
                   getUndo: (effect) => effect.DirectionalBlur_BorderMode,
                   setUndo: (effect, previous) => effect.DirectionalBlur_BorderMode = previous
                );
            };
        }

    }
}
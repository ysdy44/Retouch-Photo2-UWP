﻿// Core:              ★★
// Referenced:   
// Difficult:         ★★★
// Only:              
// Complete:      ★★★
using Retouch_Photo2.Historys;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects.Pages
{
    /// <summary>
    /// Page of <see cref = "Effect.Emboss_IsOn"/>.
    /// </summary>
    public sealed partial class EmbossPage : Page, IEffectPage
    {

        //@ViewModel
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.Emboss;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Self => this;

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
        /// Initializes a EmbossPage.
        /// </summary>
        public EmbossPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructRadius1();
            this.ConstructRadius2();

            this.ConstructAngle1();
            this.ConstructAngle2();
        }
    }

    public sealed partial class EmbossPage : Page, IEffectPage
    {

        // String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RadiusTextBlock.Text = resource.GetString("Effects_Emboss_Radius");
            this.AngleTextBlock.Text = resource.GetString("Effects_Emboss_Angle");
        }

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
                type: HistoryType.LayersProperty_ResetEffect_Emboss,
                getUndo: (effect) =>
                (
                    effect.Emboss_Radius,
                    effect.Emboss_Angle
                ),
                setUndo: (effect, previous) =>
                {
                    effect.Emboss_Radius = previous.Item1;
                    effect.Emboss_Angle = previous.Item2;
                }
            );
        }
        public void Switch(bool isOn)
        {
            this.MethodViewModel.EffectChanged<bool>
          (
              set: (effect) => effect.Emboss_IsOn = isOn,

              type: HistoryType.LayersProperty_SwitchEffect_Emboss,
              getUndo: (effect) => effect.Emboss_IsOn,
              setUndo: (effect, previous) => effect.Emboss_IsOn = previous
          );
        }
        public bool FollowButton(Effect effect) => effect.Emboss_IsOn;
        public void FollowPage(Effect effect)
        {
            this.Radius = effect.Emboss_Radius;
            this.Angle = effect.Emboss_Angle;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.Emboss_IsOn"/>.
    /// </summary>
    public sealed partial class EmbossPage : Page, IEffectPage
    {

        // Radius
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

                    type: HistoryType.LayersProperty_SetEffect_Emboss_Radius,
                    getUndo: (effect) => effect.Emboss_Radius,
                    setUndo: (effect, previous) => effect.Emboss_Radius = previous
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

                    type: HistoryType.LayersProperty_SetEffect_Emboss_Radius,
                    getUndo: (effect) => effect.StartingEmboss_Radius,
                    setUndo: (effect, previous) => effect.Emboss_Radius = previous
                );
            };
        }


        // Angle
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

                    type: HistoryType.LayersProperty_SetEffect_Emboss_Angle,
                    getUndo: (effect) => effect.Emboss_Angle,
                    setUndo: (effect, previous) => effect.Emboss_Angle = previous
                );
            };
        }

        private void ConstructAngle2()
        {
            //this.AnglePicker2.Minimum = 0;
            //this.AnglePicker2.Maximum = FanKit.Math.PiTwice;
            this.AnglePicker2.ValueChangedStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheEmboss());
            this.AnglePicker2.ValueChangedDelta += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.Emboss_Angle = radians);
            };
            this.AnglePicker2.ValueChangedCompleted += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.Emboss_Angle = (float)value,

                    type: HistoryType.LayersProperty_SetEffect_Emboss_Angle,
                    getUndo: (effect) => effect.StartingEmboss_Angle,
                    setUndo: (effect, previous) => effect.Emboss_Angle = previous
                );
            };
        }

    }
}
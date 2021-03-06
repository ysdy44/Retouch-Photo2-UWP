﻿// Core:              ★★
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
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurPage : Page, IEffectPage
    {

        //@ViewModel
        ViewModel MethodViewModel => App.MethodViewModel;


        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.GaussianBlur;
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
        /// Initializes a GaussianBlurPage.
        /// </summary>
        public GaussianBlurPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructRadius1();
            this.ConstructRadius2();

            this.ConstructIsHard();
        }
    }

    public sealed partial class GaussianBlurPage : Page, IEffectPage
    {

        // String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.RadiusTextBlock.Text = resource.GetString("Effects_GaussianBlur_Radius");

            this.IsHardBorderCheckBox.Content = resource.GetString("Effects_GaussianBlur_IsHardBorder");
        }

        public void Reset()
        {
            this.Radius = 0.0f;
            this.BorderMode = EffectBorderMode.Soft;

            this.MethodViewModel.EffectChanged<(float, EffectBorderMode)>
            (
                set: (effect) =>
                {
                    effect.GaussianBlur_Radius = 0.0f;
                    effect.GaussianBlur_BorderMode = EffectBorderMode.Soft;
                },
                type: HistoryType.LayersProperty_ResetEffect_GaussianBlur,
                getUndo: (effect) =>
                (
                    effect.GaussianBlur_Radius,
                    effect.GaussianBlur_BorderMode
                ),
                setUndo: (effect, previous) =>
                {
                    effect.GaussianBlur_Radius = previous.Item1;
                    effect.GaussianBlur_BorderMode = previous.Item2;
                }
            );
        }
        public void Switch(bool isOn)
        {
            this.MethodViewModel.EffectChanged<bool>
             (
                 set: (effect) => effect.GaussianBlur_IsOn = isOn,

                 type: HistoryType.LayersProperty_SwitchEffect_GaussianBlur,
                 getUndo: (effect) => effect.GaussianBlur_IsOn,
                 setUndo: (effect, previous) => effect.GaussianBlur_IsOn = previous
             );
        }
        public bool FollowButton(Effect effect) => effect.GaussianBlur_IsOn;
        public void FollowPage(Effect effect)
        {
            this.RadiusSlider.Value = effect.GaussianBlur_Radius;
            this.BorderMode = effect.GaussianBlur_BorderMode;
        }
    }
    
    /// <summary>
    /// Page of <see cref = "Effect.GaussianBlur_IsOn"/>.
    /// </summary>
    public sealed partial class GaussianBlurPage : Page, IEffectPage
    {


        // Radius
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
          
                    type: HistoryType.LayersProperty_SetEffect_GaussianBlur_Amount,
                    getUndo: (effect) => effect.GaussianBlur_Radius,
                    setUndo: (effect, previous) => effect.GaussianBlur_Radius = previous
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

                    type: HistoryType.LayersProperty_SetEffect_GaussianBlur_Amount,
                    getUndo: (effect) => effect.StartingGaussianBlur_Radius,
                    setUndo: (effect, previous) => effect.GaussianBlur_Radius = previous
                );
            };
        }

        // IsHardBorder
        private void ConstructIsHard()
        {
            this.IsHardBorderCheckBox.Tapped += (s, e) =>
            {
                EffectBorderMode borderMode = this.IsHardBorderCheckBox.IsChecked == true ? EffectBorderMode.Soft : EffectBorderMode.Hard;
                this.BorderMode = borderMode;

                this.MethodViewModel.EffectChangeCompleted<EffectBorderMode>
                (
                   set: (effect) => effect.GaussianBlur_BorderMode = borderMode,

                   type: HistoryType.LayersProperty_SetEffect_GaussianBlur_BoderMode,
                   getUndo: (effect) => effect.GaussianBlur_BorderMode,
                   setUndo: (effect, previous) => effect.GaussianBlur_BorderMode = previous
                );
            };
        }

    }
}
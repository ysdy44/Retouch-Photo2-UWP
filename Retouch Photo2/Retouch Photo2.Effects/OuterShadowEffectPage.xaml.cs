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
using Windows.UI;

namespace Retouch_Photo2.Effects.Models
{
    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {

        //@ViewModel
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;


        //@Content
        private float Radius
        {
            set
            {
                this.RadiusPicker.Value = (int)value;
                this.RadiusSlider.Value = value;
            }
        }
        private float Opacity2
        {
            set
            {
                this.OpacityPicker.Value = (int)(value * 100.0f);
                this.OpacitySlider.Value = value;
            }
        }
        private float Offset
        {
            set
            {
                this.OffsetPicker.Value = (int)value;
                this.OffsetSlider.Value = value;
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
        /// <summary> Color </summary>
        public Color Color
        {
            get => this.SolidColorBrush.Color;
            set
            {
                this.SolidColorBrush.Color = value;
                this.ColorPicker.Color = value;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a OuterShadowEffectPage. 
        /// </summary>
        public OuterShadowEffectPage()
        {
            this.InitializeComponent();
            this.ConstructString();

            this.ConstructIsOn();

            this.ConstructRadius1();
            this.ConstructRadius2();

            this.ConstructOpacity1();
            this.ConstructOpacity2();

            this.ConstructOffset1();
            this.ConstructOffset2();

            this.ConstructAngle1();
            this.ConstructAngle2();

            this.ConstructColor1();
            this.ConstructColor2();
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {
        //String
        private void ConstructString()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Text = resource.GetString("Effects_OuterShadow");

            this.RadiusTextBlock.Text = resource.GetString("Effects_OuterShadow_Radius");
            this.OpacityTextBlock.Text = resource.GetString("Effects_OuterShadow_Opacity");
            this.OffsetTextBlock.Text = resource.GetString("Effects_OuterShadow_Offset");

            this.AngleTextBlock.Text = resource.GetString("Effects_OuterShadow_Angle");
            this.ColorTextBlock.Text = resource.GetString("Effects_OuterShadow_Color");
        }

        //@Content
        /// <summary> Gets the type. </summary>
        public EffectType Type => EffectType.OuterShadow;
        /// <summary> Gets the page. </summary>
        public FrameworkElement Page => this;
        /// <summary> Gets the button. </summary>
        public EffectButton Button { get; } = new EffectButton
        {
            Icon = new OuterShadowIcon()
        };
        
        public void Reset()
        {
            this.Radius = 12.0f;
            this.Opacity2 = 0.5f;
            this.Offset = 0.0f;
            this.Angle = FanKit.Math.PiOver4;
            this.Color = Windows.UI.Colors.Black;

            this.MethodViewModel.EffectChangeCompleted<(float, float, float, float, Color)>
            (
                set: (effect) =>
                {
                    effect.OuterShadow_Radius = 12.0f;
                    effect.OuterShadow_Opacity = 0.5f;
                    effect.OuterShadow_Offset = 0.0f;
                    effect.OuterShadow_Angle = FanKit.Math.PiOver4;
                    effect.OuterShadow_Color = Windows.UI.Colors.Black;
                },
                type: HistoryType.LayersProperty_ResetEffect_OuterShadow,
                getUndo: (effect) =>
                (
                    effect.OuterShadow_Radius,
                    effect.OuterShadow_Opacity,
                    effect.OuterShadow_Offset,
                    effect.OuterShadow_Angle,
                    effect.OuterShadow_Color
                ),
                setUndo: (effect, previous) =>
                {
                    effect.OuterShadow_Radius = previous.Item1;
                    effect.OuterShadow_Opacity = previous.Item2;
                    effect.OuterShadow_Offset = previous.Item3;
                    effect.OuterShadow_Angle = previous.Item4;
                    effect.OuterShadow_Color = previous.Item5;
                }
            );
        }
        public void FollowButton(Effect effect)
        {
            this.Button.IsOn = effect.OuterShadow_IsOn;
        }
        public void FollowPage(Effect effect)
        {
            this.Radius = effect.OuterShadow_Radius;
            this.Opacity2 = effect.OuterShadow_Opacity;
            this.Offset = effect.OuterShadow_Offset;
            this.Angle = effect.OuterShadow_Angle;
            this.Color = effect.OuterShadow_Color;
        }
    }

    /// <summary>
    /// Page of <see cref = "Effect.OuterShadow_IsOn"/>.
    /// </summary>
    public sealed partial class OuterShadowEffectPage : Page, IEffectPage
    {

        //IsOn
        private void ConstructIsOn()
        {
            this.Button.Toggled += (isOn) => this.MethodViewModel.EffectChanged<bool>
            (
                set: (effect) => effect.OuterShadow_IsOn = isOn,

                type: HistoryType.LayersProperty_SwitchEffect_OuterShadow,
                getUndo: (effect) => effect.OuterShadow_IsOn,
                setUndo: (effect, previous) => effect.OuterShadow_IsOn = previous
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
                    set: (effect) => effect.OuterShadow_Radius = radius,

                    type: HistoryType.LayersProperty_SetEffect_OuterShadow_Radius,
                    getUndo: (effect) => effect.OuterShadow_Radius,
                    setUndo: (effect, previous) => effect.OuterShadow_Radius = previous
                );
            };
        }

        private void ConstructRadius2()
        {
            this.RadiusSlider.Minimum = 0.0d;
            this.RadiusSlider.Maximum = 100.0d;
            this.RadiusSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow());
            this.RadiusSlider.ValueChangeDelta += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Radius = radius);
            };
            this.RadiusSlider.ValueChangeCompleted += (s, value) =>
            {
                float radius = (float)value;
                this.Radius = radius;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.OuterShadow_Radius = radius,

                    type: HistoryType.LayersProperty_SetEffect_OuterShadow_Radius,
                    getUndo: (effect) => effect.StartingOuterShadow_Radius,
                    setUndo: (effect, previous) => effect.OuterShadow_Radius = previous
                );
            };
        }


        //Opacity
        private void ConstructOpacity1()
        {
            this.OpacityPicker.Unit = "%";
            this.OpacityPicker.Minimum = 0;
            this.OpacityPicker.Maximum = 100;
            this.OpacityPicker.ValueChanged += (s, value) =>
            {
                float opacity = (float)value / 100.0f;
                this.Opacity2 = opacity;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.OuterShadow_Opacity = (float)value,

                    type: HistoryType.LayersProperty_SetEffect_OuterShadow_Opacity,
                    getUndo: (effect) => effect.OuterShadow_Opacity,
                    setUndo: (effect, previous) => effect.OuterShadow_Opacity = previous
                );
            };
        }

        private void ConstructOpacity2()
        {
            this.OpacitySlider.Minimum = 0.0d;
            this.OpacitySlider.Maximum = 1.0d;
            this.OpacitySlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow());
            this.OpacitySlider.ValueChangeDelta += (s, value) =>
            {
                float opacity = (float)value;
                this.Opacity2 = opacity;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Opacity = opacity);
            };            
            this.OpacitySlider.ValueChangeCompleted += (s, value) =>
            {
                float opacity = (float)value;
                this.Opacity2 = opacity;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.OuterShadow_Opacity = (float)value,

                    type: HistoryType.LayersProperty_SetEffect_OuterShadow_Opacity,
                    getUndo: (effect) => effect.StartingOuterShadow_Opacity,
                    setUndo: (effect, previous) => effect.OuterShadow_Opacity = previous
                );
            };
        }


        //Offset
        private void ConstructOffset1()
        {
            this.OffsetPicker.Unit = null;
            this.OffsetPicker.Minimum = 0;
            this.OffsetPicker.Maximum = 100;
            this.OffsetPicker.ValueChanged += (s, value) =>
            {
                float offset = (float)value;
                this.Offset = offset;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.OuterShadow_Offset = (float)value,

                    type: HistoryType.LayersProperty_SetEffect_OuterShadow_Offset,
                    getUndo: (effect) => effect.OuterShadow_Offset,
                    setUndo: (effect, previous) => effect.OuterShadow_Offset = previous
                );
            };
        }

        private void ConstructOffset2()
        {
            this.OffsetSlider.Minimum = 0.0d;
            this.OffsetSlider.Maximum = 100.0d;
            this.OffsetSlider.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow());
            this.OffsetSlider.ValueChangeDelta += (s, value) =>
            {
                float offset = (float)value;
                this.Offset = offset;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Offset = offset);
            };
            this.OffsetSlider.ValueChangeCompleted += (s, value) =>
            {
                float offset = (float)value;
                this.Offset = offset;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.OuterShadow_Offset = offset,

                    type: HistoryType.LayersProperty_SetEffect_OuterShadow_Offset,
                    getUndo: (effect) => effect.StartingOuterShadow_Offset,
                    setUndo: (effect, previous) => effect.OuterShadow_Offset = previous
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
                    set: (effect) => effect.OuterShadow_Angle = radians,

                    type: HistoryType.LayersProperty_SetEffect_OuterShadow_Angle,
                    getUndo: (effect) => effect.OuterShadow_Angle,
                    setUndo: (effect, previous) => effect.OuterShadow_Angle = previous
                );
            };
        }

        private void ConstructAngle2()
        {
            //this.AnglePicker2.Minimum = 0;
            //this.AnglePicker2.Maximum = FanKit.Math.PiTwice;
            this.AnglePicker2.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow());
            this.AnglePicker2.ValueChangeDelta += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Angle = radians);
            };
            this.AnglePicker2.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.OuterShadow_Angle = radians,

                    type: HistoryType.LayersProperty_SetEffect_OuterShadow_Angle,
                    getUndo: (effect) => effect.StartingOuterShadow_Angle,
                    setUndo: (effect, previous) => effect.OuterShadow_Angle = previous
                );
            };
        }


        //Color
        private void ConstructColor1()
        {
            this.ColorBorder.Tapped += (s, e) =>
            {
                this.ColorFlyout.ShowAt(this.ColorBorder);
                this.ColorPicker.Color = this.Color;
            };
            
            //@Focus
            // Before Flyout Showed, Don't let TextBox Got Focus.
            // After TextBox Gots focus, disable Shortcuts in SettingViewModel.
            if (this.ColorPicker.HexPicker is TextBox textBox)
            {
                textBox.IsEnabled = false;
                this.ColorFlyout.Opened += (s, e) => textBox.IsEnabled = true;
                this.ColorFlyout.Closed += (s, e) => textBox.IsEnabled = false;
                textBox.GotFocus += (s, e) => this.SettingViewModel.UnRegisteKey();
                textBox.LostFocus += (s, e) => this.SettingViewModel.RegisteKey();
            }

            this.ColorPicker.ColorChanged += (s, value) =>
            {
                Color color = (Color)value;
                this.Color = color;

                this.MethodViewModel.EffectChanged<Color>
                (
                    set: (effect) => effect.OuterShadow_Color = color,

                    type: HistoryType.LayersProperty_SetEffect_OuterShadow_Color,
                    getUndo: (effect) => effect.OuterShadow_Color,
                    setUndo: (effect, previous) => effect.OuterShadow_Color = previous
               );
            };
        }

        private void ConstructColor2()
        {
            this.ColorPicker.ColorChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow());
            this.ColorPicker.ColorChangeDelta += (s, value) =>
            {
                Color color = (Color)value;
                this.Color = color;

                this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Color = color);
            };
            this.ColorPicker.ColorChangeCompleted += (s, value) =>
            {
                Color color = (Color)value;
                this.Color = color;

                this.MethodViewModel.EffectChangeCompleted<Color>
                (
                    set: (effect) => effect.OuterShadow_Color = color,

                    type: HistoryType.LayersProperty_SetEffect_OuterShadow_Color,
                    getUndo: (effect) => effect.StartingOuterShadow_Color,
                    setUndo: (effect, previous) => effect.OuterShadow_Color = previous
               );
            };
        }

    }
}
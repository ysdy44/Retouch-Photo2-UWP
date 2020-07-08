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
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
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

            this.Button.Text = resource.GetString("/Effects/OuterShadow");

            this.RadiusTextBlock.Text = resource.GetString("/Effects/OuterShadow_Radius");
            this.OpacityTextBlock.Text = resource.GetString("/Effects/OuterShadow_Opacity");
            this.OffsetTextBlock.Text = resource.GetString("/Effects/OuterShadow_Offset");

            this.AngleTextBlock.Text = resource.GetString("/Effects/OuterShadow_Angle");
            this.ColorTextBlock.Text = resource.GetString("/Effects/OuterShadow_Color");
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
            this.Radius  = 12.0f;
            this.Opacity = 0.5f;
            this.Offset = 0.0f;
            this.Angle = FanKit.Math.PiOver4;
            this.Color = Windows.UI.Colors.Black;

            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set effect outer shadow");

            //Selection
            this.SelectionViewModel.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                var previous1 = layer.Effect.OuterShadow_Radius;
                var previous2 = layer.Effect.OuterShadow_Opacity;
                var previous4 = layer.Effect.OuterShadow_Offset;
                var previous5 = layer.Effect.OuterShadow_Angle;
                var previous3 = layer.Effect.OuterShadow_Color;
                history.UndoAction += () =>
                {
                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layer.IsRefactoringIconRender = true;
                    layer.Effect.OuterShadow_Radius = previous1;
                    layer.Effect.OuterShadow_Opacity = previous2;
                    layer.Effect.OuterShadow_Offset = previous4;
                    layer.Effect.OuterShadow_Angle = previous5;
                    layer.Effect.OuterShadow_Color = previous3;
                };
                
                //Refactoring
                layer.IsRefactoringRender = true;
                layer.IsRefactoringIconRender = true;
                layerage.RefactoringParentsRender();
                layerage.RefactoringParentsIconRender();
                layer.Effect.OuterShadow_Radius = 12.0f;
                layer.Effect.OuterShadow_Opacity = 0.5f;
                layer.Effect.OuterShadow_Offset = 0.0f;
                layer.Effect.OuterShadow_Angle = FanKit.Math.PiOver4;
                layer.Effect.OuterShadow_Color = Windows.UI.Colors.Black;
            });

            //History
            this.ViewModel.HistoryPush(history);

            this.ViewModel.Invalidate();//Invalidate
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

                historyTitle: "Set effect outer shadow is on",
                getHistory: (effect) => effect.OuterShadow_IsOn,
                setHistory: (effect, previous) => effect.OuterShadow_IsOn = previous
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

                    historyTitle: "Set effect outer shadow radius",
                    getHistory: (effect) => effect.OuterShadow_Radius,
                    setHistory: (effect, previous) => effect.OuterShadow_Radius = previous
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

                    historyTitle: "Set effect outer shadow radius",
                    getHistory: (effect) => effect.StartingOuterShadow_Radius,
                    setHistory: (effect, previous) => effect.OuterShadow_Radius = previous
                );
            };
        }


        //Opacity
        private void ConstructOpacity1()
        {
            this.OpacityPicker.Unit = null;
            this.OpacityPicker.Minimum = 0;
            this.OpacityPicker.Maximum = 100;
            this.OpacityPicker.ValueChanged += (s, value) =>
            {
                float opacity = (float)value / 100.0f;
                this.Opacity2 = opacity;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.OuterShadow_Opacity = (float)value,

                    historyTitle: "Set effect outer shadow opacity",
                    getHistory: (effect) => effect.OuterShadow_Opacity,
                    setHistory: (effect, previous) => effect.OuterShadow_Opacity = previous
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

                    historyTitle: "Set effect outer shadow opacity",
                    getHistory: (effect) => effect.StartingOuterShadow_Opacity,
                    setHistory: (effect, previous) => effect.OuterShadow_Opacity = previous
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

                    historyTitle: "Set effect outer shadow offset",
                    getHistory: (effect) => effect.OuterShadow_Offset,
                    setHistory: (effect, previous) => effect.OuterShadow_Offset = previous
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

                    historyTitle: "Set effect outer shadow offset",
                    getHistory: (effect) => effect.StartingOuterShadow_Offset,
                    setHistory: (effect, previous) => effect.OuterShadow_Offset = previous
                );
            };
        }


        //Angle
        private void ConstructAngle1()
        {
            this.AnglePicker.Unit = null;
            this.AnglePicker.Minimum = 0;
            this.AnglePicker.Maximum = 100;
            this.AnglePicker.ValueChanged += (s, value) =>
            {
                float radians = (float)value * 180 / FanKit.Math.Pi;
                this.Angle = radians;

                this.MethodViewModel.EffectChanged<float>
                (
                    set: (effect) => effect.OuterShadow_Angle = radians,

                    historyTitle: "Set effect outer shadow angle",
                    getHistory: (effect) => effect.OuterShadow_Angle,
                    setHistory: (effect, previous) => effect.OuterShadow_Angle = previous
                );
            };
        }

        private void ConstructAngle2()
        {
            //this.AnglePicker2.Minimum = 0;
            //this.AnglePicker2.Maximum = FanKit.Math.PiTwice;
            this.AnglePicker2.ValueChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow());
            this.AnglePicker2.ValueChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Angle = (float)value);
            this.AnglePicker2.ValueChangeCompleted += (s, value) =>
            {
                float radians = (float)value;
                this.Angle = radians;

                this.MethodViewModel.EffectChangeCompleted<float>
                (
                    set: (effect) => effect.OuterShadow_Angle = radians,

                    historyTitle: "Set effect outer shadow angle",
                    getHistory: (effect) => effect.StartingOuterShadow_Angle,
                    setHistory: (effect, previous) => effect.OuterShadow_Angle = previous
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
                textBox.GotFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = false;
                textBox.LostFocus += (s, e) => this.SettingViewModel.KeyIsEnabled = true;
            }

            this.ColorPicker.ColorChanged += (s, value) =>
            {
                Color color = (Color)value;
                this.Color = color;

                this.MethodViewModel.EffectChanged<Color>
                (
                    set: (effect) => effect.OuterShadow_Color = color,

                    historyTitle: "Set effect outer shadow color",
                    getHistory: (effect) => effect.OuterShadow_Color,
                    setHistory: (effect, previous) => effect.OuterShadow_Color = previous
               );
            };
        }

        private void ConstructColor2()
        {
            this.ColorPicker.ColorChangeStarted += (s, value) => this.MethodViewModel.EffectChangeStarted(cache: (effect) => effect.CacheOuterShadow()); 
            this.ColorPicker.ColorChangeDelta += (s, value) => this.MethodViewModel.EffectChangeDelta(set: (effect) => effect.OuterShadow_Color = (Color)value);
            this.ColorPicker.ColorChangeCompleted += (s, value) =>
            {
                Color color = (Color)value;
                this.Color = color;

                this.MethodViewModel.EffectChangeCompleted<Color>
                (
                    set: (effect) => effect.OuterShadow_Color = color,

                    historyTitle: "Set effect outer shadow color",
                    getHistory: (effect) => effect.StartingOuterShadow_Color,
                    setHistory: (effect, previous) => effect.OuterShadow_Color = previous
               );
            };
        }

    }
}
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends.Icons;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Represents the combo box that is used to select blend mode.
    /// </summary>
    public sealed partial class BlendModeComboBox : UserControl
    {

        //@Delegate
        public EventHandler<BlendEffectMode?> ModeChanged;


        //@VisualState
        bool _vsIsNone = true;
        BlendEffectMode _vsBlendMode;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsNone) return this.None;

                switch (this._vsBlendMode)
                {
                    case BlendEffectMode.Multiply: return this.Multiply;
                    case BlendEffectMode.Screen: return this.Screen;

                    case BlendEffectMode.Darken: return this.Darken;
                    case BlendEffectMode.Lighten: return this.Lighten;

                    case BlendEffectMode.Dissolve: return this.Dissolve;
                    case BlendEffectMode.ColorBurn: return this.ColorBurn;
                    case BlendEffectMode.LinearBurn: return this.LinearBurn;

                    case BlendEffectMode.DarkerColor: return this.DarkerColor;
                    case BlendEffectMode.LighterColor: return this.LighterColor;

                    case BlendEffectMode.ColorDodge: return this.ColorDodge;
                    case BlendEffectMode.LinearDodge: return this.LinearDodge;

                    case BlendEffectMode.Overlay: return this.Overlay;
                    case BlendEffectMode.SoftLight: return this.SoftLight;
                    case BlendEffectMode.HardLight: return this.HardLight;
                    case BlendEffectMode.VividLight: return this.VividLight;
                    case BlendEffectMode.LinearLight: return this.LinearLight;
                    case BlendEffectMode.PinLight: return this.PinLight;

                    case BlendEffectMode.HardMix: return this.HardMix;
                    case BlendEffectMode.Difference: return this.Difference;
                    case BlendEffectMode.Exclusion: return this.Exclusion;

                    case BlendEffectMode.Hue: return this.Hue;
                    case BlendEffectMode.Saturation: return this.Saturation;
                    case BlendEffectMode.Color: return this.Color;

                    case BlendEffectMode.Luminosity: return this.Luminosity;
                    case BlendEffectMode.Subtract: return this.Subtract;
                    case BlendEffectMode.Division: return this.Division;

                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }


        #region DependencyProperty


        /// <summary> Gets or sets the blend mode. </summary>
        public BlendEffectMode? Mode
        {
            get { return (BlendEffectMode?)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BlendModeComboBox.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(BlendEffectMode?), typeof(BlendModeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BlendModeComboBox con = (BlendModeComboBox)sender;

            if (e.NewValue is BlendEffectMode value)
            {
                con._vsIsNone = false;
                con._vsBlendMode = value;
                con.VisualState = con.VisualState;//State
            }
            else
            {
                con._vsIsNone = true;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion


        //@Construct
        public BlendModeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Loaded += (s, e) =>
            {
                if (this.Mode is BlendEffectMode value)
                {
                    this._vsIsNone = false;
                    this._vsBlendMode = value;
                    this.VisualState = this.VisualState;//State
                }
                else
                {
                    this._vsIsNone = true;
                    this.VisualState = this.VisualState;//State
                }
            };
        }


        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructButton(this.NoneButton, resource.GetString("/Blends/None"), new NoneIcon(), null);

            this.ConstructButton(this.MultiplyButton, resource.GetString("/Blends/Multiply"), new MultiplyIcon(), BlendEffectMode.Multiply);
            this.ConstructButton(this.ScreenButton, resource.GetString("/Blends/Screen"), new ScreenIcon(), BlendEffectMode.Screen);

            this.ConstructButton(this.DarkenButton, resource.GetString("/Blends/Darken"), new DarkenIcon(), BlendEffectMode.Darken);
            this.ConstructButton(this.LightenButton, resource.GetString("/Blends/Lighten"), new LightenIcon(), BlendEffectMode.Lighten);

            this.ConstructButton(this.DissolveButton, resource.GetString("/Blends/Dissolve"), new DissolveIcon(), BlendEffectMode.Dissolve);
            this.ConstructButton(this.ColorBurnButton, resource.GetString("/Blends/ColorBurn"), new ColorBurnIcon(), BlendEffectMode.ColorBurn);
            this.ConstructButton(this.LinearBurnButton, resource.GetString("/Blends/LinearBurn"), new LinearBurnIcon(), BlendEffectMode.LinearBurn);

            this.ConstructButton(this.DarkerColorButton, resource.GetString("/Blends/DarkerColor"), new DarkerColorIcon(), BlendEffectMode.DarkerColor);
            this.ConstructButton(this.LighterColorButton, resource.GetString("/Blends/LighterColor"), new LighterColorIcon(), BlendEffectMode.LighterColor);

            this.ConstructButton(this.ColorDodgeButton, resource.GetString("/Blends/ColorDodge"), new ColorDodgeIcon(), BlendEffectMode.ColorDodge);
            this.ConstructButton(this.LinearDodgeButton, resource.GetString("/Blends/LinearDodge"), new LinearDodgeIcon(), BlendEffectMode.LinearDodge);

            this.ConstructButton(this.OverlayButton, resource.GetString("/Blends/Overlay"), new OverlayIcon(), BlendEffectMode.Overlay);
            this.ConstructButton(this.SoftLightButton, resource.GetString("/Blends/SoftLight"), new SoftLightIcon(), BlendEffectMode.SoftLight);
            this.ConstructButton(this.HardLightButton, resource.GetString("/Blends/HardLight"), new HardLightIcon(), BlendEffectMode.HardLight);
            this.ConstructButton(this.VividLightButton, resource.GetString("/Blends/VividLight"), new VividLightIcon(), BlendEffectMode.VividLight);
            this.ConstructButton(this.LinearLightButton, resource.GetString("/Blends/LinearLight"), new LinearLightIcon(), BlendEffectMode.LinearLight);
            this.ConstructButton(this.PinLightButton, resource.GetString("/Blends/PinLight"), new PinLightIcon(), BlendEffectMode.PinLight);

            this.ConstructButton(this.HardMixButton, resource.GetString("/Blends/HardMix"), new HardMixIcon(), BlendEffectMode.HardMix);
            this.ConstructButton(this.DifferenceButton, resource.GetString("/Blends/Difference"), new DifferenceIcon(), BlendEffectMode.Difference);
            this.ConstructButton(this.ExclusionButton, resource.GetString("/Blends/Exclusion"), new ExclusionIcon(), BlendEffectMode.Exclusion);

            this.ConstructButton(this.HueButton, resource.GetString("/Blends/Hue"), new HueIcon(), BlendEffectMode.Hue);
            this.ConstructButton(this.SaturationButton, resource.GetString("/Blends/Saturation"), new SaturationIcon(), BlendEffectMode.Saturation);
            this.ConstructButton(this.ColorButton, resource.GetString("/Blends/Color"), new ColorIcon(), BlendEffectMode.Color);

            this.ConstructButton(this.LuminosityButton, resource.GetString("/Blends/Luminosity"), new LuminosityIcon(), BlendEffectMode.Luminosity);
            this.ConstructButton(this.SubtractButton, resource.GetString("/Blends/Subtract"), new SubtractIcon(), BlendEffectMode.Subtract);
            this.ConstructButton(this.DivisionButton, resource.GetString("/Blends/Division"), new DivisionIcon(), BlendEffectMode.Division);
        }
        
        private void ConstructButton(Button button, string text, UserControl icon, BlendEffectMode? blendMode)
        {
            button.Content = text;
            button.Tag = icon;
            button.Tapped += (s, e) =>
            {
                this.ModeChanged?.Invoke(this, blendMode);//Delegate
            };
        }

    }
}
// Core:              ★
// Referenced:   
// Difficult:         ★★
// Only:              ★★
// Complete:      ★★
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends.Icons;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// ComboBox of <see cref="BlendEffect"/>
    /// </summary>
    public sealed partial class BlendModeComboBox : UserControl
    {

        //@Delegate
        /// <summary> Occurs when mode change. </summary>
        public EventHandler<BlendEffectMode?> ModeChanged;

        //@Group
        /// <summary> Occurs when group change. </summary>
        private EventHandler<BlendEffectMode?> Group;
        
        #region DependencyProperty


        /// <summary> Gets or sets the blend-type. </summary>
        public BlendEffectMode? Mode
        {
            get  => (BlendEffectMode?)base.GetValue(ModeProperty);
            set => base.SetValue(ModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "BlendModeComboBox.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(BlendEffectMode?), typeof(BlendModeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BlendModeComboBox control = (BlendModeComboBox)sender;

            if (e.NewValue is BlendEffectMode value)
            {
                control.Group?.Invoke(control, value);//Delegate
            }
            else
            {
                control.Group?.Invoke(control, null);//Delegate
            }
        }));


        /// <summary> Gets or sets the title. </summary>
        public object Title
        {
            get  => (object)base.GetValue(TitleProperty);
            set => base.SetValue(TitleProperty, value);
        }
        /// <summary> Identifies the <see cref = "BlendModeComboBox.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(object), typeof(BlendModeComboBox), new PropertyMetadata(null));


        #endregion


        //@Construct
        /// <summary>
        /// Initializes a BlendModeComboBox. 
        /// </summary>
        public BlendModeComboBox()
        {
            this.InitializeComponent();
            this.ConstructStrings();
        }

    }

    /// <summary>
    /// Represents the combo box that is used to select blend mode.
    /// </summary>
    public sealed partial class BlendModeComboBox : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructGroup(this.NoneButton, resource.GetString("Blends_None"), new NoneIcon(), null);

            this.ConstructGroup(this.MultiplyButton, resource.GetString("Blends_Multiply"), new MultiplyIcon(), BlendEffectMode.Multiply);
            this.ConstructGroup(this.ScreenButton, resource.GetString("Blends_Screen"), new ScreenIcon(), BlendEffectMode.Screen);

            this.ConstructGroup(this.DarkenButton, resource.GetString("Blends_Darken"), new DarkenIcon(), BlendEffectMode.Darken);
            this.ConstructGroup(this.LightenButton, resource.GetString("Blends_Lighten"), new LightenIcon(), BlendEffectMode.Lighten);

            this.ConstructGroup(this.DissolveButton, resource.GetString("Blends_Dissolve"), new DissolveIcon(), BlendEffectMode.Dissolve);
            this.ConstructGroup(this.ColorBurnButton, resource.GetString("Blends_ColorBurn"), new ColorBurnIcon(), BlendEffectMode.ColorBurn);
            this.ConstructGroup(this.LinearBurnButton, resource.GetString("Blends_LinearBurn"), new LinearBurnIcon(), BlendEffectMode.LinearBurn);

            this.ConstructGroup(this.DarkerColorButton, resource.GetString("Blends_DarkerColor"), new DarkerColorIcon(), BlendEffectMode.DarkerColor);
            this.ConstructGroup(this.LighterColorButton, resource.GetString("Blends_LighterColor"), new LighterColorIcon(), BlendEffectMode.LighterColor);

            this.ConstructGroup(this.ColorDodgeButton, resource.GetString("Blends_ColorDodge"), new ColorDodgeIcon(), BlendEffectMode.ColorDodge);
            this.ConstructGroup(this.LinearDodgeButton, resource.GetString("Blends_LinearDodge"), new LinearDodgeIcon(), BlendEffectMode.LinearDodge);

            this.ConstructGroup(this.OverlayButton, resource.GetString("Blends_Overlay"), new OverlayIcon(), BlendEffectMode.Overlay);
            this.ConstructGroup(this.SoftLightButton, resource.GetString("Blends_SoftLight"), new SoftLightIcon(), BlendEffectMode.SoftLight);
            this.ConstructGroup(this.HardLightButton, resource.GetString("Blends_HardLight"), new HardLightIcon(), BlendEffectMode.HardLight);
            this.ConstructGroup(this.VividLightButton, resource.GetString("Blends_VividLight"), new VividLightIcon(), BlendEffectMode.VividLight);
            this.ConstructGroup(this.LinearLightButton, resource.GetString("Blends_LinearLight"), new LinearLightIcon(), BlendEffectMode.LinearLight);
            this.ConstructGroup(this.PinLightButton, resource.GetString("Blends_PinLight"), new PinLightIcon(), BlendEffectMode.PinLight);

            this.ConstructGroup(this.HardMixButton, resource.GetString("Blends_HardMix"), new HardMixIcon(), BlendEffectMode.HardMix);
            this.ConstructGroup(this.DifferenceButton, resource.GetString("Blends_Difference"), new DifferenceIcon(), BlendEffectMode.Difference);
            this.ConstructGroup(this.ExclusionButton, resource.GetString("Blends_Exclusion"), new ExclusionIcon(), BlendEffectMode.Exclusion);

            this.ConstructGroup(this.HueButton, resource.GetString("Blends_Hue"), new HueIcon(), BlendEffectMode.Hue);
            this.ConstructGroup(this.SaturationButton, resource.GetString("Blends_Saturation"), new SaturationIcon(), BlendEffectMode.Saturation);
            this.ConstructGroup(this.ColorButton, resource.GetString("Blends_Color"), new ColorIcon(), BlendEffectMode.Color);

            this.ConstructGroup(this.LuminosityButton, resource.GetString("Blends_Luminosity"), new LuminosityIcon(), BlendEffectMode.Luminosity);
            this.ConstructGroup(this.SubtractButton, resource.GetString("Blends_Subtract"), new SubtractIcon(), BlendEffectMode.Subtract);
            this.ConstructGroup(this.DivisionButton, resource.GetString("Blends_Division"), new DivisionIcon(), BlendEffectMode.Division);

        }

        //Group
        private void ConstructGroup(Button button, string text, UserControl icon, BlendEffectMode? mode)
        {
            void group(BlendEffectMode? groupMode)
            {
                if (groupMode == mode)
                {
                    button.IsEnabled = false;

                    this.Title = text;
                }
                else button.IsEnabled = true;
            }

            //NoneButton
            group(this.Mode);

            //Buttons
            button.Content = text;
            button.Tag = icon;
            button.Click += (s, e) => this.ModeChanged?.Invoke(this, mode);//Delegate

            //Group
            this.Group += (s, e) => group(e);
        }

    }
}
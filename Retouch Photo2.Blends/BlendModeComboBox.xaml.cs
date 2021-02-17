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
            

            void constructGroup(Button button, UserControl icon, BlendEffectMode mode)
            {
                this.ConstructGroup(button, resource.GetString($"Blends_{mode}"), icon, mode);
            }

            constructGroup(this.MultiplyButton, new MultiplyIcon(), BlendEffectMode.Multiply);
            constructGroup(this.ScreenButton, new ScreenIcon(), BlendEffectMode.Screen);

            constructGroup(this.DarkenButton, new DarkenIcon(), BlendEffectMode.Darken);
            constructGroup(this.LightenButton, new LightenIcon(), BlendEffectMode.Lighten);

            constructGroup(this.DissolveButton, new DissolveIcon(), BlendEffectMode.Dissolve);
            constructGroup(this.ColorBurnButton, new ColorBurnIcon(), BlendEffectMode.ColorBurn);
            constructGroup(this.LinearBurnButton, new LinearBurnIcon(), BlendEffectMode.LinearBurn);

            constructGroup(this.DarkerColorButton, new DarkerColorIcon(), BlendEffectMode.DarkerColor);
            constructGroup(this.LighterColorButton, new LighterColorIcon(), BlendEffectMode.LighterColor);

            constructGroup(this.ColorDodgeButton, new ColorDodgeIcon(), BlendEffectMode.ColorDodge);
            constructGroup(this.LinearDodgeButton, new LinearDodgeIcon(), BlendEffectMode.LinearDodge);

            constructGroup(this.OverlayButton, new OverlayIcon(), BlendEffectMode.Overlay);
            constructGroup(this.SoftLightButton, new SoftLightIcon(), BlendEffectMode.SoftLight);
            constructGroup(this.HardLightButton, new HardLightIcon(), BlendEffectMode.HardLight);
            constructGroup(this.VividLightButton, new VividLightIcon(), BlendEffectMode.VividLight);
            constructGroup(this.LinearLightButton, new LinearLightIcon(), BlendEffectMode.LinearLight);
            constructGroup(this.PinLightButton, new PinLightIcon(), BlendEffectMode.PinLight);

            constructGroup(this.HardMixButton, new HardMixIcon(), BlendEffectMode.HardMix);
            constructGroup(this.DifferenceButton, new DifferenceIcon(), BlendEffectMode.Difference);
            constructGroup(this.ExclusionButton, new ExclusionIcon(), BlendEffectMode.Exclusion);

            constructGroup(this.HueButton, new HueIcon(), BlendEffectMode.Hue);
            constructGroup(this.SaturationButton,  new SaturationIcon(), BlendEffectMode.Saturation);
            constructGroup(this.ColorButton, new ColorIcon(), BlendEffectMode.Color);

            constructGroup(this.LuminosityButton, new LuminosityIcon(), BlendEffectMode.Luminosity);
            constructGroup(this.SubtractButton, new SubtractIcon(), BlendEffectMode.Subtract);
            constructGroup(this.DivisionButton, new DivisionIcon(), BlendEffectMode.Division);

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
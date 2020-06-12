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
            get { return (BlendEffectMode?)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BlendModeComboBox.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(BlendEffectMode?), typeof(BlendModeComboBox), new PropertyMetadata(null, (sender, e) =>
        {
            BlendModeComboBox con = (BlendModeComboBox)sender;

            if (e.NewValue is BlendEffectMode value)
            {
                con.Group?.Invoke(con, value);//Delegate
            }
            else
            {
                con.Group?.Invoke(con, null);//Delegate
            }
        }));


        /// <summary> Gets or sets the title. </summary>
        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
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

            this.ConstructGroup(this.NoneButton, resource.GetString("/Blends/None"), new NoneIcon(), null);

            this.ConstructGroup(this.MultiplyButton, resource.GetString("/Blends/Multiply"), new MultiplyIcon(), BlendEffectMode.Multiply);
            this.ConstructGroup(this.ScreenButton, resource.GetString("/Blends/Screen"), new ScreenIcon(), BlendEffectMode.Screen);

            this.ConstructGroup(this.DarkenButton, resource.GetString("/Blends/Darken"), new DarkenIcon(), BlendEffectMode.Darken);
            this.ConstructGroup(this.LightenButton, resource.GetString("/Blends/Lighten"), new LightenIcon(), BlendEffectMode.Lighten);

            this.ConstructGroup(this.DissolveButton, resource.GetString("/Blends/Dissolve"), new DissolveIcon(), BlendEffectMode.Dissolve);
            this.ConstructGroup(this.ColorBurnButton, resource.GetString("/Blends/ColorBurn"), new ColorBurnIcon(), BlendEffectMode.ColorBurn);
            this.ConstructGroup(this.LinearBurnButton, resource.GetString("/Blends/LinearBurn"), new LinearBurnIcon(), BlendEffectMode.LinearBurn);

            this.ConstructGroup(this.DarkerColorButton, resource.GetString("/Blends/DarkerColor"), new DarkerColorIcon(), BlendEffectMode.DarkerColor);
            this.ConstructGroup(this.LighterColorButton, resource.GetString("/Blends/LighterColor"), new LighterColorIcon(), BlendEffectMode.LighterColor);

            this.ConstructGroup(this.ColorDodgeButton, resource.GetString("/Blends/ColorDodge"), new ColorDodgeIcon(), BlendEffectMode.ColorDodge);
            this.ConstructGroup(this.LinearDodgeButton, resource.GetString("/Blends/LinearDodge"), new LinearDodgeIcon(), BlendEffectMode.LinearDodge);

            this.ConstructGroup(this.OverlayButton, resource.GetString("/Blends/Overlay"), new OverlayIcon(), BlendEffectMode.Overlay);
            this.ConstructGroup(this.SoftLightButton, resource.GetString("/Blends/SoftLight"), new SoftLightIcon(), BlendEffectMode.SoftLight);
            this.ConstructGroup(this.HardLightButton, resource.GetString("/Blends/HardLight"), new HardLightIcon(), BlendEffectMode.HardLight);
            this.ConstructGroup(this.VividLightButton, resource.GetString("/Blends/VividLight"), new VividLightIcon(), BlendEffectMode.VividLight);
            this.ConstructGroup(this.LinearLightButton, resource.GetString("/Blends/LinearLight"), new LinearLightIcon(), BlendEffectMode.LinearLight);
            this.ConstructGroup(this.PinLightButton, resource.GetString("/Blends/PinLight"), new PinLightIcon(), BlendEffectMode.PinLight);

            this.ConstructGroup(this.HardMixButton, resource.GetString("/Blends/HardMix"), new HardMixIcon(), BlendEffectMode.HardMix);
            this.ConstructGroup(this.DifferenceButton, resource.GetString("/Blends/Difference"), new DifferenceIcon(), BlendEffectMode.Difference);
            this.ConstructGroup(this.ExclusionButton, resource.GetString("/Blends/Exclusion"), new ExclusionIcon(), BlendEffectMode.Exclusion);

            this.ConstructGroup(this.HueButton, resource.GetString("/Blends/Hue"), new HueIcon(), BlendEffectMode.Hue);
            this.ConstructGroup(this.SaturationButton, resource.GetString("/Blends/Saturation"), new SaturationIcon(), BlendEffectMode.Saturation);
            this.ConstructGroup(this.ColorButton, resource.GetString("/Blends/Color"), new ColorIcon(), BlendEffectMode.Color);

            this.ConstructGroup(this.LuminosityButton, resource.GetString("/Blends/Luminosity"), new LuminosityIcon(), BlendEffectMode.Luminosity);
            this.ConstructGroup(this.SubtractButton, resource.GetString("/Blends/Subtract"), new SubtractIcon(), BlendEffectMode.Subtract);
            this.ConstructGroup(this.DivisionButton, resource.GetString("/Blends/Division"), new DivisionIcon(), BlendEffectMode.Division);

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
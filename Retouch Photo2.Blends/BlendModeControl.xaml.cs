using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends.Icons;
using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Represents the contorl that is used to select blend mode.
    /// </summary>
    public sealed partial class BlendModeControl : UserControl
    {

        //@Delegate
        public EventHandler<BlendEffectMode?> ModeChanged;

        //Buttons
        private IList<Button> Buttons = new List<Button>();

        #region DependencyProperty


        /// <summary> Gets or sets the blend mode. </summary>
        public BlendEffectMode? Mode
        {
            get { return (BlendEffectMode?)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BlendModeControl.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(BlendEffectMode?), typeof(BlendModeControl), new PropertyMetadata(null, (sender, e) =>
        {
            BlendModeControl con = (BlendModeControl)sender;

            if (e.NewValue is BlendEffectMode value)
            {
                con.BlendGroupType(value);
            }
            else
            {
                con.BlendGroupType();
            }
        }));


        /// <summary> Gets or sets the Title. </summary>
        public object Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BlendModeControl.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(object), typeof(BlendModeControl), new PropertyMetadata(null));


        #endregion


        //@Construct
        public BlendModeControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Loaded += (s, e) =>
            {
                if (this.Mode is BlendEffectMode value)
                {
                    this.BlendGroupType(value);
                }
                else
                {
                    this.BlendGroupType();
                }
            };
        }

       
        //NormalButton
        private void BlendGroupType()
        {
            this.NormalButton.IsEnabled = false;
            this.Title = this.NormalButton.Content;

            foreach (Button item in this.Buttons)
            {
                item.IsEnabled = true;
            }
        }
        //Buttons
        private void BlendGroupType(BlendEffectMode value)
        {
            this.NormalButton.IsEnabled = true;

            int index = (int)value;
            foreach (Button item in this.Buttons)
            {
                bool isSelected = (item.TabIndex == index);

                if (isSelected)
                {
                    this.Title = item.Content;
                    item.IsEnabled = false;
                }
                else
                {
                    item.IsEnabled = true;
                }
            }
        }

    }

    /// <summary>
    /// Retouch_Photo2 Blends 's Control.
    /// </summary>
    public sealed partial class BlendModeControl : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ConstructButton(this.NormalButton, resource.GetString("/Blends/Normal"), new NormalIcon());

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

        private void ConstructButton(Button button, string text, UserControl icon)
        {
            button.Content = text;
            button.Tag = icon;
            button.Tapped += (s, e) =>
            {
                this.ModeChanged?.Invoke(this, null);//Delegate
            };
        }
        private void ConstructButton(Button button, string text, UserControl icon, BlendEffectMode blendMode)
        {
            button.Content = text;
            button.Tag = icon;
            button.TabIndex = (int)blendMode;
            button.Tapped += (s, e) =>
            {
                this.ModeChanged?.Invoke(this, blendMode);//Delegate
            };

            this.Buttons.Add(button);
        }

    }
}
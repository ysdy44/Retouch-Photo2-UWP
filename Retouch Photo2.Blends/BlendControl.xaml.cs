using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends.Icons;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Retouch_Photo2 Blends 's Control.
    /// </summary>
    public sealed partial class BlendControl : UserControl
    {

        //@Delegate
        public EventHandler<BlendEffectMode?> BlendTypeChanged;

        //Buttons
        private IList<BlendButton> Buttons = new List<BlendButton>();

        #region DependencyProperty


        /// <summary> Gets or sets the BlendType. </summary>
        public BlendEffectMode? BlendType
        {
            get { return (BlendEffectMode?)GetValue(BlendTypeProperty); }
            set { SetValue(BlendTypeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BlendControl.BlendType" /> dependency property. </summary>
        public static readonly DependencyProperty BlendTypeProperty = DependencyProperty.Register(nameof(BlendType), typeof(BlendEffectMode?), typeof(BlendControl), new PropertyMetadata(null, (sender, e) =>
        {
            BlendControl con = (BlendControl)sender;

            if (e.NewValue is BlendEffectMode value)
            {
                con.SelectedButtons(value);
            }
            else
            {
                con.SelectedNormalButton();
            }
        }));


        /// <summary> Gets or sets the Title. </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary> Identifies the <see cref = "BlendControl.Title" /> dependency property. </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(BlendControl), new PropertyMetadata(null));


        #endregion


        //@Construct
        public BlendControl()
        {
            this.InitializeComponent();
            this.ConstructStrings();
             
            this.NormalButton.IsSelected = true;
            this.Title = this.NormalButton.Text;
        }

         
        //NormalButton
        private void SelectedNormalButton()
        {
            this.NormalButton.IsSelected = true;
            this.Title = this.NormalButton.Text;

            foreach (BlendButton item in this.Buttons)
            {
                item.IsSelected = false;
            }
        }
        //Buttons
        private void SelectedButtons(BlendEffectMode value)
        {
            this.NormalButton.IsSelected = false;

            foreach (BlendButton item in this.Buttons)
            {
                bool isSelected = (item.BlendType == value);

                if (isSelected)
                {
                    this.Title = item.Text;
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }

    }

    /// <summary>
    /// Retouch_Photo2 Blends 's Control.
    /// </summary>
    public sealed partial class BlendControl : UserControl
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.NormalButton.Text = resource.GetString("/Blends/Normal");
            this.NormalButton.CenterContent = new NormalIcon();
            this.NormalButton.BlendType = null;
            this.NormalButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, null);//Delegate

            this.MultiplyButton.Text = resource.GetString("/Blends/Multiply");
            this.MultiplyButton.CenterContent = new MultiplyIcon();
            this.MultiplyButton.BlendType = BlendEffectMode.Multiply;
            this.MultiplyButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Multiply);//Delegate
            this.Buttons.Add(this.MultiplyButton);

            this.ScreenButton.Text = resource.GetString("/Blends/Screen");
            this.ScreenButton.CenterContent = new ScreenIcon();
            this.ScreenButton.BlendType = BlendEffectMode.Screen;
            this.ScreenButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Screen);//Delegate
            this.Buttons.Add(this.ScreenButton);

            this.DarkenButton.Text = resource.GetString("/Blends/Darken");
            this.DarkenButton.CenterContent = new DarkenIcon();
            this.DarkenButton.BlendType = BlendEffectMode.Darken;
            this.DarkenButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Darken);//Delegate
            this.Buttons.Add(this.DarkenButton);

            this.LightenButton.Text = resource.GetString("/Blends/Lighten");
            this.LightenButton.CenterContent = new LightenIcon();
            this.LightenButton.BlendType = BlendEffectMode.Lighten;
            this.LightenButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Lighten);//Delegate
            this.Buttons.Add(this.LightenButton);

            this.DissolveButton.Text = resource.GetString("/Blends/Dissolve");
            this.DissolveButton.CenterContent = new DissolveIcon();
            this.DissolveButton.BlendType = BlendEffectMode.Dissolve;
            this.DissolveButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Dissolve);//Delegate
            this.Buttons.Add(this.DissolveButton);

            this.ColorBurnButton.Text = resource.GetString("/Blends/ColorBurn");
            this.ColorBurnButton.CenterContent = new ColorBurnIcon();
            this.ColorBurnButton.BlendType = BlendEffectMode.ColorBurn;
            this.ColorBurnButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.ColorBurn);//Delegate
            this.Buttons.Add(this.ColorBurnButton);

            this.LinearBurnButton.Text = resource.GetString("/Blends/LinearBurn");
            this.LinearBurnButton.CenterContent = new LinearBurnIcon();
            this.LinearBurnButton.BlendType = BlendEffectMode.LinearBurn;
            this.LinearBurnButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.LinearBurn);//Delegate
            this.Buttons.Add(this.LinearBurnButton);

            this.DarkerColorButton.Text = resource.GetString("/Blends/DarkerColor");
            this.DarkerColorButton.CenterContent = new DarkerColorIcon();
            this.DarkerColorButton.BlendType = BlendEffectMode.DarkerColor;
            this.DarkerColorButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.DarkerColor);//Delegate
            this.Buttons.Add(this.DarkerColorButton);

            this.LighterColorButton.Text = resource.GetString("/Blends/LighterColor");
            this.LighterColorButton.CenterContent = new LighterColorIcon();
            this.LighterColorButton.BlendType = BlendEffectMode.LighterColor;
            this.LighterColorButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.LighterColor);//Delegate
            this.Buttons.Add(this.LighterColorButton);

            this.ColorDodgeButton.Text = resource.GetString("/Blends/ColorDodge");
            this.ColorDodgeButton.CenterContent = new ColorDodgeIcon();
            this.ColorDodgeButton.BlendType = BlendEffectMode.ColorDodge;
            this.ColorDodgeButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.ColorDodge);//Delegate
            this.Buttons.Add(this.ColorDodgeButton);

            this.LinearDodgeButton.Text = resource.GetString("/Blends/LinearDodge");
            this.LinearDodgeButton.CenterContent = new LinearDodgeIcon();
            this.LinearDodgeButton.BlendType = BlendEffectMode.LinearDodge;
            this.LinearDodgeButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.LinearDodge);//Delegate
            this.Buttons.Add(this.LinearDodgeButton);

            this.OverlayButton.Text = resource.GetString("/Blends/Overlay");
            this.OverlayButton.CenterContent = new OverlayIcon();
            this.OverlayButton.BlendType = BlendEffectMode.Overlay;
            this.OverlayButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Overlay);//Delegate
            this.Buttons.Add(this.OverlayButton);

            this.SoftLightButton.Text = resource.GetString("/Blends/SoftLight");
            this.SoftLightButton.CenterContent = new SoftLightIcon();
            this.SoftLightButton.BlendType = BlendEffectMode.SoftLight;
            this.SoftLightButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.SoftLight);//Delegate
            this.Buttons.Add(this.SoftLightButton);

            this.HardLightButton.Text = resource.GetString("/Blends/HardLight");
            this.HardLightButton.CenterContent = new HardLightIcon();
            this.HardLightButton.BlendType = BlendEffectMode.HardLight;
            this.HardLightButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.HardLight);//Delegate
            this.Buttons.Add(this.HardLightButton);
            
            this.VividLightButton.Text = resource.GetString("/Blends/VividLight");
            this.VividLightButton.CenterContent = new VividLightIcon();
            this.VividLightButton.BlendType = BlendEffectMode.VividLight;
            this.VividLightButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.VividLight);//Delegate
            this.Buttons.Add(this.VividLightButton);

            this.LinearLightButton.Text = resource.GetString("/Blends/LinearLight");
            this.LinearLightButton.CenterContent = new LinearLightIcon();
            this.LinearLightButton.BlendType = BlendEffectMode.LinearLight;
            this.LinearLightButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.LinearLight);//Delegate
            this.Buttons.Add(this.LinearLightButton);

            this.PinLightButton.Text = resource.GetString("/Blends/PinLight");
            this.PinLightButton.CenterContent = new PinLightIcon();
            this.PinLightButton.BlendType = BlendEffectMode.PinLight;
            this.PinLightButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.PinLight);//Delegate
            this.Buttons.Add(this.PinLightButton);

            this.HardMixButton.Text = resource.GetString("/Blends/HardMix");
            this.HardMixButton.CenterContent = new HardMixIcon();
            this.HardMixButton.BlendType = BlendEffectMode.HardMix;
            this.HardMixButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.HardMix);//Delegate
            this.Buttons.Add(this.HardMixButton);

            this.DifferenceButton.Text = resource.GetString("/Blends/Difference");
            this.DifferenceButton.CenterContent = new DifferenceIcon();
            this.DifferenceButton.BlendType = BlendEffectMode.Difference;
            this.DifferenceButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Difference);//Delegate
            this.Buttons.Add(this.DifferenceButton);

            this.ExclusionButton.Text = resource.GetString("/Blends/Exclusion");
            this.ExclusionButton.CenterContent = new ExclusionIcon();
            this.ExclusionButton.BlendType = BlendEffectMode.Exclusion;
            this.ExclusionButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Exclusion);//Delegate
            this.Buttons.Add(this.ExclusionButton);

            this.HueButton.Text = resource.GetString("/Blends/Hue");
            this.HueButton.CenterContent = new HueIcon();
            this.HueButton.BlendType = BlendEffectMode.Hue;
            this.HueButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Hue);//Delegate
            this.Buttons.Add(this.HueButton);

            this.SaturationButton.Text = resource.GetString("/Blends/Saturation");
            this.SaturationButton.CenterContent = new SaturationIcon();
            this.SaturationButton.BlendType = BlendEffectMode.Saturation;
            this.SaturationButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Saturation);//Delegate
            this.Buttons.Add(this.SaturationButton);

            this.ColorButton.Text = resource.GetString("/Blends/Color");
            this.ColorButton.CenterContent = new ColorIcon();
            this.ColorButton.BlendType = BlendEffectMode.Color;
            this.ColorButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Color);//Delegate
            this.Buttons.Add(this.ColorButton);

            this.LuminosityButton.Text = resource.GetString("/Blends/Luminosity");
            this.LuminosityButton.CenterContent = new LuminosityIcon();
            this.LuminosityButton.BlendType = BlendEffectMode.Luminosity;
            this.LuminosityButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Luminosity);//Delegate
            this.Buttons.Add(this.LuminosityButton);

            this.SubtractButton.Text = resource.GetString("/Blends/Subtract");
            this.SubtractButton.CenterContent = new SubtractIcon();
            this.SubtractButton.BlendType = BlendEffectMode.Subtract;
            this.SubtractButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Subtract);//Delegate
            this.Buttons.Add(this.SubtractButton);

            this.DivisionButton.Text = resource.GetString("/Blends/Division");
            this.DivisionButton.CenterContent = new DivisionIcon();
            this.DivisionButton.BlendType = BlendEffectMode.Division;
            this.DivisionButton.Tapped += (s, e) => this.BlendTypeChanged?.Invoke(this, BlendEffectMode.Division);//Delegate            
            this.Buttons.Add(this.DivisionButton);
        }

    }
}
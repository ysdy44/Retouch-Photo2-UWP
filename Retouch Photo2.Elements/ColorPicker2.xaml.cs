using HSVColorPickers;
using Retouch_Photo2.Elements.ColorPicker2Icons;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Enum of <see cref="ColorPicker2"/>.
    /// </summary>
    public enum ColorPicker2Mode
    {
        None,

        Swatches,

        Wheel,

        RGB,
        HSV,

        PaletteHue,
        PaletteSaturation,
        PaletteValue,

        Circle,
    }

    internal class ColorPicker2Item
    {
        public ColorPicker2Mode Mode;
        public Button Button;
        public IColorPicker ColorPicker;
    };

    /// <summary>
    /// Color picker (ง •̀_•́)ง
    /// </summary>
    public sealed partial class ColorPicker2 : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the color value changes. </summary>
        public event ColorChangeHandler ColorChange;
        /// <summary> Occurs when the alpha value changes. </summary>
        public event AlphaChangeHandler AlphaChange;


        private IList<ColorPicker2Item> Items = new List<ColorPicker2Item>();


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "ColorPicker2" />'s Mode. </summary>
        public ColorPicker2Mode Mode
        {
            get { return (ColorPicker2Mode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        /// <summary> Identifies the <see cref = "ColorPicker2.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(ColorPicker2Mode), typeof(ColorPicker2), new PropertyMetadata(ColorPicker2Mode.Circle, (sender, e) =>
        {
            ColorPicker2 con = (ColorPicker2)sender;

            if (e.NewValue is ColorPicker2Mode value)
            {
                con.ColorPickerGroupType(value);
            }
        }));


        /// <summary> Gets or sets picker's color. </summary>
        public Color Color
        {
            get => Color.FromArgb(this.Alpha, this.R, this.G, this.B);
            set
            {
                if (value.A == this.Alpha)
                {
                    if (value.R == this.R)
                        if (value.G == this.G)
                            if (value.B == this.B)
                                return;
                }
                else
                {
                    this.Alpha = value.A;
                }

                this.SetCurrentColorPickerColor(this.Mode, Color.FromArgb(255, value.R, value.G, value.B));

                this.R = value.R;
                this.G = value.G;
                this.B = value.B;
            }
        }

        private Color _Color
        {
            set
            {
                if (value.A == this.Alpha)
                {
                    if (value.R == this.R)
                        if (value.G == this.G)
                            if (value.B == this.B)
                                return;
                }

                this.R = value.R;
                this.G = value.G;
                this.B = value.B;

                this.ColorChange?.Invoke(this, Color.FromArgb(this.Alpha, value.R, value.G, value.B));//Delegate
            }
        }



        /// <summary> Gets or sets picker's alpha. </summary>
        public byte Alpha
        {
            get => this.AlphaPicker.Alpha;
            set => this.AlphaPicker.Alpha = value;
        }

        private byte _Alpha
        {
            set
            {
                this.AlphaChange?.Invoke(this, value);//Delegate
                this.ColorChange?.Invoke(this, Color.FromArgb(value, this.R, this.G, this.B)); //Delegate
            }
        }

        /// <summary> Gets or sets picker's red. </summary>
        private byte R = 255;
        /// <summary> Gets or sets picker's green. </summary>
        private byte G = 255;
        /// <summary> Gets or sets picker's blue. </summary>
        private byte B = 255;


        #endregion
        

        //@Construct
        /// <summary>
        /// Construct a ColorPicker2.
        /// </summary>
        public ColorPicker2()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.Loaded += (s, e) => this.ColorPickerGroupType(this.Mode);
            this.Button.Tapped += (s, e) => this.Flyout.ShowAt(this.Button);

            //Alpha
            this.Alpha = 255;
            this.AlphaPicker.AlphaChange += (s, value) => this._Alpha = value;

            //Hex
            this.HexPicker.Color = this.Color;
            this.HexPicker.ColorChange += (s, color) =>
            {
                this._Color = color;

                this.SetCurrentColorPickerColor(this.Mode, color);

                this.StrawPicker.Color = color;
            };
            //Straw
            this.StrawPicker.Color = this.Color;
            this.StrawPicker.ColorChange += (s, color) =>
            {
                this._Color = color;

                this.SetCurrentColorPickerColor(this.Mode, color);

                this.HexPicker.Color = color;
            };
        }

        
        //Items
        private void ColorPickerGroupType(ColorPicker2Mode value)
        {
            foreach (ColorPicker2Item item in this.Items)
            {
                Button button = item.Button;
                IColorPicker colorPicker = item.ColorPicker;

                bool isSelected = (item.Mode == value);

                if (isSelected)
                {
                    this.Button.Content = button.Content;
                    button.IsEnabled = false;
                    colorPicker.Self.Visibility = Visibility.Visible;
                    colorPicker.Color = this.Color;
                }
                else
                {
                    button.IsEnabled = true;
                    colorPicker.Self.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SetCurrentColorPickerColor(ColorPicker2Mode value, Color color)
        {
            foreach (ColorPicker2Item item in this.Items)
            {
                IColorPicker colorPicker = item.ColorPicker;

                bool isSelected = (item.Mode == value);

                if (isSelected)
                {
                    colorPicker.Color = this.Color;
                }
            }
        }

    }

    /// <summary>
    /// Color picker (ง •̀_•́)ง
    /// </summary>
    public sealed partial class ColorPicker2 : UserControl
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            //Swatches
            this.ConstructButton(this.SwatchesButton, this.SwatchesPicker, resource.GetString("/ToolElements/Color_Swatches"), new SwatchesIcon(), ColorPicker2Mode.Swatches);

            //Wheel
            this.ConstructButton(this.WheelButton, this.WheelPicker, resource.GetString("/ToolElements/Color_Wheel"), new WheelIcon(), ColorPicker2Mode.Wheel);

            //RGB          
            this.ConstructButton(this.RGBButton, this.RGBPicker, resource.GetString("/ToolElements/Color_RGB"), new RGBIcon(), ColorPicker2Mode.RGB);
            //HSV
            this.ConstructButton(this.HSVButton, this.HSVPicker, resource.GetString("/ToolElements/Color_HSV"), new HSVIcon(), ColorPicker2Mode.HSV);

            //PaletteHue
            this.ConstructButton(this.PaletteHueButton, this.PaletteHuePicker, resource.GetString("/ToolElements/Color_PaletteHue"), new PaletteHueIcon(), ColorPicker2Mode.PaletteHue);
            //PaletteSaturation
            this.ConstructButton(this.PaletteSaturationButton, this.PaletteSaturationPicker, resource.GetString("/ToolElements/Color_PaletteSaturation"), new PaletteSaturationIcon(), ColorPicker2Mode.PaletteSaturation);
            //PaletteValue
            this.ConstructButton(this.PaletteValueButton, this.PaletteValuePicker, resource.GetString("/ToolElements/Color_PaletteValue"), new PaletteValueIcon(), ColorPicker2Mode.PaletteValue);

            //Circle
            this.ConstructButton(this.CircleButton, this.CirclePicker, resource.GetString("/ToolElements/Color_Circle"), new CircleIcon(), ColorPicker2Mode.Circle);
        }

        private void ConstructButton(Button button, IColorPicker colorPicker, string text, UserControl icon, ColorPicker2Mode mode)
        {          
            button.Content = text;
            button.Tag = icon;
            button.Tapped += (s, e) =>
            {
                this.Mode = mode;
                this.Flyout.Hide();
            };

            colorPicker.ColorChange += (s, value) =>
            {
                this._Color = value;

                this.HexPicker.Color = value;
                this.StrawPicker.Color = value;
            };

            this.Items.Add(new ColorPicker2Item
            {
                Mode = mode,
                Button = button,
                ColorPicker = colorPicker,
            });
        }

    }
}
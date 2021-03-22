// Core:              ★★
// Referenced:   ★★★★
// Difficult:         ★★★
// Only:              ★★★
// Complete:      ★★
using HSVColorPickers;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Color picker (ง •̀_•́)ง
    /// </summary>
    public sealed partial class ColorPicker2 : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the color value changed. </summary>
        public event ColorChangeHandler ColorChanged;
        /// <summary> Occurs when the color change starts. </summary>
        public event ColorChangeHandler ColorChangeStarted;
        /// <summary> Occurs when color change. </summary>
        public event ColorChangeHandler ColorChangeDelta;
        /// <summary> Occurs when the color change is complete. </summary>
        public event ColorChangeHandler ColorChangeCompleted;


        //@Group
        private EventHandler<ColorPicker2Mode> Group;
        private EventHandler<Color> ChangeColor;

        /// <summary> Gets hex picker. </summary>
        public TextBox HexPicker => this._HexPicker;


        #region DependencyProperty


        /// <summary> Gets or sets <see cref = "ColorPicker2" />'s Mode. </summary>
        public ColorPicker2Mode Mode
        {
            get => (ColorPicker2Mode)base.GetValue(ModeProperty);
            set => base.SetValue(ModeProperty, value);
        }
        /// <summary> Identifies the <see cref = "ColorPicker2.Mode" /> dependency property. </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(ColorPicker2Mode), typeof(ColorPicker2), new PropertyMetadata(ColorPicker2Mode.Circle, (sender, e) =>
        {
            ColorPicker2 control = (ColorPicker2)sender;

            if (e.NewValue is ColorPicker2Mode value)
            {
                control.Group?.Invoke(control, value);
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
                    if (value.R == this.R) if (value.G == this.G) if (value.B == this.B) return;
                }
                else this.Alpha = value.A;


                Color color = Color.FromArgb(255, value.R, value.G, value.B);
                this.ChangeColor?.Invoke(this, color);//Delegate
                this._HexPicker.Color = color;

                this.R = value.R;
                this.G = value.G;
                this.B = value.B;
            }
        }

        private Color _Color
        {
            set
            {
                if (value.A == this.Alpha) if (value.R == this.R) if (value.G == this.G) if (value.B == this.B) return;

                this.R = value.R;
                this.G = value.G;
                this.B = value.B;

                Color color = Color.FromArgb(this.Alpha, value.R, value.G, value.B);
                this.ColorChanged?.Invoke(this, color);//Delegate
                this._HexPicker.Color = color;
            }
        }
        private Color _ColorStarted
        {
            set
            {
                //if (value.A == this.Alpha) if (value.R == this.R) if (value.G == this.G) if (value.B == this.B) return;

                this.R = value.R;
                this.G = value.G;
                this.B = value.B;

                Color color = Color.FromArgb(this.Alpha, value.R, value.G, value.B);
                this.ColorChangeStarted?.Invoke(this, color);//Delegate
            }
        }
        private Color _ColorDelta
        {
            set
            {
                if (value.A == this.Alpha) if (value.R == this.R) if (value.G == this.G) if (value.B == this.B) return;

                this.R = value.R;
                this.G = value.G;
                this.B = value.B;

                Color color = Color.FromArgb(this.Alpha, value.R, value.G, value.B);
                this.ColorChangeDelta?.Invoke(this, color);//Delegate
            }
        }
        private Color _ColorCompleted
        {
            set
            {
                //if (value.A == this.Alpha) if (value.R == this.R) if (value.G == this.G) if (value.B == this.B) return;

                this.R = value.R;
                this.G = value.G;
                this.B = value.B;

                Color color = Color.FromArgb(this.Alpha, value.R, value.G, value.B);
                this.ColorChangeCompleted?.Invoke(this, color);//Delegate
                this._HexPicker.Color = color;
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
            set => this.ColorChanged?.Invoke(this, Color.FromArgb(value, this.R, this.G, this.B)); //Delegate
        }
        private byte _AlphaStarted
        {
            set => this.ColorChangeStarted?.Invoke(this, Color.FromArgb(value, this.R, this.G, this.B)); //Delegate
        }
        private byte _AlphaDelta
        {
            set => this.ColorChangeDelta?.Invoke(this, Color.FromArgb(value, this.R, this.G, this.B)); //Delegate
        }
        private byte _AlphaCompleted
        {
            set => this.ColorChangeCompleted?.Invoke(this, Color.FromArgb(value, this.R, this.G, this.B)); //Delegate
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
        /// Initializes a ColorPicker2.
        /// </summary>
        public ColorPicker2()
        {
            this.InitializeComponent();
            this.ConstructFlowDirection();
            this.ConstructStrings();
            this.Button.Click += (s, e) => this.Flyout.ShowAt(this.HeadGrid);
            this.Button.SizeChanged += (s, e) => this.FlyoutStackPanel.Width = e.NewSize.Width;

            //Alpha
            this.Alpha = 255;
            this.AlphaPicker.AlphaChanged += (s, value) => this._Alpha = value;
            this.AlphaPicker.AlphaChangeStarted += (s, value) => this._AlphaStarted = value;
            this.AlphaPicker.AlphaChangeDelta += (s, value) => this._AlphaDelta = value;
            this.AlphaPicker.AlphaChangeCompleted += (s, value) => this._AlphaCompleted = value;

            //Hex
            this._HexPicker.Color = this.Color;
            this._HexPicker.ColorChanged += (s, color) =>
            {
                this._Color = color;
                this.ChangeColor?.Invoke(this, color);
            };
            //Straw
            this._StrawPicker.Color = this.Color;
            this._StrawPicker.ColorChanged += (s, color) =>
            {
                this._Color = color;
                this.ChangeColor?.Invoke(this, color);
            };
        }

    }

    /// <summary>
    /// Color picker (ง •̀_•́)ง
    /// </summary>
    public sealed partial class ColorPicker2 : UserControl
    {

        //FlowDirection
        private void ConstructFlowDirection()
        {
            bool isRightToLeft = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;

            base.FlowDirection = isRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;


            this.ConstructFlowDirectionCore(this.SwatchesPicker);

            this.WheelPicker.FlowDirection = FlowDirection.LeftToRight;

            this.ConstructFlowDirectionCore(this.RGBPicker);
            this.ConstructFlowDirectionCore(this.HSVPicker);

            this.PaletteHuePicker.FlowDirection = FlowDirection.LeftToRight;
            this.PaletteSaturationPicker.FlowDirection = FlowDirection.LeftToRight;
            this.PaletteValuePicker.FlowDirection = FlowDirection.LeftToRight;
            this.CirclePicker.FlowDirection = FlowDirection.LeftToRight;

            this.ConstructFlowDirectionCore(this.AlphaPicker);
        }
        private void ConstructFlowDirectionCore(UserControl iColorPicker)
        {
            if (iColorPicker.Content is Grid grid)
            {
                foreach (UIElement element in grid.Children)
                {
                    this.ConstructFlowDirectionCoreCore(element);
                }
            }
        }
        private void ConstructFlowDirectionCoreCore(UIElement element)
        {
            if (element is CanvasControl canvasControl)
            {
                //CanvasControl
                canvasControl.FlowDirection = FlowDirection.LeftToRight;
            }
            else if (element is TouchSlider touchSlider)
            {
                //TouchSlider
                touchSlider.FlowDirection = FlowDirection.LeftToRight;
            }
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();


            //Swatches
            constructGroup(this.SwatchesButton, this.SwatchesPicker, ColorPicker2Mode.Swatches);

            //Wheel
            constructGroup(this.WheelButton, this.WheelPicker, ColorPicker2Mode.Wheel);

            //RGB          
            constructGroup(this.RGBButton, this.RGBPicker, ColorPicker2Mode.RGB);
            //HSV
            constructGroup(this.HSVButton, this.HSVPicker, ColorPicker2Mode.HSV);

            //PaletteHue
            constructGroup(this.PaletteHueButton, this.PaletteHuePicker, ColorPicker2Mode.PaletteHue);
            //PaletteSaturation
            constructGroup(this.PaletteSaturationButton, this.PaletteSaturationPicker, ColorPicker2Mode.PaletteSaturation);
            //PaletteValue
            constructGroup(this.PaletteValueButton, this.PaletteValuePicker, ColorPicker2Mode.PaletteValue);

            //Circle
            constructGroup(this.CircleButton, this.CirclePicker, ColorPicker2Mode.Circle);


            void constructGroup(Button button, IColorPicker colorPicker, ColorPicker2Mode mode)
            {
                string text = resource.GetString($"Menus_Color_{mode}");

                void group(ColorPicker2Mode groupMode)
                {
                    if (groupMode == mode)
                    {
                        button.IsEnabled = false;
                        colorPicker.Self.Visibility = Visibility.Visible;

                        colorPicker.Color = this.Color;
                        this.Button.Content = text;
                    }
                    else
                    {
                        button.IsEnabled = true;
                        colorPicker.Self.Visibility = Visibility.Collapsed;
                    }
                }

                //NoneButton
                group(this.Mode);

                //Buttons
                button.Content = text;
                button.Click += (s, e) =>
                {
                    this.Mode = mode;
                    this.Flyout.Hide();
                };
                colorPicker.ColorChanged += (s, value) => this._Color = value;
                colorPicker.ColorChangeStarted += (s, value) => this._ColorStarted = value;
                colorPicker.ColorChangeDelta += (s, value) => this._ColorDelta = value;
                colorPicker.ColorChangeCompleted += (s, value) => this._ColorCompleted = value;

                //Change
                this.Group += (s, e) => group(e);
                this.ChangeColor += (s, color) =>
                {
                    if (this.Mode == mode)
                    {
                        colorPicker.Color = color;
                    }
                };
            }
        }

    }
}
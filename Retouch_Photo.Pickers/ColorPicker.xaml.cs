using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Pickers
{

    interface IPicker
    {
        event ColorChangeHandler ColorChange;

        void SetColor(Color value);
        Color GetColor();
    }
    class Picker
    {
        public string Name;
        public IPicker Control;
    }


    public sealed partial class ColorPicker : UserControl
    {
        //Delegate
        public event ColorChangeHandler ColorChange;


        #region Picker


        Picker[] Pickers = new Picker[]
        {            
            new Picker
            {
                Name="SwatchesPicker",
                Control=new SwatchesPicker(),
            },
            new Picker
            {
                Name="WheelPicker",
                Control=new WheelPicker(),
            },
            new Picker
            {
                Name="RGBPicker",
                Control=new RGBPicker(),
            },
            new Picker
            {
                Name="HSLPicker",
                Control=new HSLPicker(),
            },
            new Picker
            {
                Name="PaletteHue",
                Control=new PalettePicker(new PaletteHue()),
            },
            new Picker
            {
                Name="PaletteSaturation",
                Control=new PalettePicker(new PaletteSaturation()),
            },
            new Picker
            {
                Name="PaletteLightness",
                Control=new PalettePicker(new PaletteLightness()),
            },
        };

        private int index;
        public int Index
        {
            get => index;
            set
            {
                IPicker newControl = this.Pickers[value].Control;

                if (value != this.index)
                {
                    IPicker oldControl = this.Pickers[this.index].Control;
                    oldControl.ColorChange -= this.Picker_ColorChange;
                }

                this.ContentControl.Content = newControl;
                newControl.SetColor(this._Color);

                newControl.ColorChange += this.Picker_ColorChange;

                index = value;
            }
        }


        #endregion


        #region Color


        /// <summary> Color of Color Picker </summary>
        public Color Color
        {
            get => Color.FromArgb(this.AlphaPicker.Alpha, this.SolidColorBrushName.Color.R, this.SolidColorBrushName.Color.G, this.SolidColorBrushName.Color.B);
            set
            {
                if (value.A != this.AlphaPicker.Alpha) this.AlphaPicker.Alpha = value.A;

                if (value.A == this.AlphaPicker.Alpha && value.R == this.SolidColorBrushName.Color.R && value.G == this.SolidColorBrushName.Color.G && value.B == this.SolidColorBrushName.Color.B)
                    return;

                Color color = Color.FromArgb(255, value.R, value.G, value.B);
                this.Pickers[this.Index].Control.SetColor(color);

                this.SolidColorBrushName.Color = color;
            }
        }

        private Color _Color
        {
            get => this.SolidColorBrushName.Color;
            set
            {
                if (value.A == this.AlphaPicker.Alpha && value.R == this.SolidColorBrushName.Color.R && value.G == this.SolidColorBrushName.Color.G && value.B == this.SolidColorBrushName.Color.B)
                    return;

                this.SolidColorBrushName.Color = Color.FromArgb(255, value.R, value.G, value.B);
                this.ColorChange?.Invoke(this, Color.FromArgb(this.AlphaPicker.Alpha, value.R, value.G, value.B));
            }
        }



        /// <summary> Alpha of Color Picker </summary>
        public byte Alpha
        {
            get => this.AlphaPicker.Alpha;
            set => this.AlphaPicker.Alpha = value;
        }

        private byte _Alpha
        {
            get => this.AlphaPicker.Alpha;
            set => this.ColorChange?.Invoke(this, Color.FromArgb(value, this.SolidColorBrushName.Color.R, this.SolidColorBrushName.Color.G, this.SolidColorBrushName.Color.B));
        }


        #endregion


        //HexOrStraw
        private bool hexOrStraw;
        public bool HexOrStraw
        {
            get => hexOrStraw;
            set
            {
                if (value)
                {
                    this.HexPicker.Visibility = Visibility.Visible;
                    this.StrawPicker.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.HexPicker.Visibility = Visibility.Collapsed;
                    this.StrawPicker.Visibility = Visibility.Visible;
                }
                hexOrStraw = value;
            }
        }

        public ColorPicker()
        {
            this.InitializeComponent();

            //Picker
            this.Index = 0;
            this.ComboBox.SelectedIndex = this.Index;
            this.ComboBox.SelectionChanged += (sender, e) => this.Index = this.ComboBox.SelectedIndex;

            //Alpha
            this.AlphaPicker.Alpha = 255;
            this.AlphaPicker.AlphaChange += (sender, value) => this._Alpha = value;

            //HexOrStraw
            this.HexPicker.Color = this.Color;
            this.HexPicker.ColorChange += this.Picker_ColorChange2;
            this.StrawPicker.ColorChange += this.Picker_ColorChange2;
            this.HexOrStraw = false;
            this.HexOrStrawButton.Tapped += (sender, value) => this.HexOrStraw = !this.HexOrStraw;
        }

        private void Picker_ColorChange(object sender, Color value)
        {
            this._Color = value;
            this.HexPicker.Color = value;
        }
        private void Picker_ColorChange2(object sender, Color value)
        {
            this._Color = value;
            this.Pickers[this.Index].Control.SetColor(value);
        }
    }
}

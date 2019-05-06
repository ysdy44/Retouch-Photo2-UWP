using HSVColorPickers;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs
{
    public sealed partial class BrushColorPicker : UserControl
    {
        //Delegate
        public event ColorChangeHandler ColorChange;


        #region Picker


        Picker[] Pickers = new Picker[]
        {
            new Picker( "Swatches",new SwatchesPicker()),
            new Picker( "Wheel",new WheelPicker()),
            new Picker( "RGB",new RGBPicker()),
            new Picker( "HSV",new HSVPicker()),
            new Picker( "Palette Hue",PalettePicker.CreateFormHue()),
            new Picker( "Palette Saturation",PalettePicker.CreateFormSaturation()),
            new Picker( "Palette Value",PalettePicker.CreateFormValue()),
        };

        private int index;
        public int Index
        {
            get => this.index;
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

                this.index = value;
            }
        }


        #endregion


        #region Color


        /// <summary> Color of Color Picker </summary>
        public Color Color
        {
            get => Color.FromArgb(this.Alpha, this._color.R, this._color.G, this._color.B);
            set
            {
                if (value.A != this.Alpha) this.Alpha = value.A;

                if (value.A == this.Alpha && value.R == this._color.R && value.G == this._color.G && value.B == this._color.B)
                    return;

                Color color = Color.FromArgb(255, value.R, value.G, value.B);
                this.Pickers[this.Index].Control.SetColor(color);

                this._color = color;
            }
        }

        private Color _Color
        {
            get => this._color;
            set
            {
                if (value.A == this.Alpha && value.R == this._color.R && value.G == this._color.G && value.B == this._color.B)
                    return;

                this._color = Color.FromArgb(255, value.R, value.G, value.B);
                this.ColorChange?.Invoke(this, Color.FromArgb(this.Alpha, value.R, value.G, value.B));//Delegate
            }
        }

        private Color _color;



        /// <summary> Alpha of Color Picker </summary>
        public byte Alpha;


        #endregion
        

        public BrushColorPicker()
        {
            this.InitializeComponent();

            //Picker
            this.Index = 0;
            this.ComboBox.SelectedIndex = this.Index;
            this.ComboBox.SelectionChanged += (s, e) => this.Index = this.ComboBox.SelectedIndex;

            //Alpha
            this.Alpha = 255;
        }

        private void Picker_ColorChange(object sender, Color value)
        {
            this._Color = value;
        }
        private void Picker_ColorChange2(object sender, Color value)
        {
            this._Color = value;
            this.Pickers[this.Index].Control.SetColor(value);
        }
    }
}


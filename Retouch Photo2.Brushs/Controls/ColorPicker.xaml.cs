using HSVColorPickers;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Brushs.Controls
{
    /// <summary>
    /// Color picker (ง •̀_•́)ง
    /// </summary>
    public sealed partial class ColorPicker : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the color value changes. </summary>
        public event ColorChangeHandler ColorChange;


        #region Picker

        /// <summary> All pickers. </summary>
        public IColorPicker[] Pickers = new IColorPicker[]
        {
            new SwatchesPicker(),
            new WheelPicker(),
            new RGBPicker(),
            new HSVPicker(),
            PalettePicker.CreateFormHue(),
            PalettePicker.CreateFormSaturation(),
            PalettePicker.CreateFormValue(),
        };

        /// <summary> Get or set index of the current picker. </summary>
        public int Index
        {
            get => this.index;
            set
            {
                IColorPicker newControl = this.Pickers[value];

                if (value != this.index)
                {
                    IColorPicker oldControl = this.Pickers[this.index];
                    oldControl.ColorChange -= this.Picker_ColorChange;
                }

                this.ContentControl.Content = newControl;
                newControl.Color = this._Color;

                newControl.ColorChange += this.Picker_ColorChange;

                this.index = value;
            }
        }
        private int index;


        #endregion


        #region Color


        /// <summary> Get or set current color. </summary>
        public Color Color
        {
            get => Color.FromArgb(255, this._color.R, this._color.G, this._color.B);
            set
            {

                if ( value.R == this._color.R && value.G == this._color.G && value.B == this._color.B)
                    return;

                Color color = Color.FromArgb(255, value.R, value.G, value.B);
                this.Pickers[this.Index].Color = color;

                this._color = color;
            }
        }

        private Color _Color
        {
            get => this._color;
            set
            {
                if (value.R == this._color.R && value.G == this._color.G && value.B == this._color.B)
                    return;

                this._color = Color.FromArgb(255, value.R, value.G, value.B);
                this.ColorChange?.Invoke(this, Color.FromArgb(255, value.R, value.G, value.B));//Delegate
            }
        }

        private Color _color
        {
            get => this.SolidColorBrushName.Color;
            set => this.SolidColorBrushName.Color = value;
        }
        

        #endregion
        

        //@Construct
        public ColorPicker()
        {
            this.InitializeComponent();
            this.ComboBox.ItemsSource = from p in this.Pickers select p.Type;

            //Picker
            this.Index = 0;
            this.ComboBox.SelectedIndex = this.Index;
            this.ComboBox.SelectionChanged += (s, e) => this.Index = this.ComboBox.SelectedIndex;
        }

        private void Picker_ColorChange(object sender, Color value)
        {
            this._Color = value;
        }
    }
}
using Retouch_Photo.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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


        #region DependencyProperty


        public Color _Color
        {
            get => this.SolidColorBrushName.Color;
            set
            {
                this.SolidColorBrushName.Color = Color.FromArgb(255, value.R, value.G, value.B);

                this.ColorChange?.Invoke(this, Color.FromArgb(this.AlphaPicker.Alpha, value.R, value.G, value.B));
            }
        }
        public Color Color
        {
            get => this.SolidColorBrushName.Color;
            set
            {
                this.AlphaPicker.Alpha = value.A;

                Color color = Color.FromArgb(255, value.R, value.G, value.B);

                this.Pickers[this.Index].Control.SetColor(color);

                this.SolidColorBrushName.Color = color;
            }
        }


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
                newControl.SetColor(this.Color);

                newControl.ColorChange += this.Picker_ColorChange;

                index = value;
            }
        }


        #endregion

        public ColorPicker()
        {
            this.InitializeComponent();

            this.Loaded += (sender2, e2) =>
            {
                this.Index = 0;
                this.ComboBox.SelectedIndex = 0;
                this.AlphaPicker.Alpha = this.Color.A;

                this.ComboBox.SelectionChanged += (sender, e) => this.Index = this.ComboBox.SelectedIndex;

                this.AlphaPicker.AlphaChange += (sender, value) => this._Color = this._Color;
                this.StrawPicker.ColorChange += (sender, value) => this.Color = this._Color = value;
            };
        }

        //Body    
        private void Picker_ColorChange(object sender, Color value) => this._Color = value;

    }
}

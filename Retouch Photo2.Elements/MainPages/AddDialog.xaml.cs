using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "AddDialog" />.
    /// </summary>
    public sealed partial class AddDialog : ContentDialog
    {
        /// <summary> <see cref = "AddDialog" /> 's BitmapSize.</summary>
        public BitmapSize Size => new BitmapSize()
        {
            Width = (uint)this.WidthNumberPicker.Value,
            Height = (uint)this.HeighNumberPicker.Value
        };

        public AddDialog()
        {
            this.InitializeComponent();

            this.WidthNumberPicker.Unit = "px";
            this.HeighNumberPicker.Unit = "px";
            this.WidthNumberPicker.Minimum = 16;
            this.HeighNumberPicker.Minimum = 16;
            this.WidthNumberPicker.Maximum = 16384;
            this.HeighNumberPicker.Maximum = 16384;
            this.WidthNumberPicker.Value = 1024;
            this.HeighNumberPicker.Value = 1024;
        }
    }
}

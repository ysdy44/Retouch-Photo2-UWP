using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Element
{
    public sealed partial class AddDialog : ContentDialog
    {
        #region Delegate

        /// <summary></summary>
        public delegate void AddSizeHandler(BitmapSize pixels);
        public event AddSizeHandler AddSize = null;

        #endregion

        public AddDialog()
        {
            this.InitializeComponent();

            this.SecondaryButtonClick += (sender, args) => this.Hide();
            this.PrimaryButtonClick += (sender, args) =>
            {
                this.Hide();

                BitmapSize pixels = new BitmapSize()
                {
                    Width = (uint)WidthNumberPicker.Value,
                    Height = (uint)HeighNumberPicker.Value
                };
                this.AddSize?.Invoke(pixels);//Delegate
            };
        }
    }
}

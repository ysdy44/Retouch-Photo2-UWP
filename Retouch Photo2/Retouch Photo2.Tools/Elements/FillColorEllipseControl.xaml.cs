using Retouch_Photo2.Brushs;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    /// <summary>
    /// Control of <see cref = "SelectionViewModel.FillColor" />.
    /// </summary>
    public sealed partial class FillColorEllipseControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        /// <summary> FillColorEllipseControl's _ColorPicker. </summary>
        public Flyout Flyout { get => this._Flyout; set => this.Flyout = value; }
        /// <summary> FillColorEllipseControl's ColorPicker. </summary>
        public HSVColorPickers.ColorPicker ColorPicker { get => this._ColorPicker; set => this._ColorPicker = value; }

        //@Construct
        public FillColorEllipseControl()
        {
            this.InitializeComponent();

            this.ColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.FillColor = value;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.StyleManager.FillBrush.Type = BrushType.Color;
                    layer.StyleManager.FillBrush.Color = value;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }
}
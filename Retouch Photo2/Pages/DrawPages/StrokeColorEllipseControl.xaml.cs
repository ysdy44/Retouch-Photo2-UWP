using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Pages.DrawPages
{
    /// <summary>
    /// Control of <see cref = "SelectionViewModel.StrokeColor" />.
    /// </summary>
    public sealed partial class StrokeColorEllipseControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        /// <summary> StrokeColorEllipseControl's _ColorPicker. </summary>
        public Flyout Flyout { get => this._Flyout; set => this.Flyout = value; }
        /// <summary> StrokeColorEllipseControl's ColorPicker. </summary>
        public HSVColorPickers.ColorPicker ColorPicker { get => this._ColorPicker; set => this._ColorPicker = value; }

        //@Construct
        public StrokeColorEllipseControl()
        {
            this.InitializeComponent();

            this.ColorPicker.ColorChange += (s, value) =>
            {
                //Selection
                this.SelectionViewModel.StrokeColor = value;
                this.SelectionViewModel.SetValue((layer) =>
                {
                    layer.StrokeColor = value;
                });

                this.ViewModel.Invalidate();//Invalidate
            };
        }
    }
}
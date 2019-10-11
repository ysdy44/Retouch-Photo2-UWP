using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Touchbars
{
    public sealed partial class GeometryStarPointsTouchbar : UserControl, ITouchbar
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        public TouchbarType Type => TouchbarType.GeometryStarPoints;
        public UIElement Self => this;

        //@Converter
        private int NumberConverter(float points) => (int)points;
        private double ValueConverter(float points) => points;

        //@Construct
        public GeometryStarPointsTouchbar()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "";
            this.TouchbarSlider.NumberMinimum = 3;
            this.TouchbarSlider.NumberMaximum = 36;
            this.TouchbarSlider.NumberChange += (sender, number) =>
            {
                int points = number;
                this.Change(points);
            };

            //Value
            this.TouchbarSlider.Minimum = 3d;
            this.TouchbarSlider.Maximum = 36d;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                int points = (int)value;
                this.Change(points);
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }

        private void Change(int points)
        {
            if (points < 3) points = 3;
            if (points > 36) points = 36;

            this.SelectionViewModel.GeometryStarPoints = points;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryStarLayer geometryStarLayer)
                {
                    geometryStarLayer.Points = points;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
    }
}
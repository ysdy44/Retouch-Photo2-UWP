using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Touchbars
{
    public sealed partial class GeometryRoundRectCornerTouchbar : UserControl, ITouchbar
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        public TouchbarType Type => TouchbarType.GeometryRoundRectCorner;
        public UIElement Self => this;

        //@Converter
        private int NumberConverter(float blurcorner) => (int)(blurcorner * 100d);
        private double ValueConverter(float blurcorner) => blurcorner * 100d;

        //@Construct
        public GeometryRoundRectCornerTouchbar()
        {
            this.InitializeComponent();

            //Number
            this.TouchbarSlider.Unit = "%";
            this.TouchbarSlider.NumberMinimum = 0;
            this.TouchbarSlider.NumberMaximum = 100;
            this.TouchbarSlider.NumberChange += (sender, number) =>
            {
                float corner = number / 100.0f;
                this.Change(corner);
            };

            //Value
            this.TouchbarSlider.Minimum = 0d;
            this.TouchbarSlider.Maximum = 100d;
            this.TouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.TouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float corner = (float)value / 100.0f;
                this.Change(corner);
            };
            this.TouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }

        private void Change(float corner)
        {
            if (corner < 0.0f) corner = 0.0f;
            if (corner > 1.0f) corner = 1.0f;

            this.SelectionViewModel.GeometryRoundRectCorner = corner;
            
            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryRoundRectLayer  roundRectLayer)
                {
                    roundRectLayer.Corner = corner;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
    }
}

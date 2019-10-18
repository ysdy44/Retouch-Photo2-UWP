using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryHeartTool"/>.
    /// </summary>
    public sealed partial class GeometryHeartPage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }
        bool _mode
        {
            set
            {
                if (value == false)
                {
                    this.SpreadTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.SpreadTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.SpreadTouchbarSlider;
                }
            }
        }

        //@Converter
        private int SpreadNumberConverter(float spread) => (int)(spread * 100.0f);
        private double SpreadValueConverter(float spread) => spread * 100d;

        //@Construct
        public GeometryHeartPage()
        {
            this.InitializeComponent();

            //Spread
            {
                //Button
                this.SpreadTouchbarButton.Unit = "%";
                this.SpreadTouchbarButton.Toggle += (s, value) =>
                {
                    this._mode = value;
                };

                //Number
                this.SpreadTouchbarSlider.Unit = "%";
                this.SpreadTouchbarSlider.NumberMinimum = 0;
                this.SpreadTouchbarSlider.NumberMaximum = 100;
                this.SpreadTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float spread = number / 100.0f;
                    this.SpreadChange(spread);
                };

                //Value
                this.SpreadTouchbarSlider.Minimum = 0d;
                this.SpreadTouchbarSlider.Maximum = 100d;
                this.SpreadTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.SpreadTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float spread = (float)value / 100.0f;
                    this.SpreadChange(spread);
                };
                this.SpreadTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }
        }

        private void SpreadChange(float spread)
        {
            if (spread < 0.0f) spread = 0.0f;
            if (spread > 1.0f) spread = 1.0f;

            this.SelectionViewModel.GeometryHeartSpread = spread;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryHeartLayer geometryHeartLayer)
                {
                    geometryHeartLayer.Spread = spread;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this._mode = false;
        }
    }
}
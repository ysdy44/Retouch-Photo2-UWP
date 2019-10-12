using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Tools;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Layers.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryDiamondTool"/>.
    /// </summary>
    public sealed partial class GeometryDiamondPage : Page, IToolPage
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
                if (value==false)
                {
                    this.MidTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.MidTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.MidTouchbarSlider;
                }
            }
        }

        //@Converter
        private int MidNumberConverter(float mid) => (int)(mid * 100.0f);
        private double MidValueConverter(float mid) => mid * 100d;

        //@Construct
        public GeometryDiamondPage()
        {
            this.InitializeComponent();

            //Mid
            {
                //Button
                this.MidTouchbarButton.Unit = "%";
                this.MidTouchbarButton.Toggle += (s, value) =>
                {
                    this._mode = value;
                };

                //Number
                this.MidTouchbarSlider.Unit = "%";
                this.MidTouchbarSlider.NumberMinimum = 0;
                this.MidTouchbarSlider.NumberMaximum = 100;
                this.MidTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float mid = number / 100.0f;
                    this.MidChange(mid);
                };

                //Value
                this.MidTouchbarSlider.Minimum = 0d;
                this.MidTouchbarSlider.Maximum = 100d;
                this.MidTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.MidTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float mid = (float)value / 100.0f;
                    this.MidChange(mid);
                };
                this.MidTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }

            this.MirrorButton.Tapped += (s, e) => this.MidMirror();
        }

        private void MidChange(float mid)
        {
            if (mid < 0.0f) mid = 0.0f;
            if (mid > 1.0f) mid = 1.0f;

            this.SelectionViewModel.GeometryDiamondMid = mid;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryDiamondLayer geometryDiamondLayer)
                {
                    geometryDiamondLayer.Mid = mid;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        private void MidMirror()
        {
            float selectionMid = 1.0f - this.SelectionViewModel.GeometryDiamondMid;
            this.SelectionViewModel.GeometryDiamondMid = selectionMid;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryDiamondLayer geometryDiamondLayer)
                {
                    float mid = 1.0f - geometryDiamondLayer.Mid;
                    geometryDiamondLayer.Mid = mid;
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
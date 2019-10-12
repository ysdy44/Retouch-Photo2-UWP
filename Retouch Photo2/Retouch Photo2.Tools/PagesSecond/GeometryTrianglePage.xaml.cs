using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryTriangleTool"/>.
    /// </summary>
    public sealed partial class GeometryTrianglePage : Page, IToolPage
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
                    this.CenterTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.CenterTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.CenterTouchbarSlider;
                }
            }
        }

        //@Converter
        private int CenterNumberConverter(float center) => (int)(center * 100.0f);
        private double CenterValueConverter(float center) => center * 100d;

        //@Construct
        public GeometryTrianglePage()
        {
            this.InitializeComponent();

            //Center
            {
                //Button
                this.CenterTouchbarButton.Unit = "%";
                this.CenterTouchbarButton.Toggle += (s, value) =>
                {
                    this._mode = value;
                };

                //Number
                this.CenterTouchbarSlider.Unit = "%";
                this.CenterTouchbarSlider.NumberMinimum = 0;
                this.CenterTouchbarSlider.NumberMaximum = 100;
                this.CenterTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float center = number / 100.0f;
                    this.CenterChange(center);
                };

                //Value
                this.CenterTouchbarSlider.Minimum = 0d;
                this.CenterTouchbarSlider.Maximum = 100d;
                this.CenterTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.CenterTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float center = (float)value / 100.0f;
                    this.CenterChange(center);
                };
                this.CenterTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }

            this.MirrorButton.Tapped += (s, e) => this.CenterMirror();
        }

        private void CenterChange(float center)
        {
            if (center < 0.0f) center = 0.0f;
            if (center > 1.0f) center = 1.0f;

            this.SelectionViewModel.GeometryTriangleCenter = center;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryTriangleLayer geometryTriangleLayer)
                {
                    geometryTriangleLayer.Center = center;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        private void CenterMirror()
        {
            float selectionCenter = 1.0f - this.SelectionViewModel.GeometryTriangleCenter;
            this.SelectionViewModel.GeometryTriangleCenter = selectionCenter;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryTriangleLayer geometryTriangleLayer)
                {
                    float center = 1.0f - geometryTriangleLayer.Center;
                    geometryTriangleLayer.Center = center;
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
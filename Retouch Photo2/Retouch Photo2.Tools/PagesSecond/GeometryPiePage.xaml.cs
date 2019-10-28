using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryPieTool"/>.
    /// </summary>
    public sealed partial class GeometryPiePage : Page, IToolPage
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
                switch (value)
                {
                    case false:
                        this.SweepAngleTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case true:
                        this.SweepAngleTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.SweepAngleTouchbarSlider;
                        break;
                }
            }
        }

        //@Converter
        private int SweepAngleNumberConverter(float sweepAngle) => (int)(sweepAngle / FanKit.Math.Pi * 180f);
        private double SweepAngleValueConverter(float sweepAngle) => sweepAngle / System.Math.PI * 180d;

        //@Construct
        public GeometryPiePage()
        {
            this.InitializeComponent();
            
            //SweepAngle
            {
                //Button
                this.SweepAngleTouchbarButton.Unit = "º";
                this.SweepAngleTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this._mode = true;
                    else
                        this._mode = false;
                };

                //Number
                this.SweepAngleTouchbarSlider.Unit = "º";
                this.SweepAngleTouchbarSlider.NumberMinimum = 0;
                this.SweepAngleTouchbarSlider.NumberMaximum = 360;
                this.SweepAngleTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float sweepAngle = number / 180f * FanKit.Math.Pi;
                    this.SweepAngleChange(sweepAngle);
                };

                //Value
                this.SweepAngleTouchbarSlider.Minimum = 0d;
                this.SweepAngleTouchbarSlider.Maximum = 360d;
                this.SweepAngleTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.SweepAngleTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float sweepAngle = (float)value / 180f * FanKit.Math.Pi;
                    this.SweepAngleChange(sweepAngle);
                };
                this.SweepAngleTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }
        }

        private void SweepAngleChange(float sweepAngle)
        {
            this.SelectionViewModel.GeometryPieSweepAngle = sweepAngle;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryPieLayer geometryPieLayer)
                {
                    geometryPieLayer.SweepAngle = sweepAngle;
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
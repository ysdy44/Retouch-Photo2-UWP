using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    internal enum GeometryStarMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Points. </summary>
        Points,

        /// <summary> Inner-radius. </summary>
        InnerRadius,
    }

    /// <summary>
    /// Page of <see cref = "GeometryStarTool"/>.
    /// </summary>
    public sealed partial class GeometryStarPage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }
        GeometryStarMode _mode
        {
            set
            {
                switch (value)
                {
                    case GeometryStarMode.None:
                        this.PointsTouchbarButton.IsSelected = false;
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case GeometryStarMode.Points:
                        this.PointsTouchbarButton.IsSelected = true;
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = this.PointsTouchbarSlider;
                        break;
                    case GeometryStarMode.InnerRadius:
                        this.PointsTouchbarButton.IsSelected = false;
                        this.InnerRadiusTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.InnerRadiusTouchbarSlider;
                        break;
                }
            }
        }

        //@Converter
        private double PointsValueConverter(float points) => points;

        private int InnerRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private double InnerRadiusValueConverter(float innerRadius) => innerRadius * 100d;

        //@Construct
        public GeometryStarPage()
        {
            this.InitializeComponent();

            //Points
            {
                //Button
                this.PointsTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this._mode = GeometryStarMode.Points;
                    else
                        this._mode = GeometryStarMode.None;
                };

                //Number
                this.PointsTouchbarSlider.NumberMinimum = 3;
                this.PointsTouchbarSlider.NumberMaximum = 36;
                this.PointsTouchbarSlider.NumberChange += (sender, number) =>
                {
                    int points = number;
                    this.PointsChange(points);
                };

                //Value
                this.PointsTouchbarSlider.Minimum = 3d;
                this.PointsTouchbarSlider.Maximum = 36d;
                this.PointsTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.PointsTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    int points = (int)value;
                    this.PointsChange(points);
                };
                this.PointsTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }

            //InnerRadius
            {
                //Button
                this.InnerRadiusTouchbarButton.Unit = "%";
                this.InnerRadiusTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this._mode = GeometryStarMode.InnerRadius;
                    else
                        this._mode = GeometryStarMode.None;
                };

                //Number
                this.InnerRadiusTouchbarSlider.Unit = "%";
                this.InnerRadiusTouchbarSlider.NumberMinimum = 0;
                this.InnerRadiusTouchbarSlider.NumberMaximum = 100;
                this.InnerRadiusTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float innerRadius = number / 100f;
                    this.InnerRadiusChange(innerRadius);
                };

                //Value
                this.InnerRadiusTouchbarSlider.Minimum = 0d;
                this.InnerRadiusTouchbarSlider.Maximum = 100d;
                this.InnerRadiusTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.InnerRadiusTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float innerRadius = (float)(value / 100d);
                    this.InnerRadiusChange(innerRadius);
                };
                this.InnerRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }
        }


        private void PointsChange(int points)
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
        private void InnerRadiusChange(float innerRadius)
        {
            this.SelectionViewModel.GeometryStarInnerRadius = innerRadius;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryStarLayer geometryStarLayer)
                {
                    geometryStarLayer.InnerRadius = innerRadius;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this._mode = GeometryStarMode.None;
        }
    }
}
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryPentagonTool"/>.
    /// </summary>
    public sealed partial class GeometryPentagonPage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }

        //@TouchBar  
        internal bool TouchBarMode
        {
            set
            {
                if (value == false)
                {
                    this.PointsTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.PointsTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.PointsTouchbarSlider;
                }
            }
        }

        //@Converter
        private double PointsValueConverter(float points) => points;

        //@Construct
        public GeometryPentagonPage()
        {
            this.InitializeComponent();

            //Points
            {
                //Button
                this.PointsTouchbarButton.Toggle += (s, value) =>
                {
                    this.TouchBarMode = value;
                };

                //Number
                this.PointsTouchbarSlider.Unit = "";
                this.PointsTouchbarSlider.NumberMinimum = 0;
                this.PointsTouchbarSlider.NumberMaximum = 100;
                this.PointsTouchbarSlider.NumberChange += (sender, number) =>
                {
                    int Points = number;
                    this.PointsChange(Points);
                };

                //Value
                this.PointsTouchbarSlider.Minimum = 0d;
                this.PointsTouchbarSlider.Maximum = 100d;
                this.PointsTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.PointsTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    int Points = (int)value;
                    this.PointsChange(Points);
                };
                this.PointsTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }
        }

        private void PointsChange(int points)
        {
            if (points < 3) points = 3;
            if (points > 36) points = 36;

            this.SelectionViewModel.GeometryPentagonPoints = points;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryPentagonLayer geometryPentagonLayer)
                {
                    geometryPentagonLayer.Points = points;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this.TouchBarMode = false;
        }
    }
}
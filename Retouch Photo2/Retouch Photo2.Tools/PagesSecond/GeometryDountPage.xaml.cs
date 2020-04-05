using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryDountTool"/>.
    /// </summary>
    public sealed partial class GeometryDountPage : Page, IToolPage
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
                switch (value)
                {
                    case false:
                        this.HoleRadiusTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case true:
                        this.HoleRadiusTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.HoleRadiusTouchbarSlider;
                        break;
                }
            }
        }

        //@Converter
        private int HoleRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private double HoleRadiusValueConverter(float innerRadius) => innerRadius * 100d;
        
        //@Construct
        public GeometryDountPage()
        {
            this.InitializeComponent();

            //HoleRadius
            {
                //Button
                this.HoleRadiusTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this.TouchBarMode = true;
                    else
                        this.TouchBarMode = false;
                };

                //Number
                this.HoleRadiusTouchbarSlider.Unit = "%";
                this.HoleRadiusTouchbarSlider.NumberMinimum = 0;
                this.HoleRadiusTouchbarSlider.NumberMaximum = 100;
                this.HoleRadiusTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float innerRadius = number / 100f;
                    this.HoleRadiusChange(innerRadius);
                };

                //Value
                this.HoleRadiusTouchbarSlider.Minimum = 0d;
                this.HoleRadiusTouchbarSlider.Maximum = 100d;
                this.HoleRadiusTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.HoleRadiusTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float innerRadius = (float)(value / 100d);
                    this.HoleRadiusChange(innerRadius);
                };
                this.HoleRadiusTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }
        }

        private void HoleRadiusChange(float innerRadius)
        {
            this.SelectionViewModel.GeometryDountHoleRadius = innerRadius;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryDountLayer geometryDountLayer)
                {
                    geometryDountLayer.HoleRadius = innerRadius;
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
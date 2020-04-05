using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    /// <summary>
    /// Page of <see cref = "GeometryRoundRectTool"/>.
    /// </summary>
    public sealed partial class GeometryRoundRectPage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;

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
                    this.CornerTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.CornerTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.CornerTouchbarSlider;
                }
            }
        }

        //@Converter
        private int CornerNumberConverter(float corner) => (int)(corner * 100.0f);
        private double CornerValueConverter(float corner) => corner * 100d;

        //@Construct
        public GeometryRoundRectPage()
        {
            this.InitializeComponent();

            //Corner
            {
                this.CornerTouchbarButton.Toggle += (s, value) =>
                {
                    this.TouchBarMode = value;
                };

                //Number
                this.CornerTouchbarSlider.Unit = "%";
                this.CornerTouchbarSlider.NumberMinimum = 0;
                this.CornerTouchbarSlider.NumberMaximum = 50;
                this.CornerTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float corner = number / 100.0f;
                    this.CornerChange(corner);
                };

                //Value
                this.CornerTouchbarSlider.Minimum = 0d;
                this.CornerTouchbarSlider.Maximum = 50d;
                this.CornerTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.CornerTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float corner = (float)value / 100.0f;
                    this.CornerChange(corner);
                };
                this.CornerTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }
        }
        
        private void CornerChange(float corner)
        {
            if (corner < 0.0f) corner = 0.0f;
            if (corner > 0.5f) corner = 0.5f;

            this.SelectionViewModel.GeometryRoundRectCorner = corner;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryRoundRectLayer roundRectLayer)
                {
                    roundRectLayer.Corner = corner;
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
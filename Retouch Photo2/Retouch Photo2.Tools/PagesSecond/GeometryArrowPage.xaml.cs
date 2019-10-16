using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    internal enum GeometryArrowMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Width (IsAbsolute = false). </summary>
        Width,

        /// <summary> Value (IsAbsolute = false). </summary>
        Value
    }

    /// <summary>
    /// Page of <see cref = "GeometryArrowTool"/>.
    /// </summary>
    public sealed partial class GeometryArrowPage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }
        GeometryArrowMode _mode
        {
            set
            {
                switch (value)
                {
                    case GeometryArrowMode.None:
                  //      this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.ValueTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case GeometryArrowMode.Width:
                      //  this.InnerRadiusTouchbarButton.IsSelected = true;
                        this.ValueTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null; //this.InnerRadiusTouchbarSlider;
                        break;
                    case GeometryArrowMode.Value:
                      //  this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.ValueTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.ValueTouchbarSlider;
                        break;
                }
            }
        }

        //@Converter
        private int ValueNumberConverter(float value) => (int)(value * 100.0f);
        private double ValueValueConverter(float value) => value * 100d;

        private int TailToIndexConverter(GeometryArrowTailType  tailType) => (int)tailType;
        
        //@Construct
        public GeometryArrowPage()
        {
            this.InitializeComponent();
            this.LeftTailComboBox.SelectionChanged += (s, e) =>
            {
                GeometryArrowTailType tailType = (GeometryArrowTailType)this.LeftTailComboBox.SelectedIndex;
                this.TailTypeChange(tailType, isLeft: true);
            };
            this.RightTailComboBox.SelectionChanged += (s, e) =>
            {
                GeometryArrowTailType tailType = (GeometryArrowTailType)this.RightTailComboBox.SelectedIndex;
                this.TailTypeChange(tailType, isLeft: false);
            };
            
            //Value
            {
                //Button
                this.ValueTouchbarButton.Unit = "%";
                this.ValueTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this._mode = GeometryArrowMode.Value;
                    else
                        this._mode = GeometryArrowMode.None;
                };

                //Number
                this.ValueTouchbarSlider.Unit = "%";
                this.ValueTouchbarSlider.NumberMinimum = 0;
                this.ValueTouchbarSlider.NumberMaximum = 100;
                this.ValueTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float value2 = number / 100f;
                    this.ValueChange(value2);
                };

                //Value
                this.ValueTouchbarSlider.Minimum = 0d;
                this.ValueTouchbarSlider.Maximum = 100d;
                this.ValueTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.ValueTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float value2 = (float)(value / 100d);
                    this.ValueChange(value2);
                };
                this.ValueTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }

        }

        private void ValueChange(float value)
        {
            this.SelectionViewModel.GeometryArrowValue = value;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryArrowLayer geometryArrowLayer)
                {
                    geometryArrowLayer.Value = value;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        private void TailTypeChange(GeometryArrowTailType tailType,bool isLeft)
        {
            if (isLeft)
                this.SelectionViewModel.GeometryArrowLeftTail = tailType;
            else
                this.SelectionViewModel.GeometryArrowRightTail = tailType;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryArrowLayer geometryArrowLayer)
                {
                    if (isLeft)
                        geometryArrowLayer.LeftTail = tailType;
                    else
                        geometryArrowLayer.RightTail = tailType;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this._mode = GeometryArrowMode.None;
        }
    }
}
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Elements
{
    public sealed partial class StrokeWidthButton : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        public void ModeNone() => this._mode = false;
        bool _mode
        {
            set
            {
                if (value == false)
                {
                    this.StrokeWidthTouchbarButton.IsSelected = false;
                    this.TipViewModel.TouchbarControl = null;
                }
                else
                {
                    this.StrokeWidthTouchbarButton.IsSelected = true;
                    this.TipViewModel.TouchbarControl = this.StrokeWidthTouchbarSlider;
                }
            }
        }

        //@Converter
        private int StrokeWidthNumberConverter(float strokeWidth) => (int)(strokeWidth * 100.0f);
        private double StrokeWidthValueConverter(float strokeWidth) => strokeWidth;

        //@Construct
        public StrokeWidthButton()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructStrokeWidth();
        }

        private void ConstructStrokeWidth()
        {
            //Button
            this.StrokeWidthTouchbarButton.Toggle += (s, value) =>
            {
                this._mode = value;
            };

            //Number
            this.StrokeWidthTouchbarSlider.Unit = "%";
            this.StrokeWidthTouchbarSlider.NumberMinimum = 0;
            this.StrokeWidthTouchbarSlider.NumberMaximum = 10000;
            this.StrokeWidthTouchbarSlider.NumberChange += (sender, value) =>
            {
                float strokeWidth = value / 100f;
                this.StrokeWidthChange(strokeWidth);
            };

            //Value
            this.StrokeWidthTouchbarSlider.Minimum = 0d;
            this.StrokeWidthTouchbarSlider.Maximum = 10d;
            this.StrokeWidthTouchbarSlider.ValueChangeStarted += (sender, value) => { };
            this.StrokeWidthTouchbarSlider.ValueChangeDelta += (sender, value) =>
            {
                float strokeWidth = (float)value;
                this.StrokeWidthChange(strokeWidth);
            };
            this.StrokeWidthTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
        }
        private void StrokeWidthChange(float strokeWidth)
        {
            //Selection
            this.SelectionViewModel.StrokeWidth = strokeWidth;
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.StyleManager.StrokeWidth = strokeWidth;
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.StrokeWidthTouchbarButton.CenterContent = resource.GetString("/ToolElements/StrokeWidth");
        }

    }
}
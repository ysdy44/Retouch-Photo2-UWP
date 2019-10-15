using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Pages
{
    internal enum GeometryCogMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Count. </summary>
        Count,

        /// <summary> Inner-radius. </summary>
        InnerRadius,

        /// <summary> Tooth. </summary>
        Tooth,

        /// <summary> Notch. </summary>
        Notch
    }

    /// <summary>
    /// Page of <see cref = "GeometryCogTool"/>.
    /// </summary>
    public sealed partial class GeometryCogPage : Page, IToolPage
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        public FrameworkElement Self => this;
        public bool IsSelected { private get; set; }
        GeometryCogMode _mode
        {
            set
            {
                switch (value)
                {
                    case GeometryCogMode.None:
                        this.CountTouchbarButton.IsSelected = false;
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.ToothTouchbarButton.IsSelected = false;
                        this.NotchTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = null;
                        break;
                    case GeometryCogMode.Count:
                        this.CountTouchbarButton.IsSelected = true;
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.ToothTouchbarButton.IsSelected = false;
                        this.NotchTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = this.CountTouchbarSlider;
                        break;
                    case GeometryCogMode.InnerRadius:
                        this.CountTouchbarButton.IsSelected = false;
                        this.InnerRadiusTouchbarButton.IsSelected = true;
                        this.ToothTouchbarButton.IsSelected = false;
                        this.NotchTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = this.InnerRadiusTouchbarSlider;
                        break;
                    case GeometryCogMode.Tooth:
                        this.CountTouchbarButton.IsSelected = false;
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.ToothTouchbarButton.IsSelected = true;
                        this.NotchTouchbarButton.IsSelected = false;
                        this.TipViewModel.TouchbarControl = this.ToothTouchbarSlider;
                        break;
                    case GeometryCogMode.Notch:
                        this.CountTouchbarButton.IsSelected = false;
                        this.InnerRadiusTouchbarButton.IsSelected = false;
                        this.ToothTouchbarButton.IsSelected = false;
                        this.NotchTouchbarButton.IsSelected = true;
                        this.TipViewModel.TouchbarControl = this.NotchTouchbarSlider;
                        break;
                }
            }
        }

        //@Converter    
        private double CountValueConverter(float count) => count;

        private int InnerRadiusNumberConverter(float innerRadius) => (int)(innerRadius * 100.0f);
        private double InnerRadiusValueConverter(float innerRadius) => innerRadius * 100d;

        private int ToothNumberConverter(float tooth) => (int)(tooth * 100.0f);
        private double ToothValueConverter(float tooth) => tooth * 100d;

        private int NotchNumberConverter(float notch) => (int)(notch * 100.0f);
        private double NotchValueConverter(float notch) => notch * 100d;
        
        //@Construct
        public GeometryCogPage()
        {
            this.InitializeComponent();

            //Count
            {
                //Button
                this.CountTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this._mode = GeometryCogMode.Count;
                    else
                        this._mode = GeometryCogMode.None;
                };

                //Number
                this.CountTouchbarSlider.NumberMinimum = 4;
                this.CountTouchbarSlider.NumberMaximum = 36;
                this.CountTouchbarSlider.NumberChange += (sender, number) =>
                {
                    int Count = number;
                    this.CountChange(Count);
                };

                //Value
                this.CountTouchbarSlider.Minimum = 4d;
                this.CountTouchbarSlider.Maximum = 36d;
                this.CountTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.CountTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    int Count = (int)value;
                    this.CountChange(Count);
                };
                this.CountTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }
            
            //InnerRadius
            {
                //Button
                this.InnerRadiusTouchbarButton.Unit = "%";
                this.InnerRadiusTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this._mode = GeometryCogMode.InnerRadius;
                    else
                        this._mode = GeometryCogMode.None;
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

            //Tooth
            {
                //Button
                this.ToothTouchbarButton.Unit = "%";
                this.ToothTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this._mode = GeometryCogMode.Tooth;
                    else
                        this._mode = GeometryCogMode.None;
                };

                //Number
                this.ToothTouchbarSlider.Unit = "%";
                this.ToothTouchbarSlider.NumberMinimum = 0;
                this.ToothTouchbarSlider.NumberMaximum = 50;
                this.ToothTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float Tooth = number / 100f;
                    this.ToothChange(Tooth);
                };

                //Value
                this.ToothTouchbarSlider.Minimum = 0d;
                this.ToothTouchbarSlider.Maximum = 50d;
                this.ToothTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.ToothTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float Tooth = (float)(value / 100d);
                    this.ToothChange(Tooth);
                };
                this.ToothTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }

            //Notch
            {
                //Button
                this.NotchTouchbarButton.Unit = "%";
                this.NotchTouchbarButton.Toggle += (s, value) =>
                {
                    if (value)
                        this._mode = GeometryCogMode.Notch;
                    else
                        this._mode = GeometryCogMode.None;
                };

                //Number
                this.NotchTouchbarSlider.Unit = "%";
                this.NotchTouchbarSlider.NumberMinimum = 0;
                this.NotchTouchbarSlider.NumberMaximum = 60;
                this.NotchTouchbarSlider.NumberChange += (sender, number) =>
                {
                    float Notch = number / 100f;
                    this.NotchChange(Notch);
                };

                //Value
                this.NotchTouchbarSlider.Minimum = 0d;
                this.NotchTouchbarSlider.Maximum = 50d;
                this.NotchTouchbarSlider.ValueChangeStarted += (sender, value) => { };
                this.NotchTouchbarSlider.ValueChangeDelta += (sender, value) =>
                {
                    float Notch = (float)(value / 100d);
                    this.NotchChange(Notch);
                };
                this.NotchTouchbarSlider.ValueChangeCompleted += (sender, value) => { };
            }
        }
        
        private void CountChange(int count)
        {
            if (count < 4) count = 4;
            if (count > 36) count = 36;

            this.SelectionViewModel.GeometryCogCount = count;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryCogLayer geometryCogLayer)
                {
                    geometryCogLayer.Count = count;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        private void InnerRadiusChange(float innerRadius)
        {
            this.SelectionViewModel.GeometryCogInnerRadius = innerRadius;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryCogLayer geometryCogLayer)
                {
                    geometryCogLayer.InnerRadius = innerRadius;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        private void ToothChange(float tooth)
        {
            this.SelectionViewModel.GeometryCogTooth = tooth;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryCogLayer geometryCogLayer)
                {
                    geometryCogLayer.Tooth = tooth;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }
        private void NotchChange(float notch)
        {
            this.SelectionViewModel.GeometryCogNotch = notch;

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                if (layer is GeometryCogLayer geometryCogLayer)
                {
                    geometryCogLayer.Notch = notch;
                }
            });

            this.ViewModel.Invalidate();//Invalidate
        }

        public void OnNavigatedTo() { }
        public void OnNavigatedFrom()
        {
            this._mode = GeometryCogMode.None;
        }
    }
}
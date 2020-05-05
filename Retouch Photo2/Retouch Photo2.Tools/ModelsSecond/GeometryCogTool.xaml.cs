using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Tools.Icons;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools.Models
{
    /// <summary>
    /// Enum of <see cref="GeometryCogTool">.
    /// </summary>
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
    /// <see cref="ITool"/>'s GeometryCogTool.
    /// </summary>
    public sealed partial class GeometryCogTool : Page, ITool
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@TouchBar
        internal GeometryCogMode TouchBarMode
        {
            set
            {
                this.CountTouchbarButton.IsSelected = (value == GeometryCogMode.Count);
                this.InnerRadiusTouchbarButton.IsSelected = (value == GeometryCogMode.InnerRadius);
                this.ToothTouchbarButton.IsSelected = (value == GeometryCogMode.Tooth);
                this.NotchTouchbarButton.IsSelected = (value == GeometryCogMode.Notch);

                switch (value)
                {
                    case GeometryCogMode.None: this.TipViewModel.TouchbarControl = null; break;
                    case GeometryCogMode.Count: this.TipViewModel.TouchbarControl = this.CountTouchbarSlider; break;
                    case GeometryCogMode.InnerRadius: this.TipViewModel.TouchbarControl = this.InnerRadiusTouchbarSlider; break;
                    case GeometryCogMode.Tooth: this.TipViewModel.TouchbarControl = this.ToothTouchbarSlider; break;
                    case GeometryCogMode.Notch: this.TipViewModel.TouchbarControl = this.NotchTouchbarSlider; break;
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
        public GeometryCogTool()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructCount();
            this.ConstructInnerRadius();
            this.ConstructTooth();
            this.ConstructNotch();
        }


        //Count
        private void ConstructCount()
        {
            //Button
            this.CountTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCogMode.Count;
                else
                    this.TouchBarMode = GeometryCogMode.None;
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
        
        //InnerRadius
        private void ConstructInnerRadius()
        {
            //Button
            this.InnerRadiusTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCogMode.InnerRadius;
                else
                    this.TouchBarMode = GeometryCogMode.None;
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
        
        //Tooth
        private void ConstructTooth()
        {
            //Button
            this.ToothTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCogMode.Tooth;
                else
                    this.TouchBarMode = GeometryCogMode.None;
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
        
        //Notch
        private void ConstructNotch()
        {
            //Button
            this.NotchTouchbarButton.Toggle += (s, value) =>
            {
                if (value)
                    this.TouchBarMode = GeometryCogMode.Notch;
                else
                    this.TouchBarMode = GeometryCogMode.None;
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
            this.TouchBarMode = GeometryCogMode.None;
        }

    }
    
    /// <summary>
    /// <see cref="ITool"/>'s GeometryCogTool.
    /// </summary>
    public partial class GeometryCogTool : Page, ITool
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.Content = resource.GetString("/ToolsSecond/GeometryCog");
            this._button.Style = this.IconSelectedButtonStyle;

            this.CountTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_Count");
            this.InnerRadiusTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_InnerRadius");
            this.ToothTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_Tooth");
            this.NotchTouchbarButton.CenterContent = resource.GetString("/ToolsSecond/GeometryCog_Notch");

            this.ConvertTextBlock.Text = resource.GetString("/ToolElements/Convert");
        }


        //@Content
        public ToolType Type => ToolType.GeometryCog;
        public FrameworkElement Icon => this._icon;
        public bool IsSelected { get => !this._button.IsEnabled; set => this._button.IsEnabled = !value; }

        public FrameworkElement Button => this._button;
        public FrameworkElement Page => this;

        readonly FrameworkElement _icon = new GeometryCogIcon();
        readonly Button _button = new Button { Tag = new GeometryCogIcon()};

        private ILayer CreateLayer(Transformer transformer)
        {
            return new GeometryCogLayer
            {
                Count = this.SelectionViewModel.GeometryCogCount,
                InnerRadius = this.SelectionViewModel.GeometryCogInnerRadius,
                Tooth = this.SelectionViewModel.GeometryCogTooth,
                Notch = this.SelectionViewModel.GeometryCogNotch,
                TransformManager = new TransformManager(transformer),
                StyleManager = this.SelectionViewModel.GetStyleManager()
            };
        }


        public void Starting(Vector2 point) => this.TipViewModel.CreateTool.Starting(point);
        public void Started(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Started(this.CreateLayer, startingPoint, point);
        public void Delta(Vector2 startingPoint, Vector2 point) => this.TipViewModel.CreateTool.Delta(startingPoint, point);
        public void Complete(Vector2 startingPoint, Vector2 point, bool isSingleStarted) => this.TipViewModel.CreateTool.Complete(startingPoint, point, isSingleStarted);

        public void Draw(CanvasDrawingSession drawingSession) => this.TipViewModel.CreateTool.Draw(drawingSession);

    }
}
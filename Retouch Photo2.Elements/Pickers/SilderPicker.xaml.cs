using HSVColorPickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Manipulation of the slider.
    /// </summary>
    public sealed partial class SilderPicker : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the value change starts. </summary>
        public event TouchValueChangeHandler ValueChangeStarted;
        /// <summary> Occurs when value change. </summary>
        public event TouchValueChangeHandler ValueChangeDelta;
        /// <summary> Occurs when the value change is complete. </summary>
        public event TouchValueChangeHandler ValueChangeCompleted;

        public float Minimum { get; set; } = 0;
        public float Maximum { get; set; } = 100;


        #region DependencyProperty


        double position;
        double Position
        {
            get => this.position;
            set
            {
                float V = (float)((value - 10) / (this.ActualWidth - 20));
                this._V = V;

                this.Value = V * (this.Maximum - this.Minimum) + this.Minimum;
                this.position = value;
            }
        }
        private float _V
        {
            set
            {
                double right = this.ActualWidth - 10;

                if (value < 0.0)
                {
                    Canvas.SetLeft(this.Thumb2, 10 - 10);
                    Canvas.SetLeft(this.Thumb1, 10 - 9);
                    this.Rectangle1.Width = 0;
                }
                else if (value > 1.0)
                {
                    Canvas.SetLeft(this.Thumb2, right - 10);
                    Canvas.SetLeft(this.Thumb1, right - 9);
                    this.Rectangle1.Width = right;
                }
                else
                {
                    double position = value * (this.ActualWidth - 20) + 10;
                    if (position < 0) position = 0; 
                    Canvas.SetLeft(this.Thumb2, position - 10);
                    Canvas.SetLeft(this.Thumb1, position - 9);
                    this.Rectangle1.Width = position;
                }
            }
        }


        private float value;
        public float Value
        {
            get => this.value;
            set
            {
                if (value < this.Minimum)
                {
                    this._V = 0;
                    this.value = this.Minimum;
                }
                else if (value > this.Maximum)
                {
                    this._V = 1;
                    this.value = this.Maximum;
                }
                else
                {
                    float v = (value - this.Minimum) / (this.Maximum - this.Minimum);

                    if (v < 0) this._V = 0;
                    else if (v > 1) this._V = 1;
                    else this._V = v;

                    this.value = value;
                }
            }
        }


        #endregion


        //@VisualState
        bool _vsIsEnabled = true;
        ClickMode _vsClickMode;
        bool _vsIsTouch;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsEnabled == false) return this.Disabled;

                if (this._vsIsTouch == false)
                {
                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Normal;
                        case ClickMode.Hover: return this.PointerOver;
                        case ClickMode.Press: return this.Pressed;
                    }
                }
                else return this.Pressed;

                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        /// <summary> VisualState's ClickMode. </summary>
        private ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@Construct
        public SilderPicker()
        {
            this.InitializeComponent();
            this.IsEnabledChanged += (s, e) =>
            {
                this._vsIsEnabled = this.IsEnabled;
                this.VisualState = this.VisualState;//State
            };

            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;


            this.Loaded += (s, e) =>
            {
                this.VisualState = this.VisualState;//State

                if (this.Value < this.Minimum)
                {
                    this.Value = this.Minimum;
                }
                else if (this.Value > this.Maximum)
                {
                    this.Value = this.Maximum;
                }
                else
                {
                    this.Value = this.Value;
                }
            };

            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                //width
                double width = e.NewSize.Width;
                this.Rectangle2.Width = width - 20;
                Canvas.SetLeft(this.Rectangle2, 10);
                Canvas.SetLeft(this.Rectangle1, 10);


                //height
                double height = e.NewSize.Height;
                double heightHalf = height / 2;

                Canvas.SetTop(this.Rectangle2, heightHalf - 2);
                Canvas.SetTop(this.Rectangle1, heightHalf - 2);

                Canvas.SetTop(this.Thumb2, heightHalf - 10);
                Canvas.SetTop(this.Thumb1, heightHalf - 9);
            };

            //Manipulation
            this.RootGrid.ManipulationMode = ManipulationModes.All;
            this.RootGrid.ManipulationStarted += (sender, e) =>
            {
                this._vsIsTouch = true;
                this.VisualState = this.VisualState;//State

                this.Position = e.Position.X;

                this.ValueChangeStarted?.Invoke(this, this.Value);//Delegate
            };
            this.RootGrid.ManipulationDelta += (sender, e) =>
            {
                this.Position += e.Delta.Translation.X;

                this.ValueChangeDelta?.Invoke(this, this.Value);//Delegate
            };
            this.RootGrid.ManipulationCompleted += (sender, e) =>
            {
                this._vsIsTouch = false;
                this.VisualState = this.VisualState;//State

                this.ValueChangeCompleted?.Invoke(this, this.Value);//Delegate
            };
        }

    }
}
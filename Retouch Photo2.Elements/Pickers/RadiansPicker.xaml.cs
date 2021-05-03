// Core:              
// Referenced:   ★★★
// Difficult:         ★
// Only:              ★
// Complete:      ★★
using HSVColorPickers;
using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Manipulation of the rotation angle.
    /// </summary>
    public sealed partial class RadiansPicker : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the value changed starts. </summary>
        public event TouchValueChangedHandler ValueChangedStarted;
        /// <summary> Occurs when value changed. </summary>
        public event TouchValueChangedHandler ValueChangedDelta;
        /// <summary> Occurs when the value changed is complete. </summary>
        public event TouchValueChangedHandler ValueChangedCompleted;


        /// <summary> 5 degree </summary>
        readonly float FiveDegrees = 0.08726646259971647884618453842443f;
        /// <summary> Half of 5 degree </summary>
        readonly float FiveHalfDegrees = 0.04363323129985823942309226921222f;
        /// <summary> Integer to 5 degree </summary>
        public float FiveInteger(float value) => value - (value + this.FiveHalfDegrees) % this.FiveDegrees;
        /// <summary> (x , y) to α⁰ </summary>
        public static float VectorToRadians(Vector2 vector) => (float)Math.Atan2(vector.Y, vector.X);
        /// <summary> α⁰ to (x , y) </summary>
        public static Vector2 RadiansToVector(float radians, float radius, Vector2 center) => new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians)) * radius + center;


        #region DependencyProperty


        private float radians;
        public float Radians
        {
            get => this.radians;
            set
            {
                this.Arrow = RadiansPicker.RadiansToVector(value, this.Radius, this.Center);

                this.radians = value;
            }
        }
        private void OnRadiansChanged(float value)
        {
            this.Arrow = RadiansPicker.RadiansToVector(value, this.Radius, this.Center);

            if (Math.Abs(value - this.radians) > this.FiveDegrees)
            {
                float integer = this.FiveInteger(value);

                this.radians = integer;
            }
        }

        private Vector2 Arrow
        {
            set
            {
                //this.WhiteLine.X2 = this.BlackLine.X2 = value.X;
                //this.WhiteLine.Y2 = this.BlackLine.Y2 = value.Y;

                Canvas.SetLeft(this.Thumb, value.X - 10);
                Canvas.SetTop(this.Thumb, value.Y - 10);
            }
        }

        private Vector2 center;
        private Vector2 Center
        {
            get => this.center;
            set
            {
                //this.WhiteLine.X1 = this.BlackLine.X1 = value.X;
                //this.WhiteLine.Y1 = this.BlackLine.Y1 = value.Y;

                Canvas.SetLeft(this.Ellipse, value.X - this.Ellipse.ActualWidth / 2);
                Canvas.SetTop(this.Ellipse, value.Y - this.Ellipse.ActualHeight / 2);

                this.center = value;
            }
        }

        private float radius;
        private float Radius
        {
            get => this.radius;
            set
            {
                this.Ellipse.Width = this.Ellipse.Height = value * 2;

                this.radius = value;
            }
        }

        //Manipulation
        Vector2 Vector;
        bool InRadians = false;


        #endregion


        //@VisualState
        bool _vsIsEnabled = true;
        ClickMode _vsClickMode;
        bool _vsIsManipulationStarted = true;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsEnabled == false) return this.Disabled;

                if (this._vsIsManipulationStarted) return this.Pressed;

                switch (this._vsClickMode)
                {
                    case ClickMode.Release: return this.Normal;
                    case ClickMode.Hover: return this.PointerOver;
                    case ClickMode.Press: return this.Pressed;
                }

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
        /// <summary>
        /// Initializes a RadiansPicker. 
        /// </summary>
        public RadiansPicker()
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

                this.Arrow = RadiansPicker.RadiansToVector(this.Radians, this.Radius, this.Center);
            };

            this.SizeChanged += (s, e) =>
            {
                this.Radius = (float)Math.Min(e.NewSize.Width, e.NewSize.Height) / 2 - 10;
                this.Center = new Vector2((float)(e.NewSize.Width / 2), (float)(e.NewSize.Height / 2));
                this.Arrow = RadiansPicker.RadiansToVector(this.Radians, this.Radius, this.Center);
            };

            //Manipulation
            this.RootGrid.ManipulationMode = ManipulationModes.All;
            this.RootGrid.ManipulationStarted += (sender, e) =>
            {
                this.Vector = e.Position.ToVector2() - this.Center;

                this.InRadians = this.Vector.Length() < this.Radius + 10;

                if (!this.InRadians) return;
                this.OnRadiansChanged(RadiansPicker.VectorToRadians(this.Vector));

                this.ValueChangedStarted?.Invoke(this, this.Radians);//Delegate

                this._vsIsManipulationStarted = true;
                this.VisualState = this.VisualState;//VisualState
            };
            this.RootGrid.ManipulationDelta += (sender, e) =>
            {
                this.Vector += e.Delta.Translation.ToVector2();

                if (!this.InRadians) return;
                this.OnRadiansChanged(RadiansPicker.VectorToRadians(this.Vector));

                this.ValueChangedDelta?.Invoke(this, this.Radians);//Delegate
            };
            this.RootGrid.ManipulationCompleted += (sender, e) =>
            {
                this.ValueChangedCompleted?.Invoke(this, this.Radians);//Delegate

                this._vsIsManipulationStarted = false;
                this.VisualState = this.VisualState;//VisualState
            };
        }
    }
}
using System;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    public sealed partial class RadiansPicker : UserControl
    {
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


        //Delegate   
        public delegate void RadiansChangeHandler(float radians);
        public event RadiansChangeHandler RadiansChange;

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
        private float _Radians
        {
            get => this.radians;
            set
            {
                this.Arrow = RadiansPicker.RadiansToVector(value, this.Radius, this.Center);

                if (Math.Abs(value - this.radians) > this.FiveDegrees)
                {
                    float integer = this.FiveInteger(value);

                    this.RadiansChange?.Invoke(integer);

                    this.radians = integer;
                }
            }
        }

        private Vector2 arrow;
        private Vector2 Arrow
        {
            get => this.arrow;
            set
            {
                this.WhiteLine.X2 = this.BlackLine.X2 = value.X;
                this.WhiteLine.Y2 = this.BlackLine.Y2 = value.Y;

                this.arrow = value;
            }
        }

        private Vector2 center;
        private Vector2 Center
        {
            get => this.center;
            set
            {
                this.WhiteLine.X1 = this.BlackLine.X1 = value.X;
                this.WhiteLine.Y1 = this.BlackLine.Y1 = value.Y;

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
        bool IsRadians = false;

        public RadiansPicker()
        {
            this.InitializeComponent();

            this.Loaded += (s, e) => this.Arrow = RadiansPicker.RadiansToVector(this.Radians, this.Radius, this.Center);
            this.SizeChanged += (s, e) =>
            {
                this.Radius = (float)Math.Min(e.NewSize.Width, e.NewSize.Height) / 2;
                this.Center = new Vector2((float)(e.NewSize.Width / 2), (float)(e.NewSize.Height / 2));
                this.Arrow = RadiansPicker.RadiansToVector(this.Radians, this.Radius, this.Center);
            };

            //Manipulation
            this.RootGrid.ManipulationMode = ManipulationModes.All;
            this.RootGrid.ManipulationStarted += (sender, e) =>
            {
                this.Vector = e.Position.ToVector2() - this.Center;

                this.IsRadians = this.Vector.Length() < this.Radius;

                if (!this.IsRadians) return;
                this._Radians = RadiansPicker.VectorToRadians(this.Vector);
            };
            this.RootGrid.ManipulationDelta += (sender, e) =>
            {
                this.Vector += e.Delta.Translation.ToVector2();

                if (!this.IsRadians) return;
                this._Radians = RadiansPicker.VectorToRadians(this.Vector);
            };
            this.RootGrid.ManipulationCompleted += (sender, e) => { };
        }
    }
}
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Controls.TransformerControls
{
    public sealed partial class RemoteControl : UserControl
    {
        //Delegate
        public delegate void RemoteVectorHandler(object sender, Vector2 value);
        public event RemoteVectorHandler ValueChangeStarted;
        public event RemoteVectorHandler ValueChangeDelta;
        public event RemoteVectorHandler ValueChangeCompleted;

        public event RemoteVectorHandler Moved;

        //Size
        private Size RootGrigSize;
        private readonly Size CanvasSize = new Size(246, 246);
        private readonly Size ManipulationSize = new Size(140, 140);
        private readonly Size RemoteSize = new Size(40, 40);

        //Vector
        private Vector2 manipulationCenter;
        public Vector2 ManipulationCenter
        {
            get => this.manipulationCenter;
            set
            {
                Canvas.SetLeft(this.ManipulationEllipse, value.X + (this.CanvasSize.Width - this.ManipulationSize.Width) / 2);
                Canvas.SetTop(this.ManipulationEllipse, value.Y + (this.CanvasSize.Height - this.ManipulationSize.Height) / 2);

                this.manipulationCenter = value;
            }
        }

        private Vector2 remoteCenter;
        public Vector2 RemoteCenter
        {
            get => this.remoteCenter;
            set
            {
                Vector2 vector = value;

                float length = value.Length() * 2;
                float manipulationEdge = (float)Math.Sqrt((this.ManipulationSize.Width - this.RemoteEllipse.Width) * (this.ManipulationSize.Height - this.RemoteEllipse.Height));

                // Exceeding the edge?
                if (length > manipulationEdge)
                {
                    // Fixed at the edge
                    vector = value * manipulationEdge / length;
                }

                //Canvas
                Canvas.SetLeft(this.RemoteEllipse, vector.X + (this.CanvasSize.Width - this.RemoteSize.Width) / 2);
                Canvas.SetTop(this.RemoteEllipse, vector.Y + (this.CanvasSize.Height - this.RemoteSize.Height) / 2);

                this.remoteCenter = value;
            }
        }

        //Manipulation
        private bool isManipulation;
        public bool IsManipulation
        {
            get => this.isManipulation;
            set
            {
                if (value)
                {
                    this.PressedManipulationStoryboard.Begin();

                    this.LeftPath.IsHitTestVisible = false;
                    this.TopPath.IsHitTestVisible = false;
                    this.RightPath.IsHitTestVisible = false;
                    this.BottomPath.IsHitTestVisible = false;
                }
                else
                {
                    this.NormalManipulationStoryboard.Begin();

                    this.LeftPath.IsHitTestVisible = true;
                    this.TopPath.IsHitTestVisible = true;
                    this.RightPath.IsHitTestVisible = true;
                    this.BottomPath.IsHitTestVisible = true;
                }

                this.isManipulation = value;
            }
        }

        Vector2 Vector;
        public RemoteControl()
        {
            this.InitializeComponent();
            this.RootGrid.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.RootGrigSize = e.NewSize;
            };


            //Canvas
            this.Canvas.Width = this.CanvasSize.Width;
            this.Canvas.Height = this.CanvasSize.Height;
            //Manipulation
            this.ManipulationEllipse.Width = this.ManipulationSize.Width;
            this.ManipulationEllipse.Height = this.ManipulationSize.Height;
            this.ManipulationCenter = Vector2.Zero;
            //Remote
            this.RemoteEllipse.Width = this.RemoteSize.Width;
            this.RemoteEllipse.Height = this.RemoteSize.Height;
            this.RemoteCenter = Vector2.Zero;


            //Left
            this.LeftPath.Tapped += (s, e) => this.Moved?.Invoke(this, new Vector2(-1, 0)); //Delegate
            this.LeftPath.PointerEntered += (s, e) => this.PointerOverLeftStoryboard.Begin();
            this.LeftPath.PointerExited += (s, e) => this.NormalLeftStoryboard.Begin();
            this.LeftPath.PointerPressed += (s, e) => this.PressedLeftStoryboard.Begin();
            this.LeftPath.PointerReleased += (s, e) => this.NormalLeftStoryboard.Begin();


            //Top
            this.TopPath.Tapped += (s, e) => this.Moved?.Invoke(this, new Vector2(0, - 1)); //Delegate
            this.TopPath.PointerEntered += (s, e) => this.PointerOverTopStoryboard.Begin();
            this.TopPath.PointerExited += (s, e) => this.NormalTopStoryboard.Begin();
            this.TopPath.PointerPressed += (s, e) => this.PressedTopStoryboard.Begin();
            this.TopPath.PointerReleased += (s, e) => this.NormalTopStoryboard.Begin();


            //Right
            this.RightPath.Tapped += (s, e) => this.Moved?.Invoke(this, new Vector2(1, 0)); //Delegate
            this.RightPath.PointerEntered += (s, e) => this.PointerOverRightStoryboard.Begin();
            this.RightPath.PointerExited += (s, e) => this.NormalRightStoryboard.Begin();
            this.RightPath.PointerPressed += (s, e) => this.PressedRightStoryboard.Begin();
            this.RightPath.PointerReleased += (s, e) => this.NormalRightStoryboard.Begin();


            //Bottom
            this.BottomPath.Tapped += (s, e) => this.Moved?.Invoke(this, new Vector2(0, 1)); //Delegate
            this.BottomPath.PointerEntered += (s, e) => this.PointerOverBottomStoryboard.Begin();
            this.BottomPath.PointerExited += (s, e) => this.NormalBottomStoryboard.Begin();
            this.BottomPath.PointerPressed += (s, e) => this.PressedBottomStoryboard.Begin();
            this.BottomPath.PointerReleased += (s, e) => this.NormalBottomStoryboard.Begin();


            //Manipulation
            this.ManipulationEllipse.ManipulationMode = ManipulationModes.All;
            this.ManipulationEllipse.ManipulationStarted += (s, e) =>
            {
                //Manipulation
                this.IsManipulation = true;

                this.Vector = Vector2.Zero;
                this.ValueChangeStarted?.Invoke(this, Vector2.Zero); //Delegate
            };
            this.ManipulationEllipse.ManipulationDelta += (s, e) =>
            {
                //Manipulation
                Point point = e.Delta.Translation;
                double x = point.X * this.CanvasSize.Width / this.RootGrigSize.Width;
                double y = point.Y * this.CanvasSize.Height / this.RootGrigSize.Height;
                this.RemoteCenter += new Vector2((float)x, (float)y);
                
                this.Vector += point.ToVector2();
                this.ValueChangeDelta?.Invoke(this, this.Vector); //Delegate
            };
            this.ManipulationEllipse.ManipulationCompleted += (s, e) =>
            {
                //Manipulation
                this.IsManipulation = false;
                this.RemoteCenter = Vector2.Zero;

                this.Vector = e.Position.ToVector2();
                this.ValueChangeCompleted?.Invoke(this, this.Vector); //Delegate
            };
        }
    }
}
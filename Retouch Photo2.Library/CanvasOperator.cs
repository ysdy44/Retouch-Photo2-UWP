using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Library
{
    /// <summary> 
    /// Provides single-finger, double-finger, mobile events to pointer events for canvas controls.
    /// </summary>
    public class CanvasOperator : DependencyObject
    {

        #region DependencyProperty


        /// <summary>
        /// <see cref = "CanvasOperator" />'s destination control.
        /// </summary>
        public Control DestinationControl
        {
            get { return (Control)GetValue(DestinationControlProperty); }
            set { SetValue(DestinationControlProperty, value); }
        }
        public static DependencyProperty DestinationControlProperty = DependencyProperty.Register(nameof(DestinationControl), typeof(Windows.UI.Xaml.Controls.Control), typeof(CanvasOperator), new PropertyMetadata(null, (sender, e) =>
        {
            CanvasOperator con = (CanvasOperator)sender;

            if (e.NewValue is Control value)
            {
                value.PointerEntered += con.Control_PointerEntered;
                value.PointerExited += con.Control_PointerExited;

                value.PointerPressed += con.Control_PointerPressed;
                value.PointerReleased += con.Control_PointerReleased;

                value.PointerMoved += con.Control_PointerMoved;
                value.PointerWheelChanged += con.Control_PointerWheelChanged;
            }
        }));


        #endregion

        #region Delegate


        /// <summary>
        /// One Finger | Mouse Left Button | Pen
        /// </summary>
        /// <param name="point"> The position of the point. </param>
        public delegate void SingleHandler(Vector2 point);
        public event SingleHandler Single_Start = null;
        public event SingleHandler Single_Delta = null;
        public event SingleHandler Single_Complete = null;

        /// <summary>
        /// Mouse right button
        /// </summary>
        /// <param name="point"> The position of the point. </param>
        public delegate void RightHandler(Vector2 point);
        public event RightHandler Right_Start = null;
        public event RightHandler Right_Delta = null;
        public event RightHandler Right_Complete = null;

        /// <summary>
        /// Two fingers
        /// </summary>
        /// <param name="center"> The center of the double finger. </param>
        /// <param name="space"> The space between two fingers. </param>
        public delegate void DoubleHandler(Vector2 center, float space);
        public event DoubleHandler Double_Start = null;
        public event DoubleHandler Double_Delta = null;
        public event DoubleHandler Double_Complete = null;

        /// <summary>Mouse Wheel Changed  </summary>
        public event DoubleHandler Wheel_Changed = null;


        #endregion

        #region PointerRouted


        /// <summary> Return pointer position. </summary>
        public Vector2 Pointer_Position(PointerRoutedEventArgs e) => e.GetCurrentPoint(this.DestinationControl).Position.ToVector2();
        /// <summary> Return pointer pressure. </summary>
        public float Pointer_Pressure(PointerRoutedEventArgs e) => e.GetCurrentPoint(this.DestinationControl).Properties.Pressure;
        /// <summary> Return pointer wheel delta. </summary>
        public float Pointer_WheelDelta(PointerRoutedEventArgs e) => e.GetCurrentPoint(this.DestinationControl).Properties.MouseWheelDelta;

        /// <summary> Touch or not. </summary>
        public bool Pointer_IsTouch(PointerRoutedEventArgs e) => e.GetCurrentPoint(this.DestinationControl).PointerDevice.PointerDeviceType == PointerDeviceType.Touch;
        /// <summary> Pen or not. </summary>
        public bool Pointer_IsPen(PointerRoutedEventArgs e) => e.GetCurrentPoint(this.DestinationControl).PointerDevice.PointerDeviceType == PointerDeviceType.Pen && e.GetCurrentPoint(this.DestinationControl).Properties.IsBarrelButtonPressed == false && e.GetCurrentPoint(this.DestinationControl).IsInContact;
        /// <summary> Barrel or not. </summary>
        public bool Pointer_IsBarrel(PointerRoutedEventArgs e) => e.GetCurrentPoint(this.DestinationControl).PointerDevice.PointerDeviceType == PointerDeviceType.Pen && e.GetCurrentPoint(this.DestinationControl).Properties.IsBarrelButtonPressed == true && e.GetCurrentPoint(this.DestinationControl).IsInContact;

        /// <summary> Mouse or not. </summary>
        public bool Pointer_IsMouse(PointerRoutedEventArgs e) => e.GetCurrentPoint(this.DestinationControl).PointerDevice.PointerDeviceType == PointerDeviceType.Mouse;
        /// <summary> Mouse left button or not. </summary>
        public bool Pointer_IsLeft(PointerRoutedEventArgs e) => e.GetCurrentPoint(this.DestinationControl).PointerDevice.PointerDeviceType == PointerDeviceType.Mouse && e.GetCurrentPoint(this.DestinationControl).Properties.IsLeftButtonPressed;
        /// <summary> Mouse right button or not. </summary>
        public bool Pointer_IsRight(PointerRoutedEventArgs e) => e.GetCurrentPoint(this.DestinationControl).Properties.IsRightButtonPressed || e.GetCurrentPoint(this.DestinationControl).Properties.IsMiddleButtonPressed;


        #endregion

        #region Point

        InputDevice Device = InputDevice.None;

        readonly HashSet<uint> _pointers = new HashSet<uint>();

        Vector2 _evenPointer; //it's ID%2==0
        Vector2 _oddPointer;//it's ID%2==1

        Vector2 _evenStartingPointer;
        Vector2 _oddStartingPointer;

        Vector2 __startingPointer;


        //Pointer Entered
        private void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
        }
        //Pointer Exited
        private void Control_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.Control_PointerReleased(sender, e);
        }


        //Pointer Pressed
        private void Control_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = this.Pointer_Position(e);

            if (this.Pointer_IsTouch(e))
            {
                this._pointers.Add(e.Pointer.PointerId);
                if (e.Pointer.PointerId % 2 == 0) this._evenPointer = point;
                if (e.Pointer.PointerId % 2 == 1) this._oddPointer = point;

                if (this._pointers.Count > 1)
                {
                    if (this.Device != InputDevice.Double)
                    {
                        this._evenStartingPointer = this._evenPointer;
                        this._oddStartingPointer = this._oddPointer;
                    }
                }
                else
                {
                    if (this.Device != InputDevice.Single)
                        this.__startingPointer = point;
                }
            }
            else
            {
                if (this.Pointer_IsRight(e))
                {
                    if (this.Device != InputDevice.Right)
                    {
                        this.Device = InputDevice.Right;
                        this.Right_Start?.Invoke(point);//Delegate
                    }
                }
                if (this.Pointer_IsLeft(e) || this.Pointer_IsPen(e))
                {
                    if (this.Device != InputDevice.Single)
                    {
                        this.Device = InputDevice.Single;
                        this.Single_Start?.Invoke(point);//Delegate
                    }
                }
            }
        }

        //Pointer Released
        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = this.Pointer_Position(e);

            if (this.Device == InputDevice.Right)
            {
                this.Device = InputDevice.None;
                this.Right_Complete?.Invoke(point);//Delegate
            }
            else if (this.Device == InputDevice.Double)
            {
                this.Device = InputDevice.None;
                this.Double_Complete?.Invoke(point, (this._oddPointer - this._evenPointer).Length());//Delegate
            }
            else if (this.Device == InputDevice.Single)
            {
                this.Device = InputDevice.None;
                this.Single_Complete?.Invoke(point);//Delegate
            }

            this._pointers.Clear();
        }




        //Pointer Moved
        private void Control_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = this.Pointer_Position(e);

            if (this.Pointer_IsTouch(e))
            {
                if (e.Pointer.PointerId % 2 == 0) this._evenPointer = point;
                if (e.Pointer.PointerId % 2 == 1) this._oddPointer = point;

                if (this._pointers.Count > 1)
                {
                    if (this.Device != InputDevice.Double)
                    {
                        if (Math.Abs(this._evenStartingPointer.X - this._evenPointer.X) > 2 || Math.Abs(this._evenStartingPointer.Y - this._evenPointer.Y) > 2 || Math.Abs(this._oddStartingPointer.X - this._oddPointer.X) > 2 || Math.Abs(this._oddStartingPointer.Y - this._oddPointer.Y) > 2)
                        {
                            this.Device = InputDevice.Double;
                            this.Double_Start?.Invoke((this._oddPointer + this._evenPointer) / 2, (this._oddPointer - this._evenPointer).Length());//Delegate
                        }
                    }
                    else if (this.Device == InputDevice.Double) this.Double_Delta?.Invoke((this._oddPointer + this._evenPointer) / 2, (this._oddPointer - this._evenPointer).Length());//Delegate
                }
                else
                {
                    if (this.Device != InputDevice.Single)
                    {
                        double length = (this.__startingPointer - point).Length();

                        if (length > 2 && length < 12)
                        {
                            this.Device = InputDevice.Single;
                            this.Single_Start?.Invoke(point);//Delegate
                        }
                    }
                    else if (this.Device == InputDevice.Single) this.Single_Delta?.Invoke(point);//Delegate
                }
            }
            else
            {
                if (this.Device == InputDevice.Right) this.Right_Delta?.Invoke(point);//Delegate

                if (this.Device == InputDevice.Single) this.Single_Delta?.Invoke(point);//Delegate
            }
        }
        //Wheel Changed
        private void Control_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = this.Pointer_Position(e);
            float space = this.Pointer_WheelDelta(e);

            this.Wheel_Changed?.Invoke(point, space);//Delegate
        }


        #endregion

    }

    /// <summary>
    /// <see cref = "CanvasOperator" />'s input device.
    /// </summary>
    internal enum InputDevice
    {
        None,

        /// <summary>One Finger | Mouse Left Button | Pen</summary>
        Single,

        /// <summary>Two Fingers</summary>
        Double,

        /// <summary>Mouse Right Button </summary>
        Right,
    }
}

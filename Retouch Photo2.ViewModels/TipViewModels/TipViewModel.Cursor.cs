using FanKit.Transformers;
using System.ComponentModel;
using Windows.Devices.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels
{
    public partial class TipViewModel : INotifyPropertyChanged
    {

        // VisualState
        public PointerDeviceType pointerDeviceType = PointerDeviceType.Touch;

        public double pointer_Angle = 0.0d;
        public bool hand_Is = false;

        public bool tool_IsPointerEntered = false;
        public bool tool_IsManipulationStarted = false;

        public bool move_IsPointerEntered = false;
        public bool move_IsManipulationStarted = false;

        public bool rotate_IsPointerEntered = false;
        public bool rotate_IsManipulationStarted = false;

        public bool skew_IsPointerEntered = false;
        public bool skew_IsManipulationStarted = false;

        public bool scale_IsPointerEntered = false;
        public bool scale_IsManipulationStarted = false;



        /// <summary>
        /// Represents the icon of mouse pointer in a specific state.
        /// </summary>
        public CoreCursorType CoreCursorType
        {
            get
            {
                //Hand
                if (this.hand_Is)
                {
                    if (this.pointerDeviceType == PointerDeviceType.Mouse || this.pointerDeviceType == PointerDeviceType.Pen)
                    {
                        return CoreCursorType.Hand;
                    }
                }

                //Tool
                if (this.tool_IsPointerEntered || this.tool_IsManipulationStarted)
                {
                    if (this.pointerDeviceType == PointerDeviceType.Mouse || this.pointerDeviceType == PointerDeviceType.Pen)
                    {
                        return CoreCursorType.Cross;
                    }
                }

                //Rotate
                if (this.rotate_IsPointerEntered || this.rotate_IsManipulationStarted)
                {
                    if (this.pointerDeviceType == PointerDeviceType.Mouse || this.pointerDeviceType == PointerDeviceType.Pen)
                    {
                        return this.SnapRotateCoreCursorType(this.pointer_Angle);
                    }
                }

                //Skew
                if (this.skew_IsPointerEntered || this.skew_IsManipulationStarted)
                {
                    if (this.pointerDeviceType == PointerDeviceType.Mouse || this.pointerDeviceType == PointerDeviceType.Pen)
                    {
                        return this.SnapRotateCoreCursorType(this.pointer_Angle);
                    }
                }

                //Scale
                if (this.scale_IsPointerEntered || this.scale_IsManipulationStarted)
                {
                    if (this.pointerDeviceType == PointerDeviceType.Mouse || this.pointerDeviceType == PointerDeviceType.Pen)
                    {
                        return this.SnapScaleCoreCursorType(this.pointer_Angle);
                    }
                }

                //Move
                if (this.move_IsPointerEntered || this.move_IsManipulationStarted)
                {
                    if (this.pointerDeviceType == PointerDeviceType.Mouse || this.pointerDeviceType == PointerDeviceType.Pen)
                    {
                        return CoreCursorType.SizeAll;
                    }
                }

                return CoreCursorType.Arrow;
            }
            set
            {
                if (Window.Current.CoreWindow.PointerCursor.Type == value) return;

                Window.Current.CoreWindow.PointerCursor = new CoreCursor(value, 0);
            }
        }



        public bool Cursor_PointerEntered_None()
        {
            this.tool_IsPointerEntered = false;
            this.move_IsPointerEntered = false;
            this.rotate_IsPointerEntered = false;
            this.skew_IsPointerEntered = false;
            this.scale_IsPointerEntered = false;
            this.CoreCursorType = this.CoreCursorType;//CoreCursorType
            return false;
        }
        public bool Cursor_PointerEntered_Tool()
        {
            this.tool_IsPointerEntered = true;
            this.move_IsPointerEntered = false;
            this.rotate_IsPointerEntered = false;
            this.skew_IsPointerEntered = false;
            this.scale_IsPointerEntered = false;
            this.CoreCursorType = this.CoreCursorType;//CoreCursorType
            return true;
        }
        public bool Cursor_PointerEntered_Move()
        {
            this.tool_IsPointerEntered = false;
            this.move_IsPointerEntered = true;
            this.rotate_IsPointerEntered = false;
            this.skew_IsPointerEntered = false;
            this.scale_IsPointerEntered = false;
            this.CoreCursorType = this.CoreCursorType;//CoreCursorType
            return true;
        }
        public bool Cursor_PointerEntered_RotateSkewScale(TransformerMode mode)
        {
            switch (mode)
            {
                case TransformerMode.None:
                    return this.Cursor_PointerEntered_None();

                case TransformerMode.Rotation:
                    this.tool_IsPointerEntered = false;
                    this.move_IsPointerEntered = false;
                    this.rotate_IsPointerEntered = true;
                    this.skew_IsPointerEntered = false;
                    this.scale_IsPointerEntered = false;
                    this.CoreCursorType = this.CoreCursorType;//CoreCursorType
                    return true;

                case TransformerMode.SkewLeft:
                case TransformerMode.SkewTop:
                case TransformerMode.SkewRight:
                case TransformerMode.SkewBottom:
                    this.tool_IsPointerEntered = false;
                    this.move_IsPointerEntered = false;
                    this.skew_IsPointerEntered = true;
                    this.scale_IsPointerEntered = false;
                    this.CoreCursorType = this.CoreCursorType;//CoreCursorType
                    return true;

                case TransformerMode.ScaleLeft:
                case TransformerMode.ScaleTop:
                case TransformerMode.ScaleRight:
                case TransformerMode.ScaleBottom:
                    this.tool_IsPointerEntered = false;
                    this.move_IsPointerEntered = false;
                    this.rotate_IsPointerEntered = false;
                    this.skew_IsPointerEntered = false;
                    this.scale_IsPointerEntered = true;
                    this.CoreCursorType = this.CoreCursorType;//CoreCursorType
                    return true;

                case TransformerMode.ScaleLeftTop:
                case TransformerMode.ScaleRightTop:
                case TransformerMode.ScaleRightBottom:
                case TransformerMode.ScaleLeftBottom:
                    this.tool_IsPointerEntered = false;
                    this.move_IsPointerEntered = false;
                    this.rotate_IsPointerEntered = false;
                    this.skew_IsPointerEntered = false;
                    this.scale_IsPointerEntered = true;
                    this.CoreCursorType = this.CoreCursorType;//CoreCursorType
                    return true;

                default:
                    return this.Cursor_PointerEntered_None();
            }
        }



        public bool Cursor_ManipulationStarted_None()
        {
            this.tool_IsManipulationStarted = false;
            this.move_IsManipulationStarted = false;
            this.rotate_IsManipulationStarted = false;
            this.skew_IsManipulationStarted = false;
            this.scale_IsManipulationStarted = false;
            this.CoreCursorType = this.CoreCursorType;//CoreCursorType
            return false;
        }
        public bool Cursor_ManipulationStarted_Tool()
        {
            this.tool_IsManipulationStarted = true;
            this.move_IsManipulationStarted = false;
            this.rotate_IsManipulationStarted = false;
            this.skew_IsManipulationStarted = false;
            this.scale_IsManipulationStarted = false;
            this.CoreCursorType = this.CoreCursorType;//CoreCursorType
            return true;
        }
        public bool Cursor_ManipulationStarted_Move()
        {
            this.tool_IsManipulationStarted = false;
            this.move_IsManipulationStarted = true;
            this.rotate_IsManipulationStarted = false;
            this.skew_IsManipulationStarted = false;
            this.scale_IsManipulationStarted = false;
            this.CoreCursorType = this.CoreCursorType;//CoreCursorType
            return true;
        }
        public bool Cursor_ManipulationStarted_RotateSkewScale(TransformerMode mode)
        {
            switch (mode)
            {
                case TransformerMode.None:
                    return this.Cursor_ManipulationStarted_None();

                case TransformerMode.Rotation:
                    this.tool_IsManipulationStarted = false;
                    this.move_IsManipulationStarted = false;
                    this.rotate_IsManipulationStarted = true;
                    this.skew_IsManipulationStarted = false;
                    this.scale_IsManipulationStarted = false;
                    this.CoreCursorType = this.CoreCursorType;//CoreCursorType
                    return true;

                case TransformerMode.SkewLeft:
                case TransformerMode.SkewTop:
                case TransformerMode.SkewRight:
                case TransformerMode.SkewBottom:
                    this.tool_IsManipulationStarted = false;
                    this.move_IsManipulationStarted = false;
                    this.skew_IsManipulationStarted = true;
                    this.scale_IsManipulationStarted = false;
                    this.CoreCursorType = this.CoreCursorType;//CoreCursorType
                    return true;

                case TransformerMode.ScaleLeft:
                case TransformerMode.ScaleTop:
                case TransformerMode.ScaleRight:
                case TransformerMode.ScaleBottom:
                    this.tool_IsManipulationStarted = false;
                    this.move_IsManipulationStarted = false;
                    this.rotate_IsManipulationStarted = false;
                    this.skew_IsManipulationStarted = false;
                    this.scale_IsManipulationStarted = true;
                    this.CoreCursorType = this.CoreCursorType;//CoreCursorType
                    return true;

                case TransformerMode.ScaleLeftTop:
                case TransformerMode.ScaleRightTop:
                case TransformerMode.ScaleRightBottom:
                case TransformerMode.ScaleLeftBottom:
                    this.tool_IsManipulationStarted = false;
                    this.move_IsManipulationStarted = false;
                    this.rotate_IsManipulationStarted = false;
                    this.skew_IsManipulationStarted = false;
                    this.scale_IsManipulationStarted = true;
                    this.CoreCursorType = this.CoreCursorType;//CoreCursorType
                    return true;

                default:
                    return this.Cursor_ManipulationStarted_None();
            }
        }



        /// <summary>
        /// Snap the angle to the rotate tick.
        /// </summary>
        /// <param name="angle"> The source angle. </param>
        /// <returns> Return a destination angle that common divisor is <see cref="SnapPointStateService.RotateTick"/>. </returns>
        public double SnapRotateTick(double angle)
        {
            while (angle < 0.0d)
                angle += 360.0d;
            while (angle > 360.0d)
                angle -= 360.0d;

            //      ↑
            // ←     →
            //      ↓ 

            //     if (angle < SnapPointStateService.RotateTickRange) // 0 + 10
            //         return 0.0d;

            //      else if (angle > 90.0d - SnapPointStateService.RotateTickRange && angle < 90.0d + SnapPointStateService.RotateTickRange) // 10 - 90 + 10
            //         return 90.0d;
            //      else if (angle > 180.0d - SnapPointStateService.RotateTickRange && angle < 180.0d + SnapPointStateService.RotateTickRange) // 10 - 180 + 10
            //           return 180.0d;

            //      else if (angle > 270.0d - SnapPointStateService.RotateTickRange && angle < 270.0d + SnapPointStateService.RotateTickRange) // 10 - 270 + 10
            //        return 270.0d;
            //     else if (angle > 360.0d - SnapPointStateService.RotateTickRange && angle < 360.0d + SnapPointStateService.RotateTickRange) // 10 - 360 + 10
            //          return 360.0d;

            //       else
            return angle;
        }


        private CoreCursorType SnapRotateCoreCursorType(double angle)
        {
            while (angle < 0.0d)
                angle += 360.0d;
            while (angle > 360.0d)
                angle -= 360.0d;

            // ↗ → ↘
            // ↑         ↓
            // ↖ ← ↙

            if (angle < 22.5) // 0 + 22.5
                return CoreCursorType.SizeWestEast; // →

            if (angle < 67.5) // 45 + 22.5
                return CoreCursorType.SizeNorthwestSoutheast; // ↘
            else if (angle < 112.5) // 90 + 22.5
                return CoreCursorType.SizeNorthSouth; // ↓
            else if (angle < 157.5) // 135 + 22.5
                return CoreCursorType.SizeNortheastSouthwest; // ↙
            else if (angle < 202.5) // 180 + 22.5
                return CoreCursorType.SizeWestEast; // ←

            else if (angle < 247.5) // 225 + 22.5
                return CoreCursorType.SizeNorthwestSoutheast; // ↖
            else if (angle < 292.5) // 270 + 22.5
                return CoreCursorType.SizeNorthSouth; // ↑
            else if (angle < 337.5) // 315 + 22.5
                return CoreCursorType.SizeNortheastSouthwest; // ↗
            else //if (angle < 382.5) // 360 + 22.5
                return CoreCursorType.SizeWestEast; // →
        }


        private CoreCursorType SnapScaleCoreCursorType(double angle)
        {
            while (angle < 0.0d)
                angle += 360.0d;
            while (angle > 360.0d)
                angle -= 360.0d;

            // ↘ ↓ ↙
            // →     ←
            // ↗ ↑ ↖

            if (angle < 22.5) // 0 + 22.5
                return CoreCursorType.SizeNorthSouth; // ↓

            if (angle < 67.5) // 45 + 22.5
                return CoreCursorType.SizeNortheastSouthwest; // ↙
            else if (angle < 112.5) // 90 + 22.5
                return CoreCursorType.SizeWestEast; // ←
            else if (angle < 157.5) // 135 + 22.5
                return CoreCursorType.SizeNorthwestSoutheast; // ↖
            else if (angle < 202.5) // 180 + 22.5
                return CoreCursorType.SizeNorthSouth; // ↑

            else if (angle < 247.5) // 225 + 22.5
                return CoreCursorType.SizeNortheastSouthwest; // ↗
            else if (angle < 292.5) // 270 + 22.5
                return CoreCursorType.SizeWestEast; // →
            else if (angle < 337.5) // 315 + 22.5
                return CoreCursorType.SizeNorthwestSoutheast; // ↘
            else //if (angle < 382.5) // 360 + 22.5
                return CoreCursorType.SizeNorthSouth; // ↓
        }


    }
}
using FanKit.Transformers;
using Windows.Devices.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provides constant and static member 
    /// for <see cref="Window.Current.CoreWindow.PointerCursor "/>.
    /// </summary>
    public static class CoreCursorExtension
    {

        //Content
        public static PointerDeviceType PointerDeviceType { get; set; } = PointerDeviceType.Touch;

        public static double Pointer_Angle { get; set; } = 0.0d;

        private static bool hand_Is = false;
        public static bool Hand_Is
        {
            set
            {
                CoreCursorExtension.hand_Is = value;
                CoreCursorExtension.UpdateCoreCursor();
            }
        }


        private static bool tool_IsPointerEntered = false;
        private static bool tool_IsManipulationStarted = false;

        private static bool move_IsPointerEntered = false;
        private static bool move_IsManipulationStarted = false;

        private static bool rotate_IsPointerEntered = false;
        private static bool rotate_IsManipulationStarted = false;

        private static bool skew_IsPointerEntered = false;
        private static bool skew_IsManipulationStarted = false;

        private static bool scale_IsPointerEntered = false;
        private static bool scale_IsManipulationStarted = false;


        public static bool IsManipulationStarted()
        {
            if (CoreCursorExtension.tool_IsManipulationStarted) return true;
            if (CoreCursorExtension.move_IsManipulationStarted) return true;
            if (CoreCursorExtension.skew_IsManipulationStarted) return true;
            if (CoreCursorExtension.scale_IsManipulationStarted) return true;
            return false;
        }


        //Update
        private static void UpdateCoreCursor()
        {
            CoreCursorType value = CoreCursorExtension.GetCoreCursorType();
            if (Window.Current.CoreWindow.PointerCursor.Type == value) return;
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(value, 0);
        }
        private static CoreCursorType GetCoreCursorType()
        {
            //Hand
            if (CoreCursorExtension.hand_Is)
            {
                if (CoreCursorExtension.PointerDeviceType == PointerDeviceType.Mouse || CoreCursorExtension.PointerDeviceType == PointerDeviceType.Pen)
                {
                    return CoreCursorType.Hand;
                }
            }

            //Tool
            if (CoreCursorExtension.tool_IsPointerEntered || CoreCursorExtension.tool_IsManipulationStarted)
            {
                if (CoreCursorExtension.PointerDeviceType == PointerDeviceType.Mouse || CoreCursorExtension.PointerDeviceType == PointerDeviceType.Pen)
                {
                    return CoreCursorType.Cross;
                }
            }

            //Rotate
            if (CoreCursorExtension.rotate_IsPointerEntered || CoreCursorExtension.rotate_IsManipulationStarted)
            {
                if (CoreCursorExtension.PointerDeviceType == PointerDeviceType.Mouse || CoreCursorExtension.PointerDeviceType == PointerDeviceType.Pen)
                {
                    return CoreCursorExtension.SnapRotateCoreCursorType(CoreCursorExtension.Pointer_Angle);
                }
            }

            //Skew
            if (CoreCursorExtension.skew_IsPointerEntered || CoreCursorExtension.skew_IsManipulationStarted)
            {
                if (CoreCursorExtension.PointerDeviceType == PointerDeviceType.Mouse || CoreCursorExtension.PointerDeviceType == PointerDeviceType.Pen)
                {
                    return CoreCursorExtension.SnapRotateCoreCursorType(CoreCursorExtension.Pointer_Angle);
                }
            }

            //Scale
            if (CoreCursorExtension.scale_IsPointerEntered || CoreCursorExtension.scale_IsManipulationStarted)
            {
                if (CoreCursorExtension.PointerDeviceType == PointerDeviceType.Mouse || CoreCursorExtension.PointerDeviceType == PointerDeviceType.Pen)
                {
                    return CoreCursorExtension.SnapScaleCoreCursorType(CoreCursorExtension.Pointer_Angle);
                }
            }

            //Move
            if (CoreCursorExtension.move_IsPointerEntered || CoreCursorExtension.move_IsManipulationStarted)
            {
                if (CoreCursorExtension.PointerDeviceType == PointerDeviceType.Mouse || CoreCursorExtension.PointerDeviceType == PointerDeviceType.Pen)
                {
                    return CoreCursorType.SizeAll;
                }
            }

            return CoreCursorType.Arrow;
        }


        //Methon
        public static void None_PointerEntered()
        {
            CoreCursorExtension.tool_IsPointerEntered = false;
            CoreCursorExtension.move_IsPointerEntered = false;
            CoreCursorExtension.rotate_IsPointerEntered = false;
            CoreCursorExtension.skew_IsPointerEntered = false;
            CoreCursorExtension.scale_IsPointerEntered = false;
            CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
        }
        public static void Tool_PointerEntered()
        {
            CoreCursorExtension.tool_IsPointerEntered = true;
            CoreCursorExtension.move_IsPointerEntered = false;
            CoreCursorExtension.rotate_IsPointerEntered = false;
            CoreCursorExtension.skew_IsPointerEntered = false;
            CoreCursorExtension.scale_IsPointerEntered = false;
            CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
        }
        public static void Move_PointerEntered()
        {
            CoreCursorExtension.tool_IsPointerEntered = false;
            CoreCursorExtension.move_IsPointerEntered = true;
            CoreCursorExtension.rotate_IsPointerEntered = false;
            CoreCursorExtension.skew_IsPointerEntered = false;
            CoreCursorExtension.scale_IsPointerEntered = false;
            CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
        }
        public static void RotateSkewScale_PointerEntered(TransformerMode mode)
        {
            switch (mode)
            {
                case TransformerMode.None:
                    CoreCursorExtension.None_PointerEntered();
                    break;

                case TransformerMode.Rotation:
                    CoreCursorExtension.tool_IsPointerEntered = false;
                    CoreCursorExtension.move_IsPointerEntered = false;
                    CoreCursorExtension.rotate_IsPointerEntered = true;
                    CoreCursorExtension.skew_IsPointerEntered = false;
                    CoreCursorExtension.scale_IsPointerEntered = false;
                    CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
                    break;

                case TransformerMode.SkewLeft:
                case TransformerMode.SkewTop:
                case TransformerMode.SkewRight:
                case TransformerMode.SkewBottom:
                    CoreCursorExtension.tool_IsPointerEntered = false;
                    CoreCursorExtension.move_IsPointerEntered = false;
                    CoreCursorExtension.skew_IsPointerEntered = true;
                    CoreCursorExtension.scale_IsPointerEntered = false;
                    CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
                    break;

                case TransformerMode.ScaleLeft:
                case TransformerMode.ScaleTop:
                case TransformerMode.ScaleRight:
                case TransformerMode.ScaleBottom:
                    CoreCursorExtension.tool_IsPointerEntered = false;
                    CoreCursorExtension.move_IsPointerEntered = false;
                    CoreCursorExtension.rotate_IsPointerEntered = false;
                    CoreCursorExtension.skew_IsPointerEntered = false;
                    CoreCursorExtension.scale_IsPointerEntered = true;
                    CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
                    break;

                case TransformerMode.ScaleLeftTop:
                case TransformerMode.ScaleRightTop:
                case TransformerMode.ScaleRightBottom:
                case TransformerMode.ScaleLeftBottom:
                    CoreCursorExtension.tool_IsPointerEntered = false;
                    CoreCursorExtension.move_IsPointerEntered = false;
                    CoreCursorExtension.rotate_IsPointerEntered = false;
                    CoreCursorExtension.skew_IsPointerEntered = false;
                    CoreCursorExtension.scale_IsPointerEntered = true;
                    CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
                    break;

                default:
                    CoreCursorExtension.None_PointerEntered();
                    break;
            }
        }



        public static void None_ManipulationStarted()
        {
            CoreCursorExtension.tool_IsManipulationStarted = false;
            CoreCursorExtension.move_IsManipulationStarted = false;
            CoreCursorExtension.rotate_IsManipulationStarted = false;
            CoreCursorExtension.skew_IsManipulationStarted = false;
            CoreCursorExtension.scale_IsManipulationStarted = false;
            CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
        }
        public static void Tool_ManipulationStarted()
        {
            CoreCursorExtension.tool_IsManipulationStarted = true;
            CoreCursorExtension.move_IsManipulationStarted = false;
            CoreCursorExtension.rotate_IsManipulationStarted = false;
            CoreCursorExtension.skew_IsManipulationStarted = false;
            CoreCursorExtension.scale_IsManipulationStarted = false;
            CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
        }
        public static void Move_ManipulationStarted()
        {
            CoreCursorExtension.tool_IsManipulationStarted = false;
            CoreCursorExtension.move_IsManipulationStarted = true;
            CoreCursorExtension.rotate_IsManipulationStarted = false;
            CoreCursorExtension.skew_IsManipulationStarted = false;
            CoreCursorExtension.scale_IsManipulationStarted = false;
            CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
        }
        public static void RotateSkewScale_ManipulationStarted(TransformerMode mode)
        {
            switch (mode)
            {
                case TransformerMode.None:
                    CoreCursorExtension.None_ManipulationStarted();
                    break;

                case TransformerMode.Rotation:
                    CoreCursorExtension.tool_IsManipulationStarted = false;
                    CoreCursorExtension.move_IsManipulationStarted = false;
                    CoreCursorExtension.rotate_IsManipulationStarted = true;
                    CoreCursorExtension.skew_IsManipulationStarted = false;
                    CoreCursorExtension.scale_IsManipulationStarted = false;
                    CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
                    break;

                case TransformerMode.SkewLeft:
                case TransformerMode.SkewTop:
                case TransformerMode.SkewRight:
                case TransformerMode.SkewBottom:
                    CoreCursorExtension.tool_IsManipulationStarted = false;
                    CoreCursorExtension.move_IsManipulationStarted = false;
                    CoreCursorExtension.skew_IsManipulationStarted = true;
                    CoreCursorExtension.scale_IsManipulationStarted = false;
                    CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
                    break;

                case TransformerMode.ScaleLeft:
                case TransformerMode.ScaleTop:
                case TransformerMode.ScaleRight:
                case TransformerMode.ScaleBottom:
                    CoreCursorExtension.tool_IsManipulationStarted = false;
                    CoreCursorExtension.move_IsManipulationStarted = false;
                    CoreCursorExtension.rotate_IsManipulationStarted = false;
                    CoreCursorExtension.skew_IsManipulationStarted = false;
                    CoreCursorExtension.scale_IsManipulationStarted = true;
                    CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
                    break;

                case TransformerMode.ScaleLeftTop:
                case TransformerMode.ScaleRightTop:
                case TransformerMode.ScaleRightBottom:
                case TransformerMode.ScaleLeftBottom:
                    CoreCursorExtension.tool_IsManipulationStarted = false;
                    CoreCursorExtension.move_IsManipulationStarted = false;
                    CoreCursorExtension.rotate_IsManipulationStarted = false;
                    CoreCursorExtension.skew_IsManipulationStarted = false;
                    CoreCursorExtension.scale_IsManipulationStarted = true;
                    CoreCursorExtension.UpdateCoreCursor();//CoreCursorType
                    break;

                default:
                    CoreCursorExtension.None_ManipulationStarted();
                    break;
            }
        }



        /// <summary>
        /// Snap the angle to the rotate tick.
        /// </summary>
        /// <param name="angle"> The source angle. </param>
        /// <returns> Return a destination angle that common divisor is <see cref="SnapPointStateService.RotateTick"/>. </returns>
        public static double SnapRotateTick(double angle)
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


        private static CoreCursorType SnapRotateCoreCursorType(double angle)
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


        private static CoreCursorType SnapScaleCoreCursorType(double angle)
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
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

        public static PointerDeviceType PointerDeviceType { private get; set; } = PointerDeviceType.Touch;
        public static bool IsPointerEntered { private get; set; } = false;
        public static bool IsManipulationStarted { private get; set; } = false;



        public static void None()
        {
            if (CoreCursorExtension.PointerDeviceType == PointerDeviceType.Mouse || CoreCursorExtension.PointerDeviceType == PointerDeviceType.Pen)
            {
                if (CoreCursorExtension.IsPointerEntered || CoreCursorExtension.IsManipulationStarted)
                {
                    return;
                }
            }

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
        }

        public static void Cross()
        {
            if (CoreCursorExtension.PointerDeviceType == PointerDeviceType.Mouse || CoreCursorExtension.PointerDeviceType == PointerDeviceType.Pen)
            {
                if (CoreCursorExtension.IsPointerEntered || CoreCursorExtension.IsManipulationStarted)
                {
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Cross, 0);
                    return;
                }
            }

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
        }

        public static void SizeAll()
        {
            if (CoreCursorExtension.PointerDeviceType == PointerDeviceType.Mouse || CoreCursorExtension.PointerDeviceType == PointerDeviceType.Pen)
            {
                if (CoreCursorExtension.IsPointerEntered || CoreCursorExtension.IsManipulationStarted)
                {
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.SizeAll, 0);
                    return;
                }
            }

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
        }

        public static void SizeTranfrom(TransformerMode mode, float angle)
        {
            if (CoreCursorExtension.PointerDeviceType == PointerDeviceType.Mouse || CoreCursorExtension.PointerDeviceType == PointerDeviceType.Pen)
            {
                if (CoreCursorExtension.IsPointerEntered || CoreCursorExtension.IsManipulationStarted)
                {
                    switch (mode)
                    {
                        case TransformerMode.None:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
                            break;

                        case TransformerMode.Rotation:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapRotateCoreCursorType(angle), 0);
                            break;

                        case TransformerMode.SkewLeft:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapRotateCoreCursorType(-90 + angle), 0);
                            break;
                        case TransformerMode.SkewTop:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapRotateCoreCursorType(angle), 0);
                            break;
                        case TransformerMode.SkewRight:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapRotateCoreCursorType(90 + angle), 0);
                            break;
                        case TransformerMode.SkewBottom:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapRotateCoreCursorType(180 + angle), 0);
                            break;

                        case TransformerMode.ScaleLeft:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapScaleCoreCursorType(-90 + angle), 0);
                            break;
                        case TransformerMode.ScaleTop:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapScaleCoreCursorType(angle), 0);
                            break;
                        case TransformerMode.ScaleRight:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapScaleCoreCursorType(90 + angle), 0);
                            break;
                        case TransformerMode.ScaleBottom:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapScaleCoreCursorType(180 + angle), 0);
                            break;

                        case TransformerMode.ScaleLeftTop:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapScaleCoreCursorType(-45 + angle), 0);
                            break;
                        case TransformerMode.ScaleRightTop:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapScaleCoreCursorType(45 + angle), 0);
                            break;
                        case TransformerMode.ScaleRightBottom:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapScaleCoreCursorType(135 + angle), 0);
                            break;
                        case TransformerMode.ScaleLeftBottom:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorExtension.SnapScaleCoreCursorType(225 + angle), 0);
                            break;

                        default:
                            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
                            break;
                    }
                    return;
                }
            }

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
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
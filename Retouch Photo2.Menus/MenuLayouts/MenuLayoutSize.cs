using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a class that calculates the position and size of the menu.
    /// </summary>
    public class MenuLayoutSize
    {
        /// <summary> Size of content. </summary>
        public Size Size;
        /// <summary> Position of the overlay on the canvas. </summary>
        public Vector2 Postion;


        /// <summary>
        /// Gets flyout's postion in windows.
        /// </summary>
        /// <param name="element"> The element on flyout. </param>
        /// <returns> The calculated postion. </returns>
        public static Vector2 GetElementVisualPostion(UIElement element) => element.TransformToVisual(Window.Current.Content).TransformPoint(new Point()).ToVector2();

        /// <summary>
        /// Gets overlay's postion on canvas.
        /// </summary>
        /// <param name="element"> The element on canvas. </param>
        /// <returns> The calculated postion. </returns>
        public static Vector2 GetElementCanvasPostion(UIElement element) => new Vector2((float)Canvas.GetLeft(element), (float)Canvas.GetTop(element));

        /// <summary>
        /// Sets overlay's postion on canvas.
        /// </summary>
        /// <param name="element"> The element on canvas. </param>
        /// <param name="postion"> The source postion. </param>
        /// <param name="size"> The source size. </param>
        public static void SetElementCanvasPostion(UIElement element, Vector2 postion, Size size)
        {
            double X;
            if (postion.X < 0) X = 0;
            else if (size.Width > Window.Current.Bounds.Width) X = 0;
            else if (postion.X > (Window.Current.Bounds.Width - size.Width)) X = (Window.Current.Bounds.Width - size.Width);
            else X = postion.X;
            Canvas.SetLeft(element, X);

            double Y;
            if (postion.Y < 0) Y = 0;
            else if (size.Height > Window.Current.Bounds.Height) Y = 0;
            else if (postion.Y > (Window.Current.Bounds.Height - size.Height)) Y = (Window.Current.Bounds.Height - size.Height);
            else Y = postion.Y;
            Canvas.SetTop(element, Y);
        }

    }
}
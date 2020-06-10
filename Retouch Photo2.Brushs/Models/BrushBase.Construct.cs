using FanKit.Transformers;
using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a brush that can have fill properties. Provides a filling method.
    /// </summary>
    public partial class BrushBase : IBrush
    {
        //@Static

        /// <summary>
        /// Initializes a ColorBrush.
        /// </summary>
        /// <param name="color"> The color. </param>
        /// <returns> The product <see cref="IBrush"/>. </returns>
        public static IBrush ColorBrush(Color color)
        {
            return new BrushBase
            {
                Type = BrushType.Color,

                Color = color,
            };
        }


        /// <summary>
        /// Initializes a LinearGradientBrush.
        /// </summary>
        /// <param name="startPoint"> The start point. </param>
        /// <param name="endPoint"> The end point. </param>
        /// <returns> The product <see cref="IBrush"/>. </returns>
        public static IBrush LinearGradientBrush(Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 center = startPoint;
            Vector2 yPoint = endPoint;

            return new BrushBase
            {
                Type = BrushType.LinearGradient,

                Stops = GreyWhiteMeshHelpher.GetGradientStopArray(),
                Extend = CanvasEdgeBehavior.Clamp,

                Center = center,
                YPoint = yPoint,
            };
        }
        /// <summary>
        /// Initializes a LinearGradientBrush.
        /// </summary>       
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The product <see cref="IBrush"/>. </returns>
        public static IBrush LinearGradientBrush(Transformer transformer)
        {
            Vector2 center = transformer.Center;
            Vector2 yPoint = transformer.CenterBottom;

            return new BrushBase
            {
                Type = BrushType.LinearGradient,

                Stops = GreyWhiteMeshHelpher.GetGradientStopArray(),
                Extend = CanvasEdgeBehavior.Clamp,

                Center = center,
                YPoint = yPoint,
            };
        }


        /// <summary>
        /// Initializes a RadialGradientBrush.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The product <see cref="IBrush"/>. </returns>
        public static IBrush RadialGradientBrush(Transformer transformer)
        {
            Vector2 center = transformer.Center;
            Vector2 yPoint = transformer.CenterBottom;

            return new BrushBase
            {
                Type = BrushType.RadialGradient,

                Stops = GreyWhiteMeshHelpher.GetGradientStopArray(),
                Extend = CanvasEdgeBehavior.Clamp,

                Center = center,
                YPoint = yPoint,
            };
        }


        /// <summary>
        /// Initializes a EllipticalGradientBrush.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <returns> The product <see cref="IBrush"/>. </returns>
        public static IBrush EllipticalGradientBrush(Transformer transformer)
        {
            Vector2 center = transformer.Center;
            Vector2 xPoint = transformer.CenterRight;
            Vector2 yPoint = transformer.CenterBottom;

            return new BrushBase
            {
                Type = BrushType.EllipticalGradient,

                Stops = GreyWhiteMeshHelpher.GetGradientStopArray(),
                Extend = CanvasEdgeBehavior.Clamp,

                Center = center,
                XPoint = xPoint,
                YPoint = yPoint,
            };
        }


        /// <summary>
        /// Initializes a ImageBrush.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="photocopier"> The photocopier. </param>
        /// <returns> The product <see cref="IBrush"/>. </returns>
        public static IBrush ImageBrush(Transformer transformer, Photocopier photocopier)
        {
            Vector2 center = transformer.Center;
            Vector2 xPoint = transformer.CenterRight;
            Vector2 yPoint = transformer.CenterBottom;

            return new BrushBase
            {
                Type = BrushType.Image,

                Photocopier = photocopier,

                Center = center,
                XPoint = xPoint,
                YPoint = yPoint,
            };
        }

        /// <summary>
        /// Initializes a ImageBrush.
        /// </summary>
        /// <param name="center"> The center. </param>
        /// <param name="width"> The photocopier width. </param>
        /// <param name="height"> The photocopier height. </param>
        /// <param name="photocopier"> The photocopier. </param>
        /// <returns> The product <see cref="IBrush"/>. </returns>
        public static IBrush ImageBrush(Vector2 center, float width, float height, Photocopier photocopier)
        {
            Vector2 xPoint = new Vector2(center.X + width / 2, center.Y);
            Vector2 yPoint = new Vector2(center.X, center.Y + height / 2);

            return new BrushBase
            {
                Type = BrushType.Image,

                Photocopier = photocopier,

                Center = center,
                XPoint = xPoint,
                YPoint = yPoint,
            };
        }
                     
    }
}
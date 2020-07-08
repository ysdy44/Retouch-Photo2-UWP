using HSVColorPickers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System.Linq;
using System.Numerics;
using Windows.UI;
using FanKit.Transformers;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Represents a brush that can have fill properties. Provides a filling method.
    /// </summary>
    public partial class BrushBase : IBrush
    {

        /// <summary>
        /// Change the brush's type.
        /// </summary>
        /// <param name="type"> The new type. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="photo"> The photo. </param>
        public void TypeChange(BrushType type, Transformer transformer, Photo photo = null)
        {
            switch (type)
            {
                case BrushType.None:
                    break;

                case BrushType.Color:
                    this._changeColor();
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                    this._changingStops();
                    this._changingLinearGradientPoints(transformer);
                    break;
                case BrushType.EllipticalGradient:
                    this._changingStops();
                    this._changingEllipticalGradientPoints(transformer);
                    break;

                case BrushType.Image:
                    {
                        //Photo
                        if (photo == null) return;

                        Photocopier photocopier = photo.ToPhotocopier();
                        float width = photo.Width;
                        float height = photo.Height;

                        this.Photocopier = photocopier;

                        Vector2 center = this.Center;
                        Vector2 yPoint = this.YPoint;

                        //this.Center = center;
                        this.XPoint = BrushBase.YToX(yPoint, center, width, height);
                        //this.YPoint = yPoint;
                        this._changingEllipticalGradientPoints(transformer);
                    }
                    break;

                default:
                    break;
            }

            this.Type = type;
        }


        private void _changeColor()
        {
            switch (this.Type)
            {
                case BrushType.Color:
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    this.Color = this.Stops.Last().Color;
                    break;
                    
                default:
                    this.Color = Colors.LightGray;
                    break;
            }
        }

        private void _changingStops()
        {
            switch (this.Type)
            {
                case BrushType.Color:
                    this.Stops = GreyWhiteMeshHelpher.GetGradientStopArray(this.Color);
                    break;

                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                case BrushType.EllipticalGradient:
                    break;

                default:
                    this.Stops = GreyWhiteMeshHelpher.GetGradientStopArray();
                    break;
            }
        }


        private void _changingLinearGradientPoints(Transformer transformer)
        {
            switch (this.Type)
            {
                case BrushType.None:
                case BrushType.Color:
                    this.Center = transformer.Center;
                    this.YPoint = transformer.CenterBottom;
                    break;
            }
        }

        private void _changingEllipticalGradientPoints(Transformer transformer)
        {
            switch (this.Type)
            {
                case BrushType.None:
                case BrushType.Color:
                    this.Center = transformer.Center;
                    this.XPoint = transformer.CenterRight;
                    this.YPoint = transformer.CenterBottom;
                    break;
                case BrushType.LinearGradient:
                case BrushType.RadialGradient:
                    this.XPoint = BrushBase.YToX(this.YPoint, this.Center);
                    break;
            }
        }
        
    }
}
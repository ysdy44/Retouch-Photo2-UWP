using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Styles;
using System.ComponentModel;
using Windows.UI;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Sets the stand style
        /// switch by <see cref="Retouch_Photo2.Layers.LayerType"/> to
        /// <see cref="ViewModel.StandGeometryStyle"/>
        /// <see cref="ViewModel.StandCurveStyle"/>
        /// <see cref="ViewModel.StandTextStyle"/>
        /// </summary>
        public Layerage StandStyleLayerage
        {
            set
            {
                ILayer layer = value.Self;
                this.StandStyleLayer = layer;
            }
        }

        /// <summary>
        /// Sets the stand style
        /// switch by <see cref="Retouch_Photo2.Layers.LayerType"/> to
        /// <see cref="ViewModel.StandGeometryStyle"/>
        /// <see cref="ViewModel.StandCurveStyle"/>
        /// <see cref="ViewModel.StandTextStyle"/>
        /// </summary>
        public ILayer StandStyleLayer
        {
            set
            {
                ILayer layer = value;

                //Switch
                switch (layer.Type)
                {
                    //Curve & Pattern
                    case LayerType.Curve:
                    case LayerType.PatternGrid:
                    case LayerType.PatternSpotted:
                        if (value != standStyleCurveLayer)
                            this.standStyleCurveLayer = layer;
                        break;

                    //Text
                    case LayerType.TextFrame:
                    case LayerType.TextArtistic:
                        if (value != standStyleTextLayer)
                            this.standStyleTextLayer = layer;
                        break;

                    //Geometry
                    default:
                        if (value != StandStyleGeometryLayer)
                            this.StandStyleGeometryLayer = layer;
                        break;
                }
            }
        }


        private ILayer StandStyleGeometryLayer;
        private ILayer standStyleCurveLayer;
        private ILayer standStyleTextLayer;
                    

        /// <summary>
        /// Gets the stand geometry style.
        /// </summary>
        public IStyle StandGeometryStyle
        {
            get
            {
                if (this.StandStyleGeometryLayer is ILayer layer)
                {
                    //CacheBrush
                    Transformer transformer = layer.Transform.Transformer;
                    IStyle style = layer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = BrushBase.ColorBrush(Colors.LightGray),
                    Stroke = BrushBase.ColorBrush(Colors.Black),
                    StrokeWidth = 0,
                    StrokeStyle = new CanvasStrokeStyle(),
                    Transparency = new BrushBase
                    {
                        Color = Colors.Transparent
                    }
                };
            }
        }

        /// <summary>
        /// Gets the stand curve style.
        /// </summary>
        public IStyle StandCurveStyle
        {
            get
            {
                if (this.standStyleCurveLayer is ILayer layer)
                {
                    //CacheBrush
                    Transformer transformer = layer.Transform.Transformer;
                    IStyle style = layer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = new BrushBase(),
                    Stroke = BrushBase.ColorBrush(Colors.Black),
                    StrokeWidth = 3,
                    StrokeStyle = new CanvasStrokeStyle(),
                    Transparency = new BrushBase
                    {
                        Color = Colors.Transparent
                    }
                };
            }
        }

        /// <summary>
        /// Gets the stand text style.
        /// </summary>
        public IStyle StandTextStyle
        {
            get
            {
                if (this.standStyleTextLayer is ILayer layer)
                {
                    //CacheBrush
                    Transformer transformer = layer.Transform.Transformer;
                    IStyle style = layer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = BrushBase.ColorBrush(Colors.Black),
                    Stroke = new BrushBase(),
                    StrokeWidth = 0,
                    StrokeStyle = new CanvasStrokeStyle(),
                    Transparency = new BrushBase
                    {
                        Color = Colors.Transparent
                    }
                };
            }
        }
        
    }
}

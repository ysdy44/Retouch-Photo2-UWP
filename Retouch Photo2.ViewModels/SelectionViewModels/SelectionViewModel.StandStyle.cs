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
        /// Sets the standard style
        /// switch by <see cref="Retouch_Photo2.Layers.LayerType"/> to
        /// <see cref="ViewModel.StandardGeometryStyle"/>
        /// <see cref="ViewModel.StandardCurveStyle"/>
        /// <see cref="ViewModel.StandardTextStyle"/>
        /// </summary>
        public Layerage StandardStyleLayerage
        {
            set
            {
                ILayer layer = value.Self;
                this.StandardStyleLayer = layer;
            }
        }

        /// <summary>
        /// Sets the standard style
        /// switch by <see cref="Retouch_Photo2.Layers.LayerType"/> to
        /// <see cref="ViewModel.StandardGeometryStyle"/>
        /// <see cref="ViewModel.StandardCurveStyle"/>
        /// <see cref="ViewModel.StandardTextStyle"/>
        /// </summary>
        public ILayer StandardStyleLayer
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
                        if (value != standardStyleCurveLayer)
                            this.standardStyleCurveLayer = layer;
                        break;

                    //Text
                    case LayerType.TextFrame:
                    case LayerType.TextArtistic:
                        if (value != standardStyleTextLayer)
                            this.standardStyleTextLayer = layer;
                        break;

                    //Geometry
                    default:
                        if (value != standardStyleGeometryLayer)
                            this.standardStyleGeometryLayer = layer;
                        break;
                }
            }
        }


        private ILayer standardStyleGeometryLayer;
        private ILayer standardStyleCurveLayer;
        private ILayer standardStyleTextLayer;


        /// <summary>
        /// Gets the standard geometry style.
        /// </summary>
        public IStyle StandardGeometryStyle
        {
            get
            {
                if (this.standardStyleGeometryLayer is ILayer layer)
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
        /// Gets the standard curve style.
        /// </summary>
        public IStyle StandardCurveStyle
        {
            get
            {
                if (this.standardStyleCurveLayer is ILayer layer)
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
        /// Gets the standard text style.
        /// </summary>
        public IStyle StandardTextStyle
        {
            get
            {
                if (this.standardStyleTextLayer is ILayer layer)
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

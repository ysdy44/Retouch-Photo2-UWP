using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Styles;
using System.ComponentModel;
using Windows.UI;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some selection propertys of the application.
    /// </summary>
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

                //Switch
                switch (layer.Type)
                {
                    case LayerType.Curve:
                        if (value != standStyleCurveLayerage)
                            this.standStyleCurveLayerage = value;
                        break;

                    case LayerType.TextFrame:
                    case LayerType.TextArtistic:
                        if (value != standStyleTextLayerage)
                            this.standStyleTextLayerage = value;
                        break;

                    default:
                        if (value != StandStyleGeometryLayerage)
                            this.StandStyleGeometryLayerage = value;
                        break;
                }
            }
        }


        private Layerage StandStyleGeometryLayerage;
        private Layerage standStyleCurveLayerage;
        private Layerage standStyleTextLayerage;
                    

        /// <summary>
        /// Gets the stand geometry style.
        /// </summary>
        public Style StandGeometryStyle
        {
            get
            {
                if (this.StandStyleGeometryLayerage != null)
                {
                    ILayer layer = this.StandStyleGeometryLayerage.Self;

                    //CacheBrush
                    Transformer transformer = layer.Transform.Transformer;
                    Style style = layer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = BrushBase.ColorBrush(Colors.LightGray),
                    Stroke = new BrushBase(),
                    StrokeWidth = 0,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }

        /// <summary>
        /// Gets the stand curve style.
        /// </summary>
        public Style StandCurveStyle
        {
            get
            {
                if (this.standStyleCurveLayerage != null)
                {
                    ILayer layer = this.standStyleCurveLayerage.Self;

                    //CacheBrush
                    Transformer transformer = layer.Transform.Transformer;
                    Style style = layer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = new BrushBase(),
                    Stroke = BrushBase.ColorBrush(Colors.Black),
                    StrokeWidth = 3,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }

        /// <summary>
        /// Gets the stand text style.
        /// </summary>
        public Style StandTextStyle
        {
            get
            {
                if (this.standStyleTextLayerage != null)
                {
                    ILayer layer = this.standStyleTextLayerage.Self;

                    //CacheBrush
                    Transformer transformer = layer.Transform.Transformer;
                    Style style = layer.Style.Clone();
                    style.OneBrushPoints(transformer);
                    return style;
                }

                return new Style
                {
                    Fill = BrushBase.ColorBrush(Colors.Black),
                    Stroke = new BrushBase(),
                    StrokeWidth = 0,
                    StrokeStyle = new CanvasStrokeStyle(),
                };
            }
        }
        
    }
}

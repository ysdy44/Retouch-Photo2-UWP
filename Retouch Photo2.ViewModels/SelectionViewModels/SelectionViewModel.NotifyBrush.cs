using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Brushs.Models;
using Retouch_Photo2.Layers;
using System.ComponentModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary> Brush's IsFollowTransform. </summary>     
        public bool IsFollowTransform
        {
            get => this.isFollowTransform;
            set
            {
                if (this.isFollowTransform == value) return;
                this.isFollowTransform = value;
                this.OnPropertyChanged(nameof(this.IsFollowTransform));//Notify 
            }
        }
        private bool isFollowTransform = true;

        /// <summary> Brush's Fill or Stroke. </summary>     
        public FillOrStroke FillOrStroke
        {
            get => this.fillOrStroke;
            set
            {
                if (this.fillOrStroke == value) return;
                this.fillOrStroke = value;
                this.OnPropertyChanged(nameof(this.FillOrStroke));//Notify 
            }
        }
        private FillOrStroke fillOrStroke = FillOrStroke.Fill;


        
        /// <summary> Retouch_Photo2's the only color. </summary>
        public Color Color
        {
            get => this.color;
            set
            {
                this.color = value;
                this.OnPropertyChanged(nameof(this.Color));//Notify 
            }
        }
        private Color color = Colors.LightGray;

        /// <summary> Sets color (width fill-brush and stroke-brush). </summary>  
        public void SetColor(Color color, FillOrStroke fillOrStroke)
        {
            if (this.FillOrStroke == fillOrStroke) this.Color = color;

            switch (fillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.FillBrush = new ColorBrush(color);
                    this.SetValue((layer) =>
                    {
                        layer.StyleManager.FillBrush = new ColorBrush(color);
                    });
                    break;

                case FillOrStroke.Stroke:
                    this.StrokeBrush = new ColorBrush(color);
                    this.SetValue((layer) =>
                    {
                        layer.StyleManager.StrokeBrush = new ColorBrush(color);
                    });
                    break;
            }
        }


        /// <summary> Retouch_Photo2's the only fill-brush. </summary>
        public IBrush FillBrush
        {
            get => this.fillBrush;
            set
            {
                if (this._isSetGeometryCurveText)
                {
                    switch (this.Type)
                    {
                        case LayerType.Curve:
                        case LayerType.CurveMulti:
                            this.fillBrushCurve = value.Clone();
                            break;

                        case LayerType.TextFrame:
                        case LayerType.TextArtistic:
                            this.fillBrushText = value.Clone();
                            break;

                        default:
                            this.fillBrushGeometry = value.Clone();
                            break;
                    }

                }

                this.fillBrush = value;
                this.OnPropertyChanged(nameof(this.FillBrush));//Notify 
            }
        }
        private IBrush fillBrush = new ColorBrush(Colors.LightGray);
        private IBrush fillBrushGeometry = new ColorBrush(Colors.LightGray);
        private IBrush fillBrushCurve = new NoneBrush();
        private IBrush fillBrushText = new ColorBrush(Colors.Black);

        /// <summary> Retouch_Photo2's the only stroke-brush. </summary>
        public IBrush StrokeBrush
        {
            get => this.strokeBrush;
            set
            {
                if (this._isSetGeometryCurveText)
                {
                    switch (this.Type)
                    {
                        case LayerType.Curve:
                        case LayerType.CurveMulti:
                            this.strokeBrushCurve = value.Clone();
                            break;

                        case LayerType.TextFrame:
                        case LayerType.TextArtistic:
                            this.strokeBrushText = value.Clone();
                            break;

                        default:
                            this.strokeBrushGeometry = value.Clone();
                            break;
                    }

                }

                this.strokeBrush = value;
                this.OnPropertyChanged(nameof(this.StrokeBrush));//Notify 
            }
        }
        private IBrush strokeBrush = new NoneBrush();
        private IBrush strokeBrushGeometry = new NoneBrush();
        private IBrush strokeBrushCurve = new ColorBrush(Colors.Black);
        private IBrush strokeBrushText = new NoneBrush();

        /// <summary> Retouch_Photo2's the only stroke-width. </summary>
        public float StrokeWidth
        {
            get => this.strokeWidth;
            set
            {
                if (this._isSetGeometryCurveText)
                {
                    switch (this.Type)
                    {
                        case LayerType.Curve:
                        case LayerType.CurveMulti:
                            this.strokeWidthCurve = value;
                            break;

                        case LayerType.TextFrame:
                        case LayerType.TextArtistic:
                            this.strokeWidthText = value;
                            break;

                        default:
                            this.strokeWidthGeometry = value;
                            break;
                    }
                }

                this.strokeWidth = value;
                this.OnPropertyChanged(nameof(this.StrokeWidth));//Notify 
            }
        }
        private float strokeWidth = 0;
        private float strokeWidthGeometry = 0;
        private float strokeWidthCurve = 1;
        private float strokeWidthText = 0;

        /// <summary> Retouch_Photo2's the only stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle
        {
            get => this.strokeStyle;
            set
            {
                if (this._isSetGeometryCurveText)
                {
                    switch (this.Type)
                    {
                        case LayerType.Curve:
                        case LayerType.CurveMulti:
                            this.strokeStyleCurve = value.Clone();
                            break;

                        case LayerType.TextFrame:
                        case LayerType.TextArtistic:
                            this.strokeStyleText = value.Clone();
                            break;

                        default:
                            this.strokeStyleGeometry = value.Clone();
                            break;
                    }

                }

                this.strokeStyle = value;
                this.OnPropertyChanged(nameof(this.StrokeStyle));//Notify 
            }
        }
        private CanvasStrokeStyle strokeStyle = new CanvasStrokeStyle();
        private CanvasStrokeStyle strokeStyleGeometry = new CanvasStrokeStyle();
        private CanvasStrokeStyle strokeStyleCurve = new CanvasStrokeStyle();
        private CanvasStrokeStyle strokeStyleText = new CanvasStrokeStyle();



        bool _isSetGeometryCurveText = true;
        /// <summary> Sets style-manager. </summary>  
        public void SetStyleManager(StyleManager styleManager)
        {
            this._isSetGeometryCurveText = false;


            if (styleManager == null)
            {
                this.IsFollowTransform = true;
                this.FillBrush = new NoneBrush();
                this.StrokeBrush = new NoneBrush();
                this.StrokeWidth = 0;
                this.StrokeStyle = new CanvasStrokeStyle();
            }
            else
            {
                this.IsFollowTransform = styleManager.IsFollowTransform;
                this.FillBrush = styleManager.FillBrush;
                this.StrokeBrush = styleManager.StrokeBrush;
                this.StrokeWidth = styleManager.StrokeWidth;
                this.StrokeStyle = styleManager.StrokeStyle;

                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        if (styleManager.FillBrush.Type == BrushType.Color)
                            this.Color = styleManager.FillBrush.Color;
                        break;
                    case FillOrStroke.Stroke:
                        if (styleManager.StrokeBrush.Type == BrushType.Color)
                            this.Color = styleManager.StrokeBrush.Color;
                        break;
                }
            }


            this._isSetGeometryCurveText = true;
        }

        /// <summary> Gets style-manager. </summary>  
        public StyleManager GetStyleManagerGeometry()
        {
            return new StyleManager
            {
                IsFollowTransform = this.isFollowTransform,
                FillBrush = this.fillBrushGeometry.Clone(),
                StrokeBrush = this.strokeBrushGeometry.Clone(),
                StrokeWidth = this.strokeWidthGeometry,
                StrokeStyle = this.strokeStyleGeometry.Clone()
            };
        }
        public StyleManager GetStyleManagerCurve()
        {
            return new StyleManager
            {
                IsFollowTransform = this.isFollowTransform,
                FillBrush = this.fillBrushCurve.Clone(),
                StrokeBrush = this.strokeBrushCurve.Clone(),
                StrokeWidth = this.strokeWidthCurve,
                StrokeStyle = this.strokeStyleCurve.Clone()
            };
        }
        public StyleManager GetStyleManagerText()
        {
            return new StyleManager
            {
                IsFollowTransform = this.isFollowTransform,
                FillBrush = this.fillBrushText.Clone(),
                StrokeBrush = this.strokeBrushText.Clone(),
                StrokeWidth = this.strokeWidthText,
                StrokeStyle = this.strokeStyleText.Clone()
            };
        }



        /// <summary> Set mode. </summary>  
        public void SetModeStyleManager()
        {
            switch (this.SelectionMode)
            {
                case ListViewSelectionMode.None:
                    this.SetStyleManager(null);
                    break;
                case ListViewSelectionMode.Single:
                    this.SetStyleManager(this.Layer.StyleManager);
                    break;
                case ListViewSelectionMode.Multiple:
                    ILayer firstLayer = this.Layers.First();
                    this.SetStyleManager(firstLayer.StyleManager);
                    break;
            }
        }

    }
}
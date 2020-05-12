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
        
        public void SetFillColor(Color color)
        {
            //Selection
            this.FillBrush = new ColorBrush(color);
            this.SetValue((layer) =>
            {
                layer.StyleManager.FillBrush = new ColorBrush(color);
            });
        }
        public void SetStrokeColor(Color color)
        {
            //Selection
            this.StrokeBrush = new ColorBrush(color);
            this.SetValue((layer) =>
            {
                layer.StyleManager.StrokeBrush = new ColorBrush(color);
            });
        }



        /// <summary> Retouch_Photo2's the only fill-brush. </summary>
        public IBrush FillBrush
        {
            get
            {
                switch (this.Type)
                {
                    case LayerType.Curve:
                    case LayerType.CurveMulti:
                        return this.fillBrushCurve; 

                    case LayerType.TextFrame:
                    case LayerType.TextArtistic:
                        return this.fillBrushText;

                    default:
                        return this.fillBrushGeometry;
                }
            }
            set
            {
                switch (this.Type)
                {
                    case LayerType.Curve:
                    case LayerType.CurveMulti:
                        this.fillBrushCurve = value;
                        break;

                    case LayerType.TextFrame:
                    case LayerType.TextArtistic:
                        this.fillBrushText = value;
                        break;

                    default:
                        this.fillBrushGeometry = value;
                        break;
                }

                this.OnPropertyChanged(nameof(this.FillBrush));//Notify 
            }
        }
        private IBrush fillBrushGeometry = new ColorBrush(Colors.LightGray);
        private IBrush fillBrushCurve = new NoneBrush();
        private IBrush fillBrushText = new ColorBrush(Colors.Black);

        /// <summary> Retouch_Photo2's the only stroke-brush. </summary>
        public IBrush StrokeBrush
        {
            get
            {
                switch (this.Type)
                {
                    case LayerType.Curve:
                    case LayerType.CurveMulti:
                        return this.strokeBrushCurve;

                    case LayerType.TextFrame:
                    case LayerType.TextArtistic:
                        return this.strokeBrushText;

                    default:
                        return this.strokeBrushGeometry;
                }
            }
            set
            {
                switch (this.Type)
                {
                    case LayerType.Curve:
                    case LayerType.CurveMulti:
                        this.strokeBrushCurve = value;
                        break;

                    case LayerType.TextFrame:
                    case LayerType.TextArtistic:
                        this.strokeBrushText = value;
                        break;

                    default:
                        this.strokeBrushGeometry = value;
                        break;
                }

                this.OnPropertyChanged(nameof(this.StrokeBrush));//Notify 
            }
        }
        private IBrush strokeBrushGeometry = new NoneBrush();
        private IBrush strokeBrushCurve = new ColorBrush(Colors.Black);
        private IBrush strokeBrushText = new NoneBrush();

        /// <summary> Retouch_Photo2's the only stroke-width. </summary>
        public float StrokeWidth
        {
            get
            {
                switch (this.Type)
                {
                    case LayerType.Curve:
                    case LayerType.CurveMulti:
                        return this.strokeWidthCurve;

                    case LayerType.TextFrame:
                    case LayerType.TextArtistic:
                        return this.strokeWidthText;

                    default:
                        return this.strokeWidthGeometry;
                }
            }
            set
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

                this.OnPropertyChanged(nameof(this.StrokeWidth));//Notify 
            }
        }
        private float strokeWidthGeometry = 0;
        private float strokeWidthCurve = 1;
        private float strokeWidthText = 0;



        /// <summary> Sets style-manager. </summary>  
        public void SetStyleManager(StyleManager styleManager)
        {
            if (styleManager == null)
            {
                this.IsFollowTransform = true;
                this.FillBrush = new NoneBrush();
                this.StrokeBrush = new NoneBrush();
                this.StrokeWidth = 0;
            }
            else
            {
                this.IsFollowTransform = styleManager.IsFollowTransform;
                this.FillBrush = styleManager.FillBrush;
                this.StrokeBrush = styleManager.StrokeBrush;
                this.StrokeWidth = styleManager.StrokeWidth;

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
        }


        /// <summary> Gets style-manager. </summary>  
        public StyleManager GetStyleManagerGeometry()
        {
            return new StyleManager
            {
                IsFollowTransform = this.isFollowTransform,
                FillBrush = this.fillBrushGeometry.Clone(),
                StrokeBrush = this.strokeBrushGeometry.Clone(),
                StrokeWidth = this.strokeWidthGeometry
            };
        }
        public StyleManager GetStyleManagerCurve()
        {
            return new StyleManager
            {
                IsFollowTransform = this.isFollowTransform,
                FillBrush = this.fillBrushCurve.Clone(),
                StrokeBrush = this.strokeBrushCurve.Clone(),
                StrokeWidth = this.strokeWidthCurve
            };
        }
        public StyleManager GetStyleManagerText()
        {
            return new StyleManager
            {
                IsFollowTransform = this.isFollowTransform,
                FillBrush = this.fillBrushText.Clone(),
                StrokeBrush = this.strokeBrushText.Clone(),
                StrokeWidth = this.strokeWidthText
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
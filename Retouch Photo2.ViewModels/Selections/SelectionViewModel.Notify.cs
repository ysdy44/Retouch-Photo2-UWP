using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.ILayer;
using Retouch_Photo2.Layers.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.ViewModels.Selections
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary> <see cref = "SelectionViewModel" />'s opacity. </summary>
        public float Opacity;
        /// <summary> Sets the <see cref = "SelectionViewModel.Opacity" />. </summary>
        public void SetOpacity(float value)
        {
            if (this.Opacity == value) return;
            this.Opacity = value;
            this.OnPropertyChanged(nameof(this.Opacity));//Notify 
        }


        /// <summary> <see cref = "SelectionViewModel" />'s blend type. </summary>
        public BlendType BlendType;
        /// <summary> Sets the <see cref = "SelectionViewModel.BlendType" />. </summary>
        public void SetBlendType(BlendType value)
        {
            if (this.BlendType == value) return;
            this.BlendType = value;
            this.OnPropertyChanged(nameof(this.BlendType));//Notify 
        }


        /// <summary> <see cref = "SelectionViewModel" />'s visibility. </summary>
        public Visibility Visibility
        {
            get => this.visibility;
            set
            {
                this.visibility = value;
                this.OnPropertyChanged(nameof(this.Visibility));//Notify 
            }
        }
        private Visibility visibility;


        /// <summary> GroupLayer's Exist. </summary>     
        public bool IsGroupLayer;
        /// <summary> Sets GroupLayer. </summary>     
        private void SetGroupLayer(Layer layer)
        {
            if (layer==null)
            {
                this.IsGroupLayer = false;
                this.OnPropertyChanged(nameof(this.IsGroupLayer));//Notify 
            }

            if (layer is GroupLayer acrylicLayer)
            {
                this.IsGroupLayer = true;
                this.OnPropertyChanged(nameof(this.IsGroupLayer));//Notify 
            }
        }


        /// <summary> <see cref = "SelectionViewModel" />'s Children. </summary>
        public ObservableCollection<Layer> Children
        {
            get => this.children;
            set
            {
                this.children = value;
                this.OnPropertyChanged(nameof(this.Children));//Notify 
            }
        }
        private ObservableCollection<Layer> children;

               
        /// <summary> <see cref = "SelectionViewModel" />'s EffectManager. </summary>
        public EffectManager EffectManager
        {
            get => this.effectManager;
            set
            {
                this.effectManager = value;
                this.OnPropertyChanged(nameof(this.EffectManager));//Notify 
            }
        }
        private EffectManager effectManager;

        /// <summary> <see cref = "SelectionViewModel" />'s AdjustmentManager. </summary>
        public AdjustmentManager AdjustmentManager
        {
            get => this.adjustmentManager;
            set
            {
                this.adjustmentManager = value;
                this.OnPropertyChanged(nameof(this.AdjustmentManager));//Notify 
            }
        }
        private AdjustmentManager adjustmentManager;


        /// <summary> AcrylicLayer's TintOpacity. </summary>     
        public float AcrylicTintOpacity = 0.5f;
        /// <summary> AcrylicLayer's BlurAmount. </summary>     
        public float AcrylicBlurAmount = 12.0f;
        /// <summary> Sets AcrylicLayer. </summary>     
        private void SetAcrylicLayer(Layer layer)
        {
            if (layer is AcrylicLayer acrylicLayer)
            {
                this.AcrylicTintOpacity = acrylicLayer.TintOpacity;
                this.OnPropertyChanged(nameof(this.AcrylicTintOpacity));//Notify 

                this.AcrylicBlurAmount = acrylicLayer.BlurAmount;
                this.OnPropertyChanged(nameof(this.AcrylicBlurAmount));//Notify 

                return;
            }
        }


        
        /// <summary> Brush's Fill or Stroke. </summary>     
        public FillOrStroke FillOrStroke;

        /// <summary> Brush's type. </summary>     
        public BrushType BrushType;

        /// <summary> Brush's color. </summary>     
        public Color BrushColor;
        /// <summary> Brush's gradient colors. </summary>     
        public CanvasGradientStop[] BrushArray;


        /// <summary> Brush''s linear gradient start point. </summary>
        public Vector2 BrushLinearGradientStartPoint;
        /// <summary> Brush''s linear gradient end point. </summary>
        public Vector2 BrushLinearGradientEndPoint;

        /// <summary> Brush''s radial gradient center point. </summary>
        public Vector2 BrushRadialGradientCenter;
        /// <summary> Brush''s radial gradient control point. </summary>
        public Vector2 BrushRadialGradientPoint;

        /// <summary> Brush''s elliptical gradient center point. </summary>
        public Vector2 BrushEllipticalGradientCenter;
        /// <summary> Brush''s elliptical gradient x-point. </summary>
        public Vector2 BrushEllipticalGradientXPoint;
        /// <summary> Brush''s elliptical gradient y-point. </summary>
        public Vector2 BrushEllipticalGradientYPoint;


        /// <summary> Sets GeometryLayer's brush. </summary>     
        public void SetBrush(Brush brush)
        {
            switch (brush.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    {
                        this.BrushColor = brush.Color;
                        this.OnPropertyChanged(nameof(this.BrushColor));//Notify 
                    }
                    break;
                case BrushType.LinearGradient:
                    {
                        this.BrushLinearGradientStartPoint = brush.LinearGradientStartPoint;
                        this.BrushLinearGradientEndPoint = brush.LinearGradientEndPoint;

                        this.BrushArray = brush.Array;
                        this.OnPropertyChanged(nameof(this.BrushArray));//Notify 
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.BrushRadialGradientCenter = brush.RadialGradientCenter;
                        this.BrushRadialGradientPoint = brush.RadialGradientPoint;

                        this.BrushArray = brush.Array;
                        this.OnPropertyChanged(nameof(this.BrushArray));//Notify 
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.BrushEllipticalGradientCenter = brush.EllipticalGradientCenter;
                        this.BrushEllipticalGradientXPoint = brush.EllipticalGradientXPoint;
                        this.BrushEllipticalGradientYPoint = brush.EllipticalGradientYPoint;

                        this.BrushArray = brush.Array;
                        this.OnPropertyChanged(nameof(this.BrushArray));//Notify 
                    }
                    break;
                case BrushType.Image:
                    break;
                default:
                    break;
            }

            this.BrushType = brush.Type;
            this.OnPropertyChanged(nameof(this.BrushType));//Notify 
        }

        /// <summary> Sets GeometryLayer. </summary>     
        private void SetGeometryLayer(Layer layer)
        {
            if (layer is IGeometryLayer geometryLayer)
            {
                switch (this.FillOrStroke)
                {
                    case FillOrStroke.Fill:
                        this.SetBrush(geometryLayer.FillBrush);
                        break;
                    case FillOrStroke.Stroke:
                        this.SetBrush(geometryLayer.StrokeBrush);
                        break;
                }
                return;
            }

            this.BrushType = BrushType.None;
            this.OnPropertyChanged(nameof(this.BrushType));//Notify 
        }

               
        public void InitializeLinearGradient(Vector2 startPoint, Vector2 endPoint)
        {
            //Brush
            this.BrushType = BrushType.LinearGradient;
            this.BrushArray = new CanvasGradientStop[]
            {
                 new CanvasGradientStop{Color= Colors.White, Position=0.0f },
                 new CanvasGradientStop{Color= Colors.Gray, Position=1.0f }
            };
            this.BrushLinearGradientStartPoint = startPoint;
            this.BrushLinearGradientEndPoint = endPoint;

            //Selection
            this.SetValue((layer) =>
            {
                if (layer is IGeometryLayer geometryLayer)
                {
                    switch (this.FillOrStroke)
                    {
                        case FillOrStroke.Fill:
                            {
                                geometryLayer.FillBrush.Type = BrushType.LinearGradient;
                                geometryLayer.FillBrush.LinearGradientStartPoint = startPoint;
                                geometryLayer.FillBrush.LinearGradientEndPoint = endPoint;
                            }
                            break;
                        case FillOrStroke.Stroke:
                            {
                                geometryLayer.FillBrush.Type = BrushType.LinearGradient;
                                geometryLayer.StrokeBrush.LinearGradientStartPoint = startPoint;
                                geometryLayer.StrokeBrush.LinearGradientEndPoint = endPoint;
                            }
                            break;
                    }
                }
            });
        }



    }
}
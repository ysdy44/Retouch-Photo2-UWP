using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.ComponentModel;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {


        /// <summary> Sets the <see cref="GeometryLayer"/>. </summary>     
        private void SetGeometryLayer(ILayer layer)
        {
            if (layer is null) return;

            switch (layer.Type)
            {
                // Geometry0
                case LayerType.GeometryRectangle: break;
                case LayerType.GeometryEllipse: break;

                // Geometry1
                case LayerType.GeometryRoundRect: this.GeometryRoundRect_Corner = ((GeometryRoundRectLayer)layer).Corner; break;
                case LayerType.GeometryTriangle: this.GeometryTriangle_Center = ((GeometryTriangleLayer)layer).Center; break;
                case LayerType.GeometryDiamond: this.GeometryDiamond_Mid = ((GeometryDiamondLayer)layer).Mid; break;

                // Geometry12
                case LayerType.GeometryPentagon: this.GeometryPentagon_Points = ((GeometryPentagonLayer)layer).Points; break;
                case LayerType.GeometryStar:
                    GeometryStarLayer starLayer = (GeometryStarLayer)layer;
                    this.GeometryStar_Points = starLayer.Points;
                    this.GeometryStar_InnerRadius = starLayer.InnerRadius;
                    break;
                case LayerType.GeometryCog:
                    GeometryCogLayer cogLayer = (GeometryCogLayer)layer;
                    this.GeometryCog_Count = cogLayer.Count;
                    this.GeometryCog_InnerRadius = cogLayer.InnerRadius;
                    this.GeometryCog_Tooth = cogLayer.Tooth;
                    this.GeometryCog_Notch = cogLayer.Notch;
                    break;

                // Geometry3
                case LayerType.GeometryDount: this.GeometryDount_HoleRadius = ((GeometryDountLayer)layer).HoleRadius; break;
                case LayerType.GeometryPie: this.GeometryPie_SweepAngle = ((GeometryPieLayer)layer).SweepAngle; break;
                case LayerType.GeometryCookie:
                    GeometryCookieLayer cookieLayer = (GeometryCookieLayer)layer;
                    this.GeometryCookie_InnerRadius = cookieLayer.InnerRadius;
                    this.GeometryCookie_SweepAngle = cookieLayer.SweepAngle;
                    break;

                // Geometry4
                case LayerType.GeometryArrow:
                    GeometryArrowLayer arrowLayer = (GeometryArrowLayer)layer;
                    this.GeometryArrow_Value = arrowLayer.Value;
                    this.GeometryArrow_LeftTail = arrowLayer.LeftTail;
                    this.GeometryArrow_RightTail = arrowLayer.RightTail;
                    break;
                case LayerType.GeometryCapsule: break;
                case LayerType.GeometryHeart: this.GeometryHeart_Spread = ((GeometryHeartLayer)layer).Spread; break;
            }
        }


        #region Geometry1


        /// <summary> <see cref="GeometryRoundRectLayer"/>'s corner. </summary>     
        public float GeometryRoundRect_Corner
        {
            get => this.geometryRoundRect_Corner;
            set
            {
                if (this.geometryRoundRect_Corner == value) return;
                this.geometryRoundRect_Corner = value;
                this.OnPropertyChanged(nameof(GeometryRoundRect_Corner)); // Notify 
            }
        }
        private float geometryRoundRect_Corner = 0.12f;


        /// <summary> <see cref="GeometryTriangleLayer"/>'s center-point. </summary>     
        public float GeometryTriangle_Center
        {
            get => this.geometryTriangle_Center;
            set
            {
                if (this.geometryTriangle_Center == value) return;
                this.geometryTriangle_Center = value;
                this.OnPropertyChanged(nameof(GeometryTriangle_Center)); // Notify 
            }
        }
        private float geometryTriangle_Center = 0.5f;


        /// <summary> <see cref="GeometryDiamondLayer"/>'s mid-point. </summary>     
        public float GeometryDiamond_Mid
        {
            get => this.geometryDiamond_Mid;
            set
            {
                if (this.geometryDiamond_Mid == value) return;
                this.geometryDiamond_Mid = value;
                this.OnPropertyChanged(nameof(GeometryDiamond_Mid)); // Notify 
            }
        }
        private float geometryDiamond_Mid = 0.5f;


        #endregion


        #region Geometry2


        /// <summary> <see cref="GeometryPentagonLayer"/>'s points. </summary>     
        public int GeometryPentagon_Points
        {
            get => this.geometryPentagon_Points;
            set
            {
                if (this.geometryPentagon_Points == value) return;
                this.geometryPentagon_Points = value;
                this.OnPropertyChanged(nameof(GeometryPentagon_Points)); // Notify 
            }
        }
        private int geometryPentagon_Points = 5;


        /// <summary> <see cref="GeometryStarLayer"/>'s points. </summary>     
        public int GeometryStar_Points
        {
            get => this.geometryStar_Points;
            set
            {
                if (this.geometryStar_Points == value) return;
                this.geometryStar_Points = value;
                this.OnPropertyChanged(nameof(GeometryStar_Points)); // Notify 
            }
        }
        private int geometryStar_Points = 5;
        /// <summary> <see cref="GeometryStarLayer"/>'s inner-radius. </summary>     
        public float GeometryStar_InnerRadius
        {
            get => this.geometryStar_InnerRadius;
            set
            {
                if (this.geometryStar_InnerRadius == value) return;
                this.geometryStar_InnerRadius = value;
                this.OnPropertyChanged(nameof(GeometryStar_InnerRadius)); // Notify 
            }
        }
        private float geometryStar_InnerRadius = 0.4f;


        /// <summary> <see cref="GeometryCogLayer"/>'s count. </summary>     
        public int GeometryCog_Count
        {
            get => this.geometryCog_Count;
            set
            {
                if (this.geometryCog_Count == value) return;
                this.geometryCog_Count = value;
                this.OnPropertyChanged(nameof(GeometryCog_Count)); // Notify 
            }
        }
        private int geometryCog_Count = 8;
        /// <summary> <see cref="GeometryCogLayer"/>'s inner-radius. </summary>     
        public float GeometryCog_InnerRadius
        {
            get => this.geometryCog_InnerRadius;
            set
            {
                if (this.geometryCog_InnerRadius == value) return;
                this.geometryCog_InnerRadius = value;
                this.OnPropertyChanged(nameof(GeometryCog_InnerRadius)); // Notify 
            }
        }
        private float geometryCog_InnerRadius = 0.7f;
        /// <summary> <see cref="GeometryCogLayer"/>'s tooth. </summary>     
        public float GeometryCog_Tooth
        {
            get => this.geometryCog_Tooth;
            set
            {
                if (this.geometryCog_Tooth == value) return;
                this.geometryCog_Tooth = value;
                this.OnPropertyChanged(nameof(GeometryCog_Tooth)); // Notify 
            }
        }
        private float geometryCog_Tooth = 0.3f;
        /// <summary> <see cref="GeometryCogLayer"/>'s notch. </summary>     
        public float GeometryCog_Notch
        {
            get => this.geometryCog_Notch;
            set
            {
                if (this.geometryCog_Notch == value) return;
                this.geometryCog_Notch = value;
                this.OnPropertyChanged(nameof(GeometryCog_Notch)); // Notify 
            }
        }
        private float geometryCog_Notch = 0.6f;


        #endregion


        #region Geometry3


        /// <summary> <see cref="GeometryPieLayer"/>'s sweep-angle. </summary>     
        public float GeometryPie_SweepAngle
        {
            get => this.geometryPie_SweepAngle;
            set
            {
                if (this.geometryPie_SweepAngle == value) return;
                this.geometryPie_SweepAngle = value;
                this.OnPropertyChanged(nameof(GeometryPie_SweepAngle)); // Notify 
            }
        }
        private float geometryPie_SweepAngle = FanKit.Math.Pi / 2f;


        /// <summary> <see cref="GeometryDountLayer"/>'s hole-radius. </summary>     
        public float GeometryDount_HoleRadius
        {
            get => this.geometryDount_HoleRadius;
            set
            {
                if (this.geometryDount_HoleRadius == value) return;
                this.geometryDount_HoleRadius = value;
                this.OnPropertyChanged(nameof(GeometryDount_HoleRadius)); // Notify 
            }
        }
        private float geometryDount_HoleRadius = 0.5f;


        /// <summary> <see cref="GeometryCookieLayer"/>'s inner-radius. </summary>     
        public float GeometryCookie_InnerRadius
        {
            get => this.geometryCookie_InnerRadius;
            set
            {
                if (this.geometryCookie_InnerRadius == value) return;
                this.geometryCookie_InnerRadius = value;
                this.OnPropertyChanged(nameof(GeometryCookie_InnerRadius)); // Notify 
            }
        }
        private float geometryCookie_InnerRadius = 0.5f;
        /// <summary> <see cref="GeometryCookieLayer"/>'s sweep-angle. </summary>     
        public float GeometryCookie_SweepAngle
        {
            get => this.geometryCookie_SweepAngle;
            set
            {
                if (this.geometryCookie_SweepAngle == value) return;
                this.geometryCookie_SweepAngle = value;
                this.OnPropertyChanged(nameof(GeometryCookie_SweepAngle)); // Notify 
            }
        }
        private float geometryCookie_SweepAngle = FanKit.Math.PiOver2;


        #endregion


        #region Geometry4


        /// <summary> <see cref="GeometryArrowLayer"/>'s value. </summary>     
        public float GeometryArrow_Value
        {
            get => this.geometryArrow_Value;
            set
            {
                if (this.geometryArrow_Value == value) return;
                this.geometryArrow_Value = value;
                this.OnPropertyChanged(nameof(GeometryArrow_Value)); // Notify 
            }
        }
        private float geometryArrow_Value = 0.5f;
        /// <summary> <see cref="GeometryArrowLayer"/>'s left-tail. </summary>     
        public GeometryArrowTailType GeometryArrow_LeftTail
        {
            get => this.geometryArrow_LeftTail;
            set
            {
                if (this.geometryArrow_LeftTail == value) return;
                this.geometryArrow_LeftTail = value;
                this.OnPropertyChanged(nameof(GeometryArrow_LeftTail)); // Notify 
            }
        }
        private GeometryArrowTailType geometryArrow_LeftTail = GeometryArrowTailType.None;
        /// <summary> <see cref="GeometryArrowLayer"/>'s right-tail. </summary>     
        public GeometryArrowTailType GeometryArrow_RightTail
        {
            get => this.geometryArrow_RightTail;
            set
            {
                if (this.geometryArrow_RightTail == value) return;
                this.geometryArrow_RightTail = value;
                this.OnPropertyChanged(nameof(GeometryArrow_RightTail)); // Notify 
            }
        }
        private GeometryArrowTailType geometryArrow_RightTail = GeometryArrowTailType.Arrow;


        /// <summary> <see cref="GeometryArrowLayer"/>'s spread. </summary>     
        public float GeometryHeart_Spread
        {
            get => this.geometryHeart_Spread;
            set
            {
                if (this.geometryHeart_Spread == value) return;
                this.geometryHeart_Spread = value;
                this.OnPropertyChanged(nameof(GeometryHeart_Spread)); // Notify 
            }
        }
        private float geometryHeart_Spread = 0.8f;


        #endregion

    }
}
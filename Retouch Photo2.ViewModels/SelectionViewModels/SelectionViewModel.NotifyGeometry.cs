using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "SelectionViewModel" />. 
    /// </summary>
    public partial class SelectionViewModel : INotifyPropertyChanged
    {

        /// <summary> Sets all IGeometryLayer. </summary>     
        private void SetIGeometryLayer(ILayer layer)
        {
            if (layer is IGeometryLayer geometryLayer)
            {

                if (layer is GeometryRoundRectLayer  roundRectLayer)
                {
                    this.GeometryRoundRectCorner = roundRectLayer.Corner;
                }
            }
        }
        

        /// <summary> GeometryRoundRectLayer's corner. </summary>     
        public float GeometryRoundRectCorner
        {
            get => this.geometryRoundRectCorner;
            set
            {
                if (this.geometryRoundRectCorner == value) return;
                this.geometryRoundRectCorner = value;
                this.OnPropertyChanged(nameof(this.GeometryRoundRectCorner));//Notify 
            }
        }
        private float geometryRoundRectCorner = 0.12f;
        

        /// <summary> GeometryTriangle's center-point. </summary>     
        public float GeometryTriangleCenter
        {
            get => this.geometryTriangleCenter;
            set
            {
                if (this.geometryTriangleCenter == value) return;
                this.geometryTriangleCenter = value;
                this.OnPropertyChanged(nameof(this.GeometryTriangleCenter));//Notify 
            }
        }
        private float geometryTriangleCenter = 0.5f;


        /// <summary> GeometryDiamond's mid-point. </summary>     
        public float GeometryDiamondMid
        {
            get => this.geometryDiamondMid;
            set
            {
                if (this.geometryDiamondMid == value) return;
                this.geometryDiamondMid = value;
                this.OnPropertyChanged(nameof(this.GeometryDiamondMid));//Notify 
            }
        }
        private float geometryDiamondMid = 0.5f;


        /// <summary> GeometryPentagon's points. </summary>     
        public int GeometryPentagonPoints
        {
            get => this.geometryPentagonPoints;
            set
            {
                if (this.geometryPentagonPoints == value) return;
                this.geometryPentagonPoints = value;
                this.OnPropertyChanged(nameof(this.GeometryPentagonPoints));//Notify 
            }
        }
        private int geometryPentagonPoints = 5;
        

        /// <summary> GeometryStar's points. </summary>     
        public int GeometryStarPoints
        {
            get => this.geometryStarPoints;
            set
            {
                if (this.geometryStarPoints == value) return;
                this.geometryStarPoints = value;
                this.OnPropertyChanged(nameof(this.GeometryStarPoints));//Notify 
            }
        }
        private int geometryStarPoints = 5;
        /// <summary> GeometryStar's inner-radius. </summary>     
        public float GeometryStarInnerRadius
        {
            get => this.geometryStarInnerRadius;
            set
            {
                if (this.geometryStarInnerRadius == value) return;
                this.geometryStarInnerRadius = value;
                this.OnPropertyChanged(nameof(this.GeometryStarInnerRadius));//Notify 
            }
        }
        private float geometryStarInnerRadius = 0.4f;
        

        /// <summary> GeometryPie's inner-radius. </summary>     
        public float GeometryPieInnerRadius
        {
            get => this.geometryPieInnerRadius;
            set
            {
                if (this.geometryPieInnerRadius == value) return;
                this.geometryPieInnerRadius = value;
                this.OnPropertyChanged(nameof(this.GeometryPieInnerRadius));//Notify 
            }
        }
        private float geometryPieInnerRadius = 0.0f;
        /// <summary> GeometryPie's sweep-angle. </summary>     
        public float GeometryPieSweepAngle
        {
            get => this.geometryPieSweepAngle;
            set
            {
                if (this.geometryPieSweepAngle == value) return;
                this.geometryPieSweepAngle = value;
                this.OnPropertyChanged(nameof(this.GeometryPieSweepAngle));//Notify 
            }
        }
        private float geometryPieSweepAngle = FanKit.Math.Pi / 2f;


        /// <summary> GeometryCog's count. </summary>     
        public int GeometryCogCount
        {
            get => this.geometryCogCount;
            set
            {
                if (this.geometryCogCount == value) return;
                this.geometryCogCount = value;
                this.OnPropertyChanged(nameof(this.GeometryCogCount));//Notify 
            }
        }
        private int geometryCogCount = 8;
        /// <summary> GeometryCog's inner-radius. </summary>     
        public float GeometryCogInnerRadius
        {
            get => this.geometryCogInnerRadius;
            set
            {
                if (this.geometryCogInnerRadius == value) return;
                this.geometryCogInnerRadius = value;
                this.OnPropertyChanged(nameof(this.GeometryCogInnerRadius));//Notify 
            }
        }
        private float geometryCogInnerRadius = 0.7f;
        /// <summary> GeometryCog's tooth. </summary>     
        public float GeometryCogTooth
        {
            get => this.geometryCogTooth;
            set
            {
                if (this.geometryCogTooth == value) return;
                this.geometryCogTooth = value;
                this.OnPropertyChanged(nameof(this.GeometryCogTooth));//Notify 
            }
        }
        private float geometryCogTooth = 0.3f;
        /// <summary> GeometryCog's notch. </summary>     
        public float GeometryCogNotch
        {
            get => this.geometryCogNotch;
            set
            {
                if (this.geometryCogNotch == value) return;
                this.geometryCogNotch = value;
                this.OnPropertyChanged(nameof(this.GeometryCogNotch));//Notify 
            }
        }
        private float geometryCogNotch = 0.6f;





    }
}